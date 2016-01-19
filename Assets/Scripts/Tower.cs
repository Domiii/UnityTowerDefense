using UnityEngine;
using System.Collections;

public class Tower : MonoBehaviour {
	public GameObject ProjectilePrefab;
	public float ShootDelaySeconds = 1;

	public Enemy CurrentTarget;
	float _lastShotTime;

	// Use this for initialization
	void Start () {
		if (ProjectilePrefab == null) {
			Debug.LogError("Tower is missing Projectile Prefab", this);
		}
		_lastShotTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		// update CurrentTarget
		if (CurrentTarget == null || !CurrentTarget.IsAlive) {
			CurrentTarget = Enemy.FindNextAlive();
		}

		if (CurrentTarget != null) {
			RotateTowardTarget ();
			ShootAtTarget ();
		}
	}

	void RotateTowardTarget() {
		Vector3 targetDir = CurrentTarget.transform.position - transform.position;
		//float step = speed * Time.deltaTime;
		Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, 100, 0.0F);
		Debug.DrawRay(transform.position, newDir, Color.red);
		//transform.LookAt (CurrentTarget.transform.position);
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
