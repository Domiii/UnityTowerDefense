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
	
	public void Damage(DamageInfo damageInfo) {
		if (!IsAlive) {
			// don't do anything when dead
			return;
		}
		
		Health -= damageInfo.Value;
		
		if (!IsAlive) {
			// died from damage
			Die(damageInfo);
		}
	}
	
	void Die(DamageInfo damageInfo) {
		tag = DeadTag;
		Health = 0;

		SendMessage ("OnDeath", damageInfo, SendMessageOptions.DontRequireReceiver);
		
		Destroy (gameObject);
	}
}
