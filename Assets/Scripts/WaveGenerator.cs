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
	public WavePath.FollowDirection PathDirection;
	public WaveTemplate[] WaveTemplates;
	public Text InfoText;

	List<Wave> waves;

	float lastUpdate;

	public Wave CurrentWave {
		get {
			return waves.LastOrDefault();
		}
	}

	public int CurrentWaveNumber {
		get {
			return waves.Count+1;
		}
	}

	/// <summary>
	/// The enemy template of the next wave (following the current wave)
	/// </summary>
	public WaveTemplate NextWaveTemplate {
		get {
			if (WaveTemplates.Length <= waves.Count) {
				// already spawned all waves
				return null;
			}
			return WaveTemplates[waves.Count];
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

		waves = new List<Wave> ();

		ResetTimer ();
	}
	
	// Update is called once per frame
	void Update () {
		// update all currently running waves
		foreach (var wave in waves) {
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
		var timeSinceLastUpdate = now - lastUpdate;

		ShowText("Next wave: " + CurrentWaveNumber + " (" + (DelayBetweenWaves - timeSinceLastUpdate).ToString ("0") + "s)");
		
		if (timeSinceLastUpdate >= DelayBetweenWaves) {
			StartNextWave ();
		}
	}
	
	public void ResetTimer() {
		// reset timer
		lastUpdate = Time.time;
	}
	
	// start next wave
	public void StartNextWave() {
		if (NextWaveTemplate == null) {
			// all waves done!
			return;
		}

		// create new wave
		var wave = new Wave (this);
		wave.WaveTemplate = NextWaveTemplate;
		waves.Add (wave);

		// spawn first enemy in new wave
		wave.Start ();

		ResetTimer ();
	}
	
	// start next enemy of given wave
	public void SpawnNextEnemy(Wave wave) {
		var followerObj = (GameObject)Instantiate (wave.WaveTemplate.EnemyPrefab, transform.position, Quaternion.identity);
		var follower = followerObj.GetComponent<PathFollower> ();
		follower.InitFollower (wave);
		wave.Enemies.Add (follower);

		// add faction
		FactionManager.SetFaction (followerObj, gameObject);

//		// set color
//		var renderer = GetComponent<SpriteRenderer> ();
//		var followerRenderer = follower.GetComponent<SpriteRenderer> ();
//		if (renderer != null) {
//			followerRenderer.color = renderer.color;
//		}
	}
}
