using UnityEngine;

using System;
using System.Linq;
using System.Collections.Generic;

namespace Spells {
	
	// TODO: Centralize life-time checks on impact and projectile objects
		// TODO: Call OnProjectileImpact()
		// TODO: Add event to check if context has been destroyed or deactivated (call OnProjectileDestroyed, and...?)
		// TODO: Run and update Impact phase timers
	// TODO: AreaAura (https://github.com/WCell/WCell/blob/master/Services/WCell.RealmServer/Spells/Auras/AreaAura.cs)
	//			(" An AreaAura either applies AreaAura-Effects to everyone in a radius around its center or repeatedly triggers a spell on everyone around it.")
	public enum SpellCastStatus {
		Casting,
		Failed,
		Active,
		Finished
	}

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

		public SpellCastStatus Status {
			get {
				if (CastPhase != null && CastPhase.IsActive) {
					return SpellCastStatus.Casting;
				}
				if (ProjectilePhase != null || ImpactPhase != null) {
					return SpellCastStatus.Active;
				}
				if (HasFailed) {
					return SpellCastStatus.Failed;
				}
				return SpellCastStatus.Finished;
			}
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

		public bool HasFailed {
			get;
			private set;
		}

		public SpellTargetCollection Targets {
			get;
			private set;
		}
		#endregion


		#region Public Methods
		public bool StartCasting(GameObject caster, Spell spell, GameObject initialTarget, ref Vector3 initialTargetPosition) {
			SpellCastContext.Caster = caster;
			SpellCastContext.Spell = spell;
			SpellCastContext.InitialTarget = initialTarget;
			SpellCastContext.InitialTargetPosition = initialTargetPosition;
			
			HasFailed = false;
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
				Cancel();
			}
		}
		
		/// <summary>
		/// Let spell cast fail and force clean up
		/// </summary>
		public void Cancel() {
			HasFailed = true;
			
			// clean up
			CleanUp();
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
			if (CastPhase.Context == null) {
				// was casting, but context got destroyed -> Cancel spell cast
				Cancel();
			}
			else if (CastTimeLeft <= 0) {
				// finished casting
				EndCastPhase();
			}
			// else: still casting
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
			if (context == null || context.ContextOwner == null) {
				// context was active but carrier object or ContextOwner have been removed
				phase.CleanUp (index);
			}
			else if (context.TimeLeft <= 0) {
				// context expired
				phase.NotifyEnd(index);
			}
			else {
				// context is still active
				++activeContextCount;
			}
		}
		#endregion

		
		#region Cast Phase
		void StartCastPhase() {
			var phaseTemplate = SpellCastContext.Spell.CastPhase;
			if (phaseTemplate != null) {
				// yes CastPhase
				CastPhase = SpellObjectManager.Instance.Obtain<CastPhase>();
				CreatePhaseContext (CastPhase, gameObject, transform.position, transform.rotation);
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
		void StartCriticalPhase(SpellPhase phase, Action<GameObject> goCb, Action<Vector3> posCb) {
			Targets.FindTargets (gameObject, SpellCastContext, SpellCastContext.Spell.Targets);

			if (Targets.TargetPosition != null) {
				posCb(Targets.TargetPosition.Value);
			}

			for (var i = 0; i < Targets.Count; ++i) {
				var target = Targets[i];
				goCb(target);
			}
		}

		void StartProjectilePhase() {
			var phaseTemplate = SpellCastContext.Spell.ProjectilePhase;
			if (phaseTemplate != null) {
				// yes ProjectilePhase
				ProjectilePhase = SpellObjectManager.Instance.Obtain<ProjectilePhase>();
				ImpactPhase = SpellObjectManager.Instance.Obtain<ImpactPhase> ();
				StartCriticalPhase(ProjectilePhase, ShootProjectileAt, ShootProjectileAt);
			}
			else {
				// no ProjectilePhase
				StartImpactPhaseWithoutProjectiles ();
			}
		}
		
		/// <summary>
		/// Impact on target without a projectile phase
		/// </summary>
		void StartImpactPhaseWithoutProjectiles() {
			ImpactPhase = SpellObjectManager.Instance.Obtain<ImpactPhase> ();
			StartCriticalPhase (ImpactPhase, ImpactOnTarget, ImpactOnTarget);
		}
		
		void ShootProjectileAt(GameObject target) {
			var position = transform.position;
			var rotation = Quaternion.LookRotation(target.transform.position - transform.position);
			GameObject contextOwner = null;		// contextOwner == carrier object
			var projectileContext = CreatePhaseContext (ProjectilePhase, contextOwner, position, rotation);
			InitializeProjectile (projectileContext, target, target.transform.position);
		}
		
		void ShootProjectileAt(Vector3 targetPosition) {
			var position = transform.position;
			var rotation = Quaternion.LookRotation(targetPosition - transform.position);
			GameObject contextOwner = null;		// contextOwner == carrier object
			var projectileContext = CreatePhaseContext (ProjectilePhase, contextOwner, position, rotation);
			InitializeProjectile (projectileContext, null, targetPosition);
		}

		void InitializeProjectile(SpellPhaseContext context, GameObject target, Vector3 targetPosition) {
			// TODO: Set projectile target and impact callback
		}

		void ImpactOnTarget(GameObject target) {
			var phaseTemplate = SpellCastContext.Spell.ImpactPhase;
			if (phaseTemplate != null) {
				CreatePhaseContext (ImpactPhase, target, target.transform.position, Quaternion.identity);
			}
			else {
				// nothing to do here...
			}
		}

		void ImpactOnTarget(Vector3 targetPosition) {
			var phaseTemplate = SpellCastContext.Spell.ImpactPhase;
			if (phaseTemplate != null) {
				CreatePhaseContext (ImpactPhase, null, targetPosition, Quaternion.identity);
			}
			else {
				// nothing to do here...
			}
		}
		#endregion


		#region Clean Up + Finalization
		/// <summary>
		/// Destroy all temporary spell objects created by this cast.
		/// </summary>
		public void CleanUp() {
			//SpellGameObjectManager.Instance.RemoveComponent (this);

			if (CastPhase != null) {
				CastPhase.CleanUp();
				SpellObjectManager.Instance.Recycle(CastPhase);
				CastPhase = null;
			}
			if (ProjectilePhase != null) {
				ProjectilePhase.CleanUp();
				SpellObjectManager.Instance.Recycle(ProjectilePhase);
				ProjectilePhase = null;
			}
			if (ImpactPhase != null) {
				ImpactPhase.CleanUp();
				SpellObjectManager.Instance.Recycle(ImpactPhase);
				ImpactPhase = null;
			}

			Targets.CleanUp ();
		}

		void OnDisable() {
			// disable = destroy
			CleanUp ();
		}
		
		void OnDestroy() {
			CleanUp ();
		}
		#endregion


		#region Helper Methods
		//SpellPhaseContext CreatePhaseContext(SpellPhase phase, SpellPhaseContext sourcePhaseContext) {
		SpellPhaseContext CreatePhaseContext(SpellPhase phase, GameObject contextOwner, Vector3 position, Quaternion rotation) {
			var prefab = phase.Template.PhaseObjectPrefab;
			GameObject carrierObject;
			//bool isTemporaryPhaseObject = prefab != null;
			bool isTemporaryPhaseObject = true;

			prefab = prefab ?? SpellManager.Instance.DefaultPhasePrefab;

			if (prefab != null) {
				carrierObject = SpellGameObjectManager.Instance.Obtain(prefab, position, rotation);
			}
			else {
				carrierObject = SpellGameObjectManager.Instance.ObtainEmpty(phase.ToString(), position, rotation);
			}

			var context = SpellPhaseContext.CreatePhaseContext (
				carrierObject,
				phase,
				contextOwner ?? carrierObject,
				isTemporaryPhaseObject);

			phase.NotifyStart(context);

			return context;
		}
		#endregion

	}

}