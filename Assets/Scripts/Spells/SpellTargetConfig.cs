using UnityEngine;
using System.Collections;

namespace Spells {
	public enum SpellTargetWhere {
		CasterSelectedUnit,
		CasterSelectedPoint,

		// spell cast is on self or starts from self
		Self,

		// randomly selected unit in spell range
		RandomUnit,

		// point randomly selected in front of spell cast source, between min and max range
		PointInFrontOfSource
	}

	public enum SpellTargetWhat {
		SingleUnit,
		SinglePoint,
		UnitsInRadis, // (radius, nMaxTargets)
		// UnitsInChain
		// UnitsInCollider
	}

	[System.Serializable]
	public class SpellTargetConfig {
		public enum Affiliation {
			Any = 0,
			Friendly,
			Hostile
		}
		
		public SpellTargetWhere Where;
		public SpellTargetWhat What;
		public float MinRange, MaxRange;
		public Affiliation RequiredAffiliation; // only used for unit targets
		// public Type Filter; // e.g. IsInFront (angle), IsWounded
	}

}