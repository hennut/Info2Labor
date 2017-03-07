using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	public int color;

	public bool isActive;
	public bool diceMode;
	public bool placeMode, placeMode6;
	public int diceValue = 0;
	int diceIndex = 0;
	
	public GameObject[] figures = new GameObject[4];
	
	public GameObject[] fields = new GameObject[40];
	public GameObject[] bases = new GameObject[4];
	public GameObject[] finishs = new GameObject[4];
	
	public GameObject[] debugDices = new GameObject[6];
	
	public bool won;

	// Use this for initialization
	void Start () {
		for(int i = 0; i < debugDices.Length; i++){
			debugDices[i].GetComponent<DebugDice>().diceValue = i + 1;
		}
		// ini the figures
		for(int i = 0; i < figures.Length; i++){
			figures[i].GetComponent<Figure>().pos = i;
			figures[i].GetComponent<Figure>().num = i;
			figures[i].transform.position = GetVector3Coordinates(i);
		}
		
		// startmode
		diceMode = true;
		placeMode = false;
	}
	
	// returns field by posCoordinate
	public GameObject GetField(int pos){
		if(pos < bases.Length){
			return bases[pos];
		}else if(pos >= bases.Length && pos < fields.Length + bases.Length){
			return fields[pos - bases.Length];
		}else{
			return finishs[pos - bases.Length - fields.Length];
		}
	}
	
	// calculates the vector3 for figures
	public Vector3 GetVector3Coordinates(int pos){
		Vector3 vec = GetField(pos).transform.position;
		vec.y += 0.04f;
		return vec;
	}
	
	// sets fields to glow if they are legal to go to
	public void ShowLegalCoordinates(int pos){
		if(transform.parent.GetComponent<GameMaster>().debugMode){
			for(int i = 0; i < bases.Length; i++){
				bases[i].GetComponent<Field>().Glow();
				finishs[i].GetComponent<Field>().Glow();
			}
			for(int i = 0; i < fields.Length; i++){
				fields[i].GetComponent<Field>().Glow();
			}
		}else{
			GetField(GetLegalCoordinates(pos)).GetComponent<Field>().Glow();
		}
	}
	
	// returns the legal coordinates
	public int GetLegalCoordinates(int pos){
		if(IsActive()){
			if(diceValue == 6 && BaseIsOccupied() && !StartIsOccupied()){
				if(pos < 4){
					return 4;
				}
				return pos;
			}else if((diceValue + pos) < (fields.Length + bases.Length + finishs.Length) && pos >= bases.Length && !FieldIsOccupied(pos + diceValue, GetFigureByCoordinates(pos).GetComponent<Figure>().num)){
				return pos + diceValue;
			}else{
				return pos;
			}
		}else{
			return pos;
		}
	}
	
	// called by figures, gives GameMaster the pos
	public void EnemyContact(int pos){
		transform.parent.GetComponent<GameMaster>().CheckFieldForEnemy(pos, color);
	}
	
	// checks whether somebody stands on the field allready
	public bool FieldIsOccupied(int pos, int num){
		for(int i = 0; i < figures.Length; i++){
			if(figures[i].GetComponent<Figure>().pos == pos && figures[i].GetComponent<Figure>().num == num){
				return true;
			}
		}
		return false;
	}
	
	// checks whether one of the figures blocks the entrance
	public bool StartIsOccupied(){
		for(int i = 0; i < figures.Length; i++){
			if(figures[i].GetComponent<Figure>().pos == 4){
				return true;
			}
		}
		return false;
	}
	
	// checks whether one of the figures is in base
	public bool BaseIsOccupied(){
		for(int i = 0; i < figures.Length; i++){
			if(figures[i].GetComponent<Figure>().pos < 4){
				return true;
			}
		}
		return false;
	}
	
	// checks whether the base is full
	public bool BaseIsFull(){
		for(int i = 0; i < figures.Length; i++){
			if(figures[i].GetComponent<Figure>().pos >= 4 && figures[i].GetComponent<Figure>().pos < 40){
				return false;
			}
		}
		return true;
	}
	
	//checks whether there is a possible draw
	public bool ExistPossibleDraw(){
		for(int i = 0; i < figures.Length; i++){
			if(figures[i].GetComponent<Figure>().pos != GetLegalCoordinates(figures[i].GetComponent<Figure>().pos)){
				return true;
			}
		}
		return false;
	}
	
	void OnGUI() {
		if(isActive){
			System.String val = "";
			if(diceValue != 0){
				val = "Dice: " + diceValue;
			}
			GUI.Label(new Rect(260, 250, 500, 500), val + " | it's your turn, player " + (color + 1));
		}
		if(won){
			GUI.Label(new Rect(260, 250, 500, 500), "Player " + (color + 1) + ", you have won!");
		}
	}
	
	// +++setter+++
	
	public void SetDiceValue(int val){
		diceValue = val;
		//checks whether a figure is hittable
		for(int i = 0; i < figures.Length; i++){
			figures[i].GetComponent<Figure>().IsEnemyHittable();
		}
			
		if(BaseIsFull() && val != 6 && diceIndex < 2){
			diceIndex ++;
		}else if(BaseIsFull() && val != 6 && diceIndex == 2){
			diceIndex = 0;
			transform.parent.GetComponent<GameMaster>().GoOn(color);
		}else{
			diceIndex = 0;
			if(ExistPossibleDraw()){
				if(val == 6){
					SetPlaceMode6(true);
				}else{
					SetPlaceMode6(false);
				}
				SetDiceMode(false);
				SetPlaceMode(true);
			}else{
				transform.parent.GetComponent<GameMaster>().GoOn(color);
			}
		}
	}
	
	public void SetDiceMode(bool mode){
		diceMode = mode;
	}
	
	public void SetPlaceMode(bool mode){
		placeMode = mode;
	}
	
	public void SetPlaceMode6(bool mode){
		placeMode6 = mode;
	}

	public void SetOn(){
		isActive = true;
		for(int i = 0; i < figures.Length; i++){
			figures[i].GetComponent<Figure>().needToHit = false;
		}
		if(!transform.parent.GetComponent<GameMaster>().debugMode){
			diceValue = 0;
		}
	}
	
	public void SetOff(){
		isActive = false;
	}
	
	// +++getter+++
	
	public int GetDiceValue(){
		return diceValue;
	}
	
	public bool GetDiceMode(){
		return diceMode;
	}
	
	public bool GetPlaceMode(){
		return placeMode;
	}
	
	public bool GetPlaceMode6(){
		return placeMode6;
	}
	
	public bool IsActive(){
		return isActive;
	}
	
	public GameObject GetFigureByCoordinates(int pos){
		for(int i = 0; i < figures.Length; i++){
			if(figures[i].GetComponent<Figure>().pos == pos){
				return figures[i];
			}
		}
		return null;
	}
	
	public int GetPosOfTargetedField(int num){
		for(int i = 0; i < bases.Length; i++){
			if(bases[i].GetComponent<Field>().isTargeted){
				return i;
			}else if(finishs[i].GetComponent<Field>().isTargeted){
				return fields.Length + bases.Length + i;
			}
		}
		for(int i = 0; i < fields.Length; i++){
			if(fields[i].GetComponent<Field>().isTargeted){
				return bases.Length + i;
			}
		}
		return -1;
	}
}
