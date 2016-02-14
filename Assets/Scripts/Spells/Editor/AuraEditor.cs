using UnityEngine;
using UnityEngine.UI;

using UnityEditor;

using System.Collections.Generic;
using System.Reflection;

using Spells;
using System;

[CustomEditor(typeof(Aura))]
public class AuraEditor : Editor {
	public override void OnInspectorGUI () {
		base.OnInspectorGUI ();

	}
}
