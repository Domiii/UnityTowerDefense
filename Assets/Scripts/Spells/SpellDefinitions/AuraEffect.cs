using UnityEngine;
using System.Collections;

namespace Spells {
	/// <summary>
	/// Definition of a single aura effect.
	/// E.g. Stun, Slow, Fast, Shield
	/// </summary>
	[System.Serializable]
	public class AuraEffect {
		public virtual void OnAuraStart (SpellPhaseContext context) {
			for (int i = 0; i < context.Targets.List.Count; ++i) {
				var target = context.Targets.List [i];
				OnAuraStart (context, target);
			}
		}
		
		public virtual void Update (SpellPhaseContext context) {
			for (int i = 0; i < context.Targets.List.Count; ++i) {
				var target = context.Targets.List [i];
				Update (context, target);
			}
		}

		public virtual void Update(SpellPhaseContext context, GameObject target) {
		}
		
		public virtual void OnAuraStart(SpellPhaseContext context, GameObject target) {
		}
		
		public virtual void Pulse (SpellPhaseContext context) {
			for (int i = 0; i < context.Targets.List.Count; ++i) {
				var target = context.Targets.List [i];
				Pulse (context, target);
			}
		}
		
		public virtual void Pulse(SpellPhaseContext context, GameObject target) {
		}
		
		public virtual void OnAuraEnd (SpellPhaseContext context) {
			for (int i = 0; i < context.Targets.List.Count; ++i) {
				var target = context.Targets.List [i];
				Pulse (context, target);
			}
		}
		
		public virtual void OnAuraEnd(SpellPhaseContext context, GameObject target) {
		}
	}
}