using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(QuizDialog))]
public class QuizDialogEditor : Editor {
	public override void OnInspectorGUI ()
	{
		base.OnInspectorGUI ();

		var dialog = (QuizDialog)target;

		if (GUILayout.Button("Update")) {
			dialog.Start ();
		}
	}
}
