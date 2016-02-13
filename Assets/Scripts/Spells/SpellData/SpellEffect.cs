using UnityEngine;
using System.Collections;

namespace Spells {
	/// <summary>
	/// Definition of a single spell effect.
	/// E.g. Damage, Heal, Summon, CastSpell, ApplyAura, RemoveAura
	/// </summary>
	[System.Serializable]
	public class SpellEffect : ScriptableObject {
		public virtual void Apply (SpellPhaseContext context) {
			for (int i = 0; i < context.Targets.Count; ++i) {
				var target = context.Targets [i];
				Apply (context, target);
			}
		}
		
		public virtual void Apply(SpellPhaseContext context, GameObject target) {
		}
	}

}