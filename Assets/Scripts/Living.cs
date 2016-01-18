using UnityEngine;
using System.Collections;

public class Living : MonoBehaviour {
	public static readonly string AliveTag = "Alive";
	public static readonly string DeadTag = "Dead";
	
	public static Living FindNextAlive() {
		var obj =  GameObject.FindGameObjectWithTag(AliveTag);
		if (obj != null) {
			return obj.GetComponent<Living> ();
		}
		return null;
	}


	public float MaxHealth = 100;
	public float Health = 100;

	// Use this for initialization
	void Start () {
		tag = AliveTag;
	}
	
	// Update is called once per frame
	void Update () {
		// move randomly
		if (!IsAlive) {
			// don't do anything when dead
			return;
		}

		//transform.position += new Vector3(Random.Range(-100, 100), Random.Range(-100, 100)) * 0.001f;
	}

	public bool IsAlive {
		get { return Health > 0; }
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

	protected void OnDeath() {
		tag = DeadTag;

	}
}
