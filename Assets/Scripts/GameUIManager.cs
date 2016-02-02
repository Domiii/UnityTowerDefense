using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameUIManager : MonoBehaviour {
	public static GameUIManager Instance;
	
	public GameObject DimmerPrefab;
	public GameObject AttackerHighlighterPrefab;

	GameObject dimmerObject;
	Selectable currentSelection;
	int currentSelectionSortingLayerID;
	

	public GameUIManager() {
		Instance = this;
	}


	// Use this for initialization
	void Start () {
		// create invisible highlighter
		if (HasDimmer) {
			dimmerObject = Instantiate (DimmerPrefab);
			dimmerObject.SetActive (false);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public bool HasDimmer {
		get {
			return DimmerPrefab != null;
		}
	}
	
	#region Selection + Highlighting
	public bool IsSelected(Selectable obj) {
		return currentSelection == obj;
	}

	public void ToggleSelect(Selectable obj) {
		if (IsSelected(obj)) {
			ClearSelection();
		} else {
			Select (obj);
		}
	}

	public void Select(Selectable obj) {
		if (obj == currentSelection) {
			return;
		}

		if (currentSelection != null) {
			ClearSelection();
		}

		// select object
		currentSelection = obj;

		// make highlighter visible
		if (HasDimmer) {
			// move object on top of dimmer
			var renderer = currentSelection.GetComponent<SpriteRenderer> ();
			if (renderer != null) {
				currentSelectionSortingLayerID = renderer.sortingLayerID;
				renderer.sortingLayerName = "Highlight";
			}

			dimmerObject.SetActive (true);
		}

		// send message
		currentSelection.SendMessage ("OnSelect", SendMessageOptions.DontRequireReceiver);
	}

	public void ClearSelection() {
		if (currentSelection != null) {
			// send message
			currentSelection.SendMessage ("OnUnselect", SendMessageOptions.DontRequireReceiver);

			// unset
			currentSelection = null;
		}

		if (HasDimmer) {
			if (currentSelection != null) {
				// reset rendering options
				var renderer = currentSelection.GetComponent<SpriteRenderer> ();
				if (renderer != null) {
					renderer.sortingLayerID = currentSelectionSortingLayerID;
				}
			}

			// make dimmer invisible
			dimmerObject.SetActive (false);
		}
	}
	#endregion
	


	#region Text
	public void UpdateText() {

	}
	#endregion
}
