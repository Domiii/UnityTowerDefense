using UnityEngine;
using UnityEngine.UI;

using UnityEditor;

using System.Collections.Generic;
using System.Reflection;
using System.Linq;

using Spells;

[CustomEditor(typeof(Spell))]
public class SpellEditor : Editor {
	SerializedObject serializedSpell;

	void OnEnable() {
		serializedSpell = new SerializedObject (target);
	}

	public override void OnInspectorGUI () {
		var spell = (Spell)target;
		spell.Cooldown = EditorGUILayout.FloatField("Cooldown", spell.Cooldown);
		
		CustomGUIUtils.DrawSeparator ();

		InspectSpellTargetSettings (spell.Targets);
		
		CustomGUIUtils.DrawSeparator ();

		if (GUI.changed) {
			// write values back
			serializedObject.ApplyModifiedProperties ();
		}
	}

	// TODO: " The serialization system also does not support inheritance / polymorphism. 
	//  When such a class is deserialized, Unity creates an instance of the variable type. It doesn't know what type of class was referenced when it get serialized." 
	// 		(see http://answers.unity3d.com/questions/735534/serializedproperty-and-abstract-custom-classes.html)

	void InspectSpellTargetSettings(SpellTargetSettings targets) {
		EditorGUILayout.LabelField("Targets", EditorStyles.boldLabel);
		CustomGUIUtils.DrawSeparator ();

		targets.Range = EditorGUILayout.FloatField("MaxRange", targets.Range);

		var collectorScripts = BehaviorScriptEditor.Scripts.GetEntries<SpellTargetCollector>();
		var collectorsProperty = serializedSpell.FindProperty("Targets");
		Debug.Log(collectorsProperty.type);
		//var collectorsProperty = serializedSpell.FindProperty("Targets.TargetCollectors");
//		for (int i = 0; i < collectorsProperty.arraySize; i++) {
//			SerializedProperty elementProperty = collectorsProperty.GetArrayElementAtIndex (i);
//			Debug.Log(elementProperty.type);
//		}
//		foreach (var script in collectorScripts) {
//			var hadCollector = targets.TargetCollectors.Any(collector => collector.GetType() == script.Type);
//			var hasCollector = EditorGUILayout.Toggle (script.Name, hadCollector);
//			if (hasCollector != hadCollector) {
//				if (!hasCollector) {
//					// remove
//					targets.TargetCollectors = targets.TargetCollectors.Where(collector => collector.GetType() == script.Type).ToArray();
//				}
//				else {
//					// add
//					var newCollector = (SpellTargetCollector)script.Type.GetConstructor(new System.Type[0]).Invoke(new System.Object[0]);
//					targets.TargetCollectors = targets.TargetCollectors.Union(new SpellTargetCollector[] {newCollector}).ToArray();
//				}
//			}
//
//			if (hasCollector) {
//				// draw collector data
//
////					SerializedProperty collectorProperty = collectorsProperty.GetArrayElementAtIndex(i);
////					EditorGUILayout.PropertyField(collectorProperty, "Collectors", true);
//			}
//		}

		//targets.TargetCollectors

	}
}
