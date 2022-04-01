using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectChecker : MonoBehaviour
{

	public Animator animator;
	public string triggerName;
	public Text resultDisplay;
    public Property[] propertyRequirements;
	public LogicExpression[] logicExpressions;
	public bool countNotFoundAsCorrect;

	private bool[] requirementsMet;
	private bool propertyFound;

	private void Awake()
	{
		DisplayRequirements();
		requirementsMet = new bool[ propertyRequirements.Length ];
	}

	public void OnTriggerEnter( Collider other )
	{
		if ( other.GetComponent<Object>() )
		{
			for ( int i = 0; i < requirementsMet.Length; i++ )
				requirementsMet[ i ] = false;

			Object obj = other.GetComponent<Object>();

			for ( int i = 0; i < propertyRequirements.Length; i++ )
			{
				propertyFound = false;
				for ( int j = 0; j < obj.properties.Length; j++ )
				{
					if ( obj.properties[ j ].property.propertyName == propertyRequirements[ i ].propertyName )
					{
						propertyFound = true;
						switch ( logicExpressions[ i ].thresholdLogic )
						{
							case ThresholdLogic.LessThan:
								if ( obj.properties[ j ].value <= logicExpressions[ i ].thresholdValue )
									requirementsMet[ i ] = true;
								break;
							case ThresholdLogic.MoreThan:
								if ( obj.properties[ j ].value >= logicExpressions[ i ].thresholdValue )
									requirementsMet[ i ] = true;
								break;
							default:
								break;
						}
					}
				}
				if ( !propertyFound )
				{
					if ( countNotFoundAsCorrect )
						requirementsMet[ i ] = true;
					else
						requirementsMet[ i ] = false;
				}
			}

			string resultsText = "";
			for ( int i = 0; i < requirementsMet.Length; i++ )
			{
				if ( requirementsMet[ i ] == true )
				{
					resultsText += propertyRequirements[ i ].propertyName + " levels correct.\n";
				}
				else
				{
					switch ( logicExpressions[ i ].thresholdLogic )
					{
						case ThresholdLogic.LessThan:
							resultsText += propertyRequirements[ i ].propertyName + " levels too high.\n";
							break;
						case ThresholdLogic.MoreThan:
							resultsText += propertyRequirements[ i ].propertyName + " levels too low.\n";
							break;
						default:
							break;
					}
				}
			}
			resultDisplay.text = resultsText;

			for ( int i = 0; i < requirementsMet.Length; i++ )
			{
				if ( requirementsMet[ i ] == false )
					break;
				else if ( i == requirementsMet.Length - 1 )
					animator.SetTrigger( triggerName );
			}
		}
	}

	private void OnTriggerExit( Collider other )
	{
		if ( other.GetComponent<Object>() )
			DisplayRequirements();
	}

	public void DisplayRequirements()
	{
		string requirements = "";

		for ( int i = 0; i < propertyRequirements.Length; i++ )
		{
			switch ( logicExpressions[ i ].thresholdLogic )
			{
				case ThresholdLogic.LessThan:
					requirements += propertyRequirements[ i ].propertyName + " below " + logicExpressions[ i ].thresholdValue + "\n";
					break;
				case ThresholdLogic.MoreThan:
					requirements += propertyRequirements[ i ].propertyName + " above " + logicExpressions[ i ].thresholdValue + "\n";
					break;
				default:
					break;
			}
		}
		resultDisplay.text = requirements;
	}
}

[System.Serializable]
public class LogicExpression
{
	public ThresholdLogic thresholdLogic;
	public float thresholdValue;
}
