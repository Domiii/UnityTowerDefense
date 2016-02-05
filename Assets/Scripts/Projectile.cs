using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {
	public float Damage = 10;
	public float Speed = 1;
	public float MaxLifeTimeSeconds = 10;

	float startTime;


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
	
	void OnTriggerEnter2D(Collider2D col) {
		// when colliding with Unit -> Cause damage
		var target = col.gameObject.GetComponent<Unit>();
		if (target != null && target.CanBeAttacked && FactionMember.AreHostile (gameObject, target.gameObject)) {
			var damageInfo = new DamageInfo { 
				Value = Damage,
				SourceFaction = FactionMember.GetFaction(gameObject)
			};
			target.Damage(damageInfo);
			Destroy (gameObject);
		}
	}
}
