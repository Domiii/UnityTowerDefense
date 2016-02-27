using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleSystemHelper : MonoBehaviour {
	public int SortingOrder = 0;
	private int sortingLayer = 0;

	public int SortingLayerId {
		get {
			var renderer = GetComponent<Renderer> ();
			if (renderer == null) {
				return sortingLayer;
			}
			return renderer.sortingLayerID;
		}
	}

	/// <summary>
	/// Recursively applies the options to the object and all its children.
	/// </summary>
	public void SetSortingOptions(int sortingLayerId, int sortingOrder) {
		sortingLayer = sortingLayerId;
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
