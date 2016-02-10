using UnityEngine;
using System.Collections;

namespace Spells {
	[System.Serializable]
	public class DamageEffect : SpellEffect {
		public int MinDamage = 10;
		public int MaxDamage = 10;
	}

	public class DamageEffectHandler : SpellEffectHandler {
		public override void Apply(Unit target) {
			var effect = GetEffect<DamageEffect> ();

			// TODO: Apply damage to target
			//target.Damage ();
		}
	}
}