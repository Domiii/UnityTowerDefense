using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/**
 * Features:
 * Enemy spawning + following along path
 * Towers shooting
 * Enemy health, damage + death
 */

/**
 * TODO: Game
 * Tower placement
 * Lives taken when Enemy makes it to the end of path	
 * Point + money system
 * Fix UI (see http://stackoverflow.com/questions/25477492/unity-4-6-how-to-scale-gui-elements-to-the-right-size-for-every-resolution)
 * Tower range
 * TowerTemplates
 * Tower upgrades
 * Solve problems to refill tower ammunition -> Problems = Coding- and design-related vocabulary exercises + maybe small-coding-problem solving
 * Replace dead enemies with decaying corpses
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
	public WaveGenerator WaveGenerator;

	private int credits;

	public int Credits {
		get { return credits; }
		set {
			credits = value;
			UpdateText();
		}
	}
	#endregion


	#region Text
	public Text CreditText;

	void UpdateText() {
		if (CreditText != null) {
			CreditText.text = "Credits: " + credits;
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
		WaveGenerator.StartNextWave ();
	}

	public void GainCredits() {
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
