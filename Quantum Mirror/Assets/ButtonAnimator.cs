using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonAnimator : MonoBehaviour
{

	public Animator animator;
	private bool isOn = false;

	private void Awake()
	{
		animator = GetComponent<Animator>();
	}

	public void AnimationCompleted()
	{
		isOn = !isOn;
		animator.SetBool( "IsOn", isOn );
	}

}
