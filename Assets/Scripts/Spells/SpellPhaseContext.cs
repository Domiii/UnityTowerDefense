using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace Spells {
	public class SpellPhaseContext : MonoBehaviour, ISpellObject {
		public static SpellPhaseContext CreatePhaseContext(
			SpellPhase spellPhase,
			GameObject phaseObject,
			bool isTemporaryPhaseObject) {
			
			var phaseContext = SpellGameObjectManager.Instance.AddComponent<SpellPhaseContext>(phaseObject);
			phaseContext.Phase = spellPhase;
			phaseContext.IsTemporaryPhaseObject = isTemporaryPhaseObject;
			return phaseContext;
		}

		protected bool isPulsing;
		protected bool hasEnded;
		protected int pulseCount;
		protected Dictionary<string, object> spellPhaseData;
		protected internal int index;
		protected Aura aura;
		
		public SpellPhaseContext() {
			isPulsing = false;
			hasEnded = true;
			Targets = SpellObjectManager.Instance.Obtain<SpellTargetCollection> ();
		}
		
		public SpellPhase Phase {
			get;
			private set;
		}
		
		public SpellTargetCollection Targets {
			get;
			private set;
		}
		
		public bool IsTemporaryPhaseObject {
			get;
			private set;
		}
		
		/// <summary>
		/// Whether this PhaseContext has already reached the end of its lifetime.
		/// </summary>
		public bool IsActive {
			get {
				return !hasEnded;
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

		public float StartTime {
			get;
			private set;
		}

		public float TimeLeft {
			get {
				return StartTime + Phase.Template.Duration - Time.time;
			}
		}
		
		public Dictionary<string, object> SpellPhaseData {
			get {
				if (spellPhaseData == null) {
					spellPhaseData = new Dictionary<string, object> ();
				}
				return spellPhaseData;
			}
		}

		#region Phase Events
		/// <summary>
		/// Called by SpellPhase.NotifyStart
		/// </summary>
		internal void NotifyStart() {
			hasEnded = false;
			pulseCount = 0;
			StartTime = Time.time;

			// apply aura
			if (Phase.Template.AuraTemplate != null) {
				aura = Aura.AddAura(gameObject, Phase.Template.AuraTemplate, 0);
			}

			// apply StartEffects
			if (Phase.Template.StartEffects != null) {
				ApplySpellEffects (Phase.Template.StartEffects);
			}

			// start pulsing
			if (Phase.Template.RepeatEffects != null) {
				isPulsing = true;
				StartCoroutine (KeepPulsing ());
			}
		}
		/// <summary>
		/// Is called by SpellPhase.NotifyEnd
		/// </summary>
		internal void NotifyEnd() {
			if (Phase.Template.EndEffects != null) {
				ApplySpellEffects (Phase.Template.EndEffects);
			}
			
			CleanUp ();
			
			isPulsing = false;
			hasEnded = true;
		}
		#endregion
		
		void ApplySpellEffects(SpellEffectCollection effects) {
			Targets.FindTargets (effects.TargetSettings, this);
			foreach (var effect in effects) {
				effect.Apply (this);
			}
		}

		#region Pulsing
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
		#endregion

		#region Cleanup + Finalization
		public void CleanUp() {
			if (aura != null) {
				aura.Remove();
				aura = null;
			}
			
			// remove self
			SpellGameObjectManager.Instance.RemoveComponent (this);
			
			if (IsTemporaryPhaseObject) {
				// this is only a temp object -> Destroy
				SpellGameObjectManager.Instance.Recycle(gameObject);
			}
		}
		#endregion
	}
}