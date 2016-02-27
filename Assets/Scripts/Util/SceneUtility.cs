using UnityEngine;


#if UNITY_EDITOR
using UnityEditor;
#endif

using System.Collections.Generic;
using System.Linq;

public static class SceneUtility {
#if UNITY_EDITOR
	public static IEnumerable<string> GetAllSceneNames() {
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

	public static string InspectSceneSelectionDropdown(string label, string currentScenename) {
		var sceneNames = SceneUtility.GetAllSceneNames ().ToArray();
		var index = System.Array.FindIndex(sceneNames, scene => scene == currentScenename);
		index = EditorGUILayout.Popup(label, index, sceneNames);
		if (index > -1 && index < sceneNames.Length) {
			return sceneNames [index];
		}
		return "";
	}
#endif
}
