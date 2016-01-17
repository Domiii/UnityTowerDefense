using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class WavePath : MonoBehaviour {
	/// <summary>
	/// Enumerates over all points
	/// </summary>
	public IEnumerator<Transform> GetPathEnumeratorForward() {
		for (var i = 0; i < transform.childCount; ++i) {
			yield return transform.GetChild(i);
		}
	}

	public Transform FirstPoint {
		get {
			if (transform.childCount == 0) {
				return null;
			}

			return transform.GetChild(0);
		}
	}

	public void OnDrawGizmos() {
		if (transform.childCount < 2) {
			return;
		}

		for (var i = 1; i < transform.childCount; ++i) {
			Gizmos.DrawLine (transform.GetChild(i-1).position, transform.GetChild(i).position);
		}
	}
}
