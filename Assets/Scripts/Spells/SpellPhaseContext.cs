using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace Spells {
	public class SpellPhaseContext : MonoBehaviour, ISpellObject {
		public static SpellPhaseContext CreatePhaseContext(
			SpellPhase spellPhase,
			GameObject phaseObject,
			Vector3 phaseStartPoint,
			SpellTargetCollection targets) {
			
			var phaseContext = SpellGameObjectManager.Instance.AddComponent<SpellPhaseContext>(phaseObject);
			phaseContext.Phase = spellPhase;
			phaseContext.PhaseStartPoint = phaseStartPoint;
			
			if (targets == null) {
				targets = SpellObjectManager.Instance.Obtain<SpellTargetCollection>();
			}
			phaseContext.Targets = targets;
			return phaseContext;
		}
		
		protected bool isPulsing;
		protected bool hasEnded;
		protected int pulseCount;
		protected Dictionary<string, object> spellPhaseData;
		protected internal int index;
		
		public SpellPhaseContext() {
			Targets = new SpellTargetCollection ();
			isPulsing = false;
			hasEnded = true;
		}
		
		public SpellPhase Phase {
			get;
			private set;
		}
		
		public SpellTargetCollection Targets {
			get;
			private set;
		}
		
		public Vector3 PhaseStartPoint {
			get;
			private set;
		}
		
		public Dictionary<string, object> SpellPhaseData {
			get {
				if (spellPhaseData == null) {
					spellPhaseData = new Dictionary<string, object> ();
				}
				return spellPhaseData;
			}
		}
		
		/// <summary>
		/// Whether this PhaseContext has already reached the end of its lifetime.
		/// </summary>
		public bool HasEnded {
			get {
				return hasEnded;
			}
		}
		
		/// <summary>
		/// Whether this PhaseContext is actively pulsing (RepeatEffects)
		/// </summary>
		public bool IsPulsing {
			get {
				return isPulsing;
			}
		}
		
		public int PulseCount {
			get {
				return pulseCount;
			}
		}
		
		public void StartPhase() {
			hasEnded = false;
			pulseCount = 0;

			// apply StartEffects
			if (Phase.Template.StartEffects != null) {
				ApplySpellEffects (Phase.Template.StartEffects);
			}

			// start AuraEffects
			if (Phase.Template.AuraEffects != null) {
				foreach (var effect in Phase.Template.AuraEffects) {
					effect.OnAuraStart(this);
				}
			}

			// start pulsing
			if (Phase.Template.RepeatEffects != null) {
				isPulsing = true;
				StartCoroutine (KeepPulsing ());
			}
		}
		
		public void EndPhase() {
			if (Phase.Template.EndEffects != null) {
				ApplySpellEffects (Phase.Template.EndEffects);
			}
			isPulsing = false;
			hasEnded = true;
		}
		
		
		IEnumerator KeepPulsing() {
			while (true) {
				yield return new WaitForSeconds(Phase.Template.RepeatDelay);
				if (!isPulsing) {
					break;
				}
				
				Pulse ();
				++pulseCount;
				
				if (Phase.Template.MaxRepetitions > 0 && pulseCount >= Phase.Template.MaxRepetitions) {
					isPulsing = false;
				}
			}
		}
		
		void Pulse() {
			ApplySpellEffects (Phase.Template.RepeatEffects);
		}
		
		void ApplySpellEffects(SpellEffectCollection effects) {
			Targets.FindTargets (this, effects.TargetSettings);
			foreach (var effect in effects) {
				effect.Apply (this);
			}
		}
	}
}