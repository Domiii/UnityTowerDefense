using UnityEngine;
using System.Collections;


namespace Spells {
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