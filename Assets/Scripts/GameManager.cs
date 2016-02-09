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
		Running,
		Finished
	}

	public static GameManager Instance;

	#region Game Variables
	public GameStatus CurrentGameStatus = GameStatus.Running;
	#endregion


	public GameManager() {
		Instance = this;
	}

	void Start() {
		DontDestroyOnLoad(gameObject);
	}
	
	public bool IsGameOver {
		get { return CurrentGameStatus == GameStatus.Finished; }
	}

	#region GameManager Methods
	public void StartNextWave() {
		var waveGenerators = FindObjectsOfType(typeof(WaveGenerator));
		foreach (var waveGenerator in waveGenerators) {
			((WaveGenerator)waveGenerator).StartNextWave ();
		}
	}
	#endregion

	#region Game Events
	public void OnLastWave() {

	}

	/// <summary>
	/// This method is called when player has beaten all waves
	/// </summary>
	public void OnFinishedAllWaves() {
		// TODO: End the game!
		CurrentGameStatus = GameStatus.Finished;
	}
   	#endregion
}
