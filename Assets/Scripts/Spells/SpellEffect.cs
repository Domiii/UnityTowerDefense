using UnityEngine;
using System.Collections;

namespace Spells {
	/// <summary>
	/// Definition of a single spell effect.
	/// E.g. Damage, Heal, Stun, ChangeSpeed, Shield, Summon, ApplyAura, CastSpell.
	/// </summary>
	[System.Serializable]
	public class SpellEffect {
	}


	public class SpellEffectHandler {
		protected GameObject _caster;
		protected SpellEffect _effect;
		protected SpellTargetCollection _targets;

		public void InitHandler(GameObject caster, SpellEffect effect, SpellTargetCollection targets) {
			_caster = caster;
			_effect = effect;
			_targets = targets;
		}

		public E GetEffect<E>()
			where E : SpellEffect
		{
			return (E)_effect;
		}

		public virtual void Apply () {
			// TODO: Apply to all targets
		}

		public virtual void Apply(Unit unit) {
		}
	}

}