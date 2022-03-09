using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienIKHandler : MonoBehaviour
{

    public SphereCollider[] subCircles;
    
    [SerializeField] LayerMask terrainLayer = default;
    [SerializeField] Transform body = default;
    // [SerializeField] AlienIKHandler otherFoot = default;
    [SerializeField] public float speed = 1;
    [SerializeField] float stepDistance = 4;
    [SerializeField] float stepLength = 4;
    [SerializeField] float stepHeight = 1;
    [SerializeField] Vector3 footOffset = default;
    float footSpacing;
    Vector3 oldPosition, currentPosition, newPosition;
    // Vector3 oldNormal, currentNormal, newNormal;
    float lerp;

    private void Start()
    {
        footSpacing = transform.localPosition.x;
        currentPosition = newPosition = oldPosition = transform.position;
        // currentNormal = newNormal = oldNormal = transform.up;
        lerp = 1;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = currentPosition;

        Ray ray = new Ray(body.position + (body.right * footSpacing), Vector3.down);

        if (Physics.Raycast(ray, out RaycastHit info, 10, terrainLayer.value))
        {
            if (Vector3.Distance(newPosition, info.point) > stepDistance && lerp >= 1)
                // && !otherFoot.IsMoving() 
            {
                lerp = 0;
                int direction = body.InverseTransformPoint(info.point).z > body.InverseTransformPoint(newPosition).z ? 1 : -1;
                newPosition = info.point + (body.forward * stepLength * direction) + footOffset;
            }
        }

        if (lerp < 1)
        {
            Vector3 tempPosition = Vector3.Lerp(oldPosition, newPosition, lerp);
            tempPosition.y += Mathf.Sin(lerp * Mathf.PI) * stepHeight;

            currentPosition = tempPosition;
            lerp += Time.deltaTime * speed;
        }
        else
        {
            oldPosition = newPosition;
        }
    }

    public bool IsMoving()
    {
        return lerp < 1;
    }
}
