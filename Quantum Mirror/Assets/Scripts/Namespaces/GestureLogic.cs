using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gestures
{
	public class GestureLogic : MonoBehaviour
	{
		public static List<int> GestureSequenceToCodeList( GestureSequence gestureSequence )
		{
			List<int> gestureCode = new List<int>();

			gestureCode.Add( gestureSequence.words.Count );
			for ( int i = 0; i < gestureSequence.words.Count; i++ )
				gestureCode.Add( gestureSequence.words[ i ].circle );

			for ( int i = 0; i < gestureSequence.words.Count; i++ )
			{
				for ( int j = 0; j < gestureSequence.words[ i ].fingers.Length; j++ )
					gestureCode.Add( gestureSequence.words[ i ].fingers[ j ] ? 1 : 0 );
			}

			return gestureCode;
		}

		public static string GestureSequenceToGCode( GestureSequence gestureSequence )
		{
			return CodeListToGCode( GestureSequenceToCodeList( gestureSequence ) );
		}

		public static List<int> GestureListToCodeList( List<Gesture> gestureList )
		{
			List<int> gestureCode = new List<int>();

			gestureCode.Add( gestureList.Count );
			for ( int i = 0; i < gestureList.Count; i++ )
				gestureCode.Add( gestureList[ i ].circle );

			for ( int i = 0; i < gestureList.Count; i++ )
			{
				for ( int j = 0; j < gestureList[ i ].fingers.Length; j++ )
					gestureCode.Add( gestureList[ i ].fingers[ j ] ? 1 : 0 );
			}

			return gestureCode;
		}

		public static string GestureListToGCode( List<Gesture> gestureList )
		{
			string gCode = "";

			gCode += gestureList.Count;
			for ( int i = 0; i < gestureList.Count; i++ )
				gCode += gestureList[ i ].circle;

			for ( int i = 0; i < gestureList.Count; i++ )
			{
				for ( int j = 0; j < gestureList[ i ].fingers.Length; j++ )
					gCode += gestureList[ i ].fingers[ j ] ? 1 : 0;
			}

			return gCode;
		}

		public static string CodeListToGCode( List<int> code ) 
		{
			string stringCode = "";
			for ( int i = 0; i < code.Count; i++ )
				stringCode += code[ i ];
			return stringCode;
		}
	}
}

