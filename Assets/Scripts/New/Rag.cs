using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Rag : Interactable {

	public Vector3 myRightHandedPourRotation;
	public Vector3 myLeftHandedPourRotation;
	// Use this for initialization
	public override void TwoHandedContextualAction(ObjectInHand _objectInOtherHand){
		//all right handed actions
		if(this.tag == "RightHand"){
			if(_objectInOtherHand == ObjectInHand.Glass){
				Sequence ragSequence = DOTween.Sequence();

				ragSequence.Append(transform.DOLocalMove(Vector3.up, 0.75f, false));
				ragSequence.Append(transform.DOLocalMove(Vector3.down, 0.75f, false));
				// ragSequence.Play();
				ragSequence.SetLoops(-1);
				ragSequence.OnComplete(()=>DoSomething());
			}
		}
		//all left-handed actions
		else if (this.tag == "LeftHand"){
			if(_objectInOtherHand == ObjectInHand.Glass){
				Sequence ragSequence = DOTween.Sequence();

				ragSequence.Append(transform.DOLocalMove(Vector3.up, 0.75f, false));
				ragSequence.Append(transform.DOLocalMove(Vector3.down, 0.75f, false).SetLoops(-1));
				ragSequence.SetLoops(-1);
			}
		}
	} 

	private void DoSomething(){
		Debug.Log("didsomething");
	}
}
