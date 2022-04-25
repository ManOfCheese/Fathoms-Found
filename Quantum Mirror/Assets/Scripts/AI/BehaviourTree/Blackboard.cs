using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackboard
{
	
	public Dictionary<string, GameObject> gameObjects = new Dictionary<string,GameObject>();
	public Dictionary<string, bool> bools = new Dictionary<string, bool>();
	public Dictionary<string, int> ints = new Dictionary<string,int>();
	public Dictionary<string, float> floats = new Dictionary<string, float>();
	public Dictionary<string, Vector2> vector2s = new Dictionary<string, Vector2>();
	public Dictionary<string, Vector3> vector3s = new Dictionary<string, Vector3>();

}
