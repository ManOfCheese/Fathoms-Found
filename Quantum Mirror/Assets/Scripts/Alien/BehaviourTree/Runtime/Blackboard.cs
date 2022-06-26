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
            PositionList,
            GameObject,
            Bool,
            String,
            Int,
            Float,
            Vector3,
            Vector2
        }

        public bool foldoutStatus = true;

        [SerializeField] public Dictionary<string, object> values = new Dictionary<string, object>();

        public bool AddData<L>( string key, L value )
		{
            if ( value != null )
			{
                if ( !values.ContainsKey( key ) )
                    values.Add( key, value );
                else
                    values[ key ] = value;
                return true;
			}
            return false;
		}

        public bool RemoveData( string key )
        {
            if ( values.ContainsKey( key ) )
			{
                values.Remove( key );
                return true;
            }
            return false;
        }

        public K GetData<K>( string key, K dataType )
		{
            if ( dataType != null )
			{
                if ( values.ContainsKey( key ) )
                    return ( K )values[ key ];
			}
            return default( K );
		}

        public bool IncrementInt( string intName )
		{
            if ( !values.ContainsKey( intName ) ) return false;
            int intToIncrement = (int)values[ intName ];
            intToIncrement++;
            AddData( intName, intToIncrement );
            return true;
		}

        public bool DecrementInt( string intName )
		{
            if ( !values.ContainsKey( intName ) ) return false;
            int intToIncrement = ( int )values[ intName ];
            intToIncrement--;
            AddData( intName, intToIncrement );
            return true;
        }
    }
}