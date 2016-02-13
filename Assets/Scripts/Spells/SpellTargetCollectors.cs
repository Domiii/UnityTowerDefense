using UnityEngine;
using System.Collections;


namespace Spells {

	// TODO: RandomUnitInRange, UnitsInChain, UnitsInCollider

	[BehaviorScript("CasterSelectedUnit")]
	public class CasterSelectedUnitTargetCollector : SpellTargetCollector {
		public override bool HasObjectTargets {
			get { return true; }
		}
		
		public override bool HasPositionTarget {
			get { return false; }
		}
		
		public override void CollectTargets(SpellTargetCollection targets) {
			if (targets.SpellCastContext.InitialTarget != null) {
				targets.AddTarget (targets.SpellCastContext.InitialTarget);
			}
		}
	}
	
	[BehaviorScript("CasterSelectedPosition")]
	public class CasterSelectedPositionTargetSelector : SpellTargetCollector {
		public override bool HasObjectTargets {
			get { return false; }
		}
		
		public override bool HasPositionTarget {
			get { return true; }
		}
		
		public override void CollectTargets(SpellTargetCollection targets) {
			targets.TargetPosition = targets.SpellCastContext.InitialTargetPosition;
		}
	}
	
	[BehaviorScript("Self")]
	public class SelfTargetSelector : SpellTargetCollector {
		public override bool HasObjectTargets {
			get { return true; }
		}
		
		public override bool HasPositionTarget {
			get { return false; }
		}
		
		public override void CollectTargets(SpellTargetCollection targets) {
			targets.AddTarget (targets.ContextOwner);
		}
	}
	
	[BehaviorScript("UnitsInRadis2D")]
	public class UnitsInRadisTargetSelector : SpellTargetCollector {
		public float Radius;
		public int MaxTargets = 0;
		Collider2D[] collidersInRange = new Collider2D[128];

		public override bool HasObjectTargets {
			get { return true; }
		}
		
		public override bool HasPositionTarget {
			get { return false; }
		}
		
		public override void CollectTargets(SpellTargetCollection targets) {
			var nResults = targets.AddTargetsInRadius2D(targets.Settings.Range, out collidersInRange);
			for (var i = 0; i < nResults; ++i) {
				var collider = collidersInRange[i];
				var unit = collider.GetComponent<Unit> ();
				if (unit != null) {
					targets.AddTarget(unit.gameObject);
				}
			}
		}
	}
}