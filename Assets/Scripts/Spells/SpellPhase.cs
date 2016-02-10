using UnityEngine;
using System.Collections;

namespace Spells {
	public class SpellPhase : SpellEffector {
		public SpellPhaseTemplate Template;

		internal bool running;
		int nPulses;

		public SpellPhase() {
		}

		protected virtual void Start() {
			running = true;
			nPulses = 0;
			ApplyEffects (Template.StartEffects);

			StartCoroutine (KeepPulsing());
		}

		IEnumerator KeepPulsing() {
			while (true) {
				yield return new WaitForSeconds(Template.RepeatDelay);
				if (!running) {
					break;
				}
				
				Pulse ();
				++nPulses;
				
				if (Template.MaxRepetitions > 0 && nPulses >= Template.MaxRepetitions) {
					running = false;
				}
			}
		}

		public void Pulse() {
			ApplyEffects (Template.RepeatEffects);
		}

		public void End() {
			ApplyEffects (Template.EndEffects);
		}
	}

}