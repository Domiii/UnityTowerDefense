using UnityEngine;
using System.Collections;
using System.Linq;

// TODO: Need to store ApplicationManager and PlayerGameState as ScriptableObject, to make them accessible across scenes

[ExecuteInEditMode]
public class GlobalManager : MonoBehaviour {
	public static GlobalManager Instance {
		get;
		private set;
	}

	[HideInInspector]
	public LevelInfo[] Levels = new LevelInfo[0];

	public GlobalManager() {
	}

	public bool IsValidLevel(string level) {
		return Levels.Any(lvlInfo => lvlInfo.SceneName == level);
	}

//	public int GetLevelIndex(string level) {
//		return Levels.ToList().FindIndex(lvlInfo => level == lvlInfo.SceneName);
//	}

	void Awake()
	{
		if(Instance) {
			//Debug.LogWarning("Trying to create more than one GlobalManager, but GlobalManager is global Singleton.", this);
			DestroyImmediate(gameObject);
		}
		else
		{
			DontDestroyOnLoad(gameObject);
			Instance = this;
		}
	}
}


[System.Serializable]
public class LevelInfo {
	public string SceneName;

	public LevelInfo(string sceneName) {
		SceneName = sceneName;
	}

	public override string ToString ()
	{
		return SceneName;
	}
}
