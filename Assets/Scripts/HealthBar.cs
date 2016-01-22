using UnityEngine;
using System.Collections;

public class HealthBar : MonoBehaviour {
	public Transform healthBarTransform;
	public Enemy enemy;

	// Use this for initialization
	void Start () {
		enemy = transform.parent.GetComponent<Enemy>();
		if (enemy == null) {
			Debug.LogError("HealthBar does not have a Enemy parent. Make sure that its parent has an Enemy script component.");
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
		if (enemy == null || healthBarTransform == null)
			return;

		// update health bar to reflect actual health value
		var ratio = enemy.Health / (float)enemy.MaxHealth;
		healthBarTransform.localScale = new Vector3(ratio, 1, 1);
	}
}
