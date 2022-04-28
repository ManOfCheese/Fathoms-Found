using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( fileName = "Action", menuName = "Action/ActionSequence" )]
public class ActionSequence : PersistentSetElement {

    public List<Action> actions;

}
