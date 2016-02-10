using UnityEngine;
using System.Collections;

namespace Spells {
	public class SpellEffector {
		protected SpellTargetCollection _targets;

		public SpellEffector() {
			_targets = new SpellTargetCollection ();
		}

		public void ApplyEffects(SpellEffectCollection effects) {
			_targets.FindTargets (effects.Targets);

			foreach (var effect in effects) {
				ApplyEffect(effect);
			}
		}

		public void ApplyEffect(SpellEffect effect) {
			// TODO: Create handler and apply
		}
	}
}