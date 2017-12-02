using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour {

	public GameObject nearestDropzone; 
	public GameObject interactableCurrentlyInRangeAndLookedAt;
	public Stack<GameObject> objectsInHand = new Stack<GameObject>();
	public bool leftHandIsFree;
	public bool rightHandIsFree;
	public bool lookingAtInteractable;
	private Vector3 rightHandPos;
	private Vector3 leftHandPos;
	public KeyCode pickUpKey;
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
		PickUp(pickUpKey);
 		InteractableRay();
		Debug.Log("number of items in stack: " + objectsInHand.Count);
 	}

	void InteractableRay(){
		Ray ray = new Ray(transform.position, transform.forward);
		float rayDist = Mathf.Infinity;
		RaycastHit hit = new RaycastHit();

		if(Physics.Raycast(ray, out hit, rayDist, layerMask)){
			//hit
			GameObject hitObj = hit.transform.gameObject;
			//check if object looked at is an interactable.
			if(hitObj.GetComponent<Interactable>() != null){
				//add the interactable gameobject to the stack.
				//only add to stack if less than two elements are in stack, meaning there is one hand free.
				//also check if the interactable is already in the stack
				// if(leftHandIsFree || rightHandIsFree){
				// 	if(!objectsInHand.Contains(hitObj) && objectsInHand.Count <= 1 && 
				// 	Vector3.Distance(hitObj.transform.position, transform.position) < 4)
				// 		objectsInHand.Push(hitObj);
				// }

				//problem now is, the first two objects you see get added to the stack. 
				//that means you can press left click and the top object in the stack will tween to your hand.
				//it has to be the object you're looking at....

				//if the object you're looking at is close enough AND is an interactable, assign it to interactableCIRAL@. 
				//BUT ONLY IF YOU CAN ACTUALLY PICK IT UP.
				if(Vector3.Distance(transform.position, hitObj.transform.position) <= 4){
					interactableCurrentlyInRangeAndLookedAt = hitObj;
					//now we have a reference to what's in range and looked at.	
				} 
				// if(leftHandIsFree && rightHandIsFree){
				// 	Debug.Log("both hands are free");
				// 	int randomHand = Random.Range(0, 2);
				// 	if (randomHand == 0){
				// 		interactable.TweenToHand(leftHandPos);
				// 		interactables.Push(hitObj.transform);
				// 		leftHandIsFree = false;
				// 	} else if (randomHand == 1){
				// 		interactable.TweenToHand(rightHandPos);
				// 		interactables.Push(hitObj.transform);
				// 		rightHandIsFree = false;
				// 	}
				// } else if (leftHandIsFree && !rightHandIsFree){
				// 	interactable.TweenToHand(leftHandPos);	
				// 	interactables.Push(hitObj.transform);
				// 	leftHandIsFree = false;
				// } else if (!leftHandIsFree && rightHandIsFree){
				// 	interactable.TweenToHand(rightHandPos);
				// 	interactables.Push(hitObj.transform);
				// 	rightHandIsFree = false;
				// } 
				// //if both hands already have something equipped, don't pick up anything.
				// else  {
				// 	Debug.Log("lol!");
				// 	// interactables.Peek().SetParent(null);
				// 	// interactables.Peek().GetComponent<Interactable>().TweenToTable(nearestDropzone.transform.position);
				// }
				
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

	void PickUp(KeyCode key){
		if(interactableCurrentlyInRangeAndLookedAt != null){
			//pick up stuff.
			if(Input.GetKeyDown(key)){
				// objectsInHand.Peek().transform.SetParent(this.transform);
				interactableCurrentlyInRangeAndLookedAt.transform.SetParent(this.transform);
				Interactable interactable = interactableCurrentlyInRangeAndLookedAt.GetComponent<Interactable>();
				//turn off the collider to avoid catching it in a raycast
				interactable.DisableCollider();
				interactable.TweenToHand(leftHandPos);
				//both hands free, looking at interactable.			
				if(leftHandIsFree && rightHandIsFree){
					// Debug.Log("both hands are free");
					int randomHand = Random.Range(0, 2);
					if (randomHand == 0){
						// Debug.Log("left hand tween!");
						interactable.TweenToHand(leftHandPos);
						interactable.tag = "LeftHand";
						leftHandIsFree = false;
						objectsInHand.Push(interactableCurrentlyInRangeAndLookedAt);
					} else {
						// Debug.Log("right hand tween!");
						interactable.TweenToHand(rightHandPos);
						interactable.tag = "RightHand";
						rightHandIsFree = false;
						objectsInHand.Push(interactableCurrentlyInRangeAndLookedAt);
					}
				} else if (leftHandIsFree && !rightHandIsFree){
					interactable.TweenToHand(leftHandPos);	
					interactable.tag = "LeftHand";
					leftHandIsFree = false;
					objectsInHand.Push(interactableCurrentlyInRangeAndLookedAt);
				} else if (!leftHandIsFree && rightHandIsFree){
					interactable.TweenToHand(rightHandPos);
					interactable.tag = "RightHand";
					rightHandIsFree = false;				
					objectsInHand.Push(interactableCurrentlyInRangeAndLookedAt);
				} 
				//swap!
				else if (!leftHandIsFree && !rightHandIsFree){
					//first check which hand is to be swapped.	
					Debug.Log("Swap!");

					if(objectsInHand.Peek().tag == "RightHand"){
						interactable.TweenToHand(rightHandPos);
						interactable.tag = "RightHand";
 						interactable.DisableCollider();
						objectsInHand.Peek().tag = "Untagged";
						objectsInHand.Peek().GetComponent<Interactable>().TweenToTable(nearestDropzone.transform.position);
						objectsInHand.Pop().transform.SetParent(null);
						objectsInHand.Push(interactable.gameObject);
					} else {
						interactable.TweenToHand(leftHandPos);
						interactable.tag = "LeftHand";				
						interactable.DisableCollider();
						objectsInHand.Peek().tag = "Untagged";
						objectsInHand.Peek().GetComponent<Interactable>().TweenToTable(nearestDropzone.transform.position);
						objectsInHand.Pop().transform.SetParent(null);
						objectsInHand.Push(interactable.gameObject);
						// Debug.Log("same shit left hand");
					}
				} 
				
			}

		} else if (interactableCurrentlyInRangeAndLookedAt == null){
			if(Input.GetKeyDown(key)){
				if (!leftHandIsFree && !rightHandIsFree){
					//drop the last grabbed object.
					if(objectsInHand.Count>0){
						objectsInHand.Peek().GetComponent<Interactable>().TweenToTable(nearestDropzone.transform.position);
						if(objectsInHand.Peek().tag == "RightHand"){
							rightHandIsFree = true;
							// objectsInHand.Peek().GetComponent<Interactable>().ToggleCollider();
							objectsInHand.Peek().tag = "Untagged";
							objectsInHand.Pop().transform.SetParent(null);
						} else {
							leftHandIsFree = true;
							// objectsInHand.Peek().GetComponent<Interactable>().ToggleCollider();
							objectsInHand.Peek().tag = "Untagged";
							objectsInHand.Pop().transform.SetParent(null);
						}
					}
				} else if (leftHandIsFree || rightHandIsFree){
					//Drop the last remaining object.
					Debug.Log("I should drop the object in my hand!");	if(objectsInHand.Count>0){
						objectsInHand.Peek().tag = "Untagged";
						// objectsInHand.Peek().GetComponent<Interactable>().ToggleCollider();
						objectsInHand.Peek().transform.SetParent(null);
						objectsInHand.Pop().GetComponent<Interactable>().TweenToTable(nearestDropzone.transform.position);
						leftHandIsFree = true;
						rightHandIsFree = true;
					}
				}
			}
		}
	}
	
}
