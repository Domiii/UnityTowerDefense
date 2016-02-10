using UnityEngine;

using System;
using System.Collections.Generic;

namespace Spells {
	// TODO: Make this MonoBehavior?
	public class SpellCast {
		public static SpellPhase StartPhase(GameObject phaseOwner, SpellPhaseTemplate template) {
			var phase = phaseOwner.AddComponent<SpellPhase>();
			phase.Template = template;
			return phase;
		}

		public SpellCaster SpellCaster;
		public Spell Spell;
		public SpellPhase CurrentPhase;

		SpellTargetCollection targets;
		int iPhase;
		Action[] phaseStarters;
		List<SpellProjectile> projectiles;


		public SpellCast() {
			iPhase = 0;
			phaseStarters = new Action[] {
				StartCastPhase,
				StartProjectilePhase,
				Impact
			};
		}

		public void NextPhase() {
			var phase = phaseStarters [iPhase++];
			phase ();
		}

		void StartCastPhase() {
			if (Spell.CastPhase == null) {
				NextPhase();
			}
			else {
				//var startGo = Instantiate();
				//CurrentPhase = StartPhase (who, Spell.CastPhase);
			}
		}

		void StartProjectilePhase() {
			if (Spell.CastPhase == null) {
				NextPhase();
			}
			else {
				// TODO: create one projectile per target
				// CurrentPhase = StartPhase (projectile, Spell.ProjectilePhase);
			}
		}

		void Impact() {
			// TODO: When do we collect impact targets?
			// TODO: How can we impact multiple times?
		}

		public void CleanUp() {
			// TODO: Clean up?
		}
	}

}