using UnityEngine;
using System.Collections.Generic;

namespace Spells {
	public class SpellManager : MonoBehaviour {
		public static SpellManager Instance {
			get;
			private set;
		}
		
		public List<SpellEffect> SpellEffects;

		public SpellManager() {
			Instance = this;

			SpellEffects = new List<SpellEffect> ();
		}


	}
}