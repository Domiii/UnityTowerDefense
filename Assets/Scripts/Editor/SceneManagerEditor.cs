using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(GlobalManager))]
public class SceneManagerEditor : Editor {
	public override void OnInspectorGUI ()
	{
		base.OnInspectorGUI ();

	}
}
