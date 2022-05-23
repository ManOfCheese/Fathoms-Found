using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AnimParamType
{
	Bool,
	Trigger,
	Float,
	Int
}

public class Door : InteractableObject
{

	public RunTimeSet<Door> doors;
	public Animator animator;
	public string animParameterName;
	public AnimParamType animParamType;
	public bool boolParamValue;
	public int intParamValue;
	public float floatParamValue;

	private void Awake()
	{
		doors.Add( this );
	}

	public void Open()
	{
		switch ( animParamType )
		{
			case AnimParamType.Bool:
				animator.SetBool( animParameterName, true );
				break;
			case AnimParamType.Trigger:
				animator.SetTrigger( animParameterName );
				break;
			case AnimParamType.Float:
				animator.SetFloat( animParameterName, floatParamValue );
				break;
			case AnimParamType.Int:
				animator.SetInteger( animParameterName, intParamValue );
				break;
			default:
				break;
		}
	}

	private void OnApplicationQuit()
	{
		doors.Items.Clear();
	}

}
