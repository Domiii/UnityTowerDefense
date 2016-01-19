using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;


public class Enemy : MonoBehaviour {
	public static readonly string AliveTag = "Alive";
	public static readonly string DeadTag = "Dead";
	
	public static Enemy FindNextAlive() {
		var obj =  GameObject.FindGameObjectWithTag(AliveTag);
		if (obj != null) {
			return obj.GetComponent<Enemy> ();
		}
		return null;
	}
	
	
	public float MaxHealth = 100;
	public float Health = 100;
	public float MaxDistanceToGoal = .1f;
	public float Speed = 5;

	public WavePath Path;

	Wave _wave;

	IEnumerator<Transform> _pathIterator;
	
	
	public bool IsAlive {
		get { return Health > 0; }
	}
	
	public void InitEnemy(Wave wave) {
		Debug.Assert (wave != null);
		
		_wave = wave;
		Path = _wave.WaveGenerator.Path;
		
		RestartPath ();
	}
	
	public void Damage(float damage) {
		if (!IsAlive) {
			// don't do anything when dead
			return;
		}
		
		Health -= damage;
		
		if (!IsAlive) {
			// died from damage
			OnDeath();
		}
	}

	#region Events
	void OnDeath() {
		tag = DeadTag;
		Health = 0;
		Destroy (gameObject);
	}
	
	void OnReachedGoal() {
		// TODO: Enemy arrived at end of path
		
	}
	#endregion

	void RestartPath() {
		if (Path != null) {
			_pathIterator = Path.GetPathEnumeratorForward ();
			_pathIterator.MoveNext ();
		}
	}

	// Use this for initialization
	void Start () {
		tag = AliveTag;

		RestartPath ();
	}

	// Update is called once per frame
	void Update () {
		if (IsAlive) {
			MoveAlongPath ();
		}
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

}
