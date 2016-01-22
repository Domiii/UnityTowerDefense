using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour {
	public static readonly string AliveTag = "Alive";
	public static readonly string DeadTag = "Dead";
	
	public static PathFollower FindNextAlive() {
		var obj =  GameObject.FindGameObjectWithTag(AliveTag);
		if (obj != null) {
			return obj.GetComponent<PathFollower> ();
		}
		return null;
	}
	
	
	public float MaxHealth = 100;
	public float Health = 100;

	void Start() {
		tag = AliveTag;
	}

	public bool IsAlive {
		get { return Health > 0; }
	}
	
	public bool CanBeAttacked {
		get { return IsAlive; }
	}
	
	public void Damage(float damage) {
		if (!IsAlive) {
			// don't do anything when dead
			return;
		}
		
		Health -= damage;
		
		if (!IsAlive) {
			// died from damage
			Die();
		}
	}
	
	void Die() {
		tag = DeadTag;
		Health = 0;
		Destroy (gameObject);

		SendMessage ("OnDeath", SendMessageOptions.DontRequireReceiver);
	}
}
