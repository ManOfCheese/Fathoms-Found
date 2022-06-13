using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class BTMakeGesture : BTActionNode
{

    public GestureSequence sentence;
    public float gestureSpeed = 1f;
    public float holdGestureFor = 1f;
    public bool clearCircle;
    public bool startAtCentre;
	public bool holdStart;
    public bool endAtCentre;
    public bool returnToIdle;
    public bool inputGesture;
    public bool fingerPosAtCentre;

	private bool gestureHandsFound = false;

    protected override void OnStart() {

		if ( context.ikManager.FindGestureHands( context.manager.gestureCircles ) )
		{
			gestureHandsFound = true;

			for ( int i = 0; i < context.ikManager.allHands.Count; i++ )
			{
				if ( context.ikManager.allHands[ i ].stateMachine.CurrentState == context.ikManager.statesByName[ "GesturingState" ] )
				{
					HandController hand = context.ikManager.allHands[ i ];

					hand.sentence = sentence;
					hand.gestureSpeed = gestureSpeed;
					hand.holdGestureFor = holdGestureFor;
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

					if ( sentence != null )
					{
						for ( int j = 0; j < sentence.words.Count; j++ )
							hand.gesturesToMake.Add( sentence.words[ j ] );
					}
					else if ( hand.gesturesToMake.Count == 0 && !endAtCentre )
						endAtCentre = true;

					if ( endAtCentre )
						hand.gesturesToMake.Add( new Gesture( 0, new bool[ 3 ] { fingerPosAtCentre, fingerPosAtCentre, fingerPosAtCentre } ) );
				}
			}
		}
	}

    protected override void OnStop() {
		gestureHandsFound = false;
	}

    protected override State OnUpdate() {
		if ( gestureHandsFound )
			return State.Success;
		else
			return State.Failure;
    }
}
