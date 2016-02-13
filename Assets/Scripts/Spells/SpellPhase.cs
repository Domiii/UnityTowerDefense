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

		protected internal abstract void NotifyStart (SpellPhaseContext context);

		public override string ToString () {
			return Template.Name;
		}
	}
	
	public class CastPhase : SpellPhase {
		bool isActive;

		public SpellPhaseContext Context {
			get;
			internal set;
		}

		public override bool IsActive {
			get {
				return isActive;
			}
		}
		
		protected internal override void NotifyStart(SpellPhaseContext context) {
			Context = context;
			isActive = true;
			context.NotifyStart ();
		}
		
		protected internal void NotifyEnd() {
			if (Context != null) {
				Context.NotifyEnd ();
			}
			isActive = false;
			Context = null;
		}
	}

	public class MultiContextPhase : SpellPhase {
		SpellPhaseContext[] contexts;
		bool[] contextActiveStatuses;

		public int ContextCount {
			get;
			private set;
		}

		public int ActiveContextCount {
			get;
			private set;
		}

		public bool[] ContextActiveStatuses {
			get {
				return contextActiveStatuses;
			}
			private set {
				contextActiveStatuses = value;
			}
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

		protected internal override void NotifyStart(SpellPhaseContext context) {
			++ActiveContextCount;
			++ContextCount;

			if (Contexts == null) {
				Contexts = new SpellPhaseContext[8];
				contextActiveStatuses = new bool[8];
			}
			else {
				if (ContextCount > contexts.Length) {
					System.Array.Resize(ref contexts, ContextCount);
					System.Array.Resize(ref contextActiveStatuses, ContextCount);
				}
			}
			context.index = ContextCount - 1;
			Contexts[context.index] = context;
			contextActiveStatuses [context.index] = true;

			context.NotifyStart ();
		}

		internal void NotifyEnd(int index) {
			if (contextActiveStatuses [index]) {
				var context = contexts [index];
				if (context != null) {
					context.NotifyEnd ();
				}
				--ActiveContextCount;
				contextActiveStatuses [index] = false;
				Contexts [index] = null;
			}
		}
	}
	
	public class ProjectilePhase : MultiContextPhase {
	}
	
	public class ImpactPhase : MultiContextPhase {
	}


}