using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Spells {
	public class SpellTargetCollection : ISpellObject {
		List<GameObject> list;
		
		public SpellTargetCollection() {
			list = new List<GameObject> ();
		}
		
		public SpellPhaseContext Context {
			get;
			private set;
		}
		
		public SpellTargetSettings Settings {
			get;
			private set;
		}

		public List<GameObject> List {
			get { return List; }
		}
		
		/// <summary>
		/// </summary>
		/// <returns><c>true</c>, if targets were found, <c>false</c> otherwise.</returns>
		public bool FindTargets(SpellPhaseContext context, SpellTargetSettings settings) {
			Context = context;
			Settings = settings;
			list = new List<GameObject> ();

			// TODO: Collect all matching targets
			
			return true;
		}
	}

}