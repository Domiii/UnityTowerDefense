using UnityEngine;
using System.Collections;

public class HealthBar : MonoBehaviour {
	public Transform healthBarTransform;
	public Unit unit;

	// Use this for initialization
	void Start () {
		unit = transform.parent.GetComponent<Unit>();
		if (unit == null) {
			Debug.LogError("HealthBar does not have a Unit parent. Make sure that its parent has a Unit script component.");
			return;
		}
		if (healthBarTransform == null) {
			healthBarTransform = transform.FindChildByTag("HealthBar");
			if (healthBarTransform == null) {
				Debug.LogError ("HealthBar does not have a HealthBar-tagged child.", transform.parent);
				return;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (unit == null || healthBarTransform == null)
			return;

		// update health bar to reflect actual health value
		var ratio = unit.Health / (float)unit.MaxHealth;
		healthBarTransform.localScale = new Vector3(ratio, 1, 1);
	}
}
