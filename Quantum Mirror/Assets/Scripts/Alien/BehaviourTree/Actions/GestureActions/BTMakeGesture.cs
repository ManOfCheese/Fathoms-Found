using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class BTMakeGesture : BTActionNode
{

    public GestureSequence sentence;
    public float gestureSpeed = 1f;
    public float holdGestureFor = 1f;
    public float legRepositionSpeed = 1f;
    public bool clearCircle;
    public bool startAtCentre;
    public bool endAtCentre;
    public bool returnToIdle;
    public bool inputGesture;
    public bool fingerPosAtCentre;

    protected override void OnStart() {
        if ( sentence == null )
            context.gc.FindGesture();
        context.moveController.agent.destination = context.moveController.agent.transform.position;

        context.gc.gestureSpeed = gestureSpeed;
        context.gc.holdGestureFor = holdGestureFor;
        context.gc.preGestureHandPos = context.gc.hands[ context.gc.gestureHandIndex ].ikHandler.transform.position;
        context.gc.wordIndex = 0;
		context.gc.hand = context.gc.hands[ context.gc.gestureHandIndex ];
		context.gc.gestureCircle = context.manager.gestureCircle;
		context.gc.fingerAnimators = context.gc.hands[ context.gc.gestureHandIndex ].ikHandler.fingerAnimators;

		if ( context.gc.gestureState != GestureState.Repositioning )
            context.gc.gestureState = GestureState.StartGesture;

		//If circles need to be cleared.
		if ( clearCircle )
		{
			for ( int i = 0; i < context.gc.gestureCircle.subCircles.Length; i++ )
			{
				bool foundGestureToClear = false;
				
				for ( int j = 0; j < context.gc.gestureCircle.subCircles[ i ].fingerSprites.Length; j++ )
				{
					if ( context.gc.gestureCircle.subCircles[ i ].fingerSprites[ j ].enabled == true && !foundGestureToClear )
					{
						context.gc.gesturesToMake.Add( new Gesture( i, new bool[ 3 ] { false, false, false } ) );
						foundGestureToClear = true;
					}
				}

			}
		}

		if ( startAtCentre )
			context.gc.gesturesToMake.Add( new Gesture( 0, new bool[ 3 ] { fingerPosAtCentre, fingerPosAtCentre, fingerPosAtCentre } ) );

		if ( context.gc.standardGesture )
		{
			for ( int i = 0; i < context.gc.standardResponse.words.Count; i++ )
				context.gc.gesturesToMake.Add( context.gc.standardResponse.words[ i ] );
		}
		else if ( sentence != null )
		{
			for ( int i = 0; i < sentence.words.Count; i++ )
				context.gc.gesturesToMake.Add( sentence.words[ i ] );
		}
		else if ( context.gc.sentenceIndex < context.gc.responses.Items.Count - 1 )
		{
			for ( int i = 0; i < context.gc.responses.Items[ context.gc.sentenceIndex ].words.Count; i++ )
				context.gc.gesturesToMake.Add( context.gc.responses.Items[ context.gc.sentenceIndex ].words[ i ] );
		}
		else if ( context.gc.gesturesToMake.Count == 0 && !endAtCentre )
			endAtCentre = true;

		if ( endAtCentre )
			context.gc.gesturesToMake.Add( new Gesture( 0, new bool[ 3 ] { fingerPosAtCentre, fingerPosAtCentre, fingerPosAtCentre } ) );
	}

    protected override void OnStop() {
		context.gc.gesturesToMake.Clear();
		context.gc.hand = null;
		context.gc.gestureCircle = null;
		context.gc.fingerAnimators = null;
	}

    protected override State OnUpdate() {
        if ( context.manager.gestureCircle != null )
            return context.gc.Gesture( sentence, clearCircle, startAtCentre, endAtCentre, returnToIdle, inputGesture, fingerPosAtCentre, legRepositionSpeed );
        else
            return State.Failure;
    }
}
