using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropzoneManager : MonoBehaviour {

	Dropzone_Manager dropzone_Manager;
	public bool isOccupied;
	// Use this for initialization
	void Start () {
		isOccupied = false;
	}
	
	// Update is called once per frame
	void Update () {
		// if(!isOccupied){
		// 	//if nothing is in it
		// 	GetComponent<MeshRenderer>().material.color = Color.red;
		// } else {
		// 	GetComponent<MeshRenderer>().material.color = Color.blue;
		// } 
	}

	void OnTriggerEnter(Collider collider){
		if(collider.GetComponent<Interactable>() != null){
			isOccupied = true;
		}
	}

	void OnTriggerStay(Collider collider){
		if(collider.GetComponent<Interactable>() != null){
			isOccupied = true;
		}
	}

	void OnTriggerExit(Collider collider){
		if(collider.GetComponent<Interactable>() != null){
			isOccupied = false;	
		}
	}
	
	public void RevealNearestDropzone(){
		// Debug.Log("I'm the closest! " + gameObject.name);
		GetComponent<MeshRenderer>().material.color = Color.green;
	}
}
