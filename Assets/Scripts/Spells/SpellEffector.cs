using UnityEngine;
using System.Collections;

namespace Spells {
	public class SpellEffector : MonoBehaviour {
		public SpellTargetCollection Targets;

		public SpellEffector() {
		}

		public void ApplyEffects(SpellEffectCollection effects) {
			Targets.FindTargets (effects.Targets);

			foreach (var effect in effects) {
				ApplyEffect(effect);
			}
		}

		public void ApplyEffect(SpellEffect effect) {
			// TODO: Create handler and apply
		}
	}
}