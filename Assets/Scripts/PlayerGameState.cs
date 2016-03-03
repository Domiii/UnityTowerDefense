using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class PlayerGameState : MonoBehaviour {
	public class GameStateData {
		public string[] FinishedLevels = new string[0];


		public void Save() {
			PlayerPrefs.SetString ("player.level", string.Join(",", FinishedLevels));
			PlayerPrefs.Save ();
		}
		
		public void Load() {
			try {
				var levels = PlayerPrefs.GetString ("player.level");
				FinishedLevels = levels.Split(',');
				//Debug.Log("Saved: " + string.Join(",", FinishedLevels));
			}
			catch (UnityException err) {
				Debug.LogError("Unable to load game: " + err);
			}
		}

		public void Reset() {
			FinishedLevels = new string[0];
			Save ();
		}
	}

	static PlayerGameState Instance;
	static GameStateData _mokData = new GameStateData();
	static bool loaded = false;
	public static GameStateData Data {
		get {
			var data = _mokData;
			if (!loaded) {
				loaded = true;
				data.Load ();
			}
//			if (Application.isEditor) {
//				return _mokData;
//			}
//			else {
//				return Instance._savedData;
//			}
			return data;
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

		Data.Save ();

//		var list = Data.FinishedLevels.ToList ();
		//		list.Add (currentLevel);
//		Data.FinishedLevels = list.ToArray();
//		Debug.Log (string.Join(", ", Data.FinishedLevels));
	}
}