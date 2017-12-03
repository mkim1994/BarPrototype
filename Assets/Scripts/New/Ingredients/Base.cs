using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Base : Interactable {
	public Ingredients.BaseType baseType;
	public string baseName;
	public Vector3 myRightHandedPourRotation;
	public Vector3 myLeftHandedPourRotation;

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
				transform.DOLocalRotate(myRightHandedPourRotation, 0.75f, RotateMode.Fast);
		}
		//all left-handed actions
		else if (this.tag == "LeftHand"){
			if(_objectInOtherHand == ObjectInHand.Glass)
				transform.DOLocalRotate(myLeftHandedPourRotation, 0.75f, RotateMode.Fast);
		}
	} 

	public override void StopTwoHandedContextualAction(){
		transform.DOLocalRotate(startRot, 0.3f, RotateMode.Fast);
	}



}
