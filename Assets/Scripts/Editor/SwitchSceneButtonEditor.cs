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
		base.OnInspectorGUI ();

		var btn = (SwitchSceneButton)target;

		// display list
		var scenes = GetAllSceneNames ().ToArray();
		btn.SceneIndex = EditorGUILayout.Popup("Scene", btn.SceneIndex, scenes);
	}
}
