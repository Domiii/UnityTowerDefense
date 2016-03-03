using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ProjectileCollisionTrigger2D))]
public class Bullet : MonoBehaviour {
	public float DamageMin = 10;
	public float DamageMax = 20;
	public float Speed = 1;
	public float MaxLifeTimeSeconds = 10;

	float startTime;
	bool hasHit = false;


	// Use this for initialization
	void Start () {
		startTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		var now = Time.time;
		var lifeTime = now - startTime;

		if (lifeTime >= MaxLifeTimeSeconds) {
			// missed target
			Destroy(gameObject);
			return;
		}
	}
	
	void OnProjectileHit(Collider2D col) {
		// when colliding with Unit -> Cause damage
		var target = col.gameObject.GetComponent<Unit>();
		if (!hasHit && target != null && target.CanBeAttacked && FactionManager.AreHostile (gameObject, target.gameObject)) {
			var damageInfo = ObjectManager.Instance.Obtain<DamageInfo>();
			damageInfo.Value = Random.Range(DamageMin, DamageMax);
			damageInfo.SourceFactionType = FactionManager.GetFactionType(gameObject);
			target.Damage(damageInfo);
			Destroy (gameObject);
			hasHit = true;
		}
	}
}
