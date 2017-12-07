using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropzoneManager : MonoBehaviour {

	public bool isOccupied;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if(isOccupied == false){
			//if nothing is in it
			GetComponent<MeshRenderer>().material.color = Color.red;
		} else {
			GetComponent<MeshRenderer>().material.color = Color.blue;
		}
	}

	void OnTriggerEnter(Collider collider){
		if(collider.GetComponent<Interactable>() != null){
			isOccupied = true;
		}
	}

	void OnTriggerExit(Collider collider){
		if(collider.GetComponent<Interactable>() != null){
			isOccupied = false;	
		}
	}
}
