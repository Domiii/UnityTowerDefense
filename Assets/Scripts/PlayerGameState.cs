using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class PlayerGameState : MonoBehaviour {
	public static PlayerGameState Instance {
		get;
		private set;
	}

	public PlayerGameState() {
		Instance = this;
	}

	[HideInInspector]
	public string[] FinishedLevels = new string[0];

	public bool HasFinishedLevel(string level) {
		if (string.IsNullOrEmpty (level)) {
			return true;
		}
		return FinishedLevels.Any (lvl => level == lvl);
	}

	public int GetHighestLevel() {
		return FinishedLevels.Max(lvl => ApplicationManager.Instance.GetLevelIndex(lvl));
	}

	public void FinishedCurrentLevel() {
		var list = FinishedLevels.ToList ();
		list.Add (Application.loadedLevelName);
		FinishedLevels = list.ToArray();
	}
}