using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Spells {
	public class SpellEffectCollection : ScriptableObject, IEnumerable<SpellEffect> {
		public SpellEffect[] Effects = new SpellEffect[0];
		public SpellTargetSettings TargetSettings = new SpellTargetSettings();

		public IEnumerator<SpellEffect> GetEnumerator() {
			return Effects.AsEnumerable().GetEnumerator();
		}
		
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
			return Effects.GetEnumerator();
		}
	}

	public class RepeatSpellEffectCollection : SpellEffectCollection {
		public float RepeatDelay;
		public float MaxRepetitions;
	}

}