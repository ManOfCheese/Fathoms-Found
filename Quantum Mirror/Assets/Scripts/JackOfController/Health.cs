using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DamageType
{
	Normal,
	Falling
}

public class Health : MonoBehaviour
{

	[Header( "Health" )]
	public FloatValue health;
	public BoolValue legsBroken;

	[Header( "Falling Damage" )]
	public bool takeFallingDamage;
	[Tooltip( "How many health points of fall damage you take per unit of distance." )]
	public float fallDamagePerUnit;
	[Tooltip( "Threshold in units at which fall damage is applied." )]
	public float fallDistanceThreshold;
	[Tooltip( "The damage threshold at which you break your legs and movement is disabled." )]
	public int legBreakThreshold;

	private JackOfController joc;
	private bool grounded;
	private float y;

	private void Awake()
	{
		joc = GetComponent<JackOfController>();
	}

	private void Update()
	{
		if ( grounded != joc.grounded ) {
			grounded = joc.grounded;

			if ( grounded && takeFallingDamage )
			{
				float distanceFallen = y - joc.transform.position.y;
				if ( distanceFallen > fallDistanceThreshold )
				{
					int fallDamage = Mathf.RoundToInt( distanceFallen * fallDamagePerUnit );
					TakeDamage( fallDamage, DamageType.Falling );
					y = joc.transform.position.y;
				}
			}
		}

		if ( !grounded )
		{
			if ( joc.transform.position.y > y )
				y = joc.transform.position.y;
		}
	}

	public void TakeDamage( int damage, DamageType damageType )
	{
		health.Value -= damage;
		if ( damageType == DamageType.Falling && damage > legBreakThreshold )
			legsBroken.Value = true;
	}

}
