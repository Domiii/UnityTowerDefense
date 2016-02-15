using UnityEngine;
using UnityEngine.UI;

using UnityEditor;

using System.Collections.Generic;
using System.Reflection;
using System.Linq;

using Spells;

// TODO: Rename or hide different options in different contexts

[CustomEditor(typeof(Spell))]
public class SpellEditor : Editor {
	private Editor _editor;

	void OnEnable() {
	}

	public override void OnInspectorGUI () {
		var spell = (Spell)target;
		serializedObject.Update();

		//EditorGUIUtility.currentViewWidth
		EditorGUILayout.TextArea (System.String.Join("\n", spell.SubAssets));

		// start drawing spell
		spell.Cooldown = EditorGUILayout.FloatField("Cooldown", spell.Cooldown);

		
		CustomGUIUtils.DrawSeparator ();
		InspectSpellTargetSettings (spell.Targets);
		
		CustomGUIUtils.DrawSeparator ();
		InspectCastPhaseTemplate ();
		
		CustomGUIUtils.DrawSeparator ();
		InspectProjectilePhaseTemplate();

		CustomGUIUtils.DrawSeparator ();
		InspectImpactPhaseTemplate();

		if (GUI.changed) {
			// write values back
			serializedObject.ApplyModifiedProperties ();
			EditorUtility.SetDirty(target);
		}

		// TODO: For duplicate, we need to also duplicate all ScriptableObject sub assets
//		if (GUILayout.Button ("Duplicate")) {
//			var newSpell = Object.Instantiate(spell) as Spell;
//			ProjectWindowUtil.StartNameEditingIfProjectWindowExists(
//				newSpell.GetInstanceID(),
//				ScriptableObject.CreateInstance<EndNameEdit>(),
//				string.Format("{0}.asset", "new spell"),
//				AssetPreview.GetMiniThumbnail(newSpell), 
//				null);
//		}
	}

	#region Spell System Inspectors
	void InspectCastPhaseTemplate() {
		var spell = (Spell)target;
		spell.CastPhase = ToggleAddRemoveScriptableObject ("Cast Phase", spell.CastPhase);
		if (spell.CastPhase != null) {
			//InspectSpellPhaseTemplate(spell.CastPhase);
		}
	}
	
	void InspectProjectilePhaseTemplate() {
		var spell = (Spell)target;
		spell.ProjectilePhase = ToggleAddRemoveScriptableObject ("Projectile Phase", spell.ProjectilePhase);
		if (spell.ProjectilePhase != null) {
			InspectSpellPhaseTemplate(spell.ProjectilePhase);
		}
	}
	
	void InspectImpactPhaseTemplate() {
		var spell = (Spell)target;
		spell.ImpactPhase = ToggleAddRemoveScriptableObject ("Impact Phase", spell.ImpactPhase);
		if (spell.ImpactPhase != null) {
			//InspectSpellPhaseTemplate(spell.ImpactPhase);
		}
	}

	void InspectSpellPhaseTemplate(SpellPhaseTemplate template) {
		//InspectUnityObject(phaseTemplate);
		++EditorGUI.indentLevel;
		template.Duration = EditorGUILayout.FloatField ("Duration", template.Duration);
		template.PhaseObjectPrefab = (GameObject)EditorGUILayout.ObjectField ("Phase Prefab", template.PhaseObjectPrefab, typeof(GameObject), false);
		
		InspectSpellEffects ("Start Effects", ref template.StartEffects);
		//InspectRepeatSpellEffects ("Repeat Effects", ref template.RepeatEffects);
		//template.EndEffects = InspectSpellEffects ("End Effects", ref template.EndEffects);
		--EditorGUI.indentLevel;
	}
	
	void InspectSpellTargetSettings(SpellTargetSettings targets) {
		EditorGUILayout.LabelField ("Targets");
		
		++EditorGUI.indentLevel;
		targets.Range = EditorGUILayout.FloatField ("MaxRange", targets.Range);
		
		InspectArrayWithInheritanceMutuallyExclusiveTypes (ref targets.TargetCollectors);
		InspectArrayWithInheritanceMutuallyExclusiveTypes (ref targets.TargetFilters);

		--EditorGUI.indentLevel;
	}
	
	void InspectSpellEffects(string label, ref SpellEffectCollection spellEffects) {
		spellEffects = ToggleAddRemoveSerializable (label, spellEffects);
		if (spellEffects != null) {
			++EditorGUI.indentLevel;
			//InspectSpellEffects(spellEffects.Effects);
			--EditorGUI.indentLevel;
		}
	}
	
	void InspectRepeatSpellEffects(string label, ref RepeatSpellEffectCollection spellEffects) {
		spellEffects = ToggleAddRemoveSerializable (label, spellEffects);
		if (spellEffects != null) {
			++EditorGUI.indentLevel;
			spellEffects.RepeatDelay = EditorGUILayout.FloatField ("Repeat Delay", spellEffects.RepeatDelay);
			spellEffects.MaxRepetitions = EditorGUILayout.FloatField ("MaxRepetitions", spellEffects.MaxRepetitions);
			//InspectSpellEffects(spellEffects.Effects);
			--EditorGUI.indentLevel;
		}
	}
	
	void InspectSpellEffects(SpellEffect[] arr) {

	}
	#endregion

	#region Generic Inspectors
	void InspectArrayWithInheritanceMutuallyExclusiveTypes<A>(ref A[] arr) 
		where A : ScriptableObject
	{
		var allEntries = CustomScriptableObjectManagerEditor.Scripts.GetEntries<A>();

		// remove empty entries
		for (var i = arr.Length-1; i >= 0; --i) {
			if (arr[i] == null) {
				ArrayUtility.RemoveAt(ref arr, i);
			}
		}

		// render all possible entries as "toggle" element
		foreach (var entry in allEntries) {
			var matchingObjectIndex = ArrayUtility.FindIndex(arr, collector => collector.GetType() == entry.Type);
			var wasAdded = matchingObjectIndex >= 0;
			var originalObj = wasAdded ? arr[matchingObjectIndex] : null;
			var obj = ToggleAddRemoveScriptableObject(entry.Name, originalObj, entry.Type);
			var isAdded = obj != null;
			if (isAdded != wasAdded) {
				if (!isAdded) {
					// remove
					ArrayUtility.Remove(ref arr, originalObj);
				}
				else {
					// add
					ArrayUtility.Add(ref arr, obj);
				}
				serializedObject.Update();
			}

			if (obj != null) {
				// render object inspector
				InspectUnityObject(obj);
			}
		}
	}
	
	void InspectUnityObject(Object obj) {
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
	#endregion

	#region Object Management
	T AddNewObjectToAsset<T>() 
		where T : ScriptableObject
	{
		return (T)AddNewObjectToAsset(typeof(T));
	}
	
	ScriptableObject AddNewObjectToAsset(System.Type type) {
		var newObj = ScriptableObject.CreateInstance(type);
		//newObj.hideFlags = HideFlags.HideInHierarchy;
		newObj.name = type.Name;
		AssetDatabase.AddObjectToAsset(newObj, target);
		AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(newObj));
		AssetDatabase.SaveAssets();

		ArrayUtility.Add(ref ((Spell)target).SubAssets, type.Name);

		return newObj;
	}
	
	void DeleteObjectFromAsset(ScriptableObject obj) {
		ArrayUtility.Remove(ref ((Spell)target).SubAssets, obj.name);
		UnityEngine.Object.DestroyImmediate (obj, true);
	}
	#endregion


	#region GUI Components
	T ToggleAddRemoveSerializable<T>(string label, T obj)
		where T : new()
	{
		var wasAdded = !EqualityComparer<T>.Default.Equals(obj, default(T));
		var isObjectAdded = EditorGUILayout.Toggle (label, wasAdded);

		if (isObjectAdded != wasAdded) {
			if (!isObjectAdded) {
				// remove
				obj = default(T);
			}
			else {
				// add
				obj = new T();
			}
			serializedObject.Update();
		}
		return obj;
	}
	
	T ToggleAddRemoveScriptableObject<T>(string label, T obj)
		where T : ScriptableObject
	{
		return ToggleAddRemoveScriptableObject (label, obj, typeof(T));
	}

	T ToggleAddRemoveScriptableObject<T>(string label, T obj, System.Type type) 
		where T : ScriptableObject
	{
		Debug.Assert (type == typeof(T) || type.IsSubclassOf(typeof(T)), "Given type (should, but) is not equal to and does not inherit from T");
		var wasAdded = obj != null;
		var isObjectAdded = EditorGUILayout.Toggle (label, wasAdded);
		if (isObjectAdded != wasAdded) {
			if (!isObjectAdded) {
				// remove
				DeleteObjectFromAsset(obj);
				obj = null;
			}
			else {
				// add
				obj = (T)AddNewObjectToAsset(type);
			}
			serializedObject.Update();
		}
		return obj;
	}
	#endregion
}
