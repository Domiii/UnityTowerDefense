using UnityEngine;
using UnityEditor;

using System.Linq;
using System.Collections.Generic;

[CustomEditor(typeof(SwitchSceneButton))]
public class SwitchSceneButtonEditor : Editor {
	public override void OnInspectorGUI ()
	{
		base.OnInspectorGUI ();

		var btn = (SwitchSceneButton)target;

		// display scene dropdown
		btn.SceneName = SceneUtility.InspectSceneSelectionDropdown("Scene", btn.SceneName);
	}
}
