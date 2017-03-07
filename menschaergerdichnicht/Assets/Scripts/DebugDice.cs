using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugDice : MonoBehaviour {

	public int diceValue;
	public GameObject player;
	
	void OnMouseUp(){
		player.GetComponent<Player>().SetDiceValue(diceValue);
	}
}
