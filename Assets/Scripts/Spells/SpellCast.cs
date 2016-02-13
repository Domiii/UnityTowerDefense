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
		Unused = 0,
		Casting,
		Failed,
		Active,
		Finished
	}

	public class SpellCast : MonoBehaviour, IPooledObject {

		public SpellCast() {
			SpellCastContext = ObjectManager.Instance.Obtain<SpellCastContext> ();
			Targets = ObjectManager.Instance.Obtain<SpellTargetCollection> ();
			Status = SpellCastStatus.Unused;
		}
		
		#region Properties
		public SpellCastContext SpellCastContext {
			get;
			private set;
		}

		public SpellCastStatus Status {
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

		public bool IsCasting {
			get { return CastPhase != null && CastPhase.IsActive; }
		}

		public SpellTargetCollection Targets {
			get;
			private set;
		}
		#endregion


		#region Getting Started
		public void Initialize(Spell spell) {
			SpellCastContext.Spell = spell;
			Targets.InitializeTargets (gameObject, SpellCastContext);
		}

		public bool CanCastSpell(GameObject initialTarget, Vector3 initialTargetPosition) {
			return CanCastSpell (initialTarget, initialTargetPosition);
		}

		public void StartCasting(GameObject initialTarget, Vector3 initialTargetPosition) {
			SpellCastContext.InitialTarget = initialTarget;
			SpellCastContext.InitialTargetPosition = initialTargetPosition;

			StartCastPhase ();
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
					OnCastFinished();
				}
			}
		}

		void UpdateCastTimer() {
			if (CastPhase.Context == null) {
				// was casting, but context got destroyed -> Cancel spell cast
				Cancel();
			}
			else if (CastPhase.Context.TimeLeft <= 0) {
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
				Status = SpellCastStatus.Casting;
				CastPhase = ObjectManager.Instance.Obtain<CastPhase>();
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
			Targets.FindTargets (SpellCastContext.Spell.Targets);

			// handle position target
			if (Targets.TargetPosition != null) {
				posCb(Targets.TargetPosition.Value);
			}

			
			// handle object target
			for (var i = 0; i < Targets.Count; ++i) {
				var target = Targets[i];
				goCb(target);
			}
		}

		void StartProjectilePhase() {
			Status = SpellCastStatus.Active;
			var phaseTemplate = SpellCastContext.Spell.ProjectilePhase;
			if (phaseTemplate != null) {
				// yes ProjectilePhase
				ProjectilePhase = ObjectManager.Instance.Obtain<ProjectilePhase>();
				ImpactPhase = ObjectManager.Instance.Obtain<ImpactPhase> ();
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
			ImpactPhase = ObjectManager.Instance.Obtain<ImpactPhase> ();
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


		#region End-of-lifetime
		void OnCastFinished() {
			Status = SpellCastStatus.Finished;
			CleanUp();
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
			Status = SpellCastStatus.Failed;
			
			// clean up
			CleanUp();
		}

		/// <summary>
		/// Destroy all temporary spell objects created by this cast.
		/// </summary>
		public void CleanUp(bool dontDestroy = false) {
			//SpellGameObjectManager.Instance.RemoveComponent (this);

			if (CastPhase != null) {
				CastPhase.CleanUp();
				ObjectManager.Instance.Recycle(CastPhase);
				CastPhase = null;
			}
			if (ProjectilePhase != null) {
				ProjectilePhase.CleanUp();
				ObjectManager.Instance.Recycle(ProjectilePhase);
				ProjectilePhase = null;
			}
			if (ImpactPhase != null) {
				ImpactPhase.CleanUp();
				ObjectManager.Instance.Recycle(ImpactPhase);
				ImpactPhase = null;
			}

			Targets.CleanUp ();

			if (!dontDestroy) {
				GameObjectManager.Instance.RemoveComponent (this);
			}
		}

//		void OnDisable() {
//			// disable = destroy
//			CleanUp ();
//		}
		
		void OnDestroy() {
			CleanUp (true);

			ObjectManager.Instance.Recycle (SpellCastContext);
			ObjectManager.Instance.Recycle (Targets);
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
				carrierObject = GameObjectManager.Instance.Obtain(prefab, position, rotation);
			}
			else {
				carrierObject = GameObjectManager.Instance.ObtainEmpty(phase.ToString(), position, rotation);
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