using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Computer : MonoBehaviour {

	public GameObject player;
	bool throwing, placing;
	
	System.Random rnd = new System.Random();
	
	// Update is called once per frame
	void Update () {
		if(player.GetComponent<Player>().IsActive()){
			if(player.GetComponent<Player>().GetPlaceMode()){
				if(!placing){
					placing = true;
					Invoke("PlaceFigure", 1);
				}
			}else if(player.GetComponent<Player>().GetDiceMode()){
				if(!throwing){
					throwing = true;
					Invoke("ThrowDice", 1);
				}
			}
		}
	}
	
	void ThrowDice(){
		ResetRandom();
		player.GetComponent<Player>().SetDiceValue(rnd.Next(1, 7));
		throwing = false;
		
	}
	
	void PlaceFigure(){
		ResetRandom();
		do{
			player.GetComponent<Player>().figures[rnd.Next(0, 4)].GetComponent<Figure>().SetPos();
		}while(player.GetComponent<Player>().GetPlaceMode());
		placing = false;
	}
	
	// i dont have an idea why this is necessary, but otherwise the cpus would not get different values
	void ResetRandom(){
		for(int i = 0; i < player.GetComponent<Player>().color; i++){
			rnd.Next(1);
		}
	}
}
