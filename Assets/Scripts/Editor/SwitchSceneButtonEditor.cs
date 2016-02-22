using UnityEngine;
using UnityEditor;

using System.Linq;
using System.Collections.Generic;

[CustomEditor(typeof(SwitchSceneButton))]
public class SwitchSceneButtonEditor : Editor {
	IEnumerable<string> GetAllSceneNames() {
		foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
		{
			if (scene.enabled)
			{
				string name = scene.path.Substring(scene.path.LastIndexOf('/')+1);
				name = name.Substring(0,name.Length-6);
				yield return name;
			}
		}
	}

	public override void OnInspectorGUI ()
	{
		Debug.Log (EditorBuildSettings.scenes.Length);
		base.OnInspectorGUI ();

		var btn = (SwitchSceneButton)target;

		EditorGUILayout.LabelField ("Scene");

		var scenes = GetAllSceneNames ().ToArray();
		var currentIndex = System.Array.IndexOf(scenes, btn.Scene);

		// display list
		currentIndex = EditorGUILayout.Popup(currentIndex, scenes);
		
		if (currentIndex > -1) {
			btn.Scene = scenes[currentIndex];
		}
	}
}
