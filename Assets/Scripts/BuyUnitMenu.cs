using UnityEngine;
using System.Collections;

public class BuyUnitMenu : MonoBehaviour {
	public GameObject ButtonPrefab;
	public FactionManager FactionManager;

	// Use this for initialization
	void Start () {
		if (ButtonPrefab == null || FactionManager == null) {
			Debug.LogErrorFormat(this, "BuyUnitMenu is missing ButtonPrefab or FactionManager");
			return;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
