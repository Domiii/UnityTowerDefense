using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class WaveGenerator : MonoBehaviour {
	/// <summary>
	/// Time between waves in seconds.
	/// </summary>
	public float DelayBetweenWaves = 30;

	public WavePath Path;
	
	public WaveTemplate[] WaveTemplates;

	public Text InfoText;

	List<Wave> _waves;

	float _lastUpdate;

	public Wave CurrentWave {
		get {
			return _waves.LastOrDefault();
		}
	}

	public int CurrentWaveNumber {
		get {
			return _waves.Count+1;
		}
	}

	/// <summary>
	/// The enemy template of the next wave (following the current wave)
	/// </summary>
	public WaveTemplate NextWaveTemplate {
		get {
			if (WaveTemplates.Length <= _waves.Count) {
				// already spawned all waves
				return null;
			}
			return WaveTemplates[_waves.Count];
		}
	}

	public bool HasMoreWaves {
		get {
			return NextWaveTemplate != null;
		}
	}

	void ShowText(string text) {
		if (InfoText != null) {
			InfoText.text = text;
		}
	}

	// Use this for initialization
	void Start () {
		if (WaveTemplates == null || WaveTemplates.Length == 0) {
			Debug.LogError("WaveTemplates are empty");
		}
		if (WaveTemplates == null || WaveTemplates.Length == 0) {
			Debug.LogError("WavePath is not set");
		}

		_waves = new List<Wave> ();

		ResetTimer ();
	}
	
	// Update is called once per frame
	void Update () {
		// update all currently running waves
		foreach (var wave in _waves) {
			wave.Update();
		}

		// check if we need to spawn another wave
		if (HasMoreWaves) {
			UpdateWaveProgress ();
		} else {
			ShowText("Last Wave!");
		}
	}

	void UpdateWaveProgress() {
		var now = Time.time;
		var timeSinceLastUpdate = now - _lastUpdate;

		ShowText("Next wave: " + CurrentWaveNumber + " (" + (DelayBetweenWaves - timeSinceLastUpdate).ToString ("0") + "s)");
		
		if (timeSinceLastUpdate >= DelayBetweenWaves) {
			StartNextWave ();
		}
	}
	
	public void ResetTimer() {
		// reset timer
		_lastUpdate = Time.time;
	}
	
	// start next wave
	public void StartNextWave() {
		if (NextWaveTemplate == null) {
			// all waves done!
			GameManager.Instance.OnLastWave();
			return;
		}

		// create new wave
		var wave = new Wave (this);
		wave.WaveTemplate = NextWaveTemplate;
		_waves.Add (wave);

		// spawn first enemy in new wave
		wave.Start ();

		ResetTimer ();
	}
	
	// start next enemy of given wave
	public void SpawnNextEnemy(Wave wave) {
		var enemyObject = Instantiate (wave.WaveTemplate.EnemyPrefab);
		var enemy = enemyObject.GetComponent<Enemy> ();
		enemy.InitEnemy (wave);
		wave.Enemies.Add (enemy);
		
		// move enemy to starting position
		enemy.transform.position = Path.FirstPoint.position;
	}
}
