using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public enum GestureMode
{
	Preset,
	Blackboard,
	Library
}

public class BTMakeGesture : BTActionNode
{

	public GestureMode gestureMode;
    public GestureSequence sentence;

	public bool overrideStandardSettings;
    public float gestureSpeed = 1f;
    public float holdGestureFor = 1f;
    public float maxGestureDistance = 1f;

    public bool clearCircle;
    public bool startAtCentre;
	public bool holdStart;
    public bool endAtCentre;
    public bool returnToIdle;
    public bool inputGesture;
    public bool fingerPosAtCentre;

	private bool gestureHandsFound = false;

    protected override void OnStart() {
		if ( context.manager.gestureCircle != null )
		{
			if ( context.ikManager.FindGestureHand( context.manager.gestureCircle, context.ikManager.maxGestureDistance ) )
			{
				HandController hand = context.ikManager.gestureHand;

				hand.sentence = sentence;
				if ( overrideStandardSettings )
				{
					hand.maxGestureDistance = maxGestureDistance;
					hand.gestureSpeed = gestureSpeed;
					hand.holdGestureFor = holdGestureFor;
				}
				else
				{
					hand.maxGestureDistance = context.ikManager.maxGestureDistance;
					hand.gestureSpeed = context.ikManager.gestureSpeed;
					hand.holdGestureFor = context.ikManager.holdGestureFor;
				}
				hand.clearCircle = clearCircle;
				hand.startAtCentre = startAtCentre;
				hand.holdStart = holdStart;
				hand.endAtCentre = endAtCentre;
				hand.returnToIdle = returnToIdle;
				hand.inputGesture = inputGesture;
				hand.fingerPosAtCentre = fingerPosAtCentre;

				if ( sentence == null )
					context.ikManager.FindGesture( hand );

				//If circles need to be cleared.
				if ( clearCircle )
				{
					for ( int j = 0; j < hand.gestureCircle.subCircles.Length; j++ )
					{
						bool foundGestureToClear = false;

						for ( int k = 0; k < hand.gestureCircle.subCircles[ j ].fingerSprites.Length; k++ )
						{
							if ( hand.gestureCircle.subCircles[ j ].fingerSprites[ k ].enabled == true && !foundGestureToClear )
							{
								hand.gesturesToMake.Add( new Gesture( j, new bool[ 3 ] { false, false, false } ) );
								foundGestureToClear = true;
							}
						}

					}
				}

				if ( startAtCentre )
					hand.gesturesToMake.Add( new Gesture( 0, new bool[ 3 ] { fingerPosAtCentre, fingerPosAtCentre, fingerPosAtCentre } ) );

				switch ( gestureMode )
				{
					case GestureMode.Preset:
						for ( int j = 0; j < sentence.words.Count; j++ )
							hand.gesturesToMake.Add( sentence.words[ j ] );
						break;
					case GestureMode.Blackboard:
						GestureSequence bbSentence = context.ikManager.gestureLibrary.Items[ blackboard.GetData( "gestureToInput", new int() ) ];
						for ( int j = 0; j < bbSentence.words.Count; j++ )
							hand.gesturesToMake.Add( bbSentence.words[ j ] );
						break;
					case GestureMode.Library:
						break;
					default:
						break;
				}
				
				if ( hand.gesturesToMake.Count == 0 && !endAtCentre )
					endAtCentre = true;

				if ( endAtCentre )
					hand.gesturesToMake.Add( new Gesture( 0, new bool[ 3 ] { fingerPosAtCentre, fingerPosAtCentre, fingerPosAtCentre } ) );
			}
		}
	}

    protected override void OnStop() {
	}

    protected override State OnUpdate() {
		if ( context.ikManager.gestureHand == null ) { return State.Failure; }

		int gesturingHands = 0;
		for ( int i = 0; i < context.ikManager.allHands.Count; i++ )
		{
			if ( context.ikManager.allHands[ i ].stateMachine.CurrentState.stateName == "GesturingState" )
				gesturingHands++;
		}

		if ( gesturingHands > 0 )
			return State.Running;
		else
			return State.Success;
    }
}
