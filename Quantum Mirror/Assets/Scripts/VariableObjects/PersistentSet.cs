using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentSet<T> : ScriptableObject where T: PersistentSetElement {

	public List<T> Items = new List<T>();
	public bool useSorting = true;

	public void Add( T item ) {
		if ( !Items.Contains( item ) ) {
			Items.Add( item );
		}
		if ( useSorting ) {
			SortByName();
		}
		CheckItems();
		AssignIDs();
	}

	public void CheckItems() {
		for ( int i = Items.Count - 1; i >= 0; i-- ) {
			if ( Items[ i ] == null ) {
				Items.RemoveAt( i );
			}
		}
	}

	public void AssignIDs() {
		// give every entry the correct ID (no checks)
		for ( int i = 0; i < Items.Count; i++ ) {
			if ( Items[ i ] != null ) {
				Items[ i ].ID = i;
			}
		}
	}

	public void SortByName() {
		if ( useSorting ) {
			Items.Sort( ( x, y ) => x.name.CompareTo( y.name ) );
		}
	}

}
