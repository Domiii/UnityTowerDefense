using UnityEngine;
using UnityEditor;
using System.Collections;

public class BehaviorScriptEditor : Editor {
	public static BehaviorScriptManager Scripts;

	[UnityEditor.Callbacks.DidReloadScripts]
	public static void ReloadScripts() {
		Scripts = BehaviorScriptManager.FindAllBehaviorScripts ();
	}
}
