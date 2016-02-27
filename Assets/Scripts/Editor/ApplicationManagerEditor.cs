using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

[CustomEditor(typeof(ApplicationManager))]
public class ApplicationManagerEditor : Editor{

	public override void OnInspectorGUI ()
	{
		base.OnInspectorGUI ();

		var mgr = (ApplicationManager)target;
		var levels = mgr.Levels;

		EditorGUILayout.BeginVertical ();
		EditorGUILayout.LabelField ("Levels (" + levels.Length + ")", EditorStyles.boldLabel);

		var toRemove = -1;
		CustomGUIUtils.DrawSeparator ();
		for (var i = 0; i < levels.Length; ++i) {
			var level = levels[i];
			EditorGUILayout.BeginHorizontal ();
			EditorGUILayout.LabelField(level.ToString());
			if (GUILayout.Button("X")) {
				toRemove = i;
			}
			EditorGUILayout.EndHorizontal ();
		}
		CustomGUIUtils.DrawSeparator ();
		
		if (toRemove >= 0) {
			ArrayUtility.RemoveAt(ref levels, toRemove);
			levels = levels.Where(lvl => SceneUtility.IsValidSceneName(lvl.SceneName)).ToArray();
		}

		var sceneName = SceneUtility.InspectSceneSelectionDropdown (null, null);

		if (SceneUtility.IsValidSceneName(sceneName)) {
			ArrayUtility.Add(ref levels, new LevelInfo(sceneName));
		}

		EditorGUILayout.EndVertical ();
		
		if (GUI.changed) {
			mgr.Levels = levels;
			EditorUtility.SetDirty (target);
		}
	}
}
