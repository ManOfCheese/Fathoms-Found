using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TheKiwiCoder {

    // This is the blackboard container shared between all nodes.
    // Use this to store temporary data that multiple nodes need read and write access to.
    // Add other properties here that make sense for your specific use case.
    [System.Serializable]
    public class Blackboard : ScriptableObject {

        public enum BlackboardValueType
        {
            GameObject,
            Bool,
            String,
            Int,
            Float,
            Vector3,
            Vector2
        }

        public bool foldoutStatus = true;

        [SerializeField] public Dictionary<string, GameObject> gameObjects = new Dictionary<string, GameObject>();
        [SerializeField] public Dictionary<string, bool> bools = new Dictionary<string, bool>();
        [SerializeField] public Dictionary<string, string> strings = new Dictionary<string, string>();
        [SerializeField] public Dictionary<string, int> ints = new Dictionary<string, int>();
        [SerializeField] public Dictionary<string, float> floats = new Dictionary<string, float>();
        [SerializeField] public Dictionary<string, Vector3> vector3s = new Dictionary<string, Vector3>();
        [SerializeField] public Dictionary<string, Vector2> vector2s = new Dictionary<string, Vector2>();

        public bool AddData<T, L>(string key, T dict, L value) where T : Dictionary<string, L>
		{
            if ( dict != null )
			{
                if ( value != null )
				{
                    if ( !dict.ContainsKey( key ) )
                        dict.Add( key, value );
                    else
                        dict[ key ] = value;
                    return true;
				}
			}
            return false;
		}

        public bool RemoveData<T, L>( string key, T dict, L value ) where T : Dictionary<string, L>
        {
            if ( dict != null )
            {
                if ( value != null )
                {
                    if ( dict.ContainsKey( key ) )
					{
                        dict.Remove( key );
                        return true;
                    }
                }
            }
            return false;
        }

        public K GetData<T, K>( string key, T dict, K dataType ) where T : Dictionary<string, K>
		{
            if ( dict != null )
			{
                if ( dataType != null )
				{
                    if ( dict.ContainsKey( key ) )
                        return ( K )dict[ key ];
				}
			}
            return default( K );
		}

        public bool IncrementInt( string intName )
		{
            if ( !ints.ContainsKey( intName ) ) return false;
            ints[ intName ]++;
            return true;
		}

        public bool DecrementInt( string intName )
		{
            if ( !ints.ContainsKey( intName ) ) return false;
            ints[ intName ]--;
            return true;
        }
    }
}