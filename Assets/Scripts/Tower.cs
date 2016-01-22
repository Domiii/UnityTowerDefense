using UnityEngine;
using System.Collections;

public class Tower : MonoBehaviour {
	public GameObject ProjectilePrefab;
	public float ShootDelaySeconds = 1;

	public Enemy CurrentTarget;
	float _lastShotTime;

	public bool HasValidTarget {
		get {
			return CurrentTarget != null && CurrentTarget.IsAlive;
		}
	}

	// Use this for initialization
	void Start () {
		if (ProjectilePrefab == null) {
			Debug.LogError("Tower is missing Projectile Prefab", this);
		}
		_lastShotTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		RotateTowardTarget ();
		ShootAtTarget ();
	}

	void UpdateCurrentTarget() {
		if (!HasValidTarget) {
			// find new target
			CurrentTarget = Enemy.FindNextAlive();
			
			if (HasValidTarget) {
				RotateTowardTarget();
			}
			else {
				ResetRotation();
			}
		}
	}

	void ResetRotation() {
		transform.rotation = Quaternion.AngleAxis (0, Vector3.forward);
	}

	void RotateTowardTarget() {
		if (!HasValidTarget) {
			return;
		}

		//transform.LookAt (CurrentTarget.transform.position);
		Vector3 dir = CurrentTarget.transform.position - transform.position;
		var angle = Mathf.Atan2 (dir.y, dir.x) * Mathf.Rad2Deg - 90;
		var rot = Quaternion.AngleAxis(angle, Vector3.forward);
		transform.rotation = rot;
	}

	void ShootAtTarget() {
		if (ProjectilePrefab == null) {
			return;
		}

		var now = Time.time;
		var delay = now - _lastShotTime;
		if (delay < ShootDelaySeconds) {
			// still on cooldown
			return;
		}
		
		// update CurrentTarget
		UpdateCurrentTarget ();
		if (!HasValidTarget) {
			return;
		}

		// shoot a new projectile
		var go = (GameObject)Instantiate(ProjectilePrefab, transform.position, transform.rotation);
		var projectile = go.GetComponent<Projectile> ();
		var rigidBody = go.GetComponent<Rigidbody2D> ();
		var direction = CurrentTarget.transform.position - go.transform.position;
		direction.Normalize ();

		rigidBody.velocity = direction * projectile.Speed;

		_lastShotTime = now;
	}
}
