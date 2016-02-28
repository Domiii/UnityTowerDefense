using UnityEngine;
using System.Collections;

public class Dimmer : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseDown() {
		// clear selection
		GameUIManager.Instance.ClearSelection ();
	}
}
