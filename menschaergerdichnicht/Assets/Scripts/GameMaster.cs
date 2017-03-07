using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour {

	public bool debugMode = false;
	public bool gameHasEnded = false;

	public GameObject[] players = new GameObject[4];
	public GameObject[] DebugDices = new GameObject[4];
	public GameObject playerDice;
	int index = 0;
	
	//	start new round
	public void GoOn(int color){
		EndOfDraw(color);
		MainLoop();
	}
	
	// main loop
	void MainLoop(){
		if(!debugMode && !gameHasEnded){
			index ++;
			if(index >= players.Length || index < 0){
				index = 0;
			}
			for(int i = 0; i < players.Length; i++){
				players[i].GetComponent<Player>().SetOff();
			}
			players[index].GetComponent<Player>().SetOn();
		}
	}
	
	public void EndOfDraw(int color){
		for(int i = 0; i < players.Length; i++){
			if(players[i].GetComponent<Player>().color == color){
				for(int j = 0; j < players[i].GetComponent<Player>().figures.Length; j++){
					if((players[i].GetComponent<Player>().figures[j].GetComponent<Figure>().pos == 4 && !players[i].GetComponent<Player>().FieldIsOccupied(players[i].GetComponent<Player>().GetLegalCoordinates(players[i].GetComponent<Player>().figures[j].GetComponent<Figure>().pos), players[i].GetComponent<Player>().figures[j].GetComponent<Figure>().num)) || players[i].GetComponent<Player>().figures[j].GetComponent<Figure>().GetNeedToHit()){
						players[i].GetComponent<Player>().figures[j].GetComponent<Figure>().pos = GetFreeBaseField(color);
					}
				}
			}
		}
	}

	// Use this for initialization
	void Start () {
		for(int i = 0; i < players.Length; i ++){
			players[i].GetComponent<Player>().color = i;
		}
		index = -1;
		GoOn(0);
	}
	
	// Figure update
	public void CheckFieldForEnemy(int pos, int color){
		for(int i = 0; i < players.Length; i++){
			if(players[i].GetComponent<Player>().color != color){
				for(int j = 0; j < players[i].GetComponent<Player>().figures.Length; j++){
					if(CompareRelativePositions(color, players[i].GetComponent<Player>().color, pos, players[i].GetComponent<Player>().figures[j].GetComponent<Figure>().pos)){
						players[i].GetComponent<Player>().figures[j].GetComponent<Figure>().pos = GetFreeBaseField(i);
						players[GetPlayerByColor(color)].GetComponent<Player>().GetFigureByCoordinates(pos).GetComponent<Figure>().SetNeedToHit(false);
					}
				}
			}
		}
	}
	
	public int GetPlayerByColor(int color){
		for(int i = 0; i < players.Length; i++){
			if(players[i].GetComponent<Player>().color == color){
				return i;
			}
		}
		return -1;
	}
	
	public bool IsAbleToHit(int pos, int color){
		for(int i = 0; i < players.Length; i++){
			if(players[i].GetComponent<Player>().color != color){
				for(int j = 0; j < players[i].GetComponent<Player>().figures.Length; j++){
					if(CompareRelativePositions(color, players[i].GetComponent<Player>().color, pos, players[i].GetComponent<Player>().figures[j].GetComponent<Figure>().pos)){
						return true;
					}
				}
			}
		}
		return false;
	}
	
	bool CompareRelativePositions(int color1, int color2, int pos1, int pos2){
		if(pos1 < 4 || pos1 > 39 || pos2 < 4 || pos2 > 39)	return false;
		int relPos1 = pos1 + color1*10;
		int relPos2 = pos2 + color2*10;
		
		relPos1 = OverlapCircle(relPos1);
		relPos2 = OverlapCircle(relPos2);
		
		return relPos1 == relPos2;
	}
	
	int OverlapCircle(int pos){
		if(pos > 40){
			return pos - 40;
		}
		return pos;
	}
	
	// returns int pos for free base field
	int GetFreeBaseField(int color){
		for(int i = 0; i < players.Length; i++){
			if(players[i].GetComponent<Player>().color == color){
				bool[] pos = new bool[players[i].GetComponent<Player>().bases.Length];
				for(int j = 0; j < pos.Length; j++){
					pos[j] = true;
				}
				for(int j = 0; j < players[i].GetComponent<Player>().figures.Length; j++){
					if(players[i].GetComponent<Player>().figures[j].GetComponent<Figure>().pos < pos.Length){
						pos[players[i].GetComponent<Player>().figures[j].GetComponent<Figure>().pos] = false;
					}
				}
				for(int j = 0; j < pos.Length; j++){
					if(pos[j]){
						return j;
					}
				}
			}
		}
		return -1;
	}
	
	void HasFinished(){
		bool[] hasFinished = new bool[players.Length];
		for(int i = 0; i < hasFinished.Length; i++){
			hasFinished[i] = true;
		}
		for(int i = 0; i < players.Length; i++){
			for(int j = 0; j < players[i].GetComponent<Player>().figures.Length; j++){
				if(players[i].GetComponent<Player>().figures[j].GetComponent<Figure>().pos < 40){
					hasFinished[i] = false;
				}
			}
		}
		for(int i = 0; i < hasFinished.Length; i++){
			if(hasFinished[i]){
				players[i].GetComponent<Player>().won = true;
				gameHasEnded = true;
			}
		}
	}
	
	// +++++Debugging+++++
	
	// Update is called once per frame
	void Update () {
		HasFinished();
		if(Input.GetKeyDown(KeyCode.D)){
			debugMode = !debugMode;
		}
		for(int i = 0; i < DebugDices.Length; i++){
			DebugDices[i].SetActive(debugMode);
		}
		playerDice.SetActive(!debugMode);
	}
}
