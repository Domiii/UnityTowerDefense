using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SwitchSceneButton : MonoBehaviour {
	[HideInInspector]
	public string SceneName;

	void Awake() {
		var btn = GetComponent<Button>();
		if (btn != null) {
			btn.onClick.AddListener(OnMouseDown);
		}
	}

	void OnMouseDown() {
		Application.LoadLevel (SceneName);
	}
}
