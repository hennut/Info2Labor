using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour {

	System.Random rnd = new System.Random();
	
	public GameObject player;
	
	void OnMouseDown(){
		if(player.GetComponent<Player>().IsActive() && player.GetComponent<Player>().GetDiceMode()){
			player.GetComponent<Player>().SetDiceValue(rnd.Next(1, 7));
		}
	}
}
