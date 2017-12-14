using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Rag : Interactable {

	public Vector3 myRightHandedPourRotation;
	public Vector3 myLeftHandedPourRotation;
	public Vector3 myTweenToTableEndRotation;

	public Vector3 myInitHandPosition, myInitHandRotation;

	// Use this for initialization

	public override void Start(){
		// startPos = transform.position;
 		// onTableRot = myTweenToTableEndRotation;
		highlightState = HighlightState.Not_highlighted;
	}
	public override void TwoHandedContextualAction(ObjectInHand _objectInOtherHand){
		//all right handed actions
		if(this.tag == "RightHand"){
			if(_objectInOtherHand == ObjectInHand.Glass){
				RightRagToGlassTween();
			}
		}
		//all left-handed actions
		else if (this.tag == "LeftHand"){
			if(_objectInOtherHand == ObjectInHand.Glass){
				LeftRagToGlassTween();
			}
		}
	} 

	public override void ReturnToInitHandPos(Vector3 _initHandPos, Vector3 _initHandRot){
		transform.DOLocalMove(myInitHandPosition, 0.5f, false);
		transform.DOLocalRotate(myInitHandRotation, 0.5f, RotateMode.Fast);
	}

	private void RightRagToGlassTween(){
		transform.DOLocalRotate(new Vector3 ( 0, -20, 0), 0.25f, RotateMode.Fast);
		Sequence ragToGlassSequence = DOTween.Sequence();
		ragToGlassSequence.Append(transform.DOLocalMove((Vector3.down*0.35f) + Vector3.forward, 0.75f, false));
		ragToGlassSequence.OnComplete(()=>RightHandCleaningTween());
	}
	private void LeftRagToGlassTween(){
		transform.DOLocalRotate(new Vector3 ( 0, 20, 0), 0.25f, RotateMode.Fast);
		Sequence ragToGlassSequence = DOTween.Sequence();
		ragToGlassSequence.Append(transform.DOLocalMove((Vector3.down*0.35f) + Vector3.forward, 0.75f, false));
		ragToGlassSequence.OnComplete(()=>LeftHandCleaningTween());
	}
	private void RightHandCleaningTween(){
		Sequence ragSequence = DOTween.Sequence();
		ragSequence.Append(transform.DOLocalMove((Vector3.down*0.55f) + Vector3.forward, 0.35f, false));
		ragSequence.Append(transform.DOLocalMove((Vector3.down*0.35f) + Vector3.forward, 0.35f, false));
		// ragSequence.Append(transform.DOLocalMove(myInitHandPosition, 0.75f, false));
		ragSequence.SetLoops(-1);
		// ragSequence.OnComplete(()=>ReturnToInitHandPos(myInitHandPosition, myInitHandRotation));
	}

	private void LeftHandCleaningTween(){
		Sequence ragSequence = DOTween.Sequence();		
		ragSequence.Append(transform.DOLocalMove((Vector3.down*0.5f) + Vector3.forward, 0.35f, false));
		ragSequence.Append(transform.DOLocalMove((Vector3.down*0.35f) + Vector3.forward, 0.35f, false));
		// ragSequence.Append(transform.DOLocalMove(myInitHandPosition, 0.75f, false));
		ragSequence.SetLoops(-1);
		// ragSequence.OnComplete(()=>ReturnToInitHandPos(myInitHandPosition, myInitHandRotation));
	}



	public override void TweenToHand(Vector3 _handPos){
		// handPos = _handPos;
		myInitHandPosition = _handPos;
		myInitHandRotation = onTableRot;
		transform.DOLocalMove(_handPos, 1f, false);
		transform.DOLocalRotate(onTableRot, 1f, RotateMode.Fast);
		// Debug.Log("tweening " + this.gameObject.name + " to hand!");
	}

	public override void TweenToTable(Vector3 _tablePos){
		// tablePos = _tablePos;
		transform.DOMove(_tablePos, 1f, false);
		transform.DORotate(myTweenToTableEndRotation, 1f, RotateMode.Fast);

		StartCoroutine(EnableColliderAfterTweenToTable(1f));
		// Debug.Log("tweening " + this.gameObject.name + " to table!");
	}
}
