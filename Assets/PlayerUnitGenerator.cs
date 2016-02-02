using UnityEngine;
using System.Collections;

public class PlayerUnitGenerator : MonoBehaviour {
	public GameObject[] UnitPrefabs;
	public WavePath Path;
	public WavePath.FollowDirection Direction;

	// Use this for initialization
	void Start () {  }
	
	// Update is called once per frame
	void Update () {  }


	public void CreateUnit(int unitIndex) {
		if (unitIndex < 0 || unitIndex > UnitPrefabs.Length) {
			// invalid index
			return;
		}

		// select prefab
		var unitPrefab = UnitPrefabs [unitIndex];

		// create new unit at current position
		var go = (GameObject)Instantiate(unitPrefab, transform.position, Quaternion.identity);

		// set path
		var pathFollower = go.GetComponent<PathFollower>();
		pathFollower.Path = Path;
		pathFollower.pathDirection = Direction;

		// add faction
		FactionMember.SetFaction (go, FactionType.Player);
	}
}
