﻿using UnityEngine;
using System.Collections;

public class RequireLevel : MonoBehaviour {
	[HideInInspector]
	public string Level;

	// Use this for initialization
	void Start () {
		if (!PlayerGameState.HasFinishedLevel(Level)) {
			gameObject.SetActive(false);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
