using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Spells {

	public abstract class SpellPhase : ISpellObject {
		public SpellCastContext SpellCastContext {
			get;
			private set;
		}
		
		public SpellPhaseTemplate Template {
			get;
			private set;
		}

		public abstract bool IsActive {
			get;
		}
	}
	
	public class CastPhase : SpellPhase {
		public SpellPhaseContext Context {
			get;
			internal set;
		}
		
		public override bool IsActive {
			get { return Context != null && !Context.HasEnded; }
		}
		
		internal void NotifyStarted(SpellPhaseContext context) {
			Context = context;
			context.StartPhase ();
		}
		
		internal void NotifyFinished(bool interrupted) {
			Context.EndPhase (interrupted);
			Context = null;
		}
	}

	public class MultiContextPhase : SpellPhase {
		SpellPhaseContext[] contexts;

		public int ContextCount {
			get;
			private set;
		}

		public int ActiveContextCount {
			get;
			private set;
		}

		public SpellPhaseContext[] Contexts {
			get {
				return contexts;
			}
			private set {
				contexts = value;
			}
		}
		
		public override bool IsActive {
			get { return ActiveContextCount > 0; }
		}

		internal void InitializePhase() {
			ActiveContextCount = 0;
			ContextCount = 0;
		}

		internal void NotifyStarted(SpellPhaseContext context) {
			++ActiveContextCount;
			++ContextCount;

			if (Contexts == null) {
				Contexts = new SpellPhaseContext[8];
			}
			else {
				if (ContextCount > contexts.Length) {
					System.Array.Resize(ref contexts, ContextCount);
				}
			}
			context.index = ContextCount - 1;
			Contexts [context.index] = context;

			context.StartPhase ();
		}

		internal void NotifyFinished(SpellPhaseContext context) {
			--ActiveContextCount;
			context.EndPhase (false);
			Contexts [context.index] = null;
		}
	}
	
	public class ProjectilePhase : MultiContextPhase {
	}
	
	public class ImpactPhase : MultiContextPhase {
	}


}