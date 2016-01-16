using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;


public class Enemy : MonoBehaviour {
	EnemyTemplate _enemyType;
	Wave _wave;
	bool _isReady = false;

	IEnumerator<Transform> _pathIterator;
	

	public float MaxDistanceToGoal = .1f;
	public float Speed = 5;


	// Use this for initialization
	void Start () {
	}


	public void InitEnemy(Wave wave, EnemyTemplate enemyType) {
		Debug.Assert (wave != null);
		Debug.Assert (enemyType != null);

		_wave = wave;
		_enemyType = enemyType;

		_pathIterator = _wave.Path.GetPathEnumeratorForward();
	}


	// Update is called once per frame
	void Update () {
		MoveAlongPath ();
	}


	void MoveAlongPath() {
		if (_pathIterator == null || _pathIterator.Current == null)
			return;
		
		// move towards target
		var targetPosition = _pathIterator.Current.position;
		transform.position = Vector3.MoveTowards (transform.position, targetPosition, Time.deltaTime * Speed);
		
		// check if we reached target
		var distanceSquared = (transform.position - targetPosition).sqrMagnitude;
		if (distanceSquared < MaxDistanceToGoal * MaxDistanceToGoal) {
			// we arrived at target -> Move to next
			_pathIterator.MoveNext();
		}
	}
}
