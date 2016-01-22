﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FriendlyBase : MonoBehaviour {
	public Text LivesText;
	public int Lives = 20;

	// Use this for initialization
	void Start () {
		UpdateText ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	// any entering enemy is removed and costs a life
	void OnTriggerEnter2D(Collider2D col) {
		var enemy = col.gameObject.GetComponent<PathFollower> ();
		if (enemy != null) {
			// enemy reached the target
			OnAttack(enemy);
		}
	}

	void OnAttack(PathFollower enemy) {
		Lives -= 1;
		Destroy (enemy.gameObject);
		UpdateText ();
	}

	void UpdateText() {
		if (LivesText != null) {
			LivesText.text = "Lives: " + Lives;
		}
	}
}