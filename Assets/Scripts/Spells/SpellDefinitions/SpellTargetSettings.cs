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

		// point in front of current spell phase object, randomly selected between min and max range
		PointInFrontOfSource
	}

	public enum SpellTargetWhat {
		SingleUnit,
		SinglePoint,
		UnitsInRadis, // (radius, nMaxTargets)
		// UnitsInChain
		// UnitsInCollider
	}

	public enum Affiliation {
		Any = 0,
		Friendly,
		Hostile
	}

	[System.Serializable]
	public class SpellTargetSettings {

		// CastPhase (while casting; fallback phase owner = caster)
		// ProjectilePhase (while projectile in flight; fallback phase owner = null, must have prefab)
		// Impact (after projectile hit; fallback phase owner = projectile, or caster (if it has no projectile))

		public SpellTargetWhere Where;
		public SpellTargetWhat What;
		public float MinRange, MaxRange;
		public Affiliation RequiredAffiliation; // only used for unit targets
		// public Type Filter; // e.g. IsInFront (angle), IsWounded
	}

}