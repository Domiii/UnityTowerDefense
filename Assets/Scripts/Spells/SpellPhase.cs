using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Spells {

	public abstract class SpellPhase : IPooledObject {
		protected Dictionary<string, object> spellPhaseData;

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

		public Dictionary<string, object> SpellPhaseData {
			get {
				if (spellPhaseData == null) {
					spellPhaseData = new Dictionary<string, object> ();
				}
				return spellPhaseData;
			}
		}

		protected internal abstract void NotifyStart (SpellPhaseContext context);

		protected internal abstract void CleanUp ();

		public override string ToString () {
			return Template.SpellPhaseId.ToString();
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
				Context = null;
			}
			isActive = false;
		}

		protected internal override void CleanUp () {
			if (Context != null) {
				Context.CleanUp();
				Context = null;
			}
			isActive = false;
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
			context.Index = ContextCount - 1;
			Contexts[context.Index] = context;
			contextActiveStatuses [context.Index] = true;

			context.NotifyStart ();
		}
		
		internal void NotifyEnd(int index) {
			if (contextActiveStatuses [index]) {
				var context = contexts [index];
				if (context != null) {
					context.NotifyEnd ();
					contexts [index] = null;
				}
				--ActiveContextCount;
				contextActiveStatuses [index] = false;
			}
		}
		
		internal void CleanUp(int index) {
			var context = contexts [index];
			if (context != null) {
				context.CleanUp();
				contexts [index] = null;
			}
			--ActiveContextCount;
			contextActiveStatuses [index] = false;
		}
		
		protected internal override void CleanUp () {
			for (var i = 0; i < ContextCount; ++i) {
				contextActiveStatuses[i] = false;
				var context = contexts[i];
				if (context != null) {
					context.CleanUp();
					contexts[i] = null;
				}
			}

			ContextCount = 0;
			ActiveContextCount = 0;
		}
	}
	
	public class ProjectilePhase : MultiContextPhase {
	}
	
	public class ImpactPhase : MultiContextPhase {
	}


}