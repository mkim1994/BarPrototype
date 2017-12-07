using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Base : Interactable {
	public Ingredients.BaseType baseType;
	public string baseName;

	protected override void Start(){
		base.Start();
		switch (baseType){
			case Ingredients.BaseType.GIN:
			baseName = "gin";
			break;

			case Ingredients.BaseType.WHISKY:
			baseName = "whiskey";
			break;

			case Ingredients.BaseType.RUM:
			baseName = "rum";
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

	public override void OneHandedContextualAction(){
		if(this.tag == "RightHand"){
			transform.DOLocalRotate(rotLeftHandOnlyAction, 0.75f, RotateMode.Fast);
			// transform.DOLocalMoveY(0, 0.75f, false);
			// transform.DOLocalMoveX(1,)
		} else if (this.tag == "LeftHand"){
			transform.DOLocalRotate(rotLeftHandOnlyAction, 0.75f, RotateMode.Fast);
			// transform.DOLocalMoveY(0, 0.75f, false);
		}	
	}



}
