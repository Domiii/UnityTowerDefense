using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(SceneManager))]
public class SceneManagerEditor : Editor {
	public override void OnInspectorGUI ()
	{
		base.OnInspectorGUI ();

	}
}
