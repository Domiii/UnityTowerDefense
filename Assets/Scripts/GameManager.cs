using UnityEngine;
using System.Collections;

/**
 * Features:
 * Enemy spawning + following along path
 * Towers shooting
 * Enemy health, damage + death
 */

/**
 * New Fixes:
 * Projectile
 * 		* Reduce Projectile mass to a fraction
 * 		* Change Projectile to trigger
 * 		* Set CD on Projectile to continuous
 * 
 * #############################
 * 
 * New Features:
 * 
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

	private int _credits;

	public int Credits {
		get { return _credits; }
		set {
			_credits = value;
			GameUIManager.Instance.UpdateText();
		}
	}
	#endregion


	public GameManager() {
		Instance = this;
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

	public void GainCredits(int credits, Vector3 srcPosition) {
		Credits += credits;
		GameUIManager.Instance.ShowGainCreditText (credits, srcPosition);
	}

	public void DeductCredits(int credits) {
		Credits -= credits;
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
