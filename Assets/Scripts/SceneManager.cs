using UnityEngine;
using System.Collections;

public class SceneManager : MonoBehaviour {
	public static SceneManager Instance {
		get;
		private set;
	}

	public string[] Levels = new string[0];

	public SceneManager() {
		if (Instance != null) {
			Debug.LogWarning("Trying to create more than one SceneManager, but SceneManager is global Singleton.", this);
		}
		Instance = this;
	}

	// Use this for initialization
	void Awake () {
		DontDestroyOnLoad (this);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
