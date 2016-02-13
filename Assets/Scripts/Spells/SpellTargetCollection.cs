using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Spells {
	public class SpellTargetCollection : ISpellObject {
		List<GameObject> list;
		
		public SpellTargetCollection() {
			list = new List<GameObject> ();
		}
		
		public SpellTargetSettings Settings {
			get;
			private set;
		}
		
		public SpellPhase Phase {
			get;
			private set;
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

		public int FindTargets(SpellTargetSettings settings, SpellPhaseContext phaseContext) {
			// TODO: Collect all matching targets
			return 0;
		}
		
		/// <summary>
		/// </summary>
		/// <returns><c>true</c>, if targets were found, <c>false</c> otherwise.</returns>
		public int FindTargets(SpellTargetSettings settings, SpellPhase phase) {
			Settings = settings;
			Phase = phase;

			// TODO: Collect all matching targets
			
			return list.Count;
		}
		
		public void Clear () {
			list.Clear ();
		}

		void Add(GameObject go) {
			list.Add (go);
		}

		void Remove(GameObject go) {
			list.Remove (go);
		}
	}

}