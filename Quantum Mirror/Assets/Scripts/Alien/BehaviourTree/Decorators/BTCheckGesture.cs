using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public enum CheckMode
{
    ContainsAnyOfTheseWords,
    ContainsTheseWords,
    PerfectMatch
}

public class BTCheckGesture : BTDecoratorNode
{

    public CheckMode checkMode;
    public List<Gesture> wordsToMatch;
    private List<Gesture> playerSentence;
    public string gCode;

    protected override void OnStart() {
        if ( context.manager.gestureCircle != null )
            playerSentence = context.manager.gestureCircle.words;
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate()
	{
		switch ( checkMode )
		{
            case CheckMode.ContainsAnyOfTheseWords:
                bool matchFound = false;
				for ( int i = 0; i < playerSentence.Count; i++ )
				{
					for ( int j = 0; j < wordsToMatch.Count; j++ )
					{
                        if ( playerSentence[ i ] == wordsToMatch[ j ] )
                            matchFound = true;
					}
				}
                if ( matchFound )
                    return child.Update();
                else
                    return State.Failure;
            case CheckMode.ContainsTheseWords:
                int matchesFound = 0;
                for ( int i = 0; i < playerSentence.Count; i++ )
                {
                    for ( int j = 0; j < wordsToMatch.Count; j++ )
                    {
                        if ( playerSentence[ i ] == wordsToMatch[ j ] )
                            matchesFound++;
                    }
                }
                if ( matchesFound == wordsToMatch.Count )
                    return child.Update();
                else
                    return State.Failure;
            case CheckMode.PerfectMatch:
                if ( gCode == Sam.Gesturing.GestureListToGCode( playerSentence ) ) 
                    return child.Update();
                else
                    return State.Failure;
            default:
                break;
		}
        return State.Failure;
    }
}
