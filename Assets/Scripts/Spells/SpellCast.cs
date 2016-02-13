using UnityEngine;

using System;
using System.Collections.Generic;

namespace Spells {
	
	// TODO: Centralize life-time checks on impact and projectile objects
		// TODO: Call OnProjectileImpact()
		// TODO: Add event to check if context has been destroyed or deactivated (call OnProjectileDestroyed, and...?)
		// TODO: Run and update Impact phase timers
	// TODO: AreaAura (https://github.com/WCell/WCell/blob/master/Services/WCell.RealmServer/Spells/Auras/AreaAura.cs)
	//			(" An AreaAura either applies AreaAura-Effects to everyone in a radius around its center or repeatedly triggers a spell on everyone around it.")

	public class SpellCast : MonoBehaviour, ISpellObject {

		public SpellCast() {
			SpellCastContext = SpellObjectManager.Instance.Obtain<SpellCastContext> ();
			Targets = SpellObjectManager.Instance.Obtain<SpellTargetCollection> ();
		}
		
		#region Properties
		public SpellCastContext SpellCastContext {
			get;
			private set;
		}
		
		public CastPhase CastPhase {
			get;
			private set;
		}
		
		public ProjectilePhase ProjectilePhase {
			get;
			private set;
		}

		public ImpactPhase ImpactPhase {
			get;
			private set;
		}
		
		public float StartTime {
			get;
			private set;
		}

		public float TimeSinceStart {
			get {
				return Time.time - StartTime;
			}
		}

		public float CastTimeLeft {
			get {
				if (CastPhase == null) {
					return 0;
				}
				var castTime = CastPhase.Template.Duration;
				return castTime - TimeSinceStart;
			}
		}

		public bool IsCasting {
			get { return CastPhase != null && CastPhase.IsActive; }
		}

		public SpellTargetCollection Targets {
			get;
			private set;
		}
		#endregion


		#region Public Methods
		public bool StartCasting(GameObject caster, Spell spell, Transform InitialTarget, ref Vector3 initialTargetPosition) {
			SpellCastContext.Caster = caster;
			SpellCastContext.Spell = spell;
			SpellCastContext.InitialTarget = InitialTarget;
			SpellCastContext.InitialTargetPosition = initialTargetPosition;

			StartTime = Time.time;

			StartCastPhase ();

			return true;
		}

		/// <summary>
		/// Interrupt spell cast if still casting
		/// </summary>
		public void Interrupt() {
			if (IsCasting) {
				// TODO: Show some "fizzle" visuals?

				// clean up
				CleanUp();
			}
		}
		#endregion


		#region Update
		void Update() {
			if (IsCasting) {
				UpdateCastTimer();
			}
			else {
				// check life-time of all projectiles and impact contexts
				var activeContextCount = 0;

				// update all projectiles
				UpdateContextStatus(ProjectilePhase, ref activeContextCount);

				// update all impact objects
				UpdateContextStatus(ImpactPhase, ref activeContextCount);

				if (activeContextCount == 0) {
					// we are done!
					CleanUp();
				}
			}
		}

		void UpdateCastTimer() {
			if (CastPhase == null || CastPhase.Context == null) {
				// was casting, but CastPhase or context got destroyed -> Something went wrong!
				CleanUp();
			}
			else if (CastTimeLeft <= 0) {
				// finished casting
				EndCastPhase();
			}
		}
		
		void UpdateContextStatus(MultiContextPhase phase, ref int activeContextCount) {
			if (phase == null || !phase.IsActive) {
				return;
			}

			for (var i = 0; i < phase.ContextCount; ++i) {
				var isActive = phase.ContextActiveStatuses[i];
				if (isActive) {
					var context = phase.Contexts[i];
					UpdateContextStatus(phase, i, context, ref activeContextCount);
				}
			}
		}
		
		void UpdateContextStatus(MultiContextPhase phase, int index, SpellPhaseContext context, ref int activeContextCount) {
			if (context == null) {
				// context is active but has been removed
				phase.NotifyEnd (index);
			}
			else {
				// TODO: Update context timer. When time has run out -> Next step or remove?
				// TODO: Keep moving projectile forward (check for Ridigdbody and ProjectileTrajectory components)
				// TODO: Check if projectile impacted

				// TODO: Call phase.NotifyEnd (index); when done
			}
		}
		#endregion

		
		#region Cast Phase
		void StartCastPhase() {
			var phaseTemplate = SpellCastContext.Spell.CastPhase;
			if (phaseTemplate != null) {
				// yes CastPhase
				CastPhase = SpellObjectManager.Instance.Obtain<CastPhase>();
				var context = CreatePhaseContext (CastPhase, null);
				CastPhase.NotifyStart(context);
			}
			else {
				// no CastPhase
				StartProjectilePhase ();
			}
		}

		void EndCastPhase() {
			CastPhase.NotifyEnd();
			StartProjectilePhase ();
		}
		#endregion


		#region Projectile + Impact Phases
		/// <summary>
		/// Find all spell targets, then either send out projectiles to or impact on each of them.
		/// </summary>
		void StartCriticalPhase(SpellPhase phase) {
			Targets.FindTargets (SpellCastContext.Spell.Targets, phase);
			
			for (var i = 0; i < Targets.Count; ++i) {
				var target = Targets[i];
				
				// TODO: Create Projectile context on caster, send toward target
				// TODO: Create Impact context on target
				
				// create one context per target
				var context = CreatePhaseContext (phase, CastPhase != null ? CastPhase.Context : null);
				phase.NotifyStart(context);
			}
		}

		void StartProjectilePhase() {
			var phaseTemplate = SpellCastContext.Spell.ProjectilePhase;
			if (phaseTemplate != null) {
				if (SpellCastContext.Spell.ProjectilePhase.PhaseObjectPrefab == null) {
					// no valid ProjectilePhase
					Debug.LogError ("Spell's ProjectilePhase is missing PhaseObjectPrefab", SpellCastContext.Spell);
					ImpactWithoutProjectile ();
					return;
				}
				
				// yes ProjectilePhase
				ProjectilePhase = SpellObjectManager.Instance.Obtain<ProjectilePhase>();
				ImpactPhase = SpellObjectManager.Instance.Obtain<ImpactPhase> ();
				StartCriticalPhase(ProjectilePhase);
			}
			else {
				// no ProjectilePhase
				ImpactWithoutProjectile ();
			}
		}
		
		/// <summary>
		/// Impact on target without a projectile phase
		/// </summary>
		void ImpactWithoutProjectile() {
			ImpactPhase = SpellObjectManager.Instance.Obtain<ImpactPhase> ();
			StartCriticalPhase (ImpactPhase);
		}

		void OnProjectileImpact(SpellPhaseContext projectileContext) {
			var phaseTemplate = SpellCastContext.Spell.ImpactPhase;
			if (phaseTemplate != null) {
				var context = CreatePhaseContext (ImpactPhase, projectileContext);
				ImpactPhase.NotifyStart (context);
			}
			else {
				// there is always an impact phase. But it could be empty.
				Debug.LogError("Invalid spell is missing ImpactPhase", SpellCastContext.Spell);
			}
		}
		#endregion


		#region Clean Up + Finalization
		void CleanUp() {
			
			// TODO: Destroy all existing contexts and temporary spell objects
			SpellGameObjectManager.Instance.RemoveComponent (this);

		}

		void OnDisable() {
			// no such thing!
			CleanUp ();
		}
		
		void OnDestroy() {
			CleanUp ();
		}
		#endregion


		#region Helper Methods
		SpellPhaseContext CreatePhaseContext(SpellPhase phase, SpellPhaseContext sourcePhaseContext) {
			var prefab = phase.Template.PhaseObjectPrefab;
			GameObject phaseObject;
			bool isTemporaryPhaseObject = prefab != null;

			var sourcePhaseTransform = sourcePhaseContext != null ? sourcePhaseContext.transform : gameObject.transform;
			if (isTemporaryPhaseObject) {
				phaseObject = SpellGameObjectManager.Instance.Obtain(prefab, sourcePhaseTransform.position, sourcePhaseTransform.rotation);
			}
			else {
				phaseObject = gameObject;
			}

			return SpellPhaseContext.CreatePhaseContext (
				phase,
				phaseObject,
				isTemporaryPhaseObject);
		}
		#endregion

	}

}