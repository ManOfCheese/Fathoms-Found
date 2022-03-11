using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( fileName = "Sentence", menuName = "Gestures/Sentence" )]
public class GestureSequence : PersistentSetElement
{

    public string sentenceName;
    public string meaning;
    public List<Gesture> words;

}

[System.Serializable]
public class Gesture {

	public Gesture( int _circle, bool[] _fingers ) {
		circle = _circle;
		fingers = _fingers;
	}

	public int circle;
	public bool[] fingers;
}
