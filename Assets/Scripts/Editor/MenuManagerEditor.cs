using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

[CustomEditor(typeof(MenuManager))]
public class MenuManagerEditor : Editor {
	public override void OnInspectorGUI ()
	{
		base.OnInspectorGUI ();

		var menuMgr = (MenuManager)target;

		EditorGUILayout.LabelField ("Start Menu");
		var canvases = menuMgr.GetCanvases ().ToArray();
		var canvasNames = canvases.Select (canvas => canvas.name).ToArray();

		// display list
		menuMgr.StartIndex = EditorGUILayout.Popup(menuMgr.StartIndex, canvasNames);

		if (menuMgr.StartIndex > -1) {
			var selected = canvases[menuMgr.StartIndex];
			menuMgr.GoToCanvas (selected);
		}
	}
}
