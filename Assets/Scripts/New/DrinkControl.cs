using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class DrinkControl : MonoBehaviour {

	private GameObject objectToPickUp;
	private GameObject objectToDrop;

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

				// rb.useGravity = true;
				// rb.isKinematic = false;
				// rb.freezeRotation = false;
				// objectToPickUp.transform.SetParent(null);
				// pickUpState = PickUpState.NOT_PICKED_UP;
				break;
			case PickUpState.HOLDING_OBJECT:
				DropObject(interactKey);
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
				}
			} else {
				objectToPickUp = null;
			} 
		} 
	}


	public void PickUp(KeyCode key){
		if(Input.GetKeyDown(key)){
			Rigidbody rb = objectToPickUp.GetComponent<Rigidbody>();
			
			if(pickUpState == PickUpState.LOOKING_AT_OBJECT){
				objectToPickUp.transform.SetParent(this.gameObject.transform);
				rb.isKinematic = true;
				rb.useGravity = false;
				rb.freezeRotation = true;
				objectToPickUp.transform.localEulerAngles = Vector3.zero;
				objectToPickUp.transform.localPosition = Vector3.forward;
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

	public void Pour(){
		if(objectToDrop.tag == "Base"){
			objectToDrop.GetComponent<Interactable>().Pour();			
		}
	}

}