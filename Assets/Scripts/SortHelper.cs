using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ParticleSystem))]
public class SortHelper : MonoBehaviour {
	public int sortingOrder = 0;
	private int _sortingLayer = 0;

	public int SortingLayerId {
		get {
			var renderer = GetComponent<Renderer> ();
			if (renderer == null) {
				return _sortingLayer;
			}
			return renderer.sortingLayerID;
		}
	}

	/// <summary>
	/// Recursively applies the options to the object and all its children.
	/// </summary>
	public void SetSortingOptions(int sortingLayerId, int sortingOrder) {
		_sortingLayer = sortingLayerId;
		SetSortingOptionsRecurse (transform, sortingLayerId, sortingOrder);
	}

	void SetSortingOptionsRecurse(Transform obj, int sortingLayerId, int sortingOrder) {
		// update sorting options
		var renderer = obj.GetComponent<Renderer> ();
		if (renderer != null) {
			renderer.sortingLayerID = sortingLayerId;
			renderer.sortingOrder = sortingOrder;
		}

		for (var i = 0; i < obj.childCount; ++i) {
			SetSortingOptionsRecurse(obj.GetChild(i), sortingLayerId, sortingOrder);
		}
	}
}
