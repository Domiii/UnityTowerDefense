using UnityEngine;
using UnityEngine.UI;

using UnityEditor;

using System.Collections.Generic;
using System.Reflection;
using System.Linq;

using Spells;

// TODO: Rename or hide different options in different contexts
// TODO: Make sure "script missing" objects get deleted

[CustomEditor(typeof(Spell))]
public class SpellEditor : Editor {
	private Editor _editor;

	void OnEnable() {
	}

	public override void OnInspectorGUI () {
		var spell = (Spell)target;
		serializedObject.Update();

		//EditorGUIUtility.currentViewWidth

		// start drawing spell
		spell.Cooldown = EditorGUILayout.FloatField("Cooldown", spell.Cooldown);
		
		CustomGUIUtils.DrawSeparator ();
		InspectSpellTargetSettings (spell.Targets);
		
		CustomGUIUtils.DrawSeparator ();
		InspectCastPhaseTemplate (spell.CastPhase);
		
		CustomGUIUtils.DrawSeparator ();
		InspectProjectilePhaseTemplate(spell.ProjectilePhase);

		CustomGUIUtils.DrawSeparator ();
		InspectImpactPhaseTemplate(spell.ImpactPhase);


		if (GUI.changed) {
			// write values back
			serializedObject.ApplyModifiedProperties ();
		}
	}
	
	void InspectCastPhaseTemplate(CastPhaseTemplate phaseTemplate) {
		InspectSpellPhaseTemplate (phaseTemplate);
	}
	
	void InspectProjectilePhaseTemplate(ProjectilePhaseTemplate phaseTemplate) {
		InspectSpellPhaseTemplate (phaseTemplate);
	}
	
	void InspectImpactPhaseTemplate(ImpactPhaseTemplate phaseTemplate) {
		InspectSpellPhaseTemplate (phaseTemplate);
	}

	void InspectSpellPhaseTemplate(SpellPhaseTemplate phaseTemplate) {
		// phaseTemplate.Duration;
	}
	
	void InspectSpellTargetSettings(SpellTargetSettings targets) {
		EditorGUILayout.LabelField ("Targets", EditorStyles.boldLabel);
		
		targets.Range = EditorGUILayout.FloatField ("MaxRange", targets.Range);
		
		InspectArrayWithInheritanceMutuallyExclusive (ref targets.TargetCollectors);
		InspectArrayWithInheritanceMutuallyExclusive (ref targets.TargetFilters);
	}

	void InspectArrayWithInheritanceMutuallyExclusive<A>(ref A[] arr) 
		where A : ScriptableObject
	{
		var allEntries = CustomScriptableObjectManagerEditor.Scripts.GetEntries<A>();

		foreach (var entry in allEntries) {
			var matchingObjectIndex = ArrayUtility.FindIndex(arr, collector => collector.GetType() == entry.Type);
			var wasTypeAdded = matchingObjectIndex >= 0;
			var obj = wasTypeAdded ? arr[matchingObjectIndex] : null;
			
			var isObjectAdded = EditorGUILayout.Toggle (entry.Name, wasTypeAdded);
			if (isObjectAdded != wasTypeAdded) {
				if (!isObjectAdded) {
					// remove
					RemoveScriptableObjectFromArray(ref arr, obj);
					obj = null;
				}
				else {
					// add
					obj = AddCustomScriptableObjectToArray(ref arr, entry.Type);
				}
				serializedObject.Update();
			}

			if (isObjectAdded) {
				// draw object
				var serializedWrapper = new SerializedObject(obj);
				var objProp = serializedWrapper.GetIterator();
				if (objProp.hasChildren) {
					++EditorGUI.indentLevel;
					objProp.NextVisible(true);		// ignore script
					while (objProp.NextVisible(true)) {
						EditorGUILayout.PropertyField(objProp, true);
					}
					--EditorGUI.indentLevel;
					serializedWrapper.ApplyModifiedProperties();
				}
			}
		}
	}

	A AddCustomScriptableObjectToArray<A>(ref A[] arr, System.Type type)
		where A : ScriptableObject
	{
		// save to asset
		var newObj = (A)ScriptableObject.CreateInstance(type);
		newObj.hideFlags = HideFlags.HideInHierarchy;
		newObj.name = type.Name;
		AssetDatabase.AddObjectToAsset(newObj, target);
		//AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(animationClip));
		AssetDatabase.SaveAssets();


		// add to array
		if (arr == null) {
			arr = new A[1];
		}
		else {
			System.Array.Resize(ref arr, arr.Length + 1);
		}

		arr[arr.Length-1] = newObj;

		return newObj;
	}

	void RemoveScriptableObjectFromArray<A>(ref A[] arr, A obj)
		where A : ScriptableObject
	{
		// delete from asset
		UnityEngine.Object.DestroyImmediate (obj, true);

		// contract array
		var index = System.Array.IndexOf(arr, obj);
		if (index != -1) {
			for (var i = index; i < arr.Length-1; ++i) {
				arr[i] = arr[i+1];
			}
			System.Array.Resize(ref arr, arr.Length - 1);
		}
	}
}
