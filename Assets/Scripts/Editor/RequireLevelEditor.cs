using UnityEngine;
using UnityEditor;

using System.Collections.Generic;
using System.Linq;

[CustomEditor(typeof(RequireLevel))]
public class RequireLevelEditor : Editor {
	public override void OnInspectorGUI ()
	{
		base.OnInspectorGUI ();

		var requireLevel = (RequireLevel)target;

		//requireLevel.Level = SceneUtility.InspectSceneSelectionDropdown ("Required Level", requireLevel.Level);

		var levelNames = ApplicationManager.Instance.Levels.Select(lvl => lvl.SceneName).ToArray();
		var index = System.Array.IndexOf(levelNames, requireLevel.Level);
		index = EditorGUILayout.Popup ("Required Level", index, levelNames);
		if (index >= 0 && index < levelNames.Length) {
			requireLevel.Level = levelNames[index];
		}
	}
}
