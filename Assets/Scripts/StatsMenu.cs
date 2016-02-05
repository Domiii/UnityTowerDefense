using UnityEngine;
using System.Collections;

public class StatsMenu : MonoBehaviour {
	public GameObject StatsMenuPrefab;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OpenMenu() {
		var menuData = new StatsMenuData ();
		SendMessage ("GetStatsData", menuData);

		var menu = (GameObject)Instantiate (StatsMenuPrefab, transform.position, Quaternion.identity);

		var attackText = menu.transform.FindChild ("AttackText");
	}
}
