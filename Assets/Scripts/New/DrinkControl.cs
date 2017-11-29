using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using Yarn.Unity;

public class DrinkControl : MonoBehaviour {
	
	public CheckFloor checkFloor;
	FirstPersonUI hud;
	[SerializeField]GameObject objectToPickUp;
	[SerializeField]GameObject objectToDrop;
	[SerializeField]GameObject objectToSwap;
	private GameObject glassInSight;

	private GameObject dropIndicator;

	private Vector3 dropZone;
	private Vector3 hideDropIndicatorPos;
	private float mindDistanceToInteractable = 4f;
	private string objectName;

	public LayerMask layerMask;
	public LayerMask projectionLayerMask;

	public bool isHoldingObject = false;

	public bool isLookingAtInteractable = false;

	public bool isLookingAtSink = false;

	public bool isPouring = false;
	private bool projectionCreated = false;

  	public KeyCode interactKey;
	
	public DialogueRunner dialogueRunner;

	// Use this for initialization


	public enum PickUpState{
		HOLDING_OBJECT,
		NOT_HOLDING_OR_LOOKING_AT_OBJECT,
		LOOKING_AT_OBJECT,
		NEAR_OBJECTIVE
	}

	// public Ingredients.BaseType baseType;
	PickUpState pickUpState;
	void Start () {
		dropIndicator = Instantiate(Services.Prefabs.DropIndicator, new Vector3(1000, 1000, 1000), Quaternion.identity) as GameObject;
		hideDropIndicatorPos = dropIndicator.transform.position;
		dialogueRunner = GameObject.FindGameObjectWithTag("Dialogue").GetComponent<DialogueRunner>();
		checkFloor = GetComponentInChildren<CheckFloor>();
 		pickUpState = PickUpState.NOT_HOLDING_OR_LOOKING_AT_OBJECT;
		hud = GetComponent<FirstPersonUI>();
	}
	
	// Update is called once per frame
	void Update () {
        if(!dialogueRunner.isDialogueRunning /*&& gameObject.GetComponent<CameraController>().safeToInteract*/){
			ShootRay();
			switch (pickUpState){
				case PickUpState.LOOKING_AT_OBJECT:
				//hide the drop indicator
					dropIndicator.SetActive(false);
					PickUp(interactKey);
					break;
				case PickUpState.HOLDING_OBJECT:
				//set the drop indicator to active.
					DropzoneProjection();
					DropObject(interactKey);
					UseInteractable();
					break;
				case PickUpState.NOT_HOLDING_OR_LOOKING_AT_OBJECT:
					// dropIndicator.SetActive(false);
					break;

				case PickUpState.NEAR_OBJECTIVE:
 					break;
				default:
					break;
			}
		} else if (dialogueRunner.isDialogueRunning){
			hud.HideDescriptionText();
		}
	}

	public void ShootRay(){
		Ray ray = new Ray(transform.position, transform.forward);
		float rayDist = Mathf.Infinity;

		RaycastHit hit = new RaycastHit();

		if(Physics.Raycast(ray, out hit, rayDist, layerMask)){
 			//check if you're looking at an Interactable
			if(hit.transform.GetComponent<Interactable>() != null){
				// Debug.Log(hit.transform.name);
				isLookingAtInteractable = true;
				objectToSwap = hit.transform.gameObject;
				float distanceToInteractable = Vector3.Distance(transform.position, objectToSwap.transform.position);
				if(distanceToInteractable <= mindDistanceToInteractable){
					objectToSwap.GetComponent<Interactable>().ChangeMaterialOnRaycastHit();
				}
				//if you're not holding an object and 
				if(pickUpState != PickUpState.HOLDING_OBJECT && !isHoldingObject && distanceToInteractable <= mindDistanceToInteractable){
					pickUpState = PickUpState.LOOKING_AT_OBJECT;
					objectToPickUp = hit.transform.gameObject;
					objectToPickUp.GetComponent<Interactable>().ChangeMaterialOnRaycastHit();
					//check what kind of Interactable you're looking at for the text description
					if (objectToPickUp.GetComponent<Base>() != null){ //if it's a base, get the baseName
						objectName = objectToPickUp.GetComponent<Base>().baseName; 
						hud.UpdateDescriptionText("Left click to pick up " + objectName);
												// pass to the FirstPersonUI class	
					}
					else if (objectToPickUp.GetComponent<Mixer>() != null){ //if it's a dilute, get the diluteName
						objectName = objectToPickUp.GetComponent<Mixer>().mixerName;
						hud.UpdateDescriptionText("Left click to pick up " + objectName);
					}
					else if (objectToPickUp.GetComponent<Glass>() != null){ //if it's a dilute, get the diluteName
						objectName = objectToPickUp.GetComponent<Glass>().glassName;
						hud.UpdateDescriptionText("Left click to pick up " + objectName);
					}			
  				} else if (pickUpState == PickUpState.HOLDING_OBJECT && isHoldingObject && isLookingAtInteractable && !isLookingAtGlass
				  && distanceToInteractable <= mindDistanceToInteractable){
					if (objectToSwap.GetComponent<Base>() != null){ //if it's a base, get the baseName
						objectName = objectToSwap.GetComponent<Base>().baseName; 
						hud.UpdateDescriptionText("Left click to pick up " + objectName);
												// pass to the FirstPersonUI class	
					}
					else if (objectToSwap.GetComponent<Mixer>() != null){ //if it's a dilute, get the diluteName
						objectName = objectToSwap.GetComponent<Mixer>().mixerName;
						hud.UpdateDescriptionText("Left click to pick up " + objectName);
					}
					else if (objectToSwap.GetComponent<Glass>() != null){ //if it's a dilute, get the diluteName
						objectName = objectToSwap.GetComponent<Glass>().glassName;
						hud.UpdateDescriptionText("Left click to pick up " + objectName);
					}	
				}
			} 
		}	
		else {
			isLookingAtInteractable = false;
			hud.HideDescriptionText();
		}
	}


	public void PickUp(KeyCode key){
		if(Input.GetKeyDown(key)){
			//if you're looking at an object and you're NOT holding anything:
				projectionCreated = false;
				float distanceToInteractable = Vector3.Distance(transform.position, objectToPickUp.transform.position);
				if(pickUpState == PickUpState.LOOKING_AT_OBJECT && !isHoldingObject && isLookingAtInteractable && distanceToInteractable <= mindDistanceToInteractable){
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
					objectToDrop.GetComponent<Interactable>().isHeld = true;
					pickUpState = PickUpState.HOLDING_OBJECT;
				}
		} 
	}

	public bool canDropObject = false;
	public void DropObject(KeyCode key){
 		if(Input.GetKeyDown(key)){
			//if you can't see the floor
			if(objectToDrop != null){
				objectToDrop.GetComponent<MeshRenderer>().enabled = true;

				if (pickUpState == PickUpState.HOLDING_OBJECT && isHoldingObject && !isLookingAtGlass && !isLookingAtInteractable && !isLookingAtSink && canDropObject){
					if(Vector3.Distance(objectToDrop.transform.position, dropZone) <= mindDistanceToInteractable){
						Rigidbody rb = objectToDrop.GetComponent<Rigidbody>();
						objectToDrop.GetComponent<Interactable>().isHeld = false;
						objectToDrop.GetComponent<Collider>().enabled = true;
						isHoldingObject = false;
						rb.useGravity = true;
						rb.freezeRotation = false;
					// rb.constraints = RigidbodyConstraints.FreezeRotationX;
					// rb.constraints = RigidbodyConstraints.FreezeRotationZ;
					// objectToDrop.transform.localPosition = Vector3.forward * 2;
						objectToDrop.transform.position = dropZone + objectToDrop.GetComponent<Interactable>().dropOffset;
						objectToDrop.transform.SetParent(null);
						rb.isKinematic = false;
						// rb.isKinematic = true;
						// rb.isKinematic = false;
						pickUpState = PickUpState.NOT_HOLDING_OR_LOOKING_AT_OBJECT;
					} 
					// rb.isKinematic = true;
					// rb.isKinematic = false;
				} 
				//OBJECT SWAPPING
				else if (pickUpState == PickUpState.HOLDING_OBJECT && isHoldingObject && !isLookingAtGlass && isLookingAtInteractable){
					Rigidbody swaprb = objectToSwap.GetComponent<Rigidbody>();
					Rigidbody rb = objectToDrop.GetComponent<Rigidbody>();
					objectToDrop.GetComponent<Interactable>().isHeld = false;
					objectToDrop.GetComponent<Collider>().enabled = true;
					isHoldingObject = false;
					rb.useGravity = true;
					rb.isKinematic = false;
					rb.freezeRotation = false;
					// rb.constraints = RigidbodyConstraints.FreezeRotationX;
					// rb.constraints = RigidbodyConstraints.FreezeRotationZ;
					objectToDrop.transform.localPosition = Vector3.forward * 2;
					// objectToDrop.transform.localPosition = dropZone;
					objectToDrop.transform.eulerAngles = objectToDrop.GetComponent<Interactable>().startRot;
					objectToDrop.transform.SetParent(null);
					rb.isKinematic = false;

					objectToSwap.transform.SetParent(this.gameObject.transform);
					swaprb.isKinematic = true;
					swaprb.useGravity = false;
					swaprb.freezeRotation = true;
					objectToSwap.transform.localEulerAngles = objectToSwap.GetComponent<Interactable>().startRot;
					objectToSwap.transform.localPosition = Vector3.forward + (Vector3.right * 0.5f) + (Vector3.down * 0.25f);
					objectToSwap.GetComponent<Interactable>().isHeld = true;
					objectToDrop = objectToSwap;
					isHoldingObject = true;
					if (objectToDrop.GetComponent<Base>() != null){ //if it's a base, get the baseName
						objectName = objectToDrop.GetComponent<Base>().baseName; 
						hud.UpdateDescriptionText("Left click to pick up " + objectName);
												// pass to the FirstPersonUI class	
					}
					else if (objectToDrop.GetComponent<Mixer>() != null){ //if it's a dilute, get the diluteName
						objectName = objectToDrop.GetComponent<Mixer>().mixerName;
						hud.UpdateDescriptionText("Left click to pick up " + objectName);
					}
					else if (objectToDrop.GetComponent<Glass>() != null){ //if it's a dilute, get the diluteName
						objectName = objectToDrop.GetComponent<Glass>().glassName;
						hud.UpdateDescriptionText("Left click to pick up " + objectName);
					}	
				} 			
			}
		}
	}

	public void UseInteractable(){
		//if you're holding a base
		FindGlassRay();
		if(objectToDrop != null){
			if(objectToDrop.tag == "Base"){ 
				// Debug.Log("Object to drop is BASE!");
				//if we're holding a base, then look for glass.
				//Need to have a check as to what kind of base is in it.
				if(Input.GetMouseButton(0) && glassInSight != null && !isPouring){
					// Debug.Log(objectToDrop.tag);
					objectToDrop.GetComponent<Interactable>().Pour();
					isPouring = true;
					Ingredients.BaseType myBaseType;
					myBaseType = objectToDrop.GetComponent<Interactable>().baseType;
					glassInSight.GetComponentInChildren<PourSimulator>().FillUpWithBase(myBaseType);
					//check what Base is in the glass
					if(glassInSight.GetComponent<Base>() == null && objectToDrop.GetComponent<Base>() != null){
						// Debug.Log("Base component added!");
						glassInSight.AddComponent<Base>();
						glassInSight.GetComponent<Base>().baseType = objectToDrop.GetComponent<Base>().baseType;	
					} 
					//force whiskey enum if you pour whisky on something else
					//first check if there's a Base in the glass.
					else if (glassInSight.GetComponent<Base>() != null){
						Base baseInGlass = glassInSight.GetComponent<Base>();
						//then, check what kind of base is in glass. If it's NOT whisky, then make it whisky      
						if(baseInGlass.baseType != Ingredients.BaseType.WHISKY 
							/*&& objectToDrop.GetComponent<Base>() != null*/ && objectToDrop.GetComponent<Base>().baseType == Ingredients.BaseType.WHISKY){
							baseInGlass.baseType = Ingredients.BaseType.WHISKY;
						}
					}
				} 
				else {
					isPouring = false;
					objectToDrop.GetComponent<Interactable>().StopPour();
				} 
			} 
			//if the object you're holding is Glass
			else if (objectToDrop.tag == "Glass"){
				//do stuff for glasses here
				if(Input.GetMouseButton(0) && objectToDrop.GetComponent<Glass>() != null && isLookingAtSink){
					objectToDrop.GetComponent<Glass>().EmptyGlass();
				} else {
					objectToDrop.GetComponent<Glass>().StopEmptyGlass();
				}
			} else if (objectToDrop.tag == "Feeling"){
				//do stuff for feeling here
			} 
			
			else if (objectToDrop.tag == "Dilute"){
				// Debug.Log("Object to drop is DiluTE!");
				// FindGlassRay();
				if(Input.GetMouseButton(0) && glassInSight != null && !isPouring){
					// Debug.Log(objectToDrop.tag);
					objectToDrop.GetComponent<Interactable>().Pour();
					isPouring = true;
					//possible place to tell the UI to update drink level?
					Ingredients.MixerType myMixerType;
					myMixerType = objectToDrop.GetComponent<Mixer>().mixerType;
					glassInSight.GetComponentInChildren<PourSimulator>().FillUpWithDilute(myMixerType);
					if(glassInSight.GetComponent<Mixer>() == null && objectToDrop.GetComponent<Mixer>() != null){
						// Debug.Log("Base component added!");
						glassInSight.AddComponent<Mixer>();
						glassInSight.GetComponent<Mixer>().mixerType = objectToDrop.GetComponent<Mixer>().mixerType;	
					} 	
				} 
				else {
					isPouring = false;
					objectToDrop.GetComponent<Interactable>().StopPour();			
				} 
			}
		}

		//if the object you're holding is a Base
	}

	public bool isLookingAtGlass;
	public void FindGlassRay(){
		Ray ray = new Ray(transform.position, transform.forward);
		float rayDist = Mathf.Infinity;

		RaycastHit hit = new RaycastHit();

		if(Physics.Raycast(ray, out hit, rayDist)){
			if(hit.transform.tag == "Glass"){
				isLookingAtGlass = true;
 				glassInSight = hit.transform.gameObject;	
				string objectToDropName;	
				if(objectToDrop != null){
					if (objectToDrop.GetComponent<Base>() != null){ //if it's a base, get the baseName
						objectToDropName = objectToDrop.GetComponent<Base>().baseName; 
						hud.UpdateDescriptionText("Left click to pour " + objectToDropName);
												// pass to the FirstPersonUI class	
					}
					else if (objectToDrop.GetComponent<Mixer>() != null){ //if it's a dilute, get the diluteName
						objectToDropName = objectToDrop.GetComponent<Mixer>().mixerName;
						hud.UpdateDescriptionText("Left click to pour " + objectToDropName);
					}
					else if (objectToDrop.GetComponent<Glass>() != null){ //if it's a dilute, get the diluteName
						objectToDropName = objectToDrop.GetComponent<Glass>().glassName;
						hud.UpdateDescriptionText("Left click to pour " + objectToDropName);
					}	
				}		
			} else if (hit.transform.tag == "Sink"){
				isLookingAtSink = true;
				isLookingAtGlass = false;	
			} 
			else {
				isLookingAtSink = false;
				isLookingAtGlass = false;
				glassInSight = null;
			}
		}
	}



	public void DropzoneProjection(){
		dropIndicator.SetActive(true);
		Ray ray = new Ray(transform.position, transform.forward);
		float rayDist = Mathf.Infinity;
		RaycastHit hit = new RaycastHit();
		if(Physics.Raycast(ray, out hit, rayDist, projectionLayerMask)){
 			if(isHoldingObject){
				if(hit.transform.tag == "Table"){
					float distanceToHit = Vector3.Distance(transform.position, hit.point);
					// if(distanceToHit <= 3f){
					dropZone = hit.point;
					if(distanceToHit <= mindDistanceToInteractable){
						canDropObject = true;
						dropIndicator.transform.position = hit.point;
					} else {
						canDropObject = false;
						dropIndicator.transform.position = hideDropIndicatorPos; 
					}
					// }
 				} else if (hit.transform.tag != "Table"){
					canDropObject = false;
					dropIndicator.SetActive(false);
				}
			} 
		} 
	}
	
 	// public void DropzoneProjection(){
	// 	Ray ray = new Ray(transform.position, transform.forward);
	// 	float rayDist = Mathf.Infinity;
	// 	RaycastHit hit = new RaycastHit();
	// 	if(Physics.Raycast(ray, out hit, rayDist)){
	// 		if(hit.transform.tag == "Table"){
	// 			float distanceToHit = Vector3.Distance(transform.position, hit.point);
	// 			if(tempProjection != objectToDrop){
	// 				tempProjection = Instantiate(objectToDrop, hit.point, Quaternion.identity);
	// 			}
	// 			dropZone = hit.point;
	// 			if(!projectionCreated){					
	// 				tempProjection.SetActive(true);
	// 				projectionCreated =true;
	// 			} 
	// 			if(distanceToHit <= 4f){
	// 				// tempProjection = objectToDrop;
	// 				if(tempProjection != null){
	// 					tempProjection.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
	// 					tempProjection.GetComponent<MeshRenderer>().enabled = false;									
	// 					tempProjection.transform.position = hit.point + transform.up * 0.25f; 
	// 					tempProjection.transform.eulerAngles = tempProjection.GetComponent<Interactable>().startRot;
	// 				}
	// 			} else {
	// 				// tempProjection.SetActive(false);
	// 				if(tempProjection != null){
	// 					tempProjection.GetComponent<MeshRenderer>().enabled = false;									
	// 					tempProjection.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;						
	// 				}
	// 			}
	// 		}
	// 	} else {
	// 		tempProjection = null;
	// 	}
	// }

}