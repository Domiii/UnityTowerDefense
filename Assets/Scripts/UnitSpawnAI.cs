using UnityEngine;
using System.Collections;

[RequireComponent(typeof(UnitManager))]
public class UnitSpawnAI : MonoBehaviour {
	UnitManager unitManager;
	int nextChoice;
	float lastPurchaseTime;

	float TimeSinceLastPurchase {
		get {
			return Time.time - lastPurchaseTime;
		}
	}

	void Awake() {
		unitManager = GetComponent<UnitManager> ();

		if (unitManager == null) {
			Debug.LogError("UnitSpawnAI is missing UnitManager component", this);
		}

		// initialize
		lastPurchaseTime = Time.time;
		PredictNextChoice ();
	}

	void Update() {
		// try buying our current target unit of choice
		if (unitManager.TryBuyUnit (nextChoice)) {
			//Debug.Log("AI bought #" + nextChoice);

			PredictNextChoice ();
			lastPurchaseTime = Time.time;
		}
		//else if (TimeSinceLastPurchase > ...) {
		// waited too long
		// PredictNextChoice ();
		//}
	}

	void PredictNextChoice() {
		nextChoice = Random.Range(0, unitManager.BuyUnitConfigs.Length);
	}
}