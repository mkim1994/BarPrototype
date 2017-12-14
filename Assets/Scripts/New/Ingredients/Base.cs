using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Base : Interactable {
	public Ingredients.BaseType baseType;
	public override void Start(){
		base.Start();
		switch (baseType){
			case Ingredients.BaseType.GIN:
			myName = "gin";
			break;

			case Ingredients.BaseType.WHISKY:
			myName = "whiskey";
			break;

			case Ingredients.BaseType.RUM:
			myName = "rum";
			break;

			default:
			break;
		}
	}

	public override void TwoHandedContextualAction(ObjectInHand _objectInOtherHand){
		//all right handed actions
		if(this.tag == "RightHand"){
			if(_objectInOtherHand == ObjectInHand.Glass)
				transform.DOLocalRotate(rotRightHandedAction, 0.75f, RotateMode.Fast);
				transform.DOLocalMoveY(0, 0.75f, false);
		}
		//all left-handed actions
		else if (this.tag == "LeftHand"){
			if(_objectInOtherHand == ObjectInHand.Glass)
				transform.DOLocalRotate(rotLeftHandedAction, 0.75f, RotateMode.Fast);
				transform.DOLocalMoveY(0, 0.75f, false);
		}
	} 

	public override void StopTwoHandedContextualAction(){
		transform.DOLocalRotate(onTableRot, 0.3f, RotateMode.Fast);
	}

	Sequence leftPourSequence;
	Sequence rightPourSequence;
	public override void OneHandedContextualAction(){
		if(this.tag == "RightHand"){
			if(!tweensAreActive){
				rightPourSequence = DOTween.Sequence();
				rightPourSequence.Append(transform.DOLocalRotate(rotRightHandOnlyAction, 0.75f, RotateMode.Fast)).OnComplete(()=>SetTweenToInactive());
				tweensAreActive = true;
			}
		} else if (this.tag == "LeftHand"){
			if(!tweensAreActive){
				leftPourSequence = DOTween.Sequence();
				leftPourSequence.Append(transform.DOLocalRotate(rotLeftHandOnlyAction, 0.75f, RotateMode.Fast)).OnComplete(()=>SetTweenToInactive());
				tweensAreActive = true;
			}
		}	
	}

	public override void KillTweenOnThis(){
		leftPourSequence.Kill(false);
		// rightPourSequence.Kill(false);
		tweensAreActive = false;
	}

	public void KillTweenOnRight(){
		rightPourSequence.Kill(false);
		tweensAreActive = false;
	}



}
