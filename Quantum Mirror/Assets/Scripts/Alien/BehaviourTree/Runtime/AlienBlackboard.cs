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
		public List<Vector3> objectTargets = new List<Vector3>();

		private void OnEnable()
		{
			AddData( "waitingForGesture", waitingForGesture );
			AddData( "tremorDetected", tremorDetected );
			AddData( "gestureSignalDetected", tremorDetected );
			AddData( "moveToPosition", moveToPosition );
			AddData( "objectTargets", objectTargets );
		}

		private void OnDisable()
		{
			RemoveData( "waitingForGesture" );
			RemoveData( "tremorDetected" );
			RemoveData( "gestureSignalDetected" );
			RemoveData( "moveToPosition" );
			RemoveData( "objectTargets" );
		}
	}
}
