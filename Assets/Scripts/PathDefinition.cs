using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PathDefinition : MonoBehaviour {
	/// <summary>
	/// Enumerates over all points, going forth and back, indefinitely.
	/// </summary>
	public IEnumerator<Transform> GetPathEnumerator() {
		if (transform.childCount == 0) {
			// empty enumerator
			yield break;
		}

		var direction = 1;
		var index = 0;

		while (true) {
			yield return transform.GetChild(index);

			if (transform.childCount == 1) {
				continue;
			}

			if (index <= 0) {
				direction = 1;
			}
			else if (index >= transform.childCount-1) {
				direction = -1;
			}

			index = index + direction;
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
