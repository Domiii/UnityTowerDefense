using UnityEngine;
using System.Collections;

namespace Spells {
	public class AuraEffectHandler : IPooledObject {
		public virtual void OnAuraStart (SpellPhaseContext context) {
			for (int i = 0; i < context.Targets.Count; ++i) {
				var target = context.Targets [i];
				OnAuraStart (context, target);
			}
		}
		
		public virtual void Update (SpellPhaseContext context) {
			for (int i = 0; i < context.Targets.Count; ++i) {
				var target = context.Targets [i];
				Update (context, target);
			}
		}
		
		public virtual void Update(SpellPhaseContext context, GameObject target) {
		}
		
		public virtual void OnAuraStart(SpellPhaseContext context, GameObject target) {
		}
		
		public virtual void Pulse (SpellPhaseContext context) {
			for (int i = 0; i < context.Targets.Count; ++i) {
				var target = context.Targets [i];
				Pulse (context, target);
			}
		}
		
		public virtual void Pulse(SpellPhaseContext context, GameObject target) {
		}
		
		public virtual void OnAuraEnd (SpellPhaseContext context) {
			for (int i = 0; i < context.Targets.Count; ++i) {
				var target = context.Targets [i];
				Pulse (context, target);
			}
		}
		
		public virtual void OnAuraEnd(SpellPhaseContext context, GameObject target) {
		}
	}
}