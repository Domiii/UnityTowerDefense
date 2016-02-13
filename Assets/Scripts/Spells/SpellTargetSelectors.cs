using UnityEngine;
using System.Collections;


namespace Spells {
	
	//	public enum SpellTargetSelector {
	//		CasterSelectedUnit,
	//		CasterSelectedPosition,
	//
	//		Self,
	//		UnitsInRadis, // (radius, nMaxTargets)
	//		RandomUnitInRange,
	//
	//		// UnitsInChain
	//		// UnitsInCollider
	//	}
	[SpellTargetSelector("CasterSelectedUnit")]
	public class CasterSelectedUnitTargetSelector : SpellTargetSelector {
		public override bool HasObjectTargets {
			get { return true; }
		}
		
		public override bool HasPositionTarget {
			get { return false; }
		}
		
		public override void FindTargets(SpellTargetCollection targets) {
			if (targets.SpellCastContext.InitialTarget != null) {
				targets.AddTarget (targets.SpellCastContext.InitialTarget);
			}
		}
	}
	
	[SpellTargetSelector("CasterSelectedPosition")]
	public class CasterSelectedPositionTargetSelector : SpellTargetSelector {
		public override bool HasObjectTargets {
			get { return false; }
		}
		
		public override bool HasPositionTarget {
			get { return true; }
		}
		
		public override void FindTargets(SpellTargetCollection targets) {
			targets.TargetPosition = targets.SpellCastContext.InitialTargetPosition;
		}
	}
	
	[SpellTargetSelector("Self")]
	public class SelfTargetSelector : SpellTargetSelector {
		public override bool HasObjectTargets {
			get { return true; }
		}
		
		public override bool HasPositionTarget {
			get { return false; }
		}
		
		public override void FindTargets(SpellTargetCollection targets) {
			targets.AddTarget (targets.ContextOwner);
		}
	}
	
	[SpellTargetSelector("UnitsInRadis")]
	public class UnitsInRadisTargetSelector : SpellTargetSelector {
		public float Radius;
		public int MaxTargets = 0;

		public override bool HasObjectTargets {
			get { return true; }
		}
		
		public override bool HasPositionTarget {
			get { return false; }
		}
		
		public override void FindTargets(SpellTargetCollection targets) {
			// TODO: Around source or target or ...?
//			var nResults = Physics2D.OverlapCircleNonAlloc(transform.position, AttackRadius, collidersInRange);
//			for (var i = 0; i < nResults; ++i) {
//				var collider = collidersInRange[i];
//				var unit = collider.GetComponent<Unit> ();
//				if (unit != null && IsValidTarget(unit)) {
//					return unit;
//				}
//			}
		}
	}
}