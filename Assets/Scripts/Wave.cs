using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class Wave {
	public WavePath Path;
	public EnemyTemplate EnemyTemplate;

	/// <summary>
	/// The set of enemies attacking this round.
	/// </summary>
	public List<Enemy> Enemies = new List<Enemy>();

	public bool HaveAllEnemiesSpawned {
		get {
			// we have spawned the total amount of enemies planned for this wave
			return Enemies.Count >= EnemyTemplate.Amount;
		}
	}

}
