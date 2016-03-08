using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RepeatSceneButton : MonoBehaviour {
	void Awake() {
		var btn = GetComponent<Button>();
		if (btn != null) {
			btn.onClick.AddListener(OnMouseDown);
		}
	}

	void OnMouseDown() {
		Application.LoadLevel (Application.loadedLevelName);
	}
}
