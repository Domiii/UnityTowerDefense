using UnityEngine;
using System.Collections;


/**
 * New Features:
 * Credits system
 * StatsMenu
 */

/**
 * TODO:
 * Spell system
 * Animations + Particles (e.g. https://www.assetstore.unity3d.com/en/#!/content/1745)
 * Start menu
 * Multiple levels + level selection
 */

/// <summary>
/// Game manager.
/// </summary>
public class GameManager : MonoBehaviour {
	public enum GameStatus
	{
		Running = 1,
		Won,
		Lost
	}

	public static GameManager Instance {
		get;
		private set;
	}


	#region Game Variables
	[HideInInspector]
	public float GameSceneSwitchDelay = 2;

	[HideInInspector]
	public GameStatus CurrentGameStatus = GameStatus.Running;
	
	[HideInInspector]
	public SceneSwitchConfig SceneAfterWin;
	
	[HideInInspector]
	public SceneSwitchConfig SceneAfterLoss;
	#endregion


	public GameManager() {
		Instance = this;
	}
	
	public bool IsRunning {
		get { return CurrentGameStatus == GameStatus.Running; }
	}

	void Start() {
	}

	#region Public Methods
	/// <summary>
	/// Starts next wave on all existing WaveGenerators
	/// </summary>
	public void StartNextWave() {
		var waveGenerators = FindObjectsOfType(typeof(WaveGenerator));
		foreach (var waveGenerator in waveGenerators) {
			((WaveGenerator)waveGenerator).StartNextWave ();
		}
	}
	#endregion


	#region Game Events
	public void WinGame() {
		// mark level as finished
		PlayerGameState.FinishedCurrentLevel ();
		OnGameOverStart(GameStatus.Won, SceneAfterWin);
	}
	
	public void LoseGame() {
		OnGameOverStart(GameStatus.Lost, SceneAfterLoss);
	}

	public void ResetGameData() {
		PlayerGameState.Data.Reset ();
	}
   	#endregion

	void OnGameOverStart(GameStatus status, SceneSwitchConfig nextScene) {
		CurrentGameStatus = status;

		// slow down everything
		Time.timeScale = 0.1f;
		
		if (nextScene != null) {
			if (nextScene.SceneSwitchPrefab != null) {
				Instantiate (nextScene.SceneSwitchPrefab);
			}

//			if (GameSceneSwitchDelay >= 0) {
//				StartCoroutine(CoroutineUtility.DelaySeconds(GameSceneSwitchDelay * Time.timeScale, () => {
//					EndGame (status, nextScene);
//				}));
//			}
		}
	}

	void EndGame(GameStatus status, SceneSwitchConfig nextScene) {
		if (nextScene.SceneName == null) {
			Debug.LogError("Next scene not set for game status: " + status);
			return;
		}

		Application.LoadLevel (nextScene.SceneName);
	}
}

[System.Serializable]
public class SceneSwitchConfig {
	[HideInInspector]
	public string SceneName;
	public GameObject SceneSwitchPrefab;
}