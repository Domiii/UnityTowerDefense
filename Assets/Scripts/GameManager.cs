using UnityEngine;
using System.Collections;

/**
 * TODO: Game
 * 1. Enemy spawning + moving
 * 2. Towers + Tower placement
 * 3. Towers shooting
 * 4. Enemy health
 * 5. Lives taken when Enemy makes it to the end of path
 * 6. Solve problems to refill tower ammunition -> Problems = Coding- and design-related vocabulary exercises + maybe small-coding-problem solving
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

	public GameStatus CurrentGameStatus = GameStatus.Running;

	public bool IsGameOver {
		get { return CurrentGameStatus == GameStatus.Finished; }
	}

	public GameManager() {
		Instance = this;
	}

	/// <summary>
	/// This method is called when player has beaten all waves
	/// </summary>
	public void OnAllWavesDone() {
		// TODO: End the game!
		CurrentGameStatus = GameStatus.Finished;
	}
}
