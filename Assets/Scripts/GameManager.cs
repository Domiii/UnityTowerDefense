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
 * New Features (old code):
 * Tower placement
 * FriendlyBase (trigger) + Lives taken when Enemy makes it to the end of path (add RigidBody2D to Enemy)
 * 
 * --
 * 
 * New Features (new code):
 * GameUIManager
 * Add sorting layer
 * Dimmer (Set sorting layer positions)
 * Add Selectable
 * Add Attacker to Tower (adds range)
 * Make complex shapes collidable + selectable (PolygonCollider2D + Point Editor)
 */

/**
 * TODO: Game
 * Attacker (Tower also needs it) + Unit (角色) + Layers for factions
 * Friendly Units
 * Units attack each other
 * Point + money system
 * Fix UI (see http://stackoverflow.com/questions/25477492/unity-4-6-how-to-scale-gui-elements-to-the-right-size-for-every-resolution)
 * TowerTemplates
 * Tower upgrades
 * Add the serious part!
 * Make it fun and interesting!
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

//	private int credits;
//
//	public int Credits {
//		get { return credits; }
//		set {
//			credits = value;
//			GameUIManager.Instance.UpdateText();
//		}
//	}
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
