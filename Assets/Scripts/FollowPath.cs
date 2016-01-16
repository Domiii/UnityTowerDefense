using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FollowPath : MonoBehaviour {
	// Use this for initialization

	public WavePath Path;
	public float Speed = 10;
	public float MaxDistanceToGoal = .1f;

	private IEnumerator<Transform> _pointIterator;

	void Start () {
		if (Path == null) {
			Debug.LogError("Path must not be null", gameObject);
		}

		_pointIterator = Path.GetPathEnumeratorForwardAndBack();
		_pointIterator.MoveNext ();

		if (_pointIterator.Current == null) {
			// empty path
			return;
		}

		// move to start position
		transform.position = _pointIterator.Current.position;
	}
	
	// Update is called once per frame
	void Update () {
		if (_pointIterator == null || _pointIterator.Current == null)
			return;

		// move towards target
		var targetPosition = _pointIterator.Current.position;
		transform.position = Vector3.MoveTowards (transform.position, targetPosition, Time.deltaTime * Speed);

		// check if we reached target
		var distanceSquared = (transform.position - targetPosition).sqrMagnitude;
		if (distanceSquared < MaxDistanceToGoal * MaxDistanceToGoal) {
			// we arrived at target -> Move to next
			_pointIterator.MoveNext();
		}
	}
}
