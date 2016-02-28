using UnityEngine;
using UnityEngine.UI;

using UnityEditor;

using System.Collections.Generic;
using System.Reflection;

using Spells;
using System;


[CustomEditor(typeof(SpellManager))]
public class SpellManagerEditor : Editor {
	public override void OnInspectorGUI ()
	{
		base.OnInspectorGUI ();

		
		if (GUILayout.Button("New Spell"))
		{
			var spell = ScriptableObject.CreateInstance(typeof(Spell));
			ProjectWindowUtil.StartNameEditingIfProjectWindowExists(
				spell.GetInstanceID(),
				ScriptableObject.CreateInstance<EndNameEdit>(),
				string.Format("{0}.asset", "new spell"),
				AssetPreview.GetMiniThumbnail(spell), 
				null);
		}
	}
}
