using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(QuizDialog))]
public class QuizDialogEditor : Editor {
	public override void OnInspectorGUI ()
	{
		base.OnInspectorGUI ();

		var dialog = (QuizDialog)target;
		
		// display scene dropdown
		dialog.NextScene = SceneUtility.InspectSceneSelectionDropdown("Next Scene", dialog.NextScene);

		if (GUILayout.Button("Update")) {
			dialog.Start ();
		}
	}
}
