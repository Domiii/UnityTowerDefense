using UnityEngine;
using UnityEditor;
using System.Collections;

public class CustomScriptableObjectManagerEditor : Editor {
	static CustomScriptableObjectManager scripts;
	public static CustomScriptableObjectManager Scripts {
		get {
			if (scripts == null) {
				OnReloadScripts();
			}
			return scripts;
		}
	}

	[UnityEditor.Callbacks.DidReloadScripts]
	public static void OnReloadScripts() {
		scripts = CustomScriptableObjectManager.FindAllCustomScriptableObjects ();
	}
}
