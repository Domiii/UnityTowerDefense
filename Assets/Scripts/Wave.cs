using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class Wave {
	public WaveGenerator WaveGenerator;
	public WaveTemplate WaveTemplate;

	/// <summary>
	/// The set of enemies attacking this round.
	/// </summary>
	public List<PathFollower> Enemies = new List<PathFollower>();

	float lastUpdate;

	public Wave(WaveGenerator waveGenerator) {
		WaveGenerator = waveGenerator;
	}

	public bool HaveAllEnemiesSpawned {
		get {
			// we have spawned the total amount of enemies planned for this wave
			return Enemies.Count >= WaveTemplate.Amount;
		}
	}
	
	
	// Use this for initialization
	public void Start () {
		WaveGenerator.SpawnNextEnemy(this);
		ResetTimer ();
	}

	public void Update() {
		if (!HaveAllEnemiesSpawned) {
			var now = Time.time;
			var timeSinceLastUpdate = now - lastUpdate;
			
			if (timeSinceLastUpdate >= WaveTemplate.DelayBetweenEnemies) {
				WaveGenerator.SpawnNextEnemy(this);
				ResetTimer ();
			}
		}
	}
	
	public void ResetTimer() {
		// reset timer
		lastUpdate = Time.time;
	}
}
