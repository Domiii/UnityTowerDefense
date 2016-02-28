using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;


public class PathFollower : MonoBehaviour {
	public float Speed = 5;
	public WavePath Path;
	public WavePath.FollowDirection PathDirection;

	float maxDistanceToGoal;
	Wave wave;
	IEnumerator<Transform> pathIterator;
	
	public void InitFollower(Wave wave) {
		Debug.Assert (wave != null);
		
		this.wave = wave;
		Path = wave.WaveGenerator.Path;
		
		RestartPath ();
	}

	#region Events
	void OnAttackStart() {
		if (!enabled)
			return;

		// add a lot of drag (so unity will not be pushed so easily)
		//var rigidbody = GetComponent<Rigidbody2D> ();
		//_originalConstraints = rigidbody.constraints;
		//rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
		//rigidbody.drag = 40f;

		enabled = false;
	}

	void OnAttackStop() {
		//var rigidbody = GetComponent<Rigidbody2D> ();
		//rigidbody.constraints = _originalConstraints;
		//rigidbody.drag = 10f;

		enabled = true;
	}
	#endregion

	void GenerateDistortions() {
	}

	void RestartPath() {
		if (wave != null) {
			PathDirection = wave.WaveGenerator.PathDirection;
		}
		if (Path != null) {
			pathIterator = Path.GetPathEnumerator (PathDirection);
			pathIterator.MoveNext ();
		}
	}

	// Use this for initialization
	void Start () {
		// compute a radius estimate to determine how close object needs to be to target
		var spriteRenderer = GetComponent<SpriteRenderer>();
		if (spriteRenderer != null) {
			maxDistanceToGoal = Mathf.Sqrt (Vector2.Distance(spriteRenderer.bounds.min, spriteRenderer.bounds.max))/2;
		} else {
			// PathFollower on non-sprite object not properly supported currently
			maxDistanceToGoal = 1;
		}

		RestartPath ();
	}

	// Update is called once per frame
	void Update () {
		MoveAlongPath ();
	}


	public void MoveAlongPath() {
		if (pathIterator == null || pathIterator.Current == null)
			return;
		
		// move towards current target
		var targetPosition = pathIterator.Current.position;
		//var rigidbody = GetComponent<Rigidbody2D> ();
		var direction = targetPosition - transform.position;
		direction.Normalize ();

		//rigidbody.velocity = direction * Speed;
		transform.position += direction * Speed * Time.deltaTime;
//		Debug.DrawRay(transform.position, direction, Color.red);
//		Debug.DrawLine (transform.position, targetPosition, Color.green);
		
		// check if we reached target
		var distanceSquared = (transform.position - targetPosition).sqrMagnitude;
		if (distanceSquared < maxDistanceToGoal * maxDistanceToGoal) {
			// we arrived at current target -> Choose next target
			pathIterator.MoveNext();
//			if (_pathIterator.Current == null) {
//				// arrived at final waypoint
//				OnReachedGoal();
//			}
		}
	}

	void OnCollisionEnter2D(Collision2D col) {
		var pathFollower = col.gameObject.GetComponent<PathFollower> ();
		if (pathFollower != null && pathFollower.wave != wave) {
			// don't let these two collide
			Physics2D.IgnoreCollision(GetComponent<Collider2D>(), col.collider, true);
		}
	}

	void GetStatsData(StatsMenuData statsMenuData) {
		statsMenuData.Speed = Speed;
	}

}
