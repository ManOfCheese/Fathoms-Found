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
		gCode = Gestures.GestureLogic.GestureListToGCode( words );
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
