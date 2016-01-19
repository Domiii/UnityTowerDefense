using UnityEngine;
using System.Collections;

public class TowerPlatform : MonoBehaviour {
	public GameObject TowerPrefab;

	void OnMouseDown() {
		// destroy platform
		Destroy (gameObject);

		// build tower
		//var towerObject = Instantiate (TowerPrefab, transform.position, transform.rotation);
		Instantiate (TowerPrefab, transform.position, transform.rotation);
	}
}
