using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

using UnityEditorInternal;
using System.Reflection;

using System.Collections;
using System.Linq;
using System.IO;

/// <summary>
/// 
/// </summary>
/// <see cref="http://answers.unity3d.com/questions/585108/how-do-you-access-sorting-layers-via-scripting.html"/>
[CustomEditor(typeof(ParticleSystemHelper))]
public class ParticleSystemHelperEditor : Editor {
	// Get the sorting layer names
	public string[] GetSortingLayerNames() {
		System.Type internalEditorUtilityType = typeof(InternalEditorUtility);
		PropertyInfo sortingLayersProperty = internalEditorUtilityType.GetProperty("sortingLayerNames", BindingFlags.Static | BindingFlags.NonPublic);
		return (string[])sortingLayersProperty.GetValue(null, new object[0]);
	}
	// Get the unique sorting layer IDs -- tossed this in for good measure
	public int[] GetSortingLayerUniqueIDs() {
		System.Type internalEditorUtilityType = typeof(InternalEditorUtility);
		PropertyInfo sortingLayerUniqueIDsProperty = internalEditorUtilityType.GetProperty("sortingLayerUniqueIDs", BindingFlags.Static | BindingFlags.NonPublic);
		return (int[])sortingLayerUniqueIDsProperty.GetValue(null, new object[0]);
	}

	public override void OnInspectorGUI () {
		base.OnInspectorGUI ();
		
		var sortHelper = (ParticleSystemHelper)target;


		var names = GetSortingLayerNames ();
		var ids = GetSortingLayerUniqueIDs ();
		var selectedIndex = System.Array.IndexOf(ids, sortHelper.SortingLayerId);

		// see https://polygoned.wordpress.com/2012/05/23/unity3d-creating-a-drop-down-menu-in-a-custom-inspector/
		EditorGUILayout.BeginHorizontal();
		selectedIndex = EditorGUILayout.Popup("Sorting Layer", selectedIndex, names, EditorStyles.popup);
		EditorGUILayout.EndHorizontal();

		if (selectedIndex >= 0) {
			sortHelper.SetSortingOptions (ids [selectedIndex], sortHelper.SortingOrder);

			EditorUtility.SetDirty (sortHelper);
		}
	}
}
