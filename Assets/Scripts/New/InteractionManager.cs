﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour {

	public GameObject nearestDropzone; 
	public GameObject interactableCurrentlyInRangeAndLookedAt;
	public Stack<GameObject> objectsInHand = new Stack<GameObject>();
	public ObjectInHand objectInLeftHand;
	public ObjectInHand objectInRightHand;
	public List<GameObject> objectsInHandList = new List<GameObject>();

	public GameObject objectInLeftHandGO;
	public GameObject objectInRightHandGO;
	public bool leftHandIsFree;
	public bool rightHandIsFree;
	public bool lookingAtInteractable;
	private Vector3 rightHandPos;
	private Vector3 leftHandPos;
	public KeyCode rightHandPickUpKey;
	public KeyCode leftHandPickUpKey;
	public KeyCode actionKey;

	public LayerMask layerMask;
	// Use this for initialization
	void Start () {
		leftHandIsFree = true;
		rightHandIsFree = true;
		leftHandPos = Vector3.forward + (Vector3.left * 0.5f) + (Vector3.down * 0.25f);
		rightHandPos = Vector3.forward + (Vector3.right * 0.5f) + (Vector3.down * 0.25f);
	}
	
	// Update is called once per frame
	void Update () {
		RightHandPickUp(rightHandPickUpKey);
		LeftHandPickUp(leftHandPickUpKey);
 		FindInteractableRay();
		if(!leftHandIsFree && !rightHandIsFree){
			TwoHandedInteractableAction(actionKey);
		} else if (leftHandIsFree && !rightHandIsFree) {
			//OneHandedInteractableAction(actionKey);
		} else if (!leftHandIsFree && rightHandIsFree){
			//OneHandedInteractableAction(actionKey);
		} 
 	}

	void FindInteractableRay(){
		Ray ray = new Ray(transform.position, transform.forward);
		float rayDist = Mathf.Infinity;
		RaycastHit hit = new RaycastHit();

		if(Physics.Raycast(ray, out hit, rayDist, layerMask)){
			//hit
			GameObject hitObj = hit.transform.gameObject;
			//check if object looked at is an interactable.
			if(hitObj.GetComponent<Interactable>() != null){
				//if the object you're looking at is close enough AND is an interactable, assign it to interactableCIRAL@. 
				//BUT ONLY IF YOU CAN ACTUALLY PICK IT UP.
				if(Vector3.Distance(transform.position, hitObj.transform.position) <= 4){
					interactableCurrentlyInRangeAndLookedAt = hitObj;
					//now we have a reference to what's in range and looked at.	
				} 
			} else {
				//ray hit, but not an interactable
				lookingAtInteractable = false;
				//since you're looking at something but it's not interactable, make this null.
				interactableCurrentlyInRangeAndLookedAt = null;
			}			
		} else {
			//if you're not looking at anything, make this null.
			interactableCurrentlyInRangeAndLookedAt = null;
			//no hit
			Debug.Log("not hitting anything");
		}
	}

	void OneHandedInteractableAction(KeyCode key){

	}

	bool isPouring = false;
	void TwoHandedInteractableAction(KeyCode key){	
		/* 
		Interactable how to get reference to the other one? 
		the interactable needs to know the ff:
			1) Which Interactable is in the other hand
			2) If it's in the left or right hand. 
				- right now this info is in InteractionManager, or also in the Interactable class itself as a tag.				
	
		Another attempt:
			1) When object is in hand, immediately store the Interactable type somewhere.
			2)   
		*/

		if(Input.GetKey(key) && !isPouring){
			foreach (GameObject objectInHand in objectsInHand){
				if(objectInHand.tag == "RightHand"){
					objectInHand.GetComponent<Interactable>().TwoHandedContextualAction(objectInLeftHand);
				} else if (objectInHand.tag == "LeftHand") {
					objectInHand.GetComponent<Interactable>().TwoHandedContextualAction(objectInRightHand);
				}
			}
			isPouring = true;
		} else if (Input.GetKeyUp(key) && isPouring){
			isPouring = false;
			// foreach (GameObject objectInHand in objectsInHand){
			// 	if(objectInHand.tag == "RightHand"){
			// 		objectInHand.GetComponent<Interactable>().TwoHandedContextualAction(objectInLeftHand);
			// 	} else if (objectInHand.tag == "LeftHand") {
			// 		objectInHand.GetComponent<Interactable>().TwoHandedContextualAction(objectInRightHand);
			// 	}
			// }
		}
	}

	void LeftHandPickUp(KeyCode key){
	if(interactableCurrentlyInRangeAndLookedAt != null){
			//pick up stuff.
			if(Input.GetKeyDown(key)){
				interactableCurrentlyInRangeAndLookedAt.transform.SetParent(this.transform);
				Interactable interactable = interactableCurrentlyInRangeAndLookedAt.GetComponent<Interactable>();
				//turn off the collider to avoid catching it in a raycast
				interactable.DisableCollider();
				// interactable.TweenToHand(leftHandPos);
				//both hands free, looking at interactable.			
				if(leftHandIsFree){
					// Debug.Log("both hands are free");
					PickUpInteractableWithLeftHand(interactable);
				}  
				//swap!
				else if (!leftHandIsFree){
					//first check which hand is to be swapped.	
					SwapInteractableInLeftHand(interactable);
				} 
				
			}
		//DROP STUFF
		} else if (interactableCurrentlyInRangeAndLookedAt == null){
			if(Input.GetKeyDown(key)){
				if (!leftHandIsFree){
					DropObject(key);
				} 
			}
		}	
	}
	void RightHandPickUp(KeyCode key){
		if(interactableCurrentlyInRangeAndLookedAt != null){
				//pick up stuff.
			if(Input.GetKeyDown(key)){
				interactableCurrentlyInRangeAndLookedAt.transform.SetParent(this.transform);
				Interactable interactable = interactableCurrentlyInRangeAndLookedAt.GetComponent<Interactable>();
				//turn off the collider to avoid catching it in a raycast
				interactable.DisableCollider();
				// interactable.TweenToHand(leftHandPos);
				//both hands free, looking at interactable.			
				if(rightHandIsFree){
					// Debug.Log("both hands are free");
					PickUpInteractableWithRightHand(interactable);
				}  
				//swap!
				else if (!rightHandIsFree){
					//first check which hand is to be swapped.	
					SwapInteractableInRightHand(interactable);
				} 
				
			}
		//DROP STUFF
		} else if (interactableCurrentlyInRangeAndLookedAt == null){
			if(Input.GetKeyDown(key)){
				if (!rightHandIsFree){
					DropObject(key);
				} 
			}
		}	
	}

	private void PickUpInteractableWithLeftHand(Interactable _interactable){
		_interactable.TweenToHand(leftHandPos);
		_interactable.tag = "LeftHand";
		if(_interactable.gameObject.GetComponent<Base>() != null || _interactable.gameObject.GetComponent<Mixer>() != null){
			//then it's a bottle
			objectInLeftHand = ObjectInHand.Bottle;
		} else if (_interactable.gameObject.GetComponent<Glass>() != null){
			objectInLeftHand = ObjectInHand.Glass;
		} else if (_interactable.gameObject.GetComponent<Rag>() != null){
			objectInLeftHand = ObjectInHand.Rag;
		} 
		leftHandIsFree = false;
		objectInLeftHandGO = interactableCurrentlyInRangeAndLookedAt;
		// objectsInHandList.Add(interactableCurrentlyInRangeAndLookedAt);
		// objectsInHand.Push(interactableCurrentlyInRangeAndLookedAt);
	}

	private void PickUpInteractableWithRightHand(Interactable _interactable){
		_interactable.TweenToHand(rightHandPos);
		_interactable.tag = "RightHand";
		if(_interactable.gameObject.GetComponent<Base>() != null || _interactable.gameObject.GetComponent<Mixer>() != null){
			//then it's a bottle
			objectInRightHand = ObjectInHand.Bottle;
		} else if (_interactable.gameObject.GetComponent<Glass>() != null){
			objectInRightHand = ObjectInHand.Glass;
		} else if (_interactable.gameObject.GetComponent<Rag>() != null){
			objectInRightHand = ObjectInHand.Rag;
		}
		rightHandIsFree = false;
		objectInRightHandGO = interactableCurrentlyInRangeAndLookedAt;
		// objectsInHand.Push(interactableCurrentlyInRangeAndLookedAt);
	}

	private void SwapInteractableInLeftHand(Interactable _interactable){
		_interactable.TweenToHand(leftHandPos);
		_interactable.tag = "LeftHand";				
		_interactable.DisableCollider();
		if(objectInLeftHandGO != null){
			objectInLeftHandGO.tag = "Untagged";
			objectInLeftHandGO.transform.SetParent(null);
			objectInLeftHandGO.GetComponent<Interactable>().TweenToTable(nearestDropzone.transform.position);
			objectInLeftHandGO = null;
			objectInLeftHandGO = _interactable.gameObject;
		}
	}

	private void SwapInteractableInRightHand(Interactable _interactable){
		_interactable.TweenToHand(rightHandPos);
		_interactable.tag = "RightHand";
		_interactable.DisableCollider();
		if(objectInRightHandGO != null){
			objectInRightHandGO.tag = "Untagged";
			objectInRightHandGO.transform.SetParent(null);
			objectInRightHandGO.GetComponent<Interactable>().TweenToTable(nearestDropzone.transform.position);
			objectInRightHandGO = null;
			objectInRightHandGO = _interactable.gameObject;
		}
	}

	private void DropObject(KeyCode key){
		//check which hand to drop object from
		if(key == leftHandPickUpKey){
 			if(objectInLeftHandGO != null){
				objectInLeftHandGO.tag = "Untagged";
				objectInLeftHandGO.transform.SetParent(null);
				objectInLeftHandGO.GetComponent<Interactable>().TweenToTable(nearestDropzone.transform.position);
				objectInLeftHandGO = null;
			}
			leftHandIsFree = true;
			objectInLeftHand = ObjectInHand.None;
		} else if (key == rightHandPickUpKey){
			if(objectInRightHandGO != null){
				objectInRightHandGO.tag = "Untagged";
				objectInRightHandGO.transform.SetParent(null);
				objectInRightHandGO.GetComponent<Interactable>().TweenToTable(nearestDropzone.transform.position);
				objectInRightHandGO = null;
			}
			rightHandIsFree = true;
			objectInRightHand = ObjectInHand.None;
		}
	}

}

	// private void DropLastPickedUpInteractable(){
	// 	if(objectsInHand.Count>0){
	// 		objectsInHand.Peek().GetComponent<Interactable>().TweenToTable(nearestDropzone.transform.position);
	// 		if(objectsInHand.Peek().tag == "RightHand"){
	// 			rightHandIsFree = true;
	// 			objectsInHand.Peek().tag = "Untagged";
	// 			objectsInHand.Pop().transform.SetParent(null);
	// 			objectInRightHand = ObjectInHand.None;
	// 		} else {
	// 			leftHandIsFree = true;
	// 			objectsInHand.Peek().tag = "Untagged";
	// 			objectsInHand.Pop().transform.SetParent(null);
	// 			objectInLeftHand = ObjectInHand.None;
	// 		}	
	// 	}
	// }

	// private void DropLastRemainingInteractable(){
	// 	if(objectsInHandLi.Count>0){
	// 		objectsInHand.Peek().tag = "Untagged";
	// 		objectsInHand.Peek().transform.SetParent(null);
	// 		objectsInHand.Pop().GetComponent<Interactable>().TweenToTable(nearestDropzone.transform.position);
	// 		leftHandIsFree = true;
	// 		rightHandIsFree = true;
	// 		objectInLeftHand = ObjectInHand.None;
	// 		objectInRightHand = ObjectInHand.None;
	// 	}	
	// }


