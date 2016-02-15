using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace Spells {
	public class SpellPhaseContext : MonoBehaviour, IPooledObject {
		public static SpellPhaseContext CreatePhaseContext(
			GameObject carrierObject,
			SpellPhase spellPhase,
			GameObject contextOwner,
			bool isTemporaryPhaseObject) {
			
			var phaseContext = GameObjectManager.Instance.AddComponent<SpellPhaseContext>(carrierObject);
			phaseContext.Phase = spellPhase;
			phaseContext.ContextOwner = contextOwner;
			phaseContext.IsTemporaryPhaseObject = isTemporaryPhaseObject;
			return phaseContext;
		}

		protected bool isPulsing;
		protected bool isActive;
		protected int pulseCount;
		
		public SpellPhaseContext() {
			isPulsing = false;
			isActive = false;
			Targets = ObjectManager.Instance.Obtain<SpellTargetCollection> ();
		}
		
		public SpellPhase Phase {
			get;
			private set;
		}

		public GameObject ContextOwner {
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
				return isActive;
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

		public int Index {
			get;
			internal set;
		}

		void Update() {
			if (ContextOwner != null && gameObject != ContextOwner) {
				// if not context owner, glue to context owner
				transform.position = ContextOwner.transform.position;
			}
		}

		#region Phase Events
		/// <summary>
		/// Called by SpellPhase.NotifyStart
		/// </summary>
		internal void NotifyStart() {
			isActive = true;
			pulseCount = 0;
			StartTime = Time.time;

			// initialize target collection
			Targets.InitializeTargets(ContextOwner, Phase.SpellCastContext);

			// apply aura
			if (Phase.Template.AuraTemplate != null && ContextOwner != null) {
				Aura.AddAura(ContextOwner.gameObject, Phase.Template.AuraTemplate, 0);
			}

			// apply StartEffects
			ApplySpellEffects (Phase.Template.StartEffects);

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
			ApplySpellEffects (Phase.Template.EndEffects);
			
			CleanUp ();
			
			isPulsing = false;
			isActive = false;
		}
		#endregion
		
		void ApplySpellEffects(SpellEffectCollection effects) {
			if (effects != null && ContextOwner != null && ContextOwner.activeInHierarchy) {
				Targets.FindTargets (effects.TargetSettings);
				foreach (var effect in effects) {
					effect.Apply (this);
				}
			}
		}

		#region Pulsing
		IEnumerator KeepPulsing() {
			var repeatEffects = Phase.Template.RepeatEffects;
			while (true) {
				yield return new WaitForSeconds(repeatEffects.RepeatDelay);
				if (!isPulsing) {
					break;
				}
				
				Pulse ();
				++pulseCount;
				
				if (repeatEffects.MaxRepetitions > 0 && pulseCount >= repeatEffects.MaxRepetitions) {
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
			if (IsTemporaryPhaseObject) {
				// this is only a temp object created to carry this Context -> Destroy
				GameObjectManager.Instance.Recycle (gameObject);
			}
			else {
				// remove self
				GameObjectManager.Instance.RemoveComponent (this);
			}
		}
		#endregion
	}
}