using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour {

	public bool isPerformingAction = false;
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
	public KeyCode leftActionKey;
	public KeyCode rightActionKey;

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
			TwoHandedInteractableAction(rightActionKey, leftActionKey);
			LeftHandAction(leftActionKey);
			RightHandAction(rightActionKey);
		} else if (leftHandIsFree && !rightHandIsFree) {
			RightHandAction(rightActionKey);
		} else if (!leftHandIsFree && rightHandIsFree){
			LeftHandAction(leftActionKey);
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
	void LeftHandAction(KeyCode leftKey){
		//only do this if (1) object in other hand is glass, or (2) if you're looking at a glass on the table.
		if(Input.GetKeyDown(leftKey) || Input.GetKeyDown(rightActionKey) && !isPerformingAction){
		//CASE 1: Object in left hand is BASE or MIXER, right hand is glass
			
			if((objectInLeftHandGO.GetComponent<Base>() != null || objectInLeftHandGO.GetComponent<Mixer>() != null) 
				&& objectInRightHand == ObjectInHand.Glass){
				objectInRightHandGO.GetComponent<Interactable>().TwoHandedContextualAction(objectInLeftHand);
				objectInLeftHandGO.GetComponent<Interactable>().TwoHandedContextualAction(objectInRightHand);
				// Debug.Log("You should be pouring with your left hand!");
				// isPerformingAction = true;
			}
		//CASE 2: If you're looking at glass
		 	else if(interactableCurrentlyInRangeAndLookedAt != null){
				 //do different pouring animation
				 if(interactableCurrentlyInRangeAndLookedAt.GetComponent<Glass>() != null && objectInLeftHand == ObjectInHand.Bottle && objectInRightHand != ObjectInHand.Glass)
				 	Debug.Log("Pour with your left to something on the table!");
			}
		} else if (Input.GetKeyUp(leftKey) || Input.GetKeyUp (rightActionKey)){
			Debug.Log("Killing tween!");
			if(!leftHandIsFree && !rightHandIsFree){
				Interactable leftHandObject = objectInLeftHandGO.GetComponent<Interactable>();
				Interactable rightHandObject = objectInRightHandGO.GetComponent<Interactable>();
				leftHandObject.KillAllTweens();
				leftHandObject.ReturnToInitHandPos(leftHandObject.myInitHandPos, leftHandObject.myInitHandRot);
				// rightHandObject.KillAllTweens();
				rightHandObject.ReturnToInitHandPos(rightHandObject.myInitHandPos, rightHandObject.myInitHandRot);
			}
		}
	}

	void RightHandAction(KeyCode rightKey){
		if(Input.GetKeyDown(rightKey) && !isPerformingAction){
		//CASE 1: Object in left hand is BASE or MIXER, right hand is glass
			if((objectInRightHandGO.GetComponent<Base>() != null || objectInRightHandGO.GetComponent<Mixer>() != null) 
				&& !rightHandIsFree && objectInLeftHand == ObjectInHand.Glass){
				objectInRightHandGO.GetComponent<Interactable>().TwoHandedContextualAction(objectInLeftHand);
				objectInLeftHandGO.GetComponent<Interactable>().TwoHandedContextualAction(objectInRightHand);
				isPerformingAction = true;
			} 
		}
	}

	void TwoHandedInteractableAction(KeyCode rightKey, KeyCode leftKey){	
		if(Input.GetKey(rightKey) && !isPerformingAction){
			if(Input.GetKey(leftKey)){
				objectInRightHandGO.GetComponent<Interactable>().TwoHandedContextualAction(objectInLeftHand);
				objectInLeftHandGO.GetComponent<Interactable>().TwoHandedContextualAction(objectInRightHand);
				isPerformingAction = true;
			}
		} else if ((Input.GetKeyUp(rightKey) || Input.GetKeyUp(leftKey)) && isPerformingAction){
			Debug.Log("Stop performing action!");
			isPerformingAction = false;
		}

		// if((Input.GetKey(rightKey) || Input.GetKey(leftKey)) && !isPerformingAction){
		// 	foreach (GameObject objectInHand in objectsInHand){
		// 		if(objectInHand.tag == "RightHand"){
		// 			objectInHand.GetComponent<Interactable>().TwoHandedContextualAction(objectInLeftHand);
		// 		} else if (objectInHand.tag == "LeftHand") {
		// 			objectInHand.GetComponent<Interactable>().TwoHandedContextualAction(objectInRightHand);
		// 		}
		// 	}
		// 	isPerformingAction = true;
		// } else if ((Input.GetKeyUp(rightKey) || Input.GetKeyUp(leftKey)) && isPerformingAction){
		// 	isPerformingAction = false;
		// }
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


