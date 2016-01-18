using UnityEngine;
using System.Collections;

public class HealthBar : MonoBehaviour {
	public Transform healthBarTransform;
	public Living living;

	// Use this for initialization
	void Start () {
		living = transform.parent.GetComponent<Living>();
		if (living == null) {
			Debug.LogError("HealthBar does not have a Living parent. Please set living property manually.");
			return;
		}
		if (healthBarTransform == null) {
			healthBarTransform = transform.FindChildByTag("HealthBar");
			if (healthBarTransform == null) {
				Debug.LogError ("HealthBar of object does not have a HealthBar-tagged child.", transform.parent);
				return;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (living == null || healthBarTransform == null)
			return;

		// update health bar to reflect actual health value
		var ratio = living.Health / (float)living.MaxHealth;
		healthBarTransform.localScale = new Vector3(ratio, 1, 1);
	}
}
