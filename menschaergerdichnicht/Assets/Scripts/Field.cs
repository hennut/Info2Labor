using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour {
	
	Color32 mainColor;
	public bool glowing;
	public bool isTargeted;
	
	// start
	void Start(){
		mainColor = GetComponent<Renderer>().materials[0].color;
	}
	
	void Update(){
		if(glowing){
			GetComponent<Renderer>().materials[0].color = new Color32(255, 195, 0, 1);
		}else{
			GetComponent<Renderer>().materials[0].color = mainColor;
		}
		glowing = false;
	}
	
	// marks a field
	public void Glow(){
		glowing = true;
	}
	
	//marks a field targeted by figure
	void OnTriggerEnter(Collider coll){
		isTargeted = coll.GetComponent<Figure>().dragging;
	}
	
	void OnTriggerExit(Collider coll){
		Invoke("SetUntargeted", 0.02f);
	}
	
	void SetUntargeted(){
		isTargeted = false;
	}
}
