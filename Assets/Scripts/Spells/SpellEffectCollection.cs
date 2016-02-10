using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Spells {
	[System.Serializable]
	public class SpellEffectCollection : IEnumerable<SpellEffect> {
		public SpellEffect[] Effects;
		public SpellTargetConfig Targets;

		public IEnumerator<SpellEffect> GetEnumerator() {
			return Effects.AsEnumerable().GetEnumerator();
		}
		
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
			return Effects.GetEnumerator();
		}
	}

}