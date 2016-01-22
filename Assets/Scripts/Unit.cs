using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour {
	public static readonly string AliveTag = "Alive";
	public static readonly string DeadTag = "Dead";

	public float MaxHealth = 100;
	public float Health = 100;

	public bool IsAlive {
		get { return Health > 0; }
	}


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
