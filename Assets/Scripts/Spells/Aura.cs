using UnityEngine;
using System.Collections;

namespace Spells {

	// TODO: Add, Pulse, then Remove AuraEffects on this target
	// TODO: AreaAura (https://github.com/WCell/WCell/blob/master/Services/WCell.RealmServer/Spells/Auras/AreaAura.cs#L269)

	public interface IAuraController {
	}

	/// <summary>
	/// Aura.
	/// </summary>
	/// <see cref="https://github.com/WCell/WCell/blob/master/Services/WCell.RealmServer/Spells/Spell.Aura.cs"/>
	public class Aura : MonoBehaviour, ISpellObject {
		public static Aura AddAura(GameObject go, AuraTemplate template) {
			var aura = SpellGameObjectManager.Instance.AddComponent<Aura>(go);
			aura.StartAura (template);
			return aura;
		}
		public static Aura AddAura(GameObject go, AuraTemplate template, float durationOverride) {
			var aura = SpellGameObjectManager.Instance.AddComponent<Aura>(go);
			aura.StartAura (template, durationOverride);
			return aura;
		}

		public AuraTemplate Template {
			get;
			private set;
		}

		public bool IsControlled {
			get;
			private set;
		}

		public IAuraController Controller {
			get;
			private set;
		}

		public float Duration {
			get;
			private set;
		}

		public float StartTime {
			get;
			private set;
		}
		
		public float TimeSinceStart {
			get {
				return Time.time - StartTime;
			}
		}
		
		public float TimeLeft {
			get {
				if (Template.Duration <= 0) {
					return float.PositiveInfinity;
				}
				return Template.Duration - TimeSinceStart;
			}
		}
			
		public void StartAura(AuraTemplate template) {
			StartAura (null, template, template.Duration);
		}
		
		public void StartAura(AuraTemplate template, float duration) {
			StartAura (null, template, duration);
		}
		
		public void StartAura(IAuraController controller, AuraTemplate template) {
			StartAura (controller, template, template.Duration);
		}
		
		public void StartAura(IAuraController controller, AuraTemplate template, float duration) {
			Controller = controller;
			IsControlled = controller != null;
			Template = template;
			StartTime = Time.time;
			Duration = duration;

			// TODO: Create Prefab
			// TODO: Create AuraEffectHandlers and start aura + pulsing
		}

		public void Remove() {
			Destroy (this);
		}

		void Update() {
			if (IsControlled && Controller == null) {
				// controller is gone -> Remove
				Destroy (this);
			}
		}
	}

}