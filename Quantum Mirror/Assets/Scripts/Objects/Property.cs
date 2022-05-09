using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( fileName = "Property", menuName = "Other/Property" )]
public class Property : PersistentSetElement
{

    public string propertyName;
    public bool isInherent;

}
