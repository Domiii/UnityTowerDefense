using UnityEngine;
using System.Collections;

public class BuyUnitMenu : MonoBehaviour {
	public GameObject ButtonPrefab;
	public UnitManager UnitManager;

	// Use this for initialization
	void Start () {
		if (ButtonPrefab == null || UnitManager == null) {
			Debug.LogErrorFormat(this, "BuyUnitMenu is missing ButtonPrefab or UnitManager");
			return;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
