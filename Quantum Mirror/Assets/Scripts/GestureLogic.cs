using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gestures
{
	public class GestureLogic : MonoBehaviour
	{
		public static List<int> SequenceToCode( GestureSequence gestureSequence )
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

		public static string SequenceToString( GestureSequence gestureSequence )
		{
			return CodeToString( SequenceToCode( gestureSequence ) );
		}

		public static List<int> WordListToCode( List<Gesture> gestureSequence )
		{
			List<int> gestureCode = new List<int>();

			gestureCode.Add( gestureSequence.Count );
			for ( int i = 0; i < gestureSequence.Count; i++ )
				gestureCode.Add( gestureSequence[ i ].circle );

			for ( int i = 0; i < gestureSequence.Count; i++ )
			{
				for ( int j = 0; j < gestureSequence[ i ].fingers.Length; j++ )
					gestureCode.Add( gestureSequence[ i ].fingers[ j ] ? 1 : 0 );
			}

			return gestureCode;
		}

		public static string CodeToString( List<int> code ) 
		{
			string stringCode = "";
			for ( int i = 0; i < code.Count; i++ )
				stringCode += code[ i ];
			return stringCode;
		}
	}
}

