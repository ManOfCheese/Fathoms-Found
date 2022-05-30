using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TheKiwiCoder
{
	[CreateAssetMenu( fileName = "NewAlienBlackboard", menuName = "Blackboards/AlienBlackboard" )]
	public class AlienBlackboard : Blackboard
	{

		public bool waitingForGesture = false;
		public bool tremorDetected;
		public bool gestureSignalDetected;
		public Vector3 moveToPosition = Vector3.zero;

		private void OnEnable()
		{
			AddData( "waitingForGesture", bools, waitingForGesture );
			AddData( "tremorDetected", bools, tremorDetected );
			AddData( "gestureSignalDetected", bools, tremorDetected );
			AddData( "moveToPosition", vector3s, moveToPosition );
		}

		private void OnDisable()
		{
			RemoveData( "waitingForGesture", bools, waitingForGesture );
			RemoveData( "tremorDetected", bools, tremorDetected );
			RemoveData( "gestureSignalDetected", bools, tremorDetected );
			RemoveData( "moveToPosition", vector3s, moveToPosition );
		}
	}
}
