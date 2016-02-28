using UnityEngine;


#if UNITY_EDITOR
using UnityEditor;
#endif

using System.Collections.Generic;
using System.Linq;

public static class SceneUtility {
#if UNITY_EDITOR
	public static string GetSceneName(EditorBuildSettingsScene scene) {
		var name = scene.path.Substring(scene.path.LastIndexOf('/')+1);
		name = name.Substring(0,name.Length-6);
		return name;
	}

	public static IEnumerable<string> GetAllSceneNames() {
		foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
		{
			if (scene.enabled)
			{
				string name = GetSceneName (scene);
				yield return name;
			}
		}
	}

	public static string InspectSceneSelectionDropdown(string label, string currentSceneName) {
		var sceneNames = SceneUtility.GetAllSceneNames ().ToArray();
		var index = System.Array.FindIndex(sceneNames, scene => scene == currentSceneName);
		if (label != null) {
			index = EditorGUILayout.Popup (label, index, sceneNames);
		} else {
			index = EditorGUILayout.Popup (index, sceneNames);
		}
		if (index > -1 && index < sceneNames.Length) {
			return sceneNames [index];
		}
		return "";
	}

	public static bool IsValidSceneName(string sceneName) {
		var match = EditorBuildSettings.scenes.FirstOrDefault(scene => GetSceneName(scene) == sceneName);
		return match != null && match.enabled;
}
#endif
}
