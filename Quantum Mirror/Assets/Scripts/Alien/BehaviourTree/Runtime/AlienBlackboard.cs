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
		public bool tremorDetected = false;
		public bool gestureSignalDetected = false;
		public bool checkForTermors = true;
		public bool checkForSignals = true;
		public Vector3 moveToPosition = Vector3.zero;
		public List<Vector3> objectTargets = new List<Vector3>();

		private void OnEnable()
		{
			AddData( "waitingForGesture", waitingForGesture );
			AddData( "tremorDetected", tremorDetected );
			AddData( "gestureSignalDetected", gestureSignalDetected );
			AddData( "checkForTremors", checkForTermors );
			AddData( "checkForSignals", checkForSignals );
			AddData( "moveToPosition", moveToPosition );
			AddData( "objectTargets", objectTargets );
		}

		private void OnDisable()
		{
			RemoveData( "waitingForGesture" );
			RemoveData( "tremorDetected" );
			RemoveData( "gestureSignalDetected" );
			RemoveData( "checkForTremors" );
			RemoveData( "checkForSignals" );
			RemoveData( "moveToPosition" );
			RemoveData( "objectTargets" );
		}
	}
}
