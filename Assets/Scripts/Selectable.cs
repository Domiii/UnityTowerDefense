using UnityEngine;
using System.Collections;

public class Selectable : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseDown() {
		GameUIManager.Instance.ToggleSelect (this);
	}
}
