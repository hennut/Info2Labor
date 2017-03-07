using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Figure : MonoBehaviour {

	public bool dragging;
	float distance;
	
	public int pos, num;
	public bool needToHit;
	
	// Update is called once per frame
	void Update () {
		if(dragging){
			// target
			transform.parent.parent.GetComponent<Player>().ShowLegalCoordinates(pos);
			
			// movement
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 rayPoint = ray.GetPoint(distance);
			rayPoint.y = -0.055f;
            transform.position = rayPoint;
		}else{
			transform.position = transform.parent.parent.GetComponent<Player>().GetVector3Coordinates(pos);
		}
	}
	
	void OnMouseDown() {
		if(transform.parent.parent.GetComponent<Player>().IsActive() && transform.parent.parent.GetComponent<Player>().GetPlaceMode() || transform.parent.parent.parent.GetComponent<GameMaster>().debugMode){
			distance = Vector3.Distance(transform.position, Camera.main.transform.position);
			dragging = true;
		}
    }
 
    void OnMouseUp() {
		if(transform.parent.parent.GetComponent<Player>().IsActive() && transform.parent.parent.GetComponent<Player>().GetPlaceMode() || transform.parent.parent.parent.GetComponent<GameMaster>().debugMode){
			if(GetTargetCoordinates() == transform.parent.parent.GetComponent<Player>().GetLegalCoordinates(pos) || transform.parent.parent.parent.GetComponent<GameMaster>().debugMode){
				DragToPos();
			}
		}
		dragging = false;
    }
	
	public void DragToPos(){
		if(!transform.parent.parent.parent.GetComponent<GameMaster>().debugMode){
			pos = GetTargetCoordinates();
			transform.parent.parent.GetComponent<Player>().EnemyContact(pos);
			transform.parent.parent.GetComponent<Player>().SetDiceMode(true);
			transform.parent.parent.GetComponent<Player>().SetPlaceMode(false);
			if(!transform.parent.parent.GetComponent<Player>().GetPlaceMode6()){
				transform.parent.parent.parent.GetComponent<GameMaster>().GoOn(GetColor());
			}
		}else{
			if(!transform.parent.parent.GetComponent<Player>().FieldIsOccupied(GetTargetCoordinates(), num)){
				pos = GetTargetCoordinates();
				transform.parent.parent.GetComponent<Player>().EnemyContact(pos);
			}
		}
	}
	
	public void SetPos(){
		if(transform.parent.parent.GetComponent<Player>().GetLegalCoordinates(pos) != pos){
			pos = transform.parent.parent.GetComponent<Player>().GetLegalCoordinates(pos);
			transform.parent.parent.GetComponent<Player>().EnemyContact(pos);
			transform.parent.parent.GetComponent<Player>().SetDiceMode(true);
			transform.parent.parent.GetComponent<Player>().SetPlaceMode(false);
				
			if(!transform.parent.parent.GetComponent<Player>().GetPlaceMode6()){
				transform.parent.parent.parent.GetComponent<GameMaster>().GoOn(GetColor());
			}
		}
	}
	
	public int GetColor(){
		return transform.parent.parent.GetComponent<Player>().color;
	}
	
	// checks whether an enemy is able to get hit
	public void IsEnemyHittable(){
		if(transform.parent.parent.parent.GetComponent<GameMaster>().IsAbleToHit(pos, GetColor())){
			SetNeedToHit(true);
		}
	}
	
	public void SetNeedToHit(bool need){
		needToHit = need;
	}
	
	public bool GetNeedToHit(){
		return needToHit;
	}
	
	int GetTargetCoordinates(){
		int pos = transform.parent.parent.GetComponent<Player>().GetPosOfTargetedField(num);
		if(pos == -1){
			return this.pos;
		}
		return pos;
	}
}
