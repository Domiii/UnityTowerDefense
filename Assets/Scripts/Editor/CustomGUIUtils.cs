using UnityEngine;
using UnityEditor;
using System.Collections;

public static class CustomGUIUtils {
	public static void DrawSeparator() {
		GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(1));
	}
}
