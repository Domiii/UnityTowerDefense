using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Spells {
	public class SpellTargetSelectorAttribute : System.Attribute {
		public string Name;

		public SpellTargetSelectorAttribute(string name = "") {
			Name = name;
		}
	}

	[System.Serializable]
	public abstract class SpellTargetSelector : ScriptableObject {
		public string Name;
		
		public abstract bool HasObjectTargets {
			get;
		}
		
		public abstract bool HasPositionTarget {
			get;
		}
		
		public abstract void FindTargets(SpellTargetCollection targets);
	}

	public class SpellTargetCollection : ISpellObject {
		List<GameObject> list;
		
		public SpellTargetCollection() {
			list = new List<GameObject> ();
		}
		
		public SpellTargetSettings Settings {
			get;
			private set;
		}
		
		public SpellCastContext SpellCastContext {
			get;
			private set;
		}
		
		public GameObject ContextOwner {
			get;
			private set;
		}

		public Vector3? TargetPosition {
			get;
			set;
		}
		
		public int Count
		{
			get { return list.Count; }
		}
		
		public GameObject this [int index] {
			get {
				return list[index];
			}
			set {
				list[index] = value;
			}
		}

		public void FindTargets(SpellTargetSettings settings, SpellPhaseContext phaseContext) {
			FindTargets(phaseContext.ContextOwner, phaseContext.Phase.SpellCastContext, settings);
		}
		
		/// <summary>
		/// </summary>
		/// <returns><c>true</c>, if targets were found, <c>false</c> otherwise.</returns>
		public void FindTargets(GameObject contextOwner, SpellCastContext spellCastContext, SpellTargetSettings settings) {
			if (contextOwner == null) {
				return;
			}

			ContextOwner = contextOwner;
			Settings = settings;
			SpellCastContext = spellCastContext;

			for (int i = 0; i < Settings.TargetSelectors.Length; i++) {
				var selector = Settings.TargetSelectors[i];
				selector.FindTargets(this);
			}
		}
		
		public void Clear () {
			// TODO: Don't de-allocate
			list.Clear ();
			TargetPosition = null;
		}

		public void CleanUp() {
			Clear();
		}

		public void AddTarget(GameObject go) {
			if (!list.Contains (go)) {
				list.Add (go);
			}
		}

		public void Remove(GameObject go) {
			// TODO: Don't de-allocate
			list.Remove (go);
		}
	}

}