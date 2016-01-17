using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;


public class Enemy : MonoBehaviour {
	Wave _wave;

	IEnumerator<Transform> _pathIterator;
	

	public float MaxDistanceToGoal = .1f;
	public float Speed = 5;


	// Use this for initialization
	public void InitEnemy(Wave wave) {
		Debug.Assert (wave != null);

		_wave = wave;

		_pathIterator = _wave.WaveGenerator.Path.GetPathEnumeratorForward();
		_pathIterator.MoveNext ();
	}


	// Update is called once per frame
	void Update () {
		MoveAlongPath ();
	}


	void MoveAlongPath() {
		if (_pathIterator == null || _pathIterator.Current == null)
			return;
		
		// move towards current target
		var targetPosition = _pathIterator.Current.position;
		transform.position = Vector3.MoveTowards (transform.position, targetPosition, Time.deltaTime * Speed);
		
		// check if we reached target
		var distanceSquared = (transform.position - targetPosition).sqrMagnitude;
		if (distanceSquared < MaxDistanceToGoal * MaxDistanceToGoal) {
			// we arrived at current target -> Choose next target
			_pathIterator.MoveNext();
			if (_pathIterator.Current == null) {
				OnReachedGoal();
			}
		}
	}

	void OnReachedGoal() {
		// TODO: Enemy arrived at end of path

	}

}
