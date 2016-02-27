using UnityEngine;
using System.Collections;

public class ApplicationManager : MonoBehaviour {
	public static ApplicationManager Instance {
		get;
		private set;
	}

	public string[] Levels = new string[0];

	public ApplicationManager() {
	}

	void Awake()
	{
		if(Instance) {
			//Debug.LogWarning("Trying to create more than one SceneManager, but SceneManager is global Singleton.", this);
			DestroyImmediate(gameObject);
		}
		else
		{
			DontDestroyOnLoad(gameObject);
			Instance = this;
		}
	}
}
