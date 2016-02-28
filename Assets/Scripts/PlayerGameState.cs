using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class PlayerGameState : MonoBehaviour {
	public class GameStateData {
		public string[] FinishedLevels = new string[0];
	}

	static PlayerGameState Instance;
	static GameStateData _mokData = new GameStateData();
	public static GameStateData Data {
		get {
//			if (Application.isEditor) {
//				return _mokData;
//			}
//			else {
//				return Instance._savedData;
//			}
			return _mokData;
		}
	}
	
	[HideInInspector]
	public GameStateData _savedData = new GameStateData();

	public PlayerGameState() {
		//Instance = this;
	}

	public static bool HasFinishedLevel(string level) {
		if (string.IsNullOrEmpty (level)) {
			return true;
		}
		return Data.FinishedLevels.Any (lvl => level == lvl);
	}

//	public int GetHighestLevel() {
//		return FinishedLevels.Max(lvl => GlobalManager.Instance.GetLevelIndex(lvl));
//	}

	public static void FinishedCurrentLevel() {
		var currentLevel = Application.loadedLevelName;

		System.Array.Resize (ref Data.FinishedLevels, Data.FinishedLevels.Length + 1);
		Data.FinishedLevels[Data.FinishedLevels.Length-1] = currentLevel;
//		var list = Data.FinishedLevels.ToList ();
		//		list.Add (currentLevel);
//		Data.FinishedLevels = list.ToArray();
//		Debug.Log (string.Join(", ", Data.FinishedLevels));
	}
}