using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class WaveGenerator : MonoBehaviour {
	/// <summary>
	/// Time between waves in seconds.
	/// </summary>
	public float DelayBetweenWaves = 30;
	
	/// <summary>
	/// Time between en enemies of the same wave in seconds.
	/// </summary>
	public float DelayBetweenEnemies = 2;

	public WavePath Path;
	
	public EnemyTemplate[] EnemyTemplates;

	List<Wave> _waves;

	float _lastUpdate;

	public Wave CurrentWave {
		get {
			return _waves.LastOrDefault();
		}
	}

	/// <summary>
	/// The enemy template of the next wave (following the current wave)
	/// </summary>
	public EnemyTemplate NextWaveEnemyTemplate {
		get {
			if (EnemyTemplates.Length <= _waves.Count) {
				// already spawned all waves
				return null;
			}
			return EnemyTemplates[_waves.Count];
		}
	}

	public bool IsNextWaveReady {
		get {
			return CurrentWave == null || CurrentWave.HaveAllEnemiesSpawned;
		}
	}

	// Use this for initialization
	void Start () {
		if (EnemyTemplates == null || EnemyTemplates.Length == 0) {
			Debug.LogError("There must be at least one wave");
		}

		_waves = new List<Wave> ();

		// start timer
		_lastUpdate = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		if (GameManager.Instance.IsGameOver) {
			// game already finished, don't update again
			return;
		}

		var now = Time.time;
		var timeSinceLastUpdate = now - _lastUpdate;

		var waitTime = IsNextWaveReady ? DelayBetweenWaves : DelayBetweenEnemies;

		if (timeSinceLastUpdate >= waitTime) {
			if (IsNextWaveReady) {
				SpawnNextWave();
			} else {
				SpawnNextEnemy();
			}
		}
	}
	
	// spawn next wave
	void SpawnNextWave() {
		var nextEnemyTemplate = NextWaveEnemyTemplate;
		if (nextEnemyTemplate == null) {
			// all waves done!
			GameManager.Instance.OnFinishedAllWaves();
			return;
		}

		// create new wave
		var wave = new Wave ();
		wave.EnemyTemplate = nextEnemyTemplate;
		wave.Path = Path;
		_waves.Add (wave);

		// spawn first enemy in new wave
		SpawnNextEnemy ();
	}
	
	// spawn next enemy within current wave
	void SpawnNextEnemy() {
		var currentWave = CurrentWave;
		var enemy = Instantiate (currentWave.EnemyTemplate.EnemyPrefab);

		// TODO: register enemy
		// TODO: move enemy to starting position
	}
}
