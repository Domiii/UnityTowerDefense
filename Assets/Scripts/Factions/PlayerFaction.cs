using UnityEngine;
using System.Collections;

public class PlayerFaction : Faction {


	public PlayerFaction() : base(FactionType.Player) {
	}

	// Use this for initialization
	void Start () {
		Credits += QuizDialog.GetExtraStartCredits();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
