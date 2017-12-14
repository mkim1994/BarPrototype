using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

public class SnapTriggerArea : DropzoneManager {

	private Text descriptionText;
	private FirstPersonUI hud;
 	public GameObject interactable;
	
	public int evaluateDrink;
	private Vector3 snapPos;
	private Vector3 snapRot;
	
	public Vector3 posOffset;
	public Vector3 rotOffset;

	public DialogueRunner dialogueRunner;
	public enum SnapTriggerAreaState{
		INTERACTABLE_IS_IN,
		INTERACTABLE_IS_OUT,
		INTERACTABLE_IS_POSITIONED,
        INTERACTABLE_HASBEEN_POSITIONED

	}

	public SnapTriggerAreaState snapState;

	public override void Start () {
		base.Start();
		snapPos = transform.parent.position;
		snapRot = transform.parent.eulerAngles;
		snapState = SnapTriggerAreaState.INTERACTABLE_IS_OUT;
		hud = GameObject.Find("FirstPersonCharacter").GetComponent<FirstPersonUI>();
		dialogueRunner = FindObjectOfType<DialogueRunner>();
  	}
	
	// Update is called once per frame
	void Update () {
		// DeactivateCollidersDuringDialogue();
		switch(snapState){
			case SnapTriggerAreaState.INTERACTABLE_IS_IN:
     			break;
			case SnapTriggerAreaState.INTERACTABLE_IS_OUT:
				// SnapToTarget();
			    break;
			case SnapTriggerAreaState.INTERACTABLE_IS_POSITIONED:
			    EvaluateDrink();
				// ActivateStains();
			    //Detect drink type.
			    break;
			default:
			    break;
		}
		
	}

	void DeactivateCollidersDuringDialogue(){
		if(interactable != null){
			if(dialogueRunner.isDialogueRunning){
				interactable.layer = 2;
				this.gameObject.layer = 2;
			} else {
				interactable.layer = 0;
				this.gameObject.layer = 17;
			}
		}
	}

	void OnTriggerEnter(Collider interactable_){
		if(interactable_.GetComponent<Interactable>() != null){
			//assign object that just got placed in trigger area to "interactable"
			interactable = interactable_.gameObject;
			snapState = SnapTriggerAreaState.INTERACTABLE_IS_POSITIONED;
 		}
	}

	void OnTriggerExit(Collider interactable_){
		if(interactable_.GetComponent<Interactable>() != null)
			interactable = null;
			snapState = SnapTriggerAreaState.INTERACTABLE_IS_OUT;
	}

	void SnapToTarget(){
		if(!interactable.GetComponent<Interactable>().isHeld){
			Debug.Log("Snapping to target!");
			hud.HideDescriptionText();
			interactable.transform.position = snapPos + posOffset;
			// interactable.transform.eulerAngles = snapRot + rotOffset;
			// interactable.GetComponent<Rigidbody>().isKinematic = true;
			snapState = SnapTriggerAreaState.INTERACTABLE_IS_POSITIONED;
		}
	}

	void ActivateStains(){
		if(interactable.GetComponent<Glass>() != null){
			Glass glass = interactable.GetComponent<Glass>();
			glass.ShowStains();
		}
	}

	void EvaluateDrink(){

		//2 is get glass of whiskey!
		//-1 is get NOT whiskey!
		//1 is full bottle 
		if(interactable.GetComponent<Glass>() != null){
			Base thisDrink = interactable.GetComponent<Base>();
			Cocktail thisCocktail = interactable.GetComponent<Cocktail>();
			//correct!
			if(thisCocktail.whiskyVolume > 0){
				evaluateDrink = 2;
			} 
			//NOT WHISKEY, but some other drink 
			else if (thisCocktail.whiskyVolume <= 0){
				evaluateDrink = -1;
			} else if (thisCocktail.totalVolume <= 0){
				evaluateDrink = -2;
			}
			// else if (thisCocktail) 		   
		    // if (thisDrink.baseType == Ingredients.BaseType.WHISKY &&
            //     thisDrink.GetComponent<Glass>() != null){
            //     evaluateDrink = 2;
            // }
			// else if(thisDrink.baseType == Ingredients.BaseType.WHISKY){
			// 	evaluateDrink = 1;
			// 	// Debug.Log("Whiskey dropped!");
            // }else{
			// 	evaluateDrink = -1;
			// 	// Debug.Log("THIS IS NOT WHISKEY!");
            // }
        } 
		//if it's a bottle of whiskey
		else if (interactable.GetComponent<Glass>() == null && interactable.GetComponent<Base> () != null 
		&& interactable.GetComponent<Base>().baseType == Ingredients.BaseType.WHISKY){
			evaluateDrink = 1;
		}
		//if it's not a glass nor whiskey
		else {
            evaluateDrink = -1;
        }
        snapState = SnapTriggerAreaState.INTERACTABLE_HASBEEN_POSITIONED;
	}

}
