using UnityEngine;
using UnityEngine.UI;

using UnityEditor;
using UnityEditorInternal;

using System.Collections.Generic;
using System.Reflection;
using System.Linq;

using Spells;


[CustomEditor(typeof(Spell))]
public class SpellEditor : Editor {
	ReorderableListManager<SpellEffect> spellEffectLists;


	void OnEnable() {
		//var effects = CustomScriptableObjectManagerEditor.Scripts.GetEntries<SpellEffect>();
		spellEffectLists = new ReorderableListManager<SpellEffect> (target);
	}

	public override void OnInspectorGUI () {
		var spell = (Spell)target;
		serializedObject.Update();

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

		//if (GUI.changed) {
			// write values back
			serializedObject.ApplyModifiedProperties ();
			EditorUtility.SetDirty(target);
		//}

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
		spell.CastPhase = ScriptableObjectEditorUtil.ToggleAddRemoveScriptableObject ("Cast Phase", target, spell.CastPhase);
		if (spell.CastPhase != null) {
			InspectSpellPhaseTemplate(spell.CastPhase);
		}
	}
	
	void InspectProjectilePhaseTemplate() {
		var spell = (Spell)target;
		spell.ProjectilePhase = ScriptableObjectEditorUtil.ToggleAddRemoveScriptableObject ("Projectile Phase", target, spell.ProjectilePhase);
		if (spell.ProjectilePhase != null) {
			InspectSpellPhaseTemplate(spell.ProjectilePhase);
		}
	}
	
	void InspectImpactPhaseTemplate() {
		var spell = (Spell)target;
		spell.ImpactPhase = ScriptableObjectEditorUtil.ToggleAddRemoveScriptableObject ("Impact Phase", target, spell.ImpactPhase);
		if (spell.ImpactPhase != null) {
			InspectSpellPhaseTemplate(spell.ImpactPhase);
		}
	}

	void InspectSpellPhaseTemplate(SpellPhaseTemplate template) {
		//InspectUnityObject(phaseTemplate);
		++EditorGUI.indentLevel;
		template.Duration = EditorGUILayout.FloatField ("Duration", template.Duration);
		template.PhaseObjectPrefab = (GameObject)EditorGUILayout.ObjectField ("Phase Prefab", template.PhaseObjectPrefab, typeof(GameObject), false);

		InspectSpellEffects ("Start Effects", ref template.StartEffects);
		InspectRepeatSpellEffects ("Repeat Effects", ref template.RepeatEffects);
		InspectSpellEffects ("End Effects", ref template.EndEffects);
		InspectAuraTemplate ("Aura", ref template.AuraTemplate);
		--EditorGUI.indentLevel;
	}
	
	void InspectSpellEffects(string label, ref SpellEffectCollection spellEffects) {
		spellEffects = ScriptableObjectEditorUtil.ToggleAddRemoveScriptableObject(label, target, spellEffects);
		if (spellEffects != null) {
			++EditorGUI.indentLevel;
			InspectSpellEffects(spellEffects);
			--EditorGUI.indentLevel;
		}
	}
	
	void InspectRepeatSpellEffects(string label, ref RepeatSpellEffectCollection spellEffects) {
		spellEffects = ScriptableObjectEditorUtil.ToggleAddRemoveScriptableObject (label, target, spellEffects);
		if (spellEffects != null) {
			++EditorGUI.indentLevel;
			spellEffects.RepeatDelay = EditorGUILayout.FloatField ("Repeat Delay", spellEffects.RepeatDelay);
			spellEffects.MaxRepetitions = EditorGUILayout.FloatField ("MaxRepetitions", spellEffects.MaxRepetitions);
			InspectSpellEffects(spellEffects);
			--EditorGUI.indentLevel;
		}
	}
	
	void InspectAuraTemplate(string label, ref AuraTemplate template) {
		template = ScriptableObjectEditorUtil.ToggleAddRemoveScriptableObject(label, target, template);
		if (template != null) {
			++EditorGUI.indentLevel;
			// TODO
			//InspectSpellEffects(spellEffects);
			--EditorGUI.indentLevel;
		}
	}
	
	void InspectSpellEffects(SpellEffectCollection collection) {
		//InspectArrayWithInheritance(ref arr);

		// for some reason we cannot find the children of the original SO, so we create a new one here, and it works magically
		var so = new SerializedObject (collection);
		var prop = so.FindProperty ("Effects");
		
		var arr = collection.Effects;

		var list = spellEffectLists.GetOrCreate (null, so, prop, arr);
		list.Render ();
	}
	
	void InspectSpellTargetSettings(SpellTargetSettings targets) {
		EditorGUILayout.LabelField ("Targets");
		
		++EditorGUI.indentLevel;
		targets.Range = EditorGUILayout.FloatField ("MaxRange", targets.Range);
		
		InspectArrayWithInheritanceMutuallyExclusiveTypes (ref targets.TargetCollectors);
		InspectArrayWithInheritanceMutuallyExclusiveTypes (ref targets.TargetFilters);
		
		--EditorGUI.indentLevel;
	}
	#endregion

	#region Generic Inspectors
	void InspectArrayWithInheritance<A>(ref A[] arr) 
		where A : ScriptableObject
	{
		//var allEntries = CustomScriptableObjectManagerEditor.Scripts.GetEntries<A>();
		
		// remove empty entries
		for (var i = arr.Length-1; i >= 0; --i) {
			if (arr[i] == null) {
				ArrayUtility.RemoveAt(ref arr, i);
			}
		}

		for (var i = arr.Length-1; i >= 0; --i) {
			var obj = arr[i];

			if (obj != null) {
				// render object inspector
				InspectUnityObject(obj);
			}
		}


	}

	void InspectArrayWithInheritanceMutuallyExclusiveTypes<A>(ref A[] arr) 
		where A : ScriptableObject
	{
		var allEntries = ScriptableObjectEditorUtil.Scripts.GetEntries<A>();
		
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
			var obj = ScriptableObjectEditorUtil.ToggleAddRemoveScriptableObject(entry.Name, target, originalObj, entry.Type);
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
			}
			
			if (obj != null) {
				// render object inspector
				InspectUnityObject(obj);
			}
		}
	}
	
	public static void InspectUnityObject(Object obj) {
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
		}
		return obj;
	}
	#endregion
}
