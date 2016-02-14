using UnityEngine;
using System.Collections;


namespace Spells {

	// TODO: RandomUnitInRange, UnitsInChain, UnitsInCollider

	[CustomScriptableObject("CasterSelectedUnit")]
	[System.Serializable]
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
	
	[CustomScriptableObject("CasterSelectedPosition")]
	[System.Serializable]
	public class CasterSelectedPositionTargetCollector : SpellTargetCollector {
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
	
	[CustomScriptableObject("Caster")]
	[System.Serializable]
	public class CasterTargetCollector : SpellTargetCollector {
		public override bool HasObjectTargets {
			get { return true; }
		}
		
		public override bool HasPositionTarget {
			get { return false; }
		}
		
		public override void CollectTargets(SpellTargetCollection targets) {
			targets.AddTarget (targets.SpellCastContext.Caster);
		}
	}
	
	[CustomScriptableObject("ContextOwner")]
	[System.Serializable]
	public class ContextOwnerTargetCollector : SpellTargetCollector {
		public float xxxxx;
		public float yyyyy;

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
	
	[CustomScriptableObject("UnitsAroundContextOwner2D")]
	[System.Serializable]
	public class UnitsAroundContextOwner2DCollector : SpellTargetCollector {
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
			var nResults = targets.AddTargetsInRadius2D(targets.ContextOwner.transform, targets.Settings.Range, out collidersInRange);
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