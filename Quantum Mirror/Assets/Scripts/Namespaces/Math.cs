using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sam {
    public class Math {
        public static Transform ScaleAround(Vector3 worldPos, Vector3 newScale, Transform transform) {
            Vector3 localScalePos = transform.InverseTransformPoint(worldPos);
            Vector3 scaleVector = transform.localPosition - localScalePos;
            Vector3 oldScale = transform.localScale;
            Vector3 scaleRatio = Div(newScale, oldScale);
            transform.localScale = newScale;
            transform.localPosition = Scale(scaleVector, scaleRatio) + localScalePos;
            return transform;
        }

        private static Vector3 Scale(Vector3 a, Vector3 b) {
            return new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);
        }

        private static Vector3 Div(Vector3 a, Vector3 b) {
            return new Vector3(b.x == 0f ? 0 : a.x / b.x, b.y == 0f ? 0 : a.y / b.y, b.z == 0f ? 0 : a.z / b.z);
        }

        public static float Map(float value, float inputFrom, float inputTo, float outputFrom, float outputTo) {
            return (value - inputFrom) / (inputTo - inputFrom) * (outputTo - outputFrom) + outputFrom;
        }

        //Calculates a velocity that will throw the projectile in arc towards the target.
        public static Vector3 BallisticVelocity(Transform origin, Transform target) {
            Vector3 dir = target.position - (origin.position + origin.up); // get target direction
            float h = dir.y;  // get height difference
            dir.y = 0;  // retain only the horizontal direction
            float dist = dir.magnitude;  // get horizontal distance
            dir.y = dist;  // set elevation to 45 degrees
            dist += h;  // correct for different heights
            float vel = Mathf.Sqrt(dist * Physics.gravity.magnitude);
            return vel * dir.normalized;  // returns Vector3 velocity
        }
    }
}
