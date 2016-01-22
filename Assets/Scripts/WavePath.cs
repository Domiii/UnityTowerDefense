using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class WavePath : MonoBehaviour {
	public enum FollowDirection {
		Forward,
		Backward
	}

	public IEnumerator<Transform> GetPathEnumerator(FollowDirection direction) {
		return direction == FollowDirection.Forward ? GetPathEnumeratorForward () : GetPathEnumeratorBackward ();
	}
	
	/// <summary>
	/// Enumerates over all points
	/// </summary>
	public IEnumerator<Transform> GetPathEnumeratorForward() {
		for (var i = 0; i < transform.childCount; ++i) {
			yield return transform.GetChild(i);
		}
	}
	
	/// <summary>
	/// Enumerates over all points
	/// </summary>
	public IEnumerator<Transform> GetPathEnumeratorBackward() {
		for (var i = transform.childCount-1; i >= 0; --i) {
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
