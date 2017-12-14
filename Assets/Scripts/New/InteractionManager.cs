using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Yarn.Unity;
public class InteractionManager : MonoBehaviour {
	

	private float maxInteractionDist = 5f;
	public bool isPerformingAction = false;
	public GameObject nearestDropzone; 
	public GameObject interactableCurrentlyInRangeAndLookedAt;

	public GameObject coasterInRangeAndLookedAt;

	public bool lookingAtCoaster = false;
	public Stack<GameObject> objectsInHand = new Stack<GameObject>();
	public ObjectInHand objectInLeftHand;
	public ObjectInHand objectInRightHand;
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
	public GameObject[] dropzoneArray;
	public float[] dropzoneDistance;

	public DialogueRunner dialogueRunner;
	private List<Interactable> allInteractables = new List<Interactable>();
	public List<Interactable> activeInteractables = new List<Interactable>();

	public LayerMask layerMask;

	public LayerMask coasterMask;
	// Use this for initialization
	void Start () {
		dropzoneArray = GameObject.FindGameObjectsWithTag("DropZone");
		leftHandIsFree = true;
		rightHandIsFree = true;
		// leftHandPos = (Vector3.forward*2) + (Vector3.left * 0.5f) + (Vector3.down * 0.25f);
		// rightHandPos = (Vector3.forward*2) + (Vector3.right * 0.5f) + (Vector3.down * 0.25f);
		leftHandPos = new Vector3 (-0.462f, -0.5f, 0.719f);
		rightHandPos = new Vector3 (0.477f, -0.5f, 0.719f);
		dropzoneDistance = new float[dropzoneArray.Length];
		allInteractables.AddRange(FindObjectsOfType<Interactable>());
		Services.Dropzone_Manager.maxDistToPlayer = maxInteractionDist;
		dialogueRunner = FindObjectOfType<DialogueRunner>();
	}
	
	
	// Update is called once per frame
	void Update () {
		// DetectNearestDropZone();
		// FindTheClosestFreeDropZone();

		// CheckForAnyActiveTweens();
		if(!CheckForAnyActiveTweens()){
			RightHandPickUp(rightHandPickUpKey);
			LeftHandPickUp(leftHandPickUpKey);	
			FindSnapTriggerAreaRay();
	 		FindInteractableRay();
		}
			if(!leftHandIsFree && !rightHandIsFree){
				if(objectInLeftHand == ObjectInHand.Bottle && objectInRightHand == ObjectInHand.Bottle){
					if(interactableCurrentlyInRangeAndLookedAt != null){
						OneHandedPour();
						// DualPour(rightActionKey, leftActionKey);
					}
				} else {
					TwoHandedInteractableAction(rightActionKey, leftActionKey);
				}
			}
			
			if (interactableCurrentlyInRangeAndLookedAt != null){
				OneHandedPour();
			}
		// else if (leftHandIsFree && !rightHandIsFree) {
		// 	// RightHandAction(rightActionKey);
		// 	OneHandedAction(leftActionKey);
		// } else if (!leftHandIsFree && rightHandIsFree){
		// 	OneHandedAction(leftActionKey);
		// } 
 	}
	public bool CheckForAnyActiveTweens(){
		//if there are no tweens, i
		foreach (Interactable interactable in allInteractables){
			if(interactable.tweensAreActive){
				//set private tweensAreActive bool to true
 				if(!activeInteractables.Contains(interactable)){
					activeInteractables.Add(interactable);
				}
			} else {
				//if interactables aren't tweening,
 				if(activeInteractables.Contains(interactable)){
					activeInteractables.Remove(interactable);
				}
			}
		}
		if(activeInteractables.Count > 0){
			return true;
		}
		return false;
	}

	void FindSnapTriggerAreaRay(){
		Ray ray = new Ray(transform.position, transform.forward);
		float rayDist = Mathf.Infinity;
		RaycastHit hit = new RaycastHit();

		if(Physics.Raycast(ray, out hit, rayDist, coasterMask)){
			GameObject hitObj = hit.transform.gameObject;
			//only check if you're looking at a coaster if you're holding something
			if(!rightHandIsFree || !leftHandIsFree){
				if(hitObj.GetComponent<SnapTriggerArea>() != null){
					lookingAtCoaster = true;
					coasterInRangeAndLookedAt = hitObj;
				} else {
					lookingAtCoaster = false;
				}
			}
		} else {
			lookingAtCoaster = false;
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
				if(Vector3.Distance(transform.position, hitObj.transform.position) <= maxInteractionDist){
					interactableCurrentlyInRangeAndLookedAt = hitObj;
					//now we have a reference to what's in range and looked at.	
				} 
			} 
			else if ( hitObj.GetComponent<SnapTriggerArea>() != null 
				&& Vector3.Distance(transform.position, hitObj.transform.position) <= maxInteractionDist
				// hitObj.name.Contains("customer_dropzone")
			){
 			} 
			else {
				//ray hit, but not an interactable
				lookingAtInteractable = false;
 				hitObj = null;
				//since you're looking at something but it's not interactable, make this null.
				interactableCurrentlyInRangeAndLookedAt = null;
			}			
		} else {
			//if you're not looking at anything, make this null.
			interactableCurrentlyInRangeAndLookedAt = null;
			//no hit
 		}
	}

	public void DetectNearestDropZone(){
		for (int i = 0; i < dropzoneArray.Length; i++) {
			dropzoneDistance[i] = Vector3.Distance(dropzoneArray[i].transform.position, transform.position);
            for (int j = i+1; j < dropzoneArray.Length; j++) {
                if ( (dropzoneDistance[i] > dropzoneDistance[j]) && (i != j) ) {
					GameObject tempGameObject;
					tempGameObject = dropzoneArray[j];
					dropzoneArray[j] = dropzoneArray[i];
					dropzoneArray[i] = tempGameObject;
                }
            }
			nearestDropzone = dropzoneArray[0];
			// nearestDropzone.GetComponent<DropzoneManager>().RevealNearestDropzone();         
			// if(!dropzoneArray[i].GetComponent<DropzoneManager>().isOccupied){
			// 	nearestDropzone = dropzoneArray[0];
			// } else { 
			// 	nearestDropzone = dropzoneArray[i];
			// 	break;
			// }
			// if(nearestDropzone.GetComponent<DropzoneManager>().isOccupied)
			// 	nearestDropzone = dropzoneArray[i];
			// 	break;
		}
	}

	void OneHandedPour(){
			if(interactableCurrentlyInRangeAndLookedAt.GetComponent<Glass>() != null){
					//case 1: bottle in left hand
				if(!leftHandIsFree && rightHandIsFree){
					if(objectInLeftHand == ObjectInHand.Bottle){
						if(Input.GetKeyDown(leftActionKey) || Input.GetKeyDown(rightActionKey)){
							Interactable leftHandObject = objectInLeftHandGO.GetComponent<Interactable>();
							leftHandObject.OneHandedContextualAction();
							if(objectInLeftHandGO.GetComponent<Base>() != null){
								interactableCurrentlyInRangeAndLookedAt.GetComponentInChildren<PourSimulator>().FillUpWithBase(objectInLeftHandGO.GetComponent<Base>().baseType);
							} else if (objectInLeftHandGO.GetComponent<Mixer>() != null){
								interactableCurrentlyInRangeAndLookedAt.GetComponentInChildren<PourSimulator>().FillUpWithMixer(objectInLeftHandGO.GetComponent<Mixer>().mixerType);
							}
						} else if (Input.GetKeyUp(leftActionKey) || Input.GetKeyUp(rightActionKey)) {
							interactableCurrentlyInRangeAndLookedAt.GetComponentInChildren<PourSimulator>().StopFillingUp();
							Interactable leftHandObject = objectInLeftHandGO.GetComponent<Interactable>();
							leftHandObject.KillAllTweens();
							// DOTween.Kill(leftHandObject);
 							leftHandObject.ReturnToInitHandPos(leftHandObject.myInitHandPos, leftHandObject.myInitHandRot);
							isPerformingAction = false;	
						}
					}
				} 
				//case 2: bottle in right hand
				if(leftHandIsFree && !rightHandIsFree){
					if(objectInRightHand == ObjectInHand.Bottle){
						if(Input.GetKeyDown(leftActionKey) || Input.GetKeyDown(rightActionKey)){
							Interactable rightHandObject = objectInRightHandGO.GetComponent<Interactable>();
							rightHandObject.OneHandedContextualAction();
							if(objectInRightHandGO.GetComponent<Base>() != null){
								interactableCurrentlyInRangeAndLookedAt.GetComponentInChildren<PourSimulator>().FillUpWithBase(objectInRightHandGO.GetComponent<Base>().baseType);
							} else if (objectInRightHandGO.GetComponent<Mixer>() != null){
								interactableCurrentlyInRangeAndLookedAt.GetComponentInChildren<PourSimulator>().FillUpWithMixer(objectInRightHandGO.GetComponent<Mixer>().mixerType);
							}
						} else if (Input.GetKeyUp(leftActionKey) || Input.GetKeyUp(rightActionKey)) {
							interactableCurrentlyInRangeAndLookedAt.GetComponentInChildren<PourSimulator>().StopFillingUp();
							Interactable rightHandObject = objectInRightHandGO.GetComponent<Interactable>();
							rightHandObject.KillAllTweens();
							// DOTween.Kill(rightHandObject);
 							rightHandObject.ReturnToInitHandPos(rightHandObject.myInitHandPos, rightHandObject.myInitHandRot);
							isPerformingAction = false;	
						}
					}
				}

				//case 3: dual wield bottles
				if(!leftHandIsFree && !rightHandIsFree){
					if(objectInLeftHand == ObjectInHand.Bottle && objectInRightHand == ObjectInHand.Bottle){
						if(Input.GetKeyDown(leftActionKey)){
							Interactable leftHandObject = objectInLeftHandGO.GetComponent<Interactable>();
							leftHandObject.OneHandedContextualAction();
							if(objectInLeftHandGO.GetComponent<Base>() != null){
								interactableCurrentlyInRangeAndLookedAt.GetComponentInChildren<PourSimulator>().FillUpWithBase(objectInLeftHandGO.GetComponent<Base>().baseType);
							} else if (objectInLeftHandGO.GetComponent<Mixer>() != null){
								interactableCurrentlyInRangeAndLookedAt.GetComponentInChildren<PourSimulator>().FillUpWithMixer(objectInLeftHandGO.GetComponent<Mixer>().mixerType);
							}
						} else if (Input.GetKeyUp(leftActionKey)) {
							interactableCurrentlyInRangeAndLookedAt.GetComponentInChildren<PourSimulator>().StopFillingUp();
							Interactable leftHandObject = objectInLeftHandGO.GetComponent<Interactable>();
							// leftHandObject.KillAllTweens();
							// DOTween.Kill(leftHandObject);
							// leftHandObject.tweensAreActive = false;
							leftHandObject.KillTweenOnThis();
 							leftHandObject.ReturnToInitHandPos(leftHandObject.myInitHandPos, leftHandObject.myInitHandRot);
							isPerformingAction = false;	
						}

						if(Input.GetKeyDown(rightActionKey)){
							Interactable rightHandObject = objectInRightHandGO.GetComponent<Interactable>();
							rightHandObject.OneHandedContextualAction();
							if(objectInRightHandGO.GetComponent<Base>() != null){
								interactableCurrentlyInRangeAndLookedAt.GetComponentInChildren<PourSimulator>().FillUpWithBase(objectInRightHandGO.GetComponent<Base>().baseType);
							} else if (objectInRightHandGO.GetComponent<Mixer>() != null){
								interactableCurrentlyInRangeAndLookedAt.GetComponentInChildren<PourSimulator>().FillUpWithMixer(objectInRightHandGO.GetComponent<Mixer>().mixerType);
							}
						} else if (Input.GetKeyUp(rightActionKey)) {
							interactableCurrentlyInRangeAndLookedAt.GetComponentInChildren<PourSimulator>().StopFillingUp();
							Base rightHandObject = objectInRightHandGO.GetComponent<Base>();
							// rightHandObject.KillAllTweens();
							// DOTween.Kill(rightHandObject);
							// rightHandObject.tweensAreActive = false;
							// rightHandObject.KillTweenOnThis();
							rightHandObject.KillTweenOnRight();
 							rightHandObject.ReturnToInitHandPos(rightHandObject.myInitHandPos, rightHandObject.myInitHandRot);
							isPerformingAction = false;	
						}
					
					}
				} 
			} else {
				//if interactable is no longer in view.				
				// DOTween.KillAll(true);
			}
		
	}

	void TwoHandedInteractableAction(KeyCode rightKey, KeyCode leftKey){	
		if(!isPerformingAction){
			if(Input.GetKey(leftKey) || Input.GetKey(rightKey)){
				objectInRightHandGO.GetComponent<Interactable>().TwoHandedContextualAction(objectInLeftHand);
				objectInLeftHandGO.GetComponent<Interactable>().TwoHandedContextualAction(objectInRightHand);
				if(objectInLeftHandGO.GetComponent<Base>() != null){
					objectInRightHandGO.GetComponentInChildren<PourSimulator>().FillUpWithBase(objectInLeftHandGO.GetComponent<Base>().baseType);
				} else if (objectInLeftHandGO.GetComponent<Mixer>() != null){
					objectInRightHandGO.GetComponentInChildren<PourSimulator>().FillUpWithMixer(objectInLeftHandGO.GetComponent<Mixer>().mixerType);
				}

				if(objectInRightHandGO.GetComponent<Base>() != null){
					objectInLeftHandGO.GetComponentInChildren<PourSimulator>().FillUpWithBase(objectInRightHandGO.GetComponent<Base>().baseType);
				} else if (objectInRightHandGO.GetComponent<Mixer>() != null){
					objectInLeftHandGO.GetComponentInChildren<PourSimulator>().FillUpWithMixer(objectInRightHandGO.GetComponent<Mixer>().mixerType);
				}
				isPerformingAction = true;
			}
		} else if ((Input.GetKeyUp(rightKey) || Input.GetKeyUp(leftKey)) && isPerformingAction){
 			Interactable leftHandObject = objectInLeftHandGO.GetComponent<Interactable>();
			Interactable rightHandObject = objectInRightHandGO.GetComponent<Interactable>();
			leftHandObject.KillAllTweens();
			leftHandObject.ReturnToInitHandPos(leftHandObject.myInitHandPos, leftHandObject.myInitHandRot);
			rightHandObject.ReturnToInitHandPos(rightHandObject.myInitHandPos, rightHandObject.myInitHandRot);
			//identify which hand has the glass.
			if(objectInLeftHandGO.GetComponent<Glass> () != null){
				objectInLeftHandGO.GetComponentInChildren<PourSimulator>().StopFillingUp();
			} else if(objectInRightHandGO.GetComponent<Glass>() != null){
				objectInRightHandGO.GetComponentInChildren<PourSimulator>().StopFillingUp();
			}
			isPerformingAction = false;
		}
	}

	void LeftHandPickUp(KeyCode key){
	if(interactableCurrentlyInRangeAndLookedAt != null){
			//pick up stuff.
			Interactable interactable = interactableCurrentlyInRangeAndLookedAt.GetComponent<Interactable>();

			if(Input.GetKeyDown(key)){
				interactableCurrentlyInRangeAndLookedAt.transform.SetParent(this.transform);
				// Interactable interactable = interactableCurrentlyInRangeAndLookedAt.GetComponent<Interactable>();
				//turn off the collider to avoid catching it in a raycast
				// interactable.DisableCollider();
				// interactable.TweenToHand(leftHandPos);
				//both hands free, looking at interactable.			
				if(leftHandIsFree && !interactable.tweensAreActive){
					// Debug.Log("both hands are free");
					PickUpInteractableWithLeftHand(interactable);
				}  
				//swap!
				else if (!leftHandIsFree && !interactable.tweensAreActive){
					//first check which hand is to be swapped.	
					SwapInteractableInLeftHand(interactable);
				} 
				
			}
		//DROP STUFF
		} else if (interactableCurrentlyInRangeAndLookedAt == null){
			if(Input.GetKeyDown(key)){
				if (!leftHandIsFree){
					// DOTween.Kill(objectInLeftHandGO);
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
				// interactable.DisableCollider();
				// interactable.TweenToHand(leftHandPos);
				//both hands free, looking at interactable.			
				if(rightHandIsFree && !interactable.tweensAreActive){
					// Debug.Log("both hands are free");
					PickUpInteractableWithRightHand(interactable);
				}  
				//swap!
				else if (!rightHandIsFree && !interactable.tweensAreActive){
					//first check which hand is to be swapped.	
					SwapInteractableInRightHand(interactable);
				} 
				
			}
		//DROP STUFF
		} else if (interactableCurrentlyInRangeAndLookedAt == null){
			if(Input.GetKeyDown(key)){
				if (!rightHandIsFree){
					DOTween.Kill(objectInRightHandGO);
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
		Debug.Log("Tweening to right hand!");
		// _interactable.DisableCollider();
		if(objectInLeftHandGO != null && !objectInLeftHandGO.GetComponent<Interactable>().tweensAreActive){
			objectInLeftHandGO.tag = "Untagged";
			if(!lookingAtCoaster){
				objectInLeftHandGO.GetComponent<Interactable>().TweenToTable(Services.Dropzone_Manager.nearest.transform.position);
			} else if (lookingAtCoaster && coasterInRangeAndLookedAt.GetComponent<SnapTriggerArea>().snapState == SnapTriggerArea.SnapTriggerAreaState.INTERACTABLE_HASBEEN_POSITIONED){
				objectInLeftHandGO.GetComponent<Interactable>().TweenToTable(coasterInRangeAndLookedAt.transform.position);
				Debug.Log("Tweening to table from right hand!");
			}
			// objectInLeftHandGO.GetComponent<Interactable>().TweenToTable(Services.Dropzone_Manager.nearest.transform.position);
			objectInLeftHandGO.transform.SetParent(null);
			objectInLeftHandGO = null;
			objectInLeftHandGO = _interactable.gameObject;
			if(_interactable.gameObject.GetComponent<Base>() != null || _interactable.gameObject.GetComponent<Mixer>() != null){
				//then it's a bottle
				objectInLeftHand = ObjectInHand.Bottle;
			} else if (_interactable.gameObject.GetComponent<Glass>() != null){
				objectInLeftHand = ObjectInHand.Glass;
			} else if (_interactable.gameObject.GetComponent<Rag>() != null){
				objectInLeftHand = ObjectInHand.Rag;
			} 
		}
	}

	private void SwapInteractableInRightHand(Interactable _interactable){
		// _interactable.DisableCollider();
		_interactable.TweenToHand(rightHandPos);
		_interactable.tag = "RightHand";
		Debug.Log("Tweening to right hand!");
		if(objectInRightHandGO != null && !objectInRightHandGO.GetComponent<Interactable>().tweensAreActive){
			objectInRightHandGO.tag = "Untagged";
			if(!lookingAtCoaster){
				objectInRightHandGO.GetComponent<Interactable>().TweenToTable(Services.Dropzone_Manager.nearest.transform.position);
			} else if (lookingAtCoaster && coasterInRangeAndLookedAt.GetComponent<SnapTriggerArea>().snapState == SnapTriggerArea.SnapTriggerAreaState.INTERACTABLE_HASBEEN_POSITIONED){
				objectInRightHandGO.GetComponent<Interactable>().TweenToTable(coasterInRangeAndLookedAt.transform.position);
				Debug.Log("Tweening to table from right hand!");
			}
			// objectInRightHandGO.GetComponent<Interactable>().TweenToTable(Services.Dropzone_Manager.nearest.transform.position);
			objectInRightHandGO.transform.SetParent(null);
			objectInRightHandGO = null;
			objectInRightHandGO = _interactable.gameObject;
			if(_interactable.gameObject.GetComponent<Base>() != null || _interactable.gameObject.GetComponent<Mixer>() != null){
				//then it's a bottle
				objectInRightHand = ObjectInHand.Bottle;
			} else if (_interactable.gameObject.GetComponent<Glass>() != null){
				objectInRightHand = ObjectInHand.Glass;
			} else if (_interactable.gameObject.GetComponent<Rag>() != null){
				objectInRightHand = ObjectInHand.Rag;
			} 
		}
	
	}

	private void DropObject(KeyCode key){
		//check which hand to drop object from
		if(key == leftHandPickUpKey){
 
 			if(objectInLeftHandGO != null && !objectInLeftHandGO.GetComponent<Interactable>().tweensAreActive){
				objectInLeftHandGO.tag = "Untagged";
				if(!lookingAtCoaster){
					objectInLeftHandGO.GetComponent<Interactable>().TweenToTable(Services.Dropzone_Manager.nearest.transform.position);
				} else if (lookingAtCoaster && coasterInRangeAndLookedAt.GetComponent<SnapTriggerArea>().snapState == SnapTriggerArea.SnapTriggerAreaState.INTERACTABLE_IS_OUT){
					objectInLeftHandGO.GetComponent<Interactable>().TweenToTable(coasterInRangeAndLookedAt.transform.position);
				}
				objectInLeftHandGO.transform.SetParent(null);
				objectInLeftHandGO = null;
				leftHandIsFree = true;
				objectInLeftHand = ObjectInHand.None;
			}
		} else if (key == rightHandPickUpKey){
 			if(objectInRightHandGO != null && !objectInRightHandGO.GetComponent<Interactable>().tweensAreActive){
				
				objectInRightHandGO.tag = "Untagged";
				if(!lookingAtCoaster){
					objectInRightHandGO.GetComponent<Interactable>().TweenToTable(Services.Dropzone_Manager.nearest.transform.position);
				} else if (lookingAtCoaster && coasterInRangeAndLookedAt.GetComponent<SnapTriggerArea>().snapState == SnapTriggerArea.SnapTriggerAreaState.INTERACTABLE_IS_OUT){
					objectInRightHandGO.GetComponent<Interactable>().TweenToTable(coasterInRangeAndLookedAt.transform.position);
				}
				objectInRightHandGO.transform.SetParent(null);
				objectInRightHandGO = null;
				rightHandIsFree = true;
				objectInRightHand = ObjectInHand.None;
			}
		}
	}

}



