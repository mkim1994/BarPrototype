using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class DrinkControl : MonoBehaviour {
	FirstPersonUI hud;
	[SerializeField]GameObject objectToPickUp;
	[SerializeField] GameObject objectToDrop;
	private GameObject glassInSight;

	private string objectName;

	public LayerMask layerMask;

	public bool isHoldingObject = false;

	public bool isLookingAtInteractable = false;

  	public KeyCode interactKey;
	// Use this for initialization

	public enum PickUpState{
		HOLDING_OBJECT,
		NOT_HOLDING_OR_LOOKING_AT_OBJECT,
		LOOKING_AT_OBJECT,
		NEAR_OBJECTIVE
	}

	public Ingredients.BaseType baseType;
	PickUpState pickUpState;
	void Start () {
 		pickUpState = PickUpState.NOT_HOLDING_OR_LOOKING_AT_OBJECT;
		hud = GetComponent<FirstPersonUI>();

	}
	
	// Update is called once per frame
	void Update () {
		ShootRay();
		switch (pickUpState){
			case PickUpState.LOOKING_AT_OBJECT:
				PickUp(interactKey);
				break;
			case PickUpState.HOLDING_OBJECT:
				DropObject(interactKey);
				UseInteractable();
				break;
			case PickUpState.NOT_HOLDING_OR_LOOKING_AT_OBJECT:
				break;

			case PickUpState.NEAR_OBJECTIVE:
				
				break;
			default:
				break;
		}
	}

	public void ShootRay(){
		Ray ray = new Ray(transform.position, transform.forward);
		float rayDist = 2f;

		RaycastHit hit = new RaycastHit();
		Debug.DrawRay(transform.position, transform.forward * 2f, Color.red);

		if(Physics.Raycast(ray, out hit, rayDist, layerMask)){
 			//check if you're looking at an Interactable
			// if(hit.transform.tag == "Glass" || hit.transform.tag == "Base" || hit.transform.tag == "Dilute"){
			if(hit.transform.GetComponent<Interactable>() != null){
				isLookingAtInteractable = true;
				if(pickUpState != PickUpState.HOLDING_OBJECT && !isHoldingObject){
					pickUpState = PickUpState.LOOKING_AT_OBJECT;
					objectToPickUp = hit.transform.gameObject;
					objectToPickUp.GetComponent<Interactable>().ChangeMaterialOnRaycastHit();
					//check what kind of Interactable you're looking at for the text description
					if (objectToPickUp.GetComponent<Base>() != null){ //if it's a base, get the baseName
						objectName = objectToPickUp.GetComponent<Base>().baseName; 
						hud.UpdateDescriptionText("Left click to pick up " + objectName);						// pass to the FirstPersonUI class	
					}
					else if (objectToPickUp.GetComponent<Dilute>() != null){ //if it's a dilute, get the diluteName
						objectName = objectToPickUp.GetComponent<Dilute>().diluteName;
						hud.UpdateDescriptionText("Left click to pick up " + objectName);
					}
					else if (objectToPickUp.GetComponent<Glass>() != null){ //if it's a dilute, get the diluteName
						objectName = objectToPickUp.GetComponent<Glass>().name;
						hud.UpdateDescriptionText("Left click to pick up " + objectName);
					}			
  				}
			} 
		} else {
			isLookingAtInteractable = false;
			hud.HideDescriptionText();
		}
	}


	public void PickUp(KeyCode key){
		if(Input.GetKeyDown(key)){
			//if you're looking at an object and you're NOT holding anything:
				if(pickUpState == PickUpState.LOOKING_AT_OBJECT && !isHoldingObject && isLookingAtInteractable){
					isHoldingObject = true;
					Rigidbody rb = objectToPickUp.GetComponent<Rigidbody>();
					objectToPickUp.transform.SetParent(this.gameObject.transform);
					objectToPickUp.GetComponent<Collider>().enabled = false;
					rb.isKinematic = true;
					rb.useGravity = false;
					rb.freezeRotation = true;
					hud.UpdateDescriptionText("Left click to drop " + objectName);
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

			if (pickUpState == PickUpState.HOLDING_OBJECT && isHoldingObject){
				objectToDrop.GetComponent<Collider>().enabled = true;
				isHoldingObject = false;
				rb.useGravity = true;
				rb.isKinematic = false;
				rb.freezeRotation = false;
				objectToDrop.transform.localPosition = Vector3.forward * 2;
 				objectToDrop.transform.SetParent(null);
				pickUpState = PickUpState.NOT_HOLDING_OR_LOOKING_AT_OBJECT;
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
				Debug.Log(objectToDrop.tag);
				objectToDrop.GetComponent<Interactable>().Pour();
				Ingredients.BaseType myBaseType;
				myBaseType = objectToDrop.GetComponent<Interactable>().baseType;
				glassInSight.GetComponentInChildren<PourSimulator>().FillUpWithBase(myBaseType);	
			} 
			else {
				objectToDrop.GetComponent<Interactable>().StopPour();
			} 
		} else if (objectToDrop.tag == "Glass"){
			//do stuff for glasses here
		} else if (objectToDrop.tag == "Feeling"){
			//do stuff for feeling here
		} else if (objectToDrop.tag == "Dilute"){
			FindGlassRay();
			if(Input.GetMouseButton(1) && glassInSight != null){
				Debug.Log(objectToDrop.tag);
				objectToDrop.GetComponent<Interactable>().Pour();
				//possible place to tell the UI to update drink level?
				Ingredients.DiluteType myDiluteType;
				myDiluteType = objectToDrop.GetComponent<Dilute>().diluteType;
				glassInSight.GetComponentInChildren<PourSimulator>().FillUpWithDilute(myDiluteType);	
			} 
			else {
				objectToDrop.GetComponent<Interactable>().StopPour();
			} 
		}
	}

	public void FindGlassRay(){
		Ray ray = new Ray(transform.position, transform.forward);
		float rayDist = 10f;

		RaycastHit hit = new RaycastHit();

		if(Physics.Raycast(ray, out hit, rayDist)){
			if(hit.transform.tag == "Glass"){
 				glassInSight = hit.transform.gameObject;		
				if(objectToDrop != null){
					hud.UpdateDescriptionText("Right click to pour " + objectName);
				}		
			} else {
				glassInSight = null;
			}
		}
	}

}