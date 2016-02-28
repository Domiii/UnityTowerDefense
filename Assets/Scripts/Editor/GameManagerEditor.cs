using UnityEngine;
using UnityEditor;

using System.Collections.Generic;
using System.Linq;

[CustomEditor(typeof(GameManager))]
public class GameManagerEditor : Editor {
	public override void OnInspectorGUI ()
	{
		base.OnInspectorGUI ();

		
		// btn.SceneName = SceneUtil.InspectSceneSelectionDropdown("Scene", btn.SceneName);
		EditorGUILayout.LabelField ("When Won", EditorStyles.boldLabel);
		InspectSceneConfig("SceneAfterWin");

		EditorGUILayout.LabelField ("When Lost", EditorStyles.boldLabel);
		InspectSceneConfig("SceneAfterLoss");
	}

	void InspectSceneConfig(string name) {
		var sceneConfigWrapper = serializedObject.FindProperty(name);
		
		var prefabWrapper = sceneConfigWrapper.FindPropertyRelative ("SceneSwitchPrefab");
		EditorGUILayout.PropertyField (prefabWrapper, new GUIContent("Show prefab"));

//		var sceneWrapper = sceneConfigWrapper.FindPropertyRelative ("SceneName");
//		sceneWrapper.stringValue = SceneUtility.InspectSceneSelectionDropdown ("Switch to Scene", sceneWrapper.stringValue);

		serializedObject.ApplyModifiedProperties();
	}
}
