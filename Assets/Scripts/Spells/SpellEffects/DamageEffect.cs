using UnityEngine;
using System.Collections;

namespace Spells {
	[System.Serializable]
	public class DamageEffect : SpellEffect {
		public int MinDamage = 10;
		public int MaxDamage = 10;

		public override void Apply(SpellPhaseContext context, GameObject target) {
			var unit = target.GetComponent<Unit> ();
			if (unit != null) {
				// TODO: Apply damage to target
				//target.Damage ();
			}
		}
	}
}