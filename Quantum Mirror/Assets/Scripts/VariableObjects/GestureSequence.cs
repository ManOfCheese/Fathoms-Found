using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( fileName = "Sentence", menuName = "Gestures/Sentence" )]
public class GestureSequence : PersistentSetElement
{

    public string sentenceName;
    public string meaning;
	public List<Gesture> words;

	[ReadOnly] public string gCode;

	[HideInInspector] public int[] gestureCode;
	[HideInInspector] public int fingerCount;

	private void OnValidate() {
		//Set finger count;
		if ( words.Count > 0 )
			fingerCount = words[ 0 ].fingers.Length;

		//Create gCode;
		int length = words.Count * 4 + 1;
		gestureCode = new int[ length ];
		gestureCode[ 0 ] = words.Count;
		int codeIndex = 1;

		for ( int i = 0; i < words.Count; i++ ) {
			gestureCode[ codeIndex ] = words[ i ].circle;
			codeIndex++;
		}

		for ( int i = 0; i < words.Count; i++ ) {
			for ( int j = 0; j < words[ i ].fingers.Length; j++ ) {
				gestureCode[ codeIndex ] = words[ i ].fingers[ j ] ? 1 : 0;
				codeIndex++;
			}
		}

		gCode = "";
		for ( int i = 0; i < gestureCode.Length; i++ ) {
			gCode += gestureCode[ i ];
		}
	}

}

[System.Serializable]
public class Gesture {

	public Gesture( int _circle, bool[] _fingers ) {
		circle = _circle;
		fingers = new bool[ _fingers.Length ];
		for ( int i = 0; i < _fingers.Length; i++ ) {
			fingers[ i ] = _fingers[ i ];
		}
	}

	public int circle;
	public bool[] fingers;
}
