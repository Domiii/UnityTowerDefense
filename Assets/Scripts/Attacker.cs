using UnityEngine;
using System.Collections;

public class Attacker : MonoBehaviour {
	public GameObject BulletPrefab;
	public float ShootDelaySeconds = 1;
	public float AttackRadius = 30.0f;

	Collider2D[] collidersInRange = new Collider2D[128];
	Unit currentTarget;
	bool isAttacking;
	float lastShotTime;

	SpriteRenderer _highlighter;


	#region Default Events
	// Use this for initialization
	void Start () {
		if (BulletPrefab == null) {
			Debug.LogError("Attacker is missing Bullet Prefab", this);
			return;
		}

		isAttacking = false;
		//lastShotTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		AimAndShoot ();

		if (currentTarget != null) {
			var dir = (currentTarget.transform.position - transform.position).normalized * AttackRadius;
			Debug.DrawRay (transform.position, dir);
		}

		if (_highlighter != null && _highlighter.gameObject.activeInHierarchy) {
			_highlighter.transform.position = transform.position;
		}
	}

	void OnDeath(DamageInfo damageInfo) {
		enabled = false;
		OnUnselect ();
	}
	#endregion
	
	
	#region Targeting
	public bool IsAttacking {
		get { return HasValidTarget; }
	}

	public bool HasValidTarget {
		get {
			return currentTarget != null && IsValidTarget(currentTarget);
		}
	}

	public bool IsValidTarget(Unit target) {
		return target.CanBeAttacked && FactionManager.AreHostile (gameObject, target.gameObject);
	}

	Unit FindTarget() {
		var nResults = Physics2D.OverlapCircleNonAlloc(transform.position, AttackRadius, collidersInRange);
		for (var i = 0; i < nResults; ++i) {
			var collider = collidersInRange[i];
			var unit = collider.GetComponent<Unit> ();
			if (unit != null && IsValidTarget(unit)) {
				return unit;
			}
		}

		// no valid target found
		return null;
	}
	
	bool UpdateCurrentTarget() {
		// find new target
		currentTarget = FindTarget();
		
		if (HasValidTarget) {
			return true;
		}
		else {
			ResetRotation();
		}
		return false;
	}
	#endregion


	#region Highlighting
	SpriteRenderer CreateHighlighterObject() {
		var go = (GameObject)Instantiate(GameUIManager.Instance.AttackerHighlighterPrefab, transform.position, Quaternion.identity);
		var highlighter = go.GetComponent<SpriteRenderer>();
		if (highlighter == null) {
			Debug.LogError("Attack has invalid Highlighter Prefab. Highlighter must have a SpriteRenderer.");
			Destroy (go);
			return null;
		}

		highlighter.sortingLayerName = "Highlight";
		
		// set world-space bounds
		var max = highlighter.transform.InverseTransformPoint(highlighter.bounds.max);
		var min = highlighter.transform.InverseTransformPoint(highlighter.bounds.min);

		var diameter = 2 * AttackRadius;
		var realDiameter = Mathf.Max (max.x - min.x, max.y - min.y);
		var newScale = diameter / realDiameter;
		//var yFactor = diameter / realDiameter;

		//var scale = highlighter.transform.localScale;
		//highlighter.transform.localScale = new Vector3(scale.x * xFactor, scale.y * yFactor, 1);
		highlighter.transform.localScale = new Vector3(newScale, newScale, 1);

		return highlighter;
	}

	SpriteRenderer HighlighterObject {
		get {
			if (_highlighter == null) {
				if (GameUIManager.Instance.AttackerHighlighterPrefab != null) {
					_highlighter = CreateHighlighterObject();
				}
			}
			return _highlighter;
		}
	}

	void OnSelect() {
		var highlighterObject = HighlighterObject;
		if (highlighterObject == null) {
			return;
		}
		
		highlighterObject.gameObject.SetActive (true);
	}
	
	void OnUnselect() {
		if (_highlighter == null) {
			return;
		}
		_highlighter.gameObject.SetActive (false);
	}
	#endregion

	Quaternion GetRotationToward(Transform targetTransform) {
		Vector3 dir = targetTransform.position - transform.position;
		var angle = Mathf.Atan2 (dir.y, dir.x) * Mathf.Rad2Deg - 90;
		return Quaternion.AngleAxis(angle, Vector3.forward);
	}
	
	void ResetRotation() {
		//transform.rotation = Quaternion.AngleAxis (0, Vector3.forward);
	}
	
	void RotateTowardTarget() {
		if (!HasValidTarget) {
			return;
		}
		
		var rigidbody = GetComponent<Rigidbody2D> ();
		if (rigidbody != null && (rigidbody.constraints & RigidbodyConstraints2D.FreezeRotation) != 0) {
			// don't rotate if rotation has been constrained
			return;
		}
		
		//transform.LookAt (CurrentTarget.transform.position);

		transform.rotation = GetRotationToward(currentTarget.transform);
	}

	#region Attack
	void AimAndShoot() {
		if (BulletPrefab == null) {
			return;
		}
		
		if (!UpdateCurrentTarget ()) {
			// no valid target
			if (isAttacking) {
				// we have stopped attacking
				StopAttacking ();
			}
		} else {
			if (!isAttacking) {
				// we have started attacking
				StartAttacking ();
			}


			var delay = Time.time - lastShotTime;
			if (delay < ShootDelaySeconds) {
				// still on cooldown
				return;
			}
			
			RotateTowardTarget();
			ShootAt (currentTarget);
		}
	}
	
	public void ShootAt(Unit target) {
		// create a new bullet
		var bulletObj = (GameObject)Instantiate(BulletPrefab, transform.position, GetRotationToward(currentTarget.transform));

		// set faction
		FactionManager.SetFaction (bulletObj, gameObject);

		// set velocity
		var bullet = bulletObj.GetComponent<Bullet> ();
		var rigidbody = bulletObj.GetComponent<Rigidbody2D> ();
		var direction = target.transform.position - bulletObj.transform.position;
		direction.Normalize ();
		rigidbody.velocity = direction * bullet.Speed;

		// reset shoot time
		lastShotTime = Time.time;
	}
	
	void StartAttacking() {
		isAttacking = true;
		SendMessage ("OnAttackStart", SendMessageOptions.DontRequireReceiver);
	}
	
	void StopAttacking() {
		isAttacking = false;
		SendMessage ("OnAttackStop", SendMessageOptions.DontRequireReceiver);
	}
	#endregion
	
	void GetStatsData(StatsMenuData statsMenuData) {
		statsMenuData.AttackRadius = AttackRadius;

		// get damage from attacker's bullet
		var bullet = BulletPrefab != null ? BulletPrefab.GetComponent<Bullet>() : null;
		if (bullet != null) {
			statsMenuData.DamageMax = bullet.DamageMax;
			statsMenuData.DamageMin = bullet.DamageMin;
		}
	}
}
