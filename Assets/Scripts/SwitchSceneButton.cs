using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SwitchSceneButton : MonoBehaviour {
	[HideInInspector]
	public int SceneIndex;

	void Awake() {
		var btn = GetComponent<Button>();
		if (btn != null) {
			btn.onClick.AddListener(OnMouseDown);
		}
	}

	void OnMouseDown() {
		Application.LoadLevel (SceneIndex);
	}
}
