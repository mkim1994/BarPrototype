﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class DrinkControl : MonoBehaviour {

	private GameObject objectToPickUp;
	private GameObject objectToDrop;
	private GameObject glassInSight;

  	public KeyCode interactKey;
	// Use this for initialization

	public enum PickUpState{
		HOLDING_OBJECT,
		NOT_HOLDING_OBJECT,
		LOOKING_AT_OBJECT,
		NEAR_OBJECTIVE
	}

	PickUpState pickUpState;
	void Start () {
 		pickUpState = PickUpState.NOT_HOLDING_OBJECT;
	}
	
	// Update is called once per frame
	void Update () {
		ShootRay();

		Debug.Log(pickUpState);

		switch (pickUpState){
			case PickUpState.LOOKING_AT_OBJECT:
				PickUp(interactKey);
				break;
			case PickUpState.HOLDING_OBJECT:
				DropObject(interactKey);
				UseInteractable();
				break;
			case PickUpState.NOT_HOLDING_OBJECT:
				break;

			case PickUpState.NEAR_OBJECTIVE:
				
				break;
			default:
				break;
		}
	}

	public void ShootRay(){
		Ray ray = new Ray(transform.position, transform.forward);
		float rayDist = 10f;

		RaycastHit hit = new RaycastHit();

		if(Physics.Raycast(ray, out hit, rayDist)){
			if(hit.transform.GetComponent<Interactable>() != null){
				if(pickUpState != PickUpState.HOLDING_OBJECT){
					pickUpState = PickUpState.LOOKING_AT_OBJECT;
					objectToPickUp = hit.transform.gameObject;
					objectToPickUp.GetComponent<Interactable>().ChangeMaterialOnRaycastHit();
					// objectToPickUp.GetComponent<MeshRenderer>().material 
 				}
			} else {
 				objectToPickUp = null;
			} 
		} 
	}


	public void PickUp(KeyCode key){
		if(Input.GetKeyDown(key)){
			
			if(pickUpState == PickUpState.LOOKING_AT_OBJECT){
				Rigidbody rb = objectToPickUp.GetComponent<Rigidbody>();
				objectToPickUp.transform.SetParent(this.gameObject.transform);
				rb.isKinematic = true;
				rb.useGravity = false;
				rb.freezeRotation = true;
				objectToPickUp.transform.localEulerAngles = objectToPickUp.GetComponent<Interactable>().startRot;
				objectToPickUp.transform.localPosition = Vector3.forward + (Vector3.right * 0.5f) + (Vector3.down * 0.25f);
				objectToDrop = objectToPickUp;
				pickUpState = PickUpState.HOLDING_OBJECT;
			}
		} 
	}

	public void DropObject(KeyCode key){
		if(Input.GetKeyDown(key)){
			Rigidbody rb = objectToDrop.GetComponent<Rigidbody>();

			if (pickUpState == PickUpState.HOLDING_OBJECT){
				rb.useGravity = true;
				rb.isKinematic = false;
				rb.freezeRotation = false;
				objectToDrop.transform.SetParent(null);
				pickUpState = PickUpState.NOT_HOLDING_OBJECT;
			}
		}
	}

	public void UseInteractable(){
		//if you're holding a base
		if(objectToDrop.tag == "Base"){ 
			//if we're holding a base, then look for glass.
			//Need to have a check as to what kind of base is in it.
			FindGlassRay();
			if(Input.GetMouseButton(1) && glassInSight != null){
				objectToDrop.GetComponent<Interactable>().Pour();
				glassInSight.GetComponentInChildren<PourSimulator>().FillUp();												
			} 
			else {
				objectToDrop.GetComponent<Interactable>().StopPour();
			} 
		} else if (objectToDrop.tag == "Glass"){
			//do stuff for glasses here
		} else if (objectToDrop.tag == "Feeling"){
			//do stuff for feeling here
		}
	}

	public void FindGlassRay(){
		Ray ray = new Ray(transform.position, transform.forward);
		float rayDist = 10f;

		RaycastHit hit = new RaycastHit();

		if(Physics.Raycast(ray, out hit, rayDist)){
			if(hit.transform.tag == "Glass"){
 				glassInSight = hit.transform.gameObject;				
			} else {
				glassInSight = null;
			}
		}
	}

}