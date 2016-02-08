using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StatsMenu : MonoBehaviour {
	static void SetText(GameObject menu, string textName, object content) {
		var textObject = menu.transform.FindChild (textName);
		if (textObject == null) {
			return;
		}

		var text = textObject.GetComponent<Text> ();
		if (text == null) {
			return;
		}
		text.text = content.ToString();
	}

	public GameObject StatsMenuPrefab;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	/// <summary>
	/// Format a float with up to two decimal places.
	/// </summary>
	static string FormatFloat(float val) {
		return string.Format ("{0:0.##}", val);
	}

	public void OpenMenu() {
		var menuData = new StatsMenuData ();
		SendMessage ("GetStatsData", menuData);

		var menu = (GameObject)Instantiate (StatsMenuPrefab, transform.position, Quaternion.identity);

		SetText (menu, "AttackText", FormatFloat(menuData.DamageMin) + " - " + FormatFloat(menuData.DamageMax));
		SetText (menu, "HealthText", FormatFloat(menuData.Health) + " / " + FormatFloat(menuData.MaxHealth));
		SetText (menu, "RangeText", FormatFloat(menuData.AttackRadius));
		SetText (menu, "SpeedText", FormatFloat(menuData.Speed));
	}
}
