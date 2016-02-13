using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Spells {
	[System.Serializable]
	public class SpellEffectCollection : ScriptableObject, IEnumerable<SpellEffect> {
		public SpellEffect[] Effects;
		public SpellTargetSettings TargetSettings;

		public IEnumerator<SpellEffect> GetEnumerator() {
			return Effects.AsEnumerable().GetEnumerator();
		}
		
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
			return Effects.GetEnumerator();
		}
	}

}