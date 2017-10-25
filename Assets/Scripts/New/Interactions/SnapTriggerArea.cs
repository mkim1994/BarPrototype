using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapTriggerArea : MonoBehaviour {

	
 	private GameObject interactable;
	
	public int evaluateDrink;
	private Vector3 snapPos;
	private Quaternion snapRot;
	
	public Vector3 offset;
	
	public enum SnapTriggerAreaState{
		INTERACTABLE_IS_IN,
		INTERACTABLE_IS_OUT,
		INTERACTABLE_IS_POSITIONED

	}

	public SnapTriggerAreaState snapState;

	void Start () {
		snapPos = transform.parent.position;
		snapRot = transform.parent.rotation;
		snapState = SnapTriggerAreaState.INTERACTABLE_IS_OUT;
  	}
	
	// Update is called once per frame
	void Update () {
		switch(snapState){
			case SnapTriggerAreaState.INTERACTABLE_IS_IN:
    			SnapToTarget();
    			break;
			case SnapTriggerAreaState.INTERACTABLE_IS_OUT:
			    break;
			case SnapTriggerAreaState.INTERACTABLE_IS_POSITIONED:
			    EvaluateDrink();
			    //Detect drink type.
			    break;
			default:
			    break;
		}
		
	}

	void OnTriggerEnter(Collider interactable_){
		if(interactable_.GetComponent<Interactable>() != null){
			interactable = interactable_.gameObject;
			snapState = SnapTriggerAreaState.INTERACTABLE_IS_IN;
 		}
	}

	void OnTriggerExit(Collider interactable_){
		if(interactable_.GetComponent<Interactable>() != null)
			interactable = null;
			snapState = SnapTriggerAreaState.INTERACTABLE_IS_OUT;
	}

	void SnapToTarget(){
		if(!interactable.GetComponent<Interactable>().isHeld){
			interactable.transform.position = snapPos + offset;
			interactable.transform.rotation = snapRot;
			snapState = SnapTriggerAreaState.INTERACTABLE_IS_POSITIONED;
		}
	}

	void EvaluateDrink(){
		if(interactable.GetComponent<Base>() != null){
			Base thisDrink = interactable.GetComponent<Base>();
			if(thisDrink.baseType == Ingredients.BaseType.WHISKY){
				evaluateDrink = 1;
				Debug.Log("Whiskey dropped!");
			} else{
				evaluateDrink = -1;
				Debug.Log("THIS IS NOT WHISKEY!");
			}
		}
	}

}
