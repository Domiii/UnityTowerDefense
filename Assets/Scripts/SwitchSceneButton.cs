using UnityEngine;
using System.Collections;

public class SwitchSceneButton : MonoBehaviour {
	[HideInInspector]
	public string scene;
	public string Scene {
		get {
			return scene;
		}
		set {
			scene = value;
		}
	}

	void OnMouseDown() {
		Application.LoadLevel (Scene);
	}
}
