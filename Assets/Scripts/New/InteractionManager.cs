using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour {

	public GameObject nearestDropzone; 
	Stack<GameObject> interactableGameObjects = new Stack<GameObject>();
	public bool leftHandIsFree = true;
	public bool rightHandIsFree = true;
	private Vector3 rightHandPos;
	private Vector3 leftHandPos;
	public KeyCode pickUpKey;
	public KeyCode actionKey;

	public LayerMask layerMask;
	// Use this for initialization
	void Start () {
		leftHandPos = Vector3.forward + (Vector3.left * 0.5f) + (Vector3.down * 0.25f);
		rightHandPos = Vector3.forward + (Vector3.right * 0.5f) + (Vector3.down * 0.25f);
	}
	
	// Update is called once per frame
	void Update () {
		PickUp(pickUpKey);
		InteractableRay();
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
				//pick it up by parenting.
				// hitObj.transform.SetParent(this.transform);

				//add the interactable gameobject to the stack.
				interactableGameObjects.Push(hitObj);
				
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
				
			}			
		} else {
			//no hit
			Debug.Log("not hitting anything");
		}
	}

	void PickUp(KeyCode key){
		if(Input.GetKeyDown(key)){
			interactableGameObjects.Peek().transform.SetParent(this.transform);
			Interactable interactable = interactableGameObjects.Peek().GetComponent<Interactable>();
			interactable.TweenToHand(leftHandPos);			
		}
	}
	
}
