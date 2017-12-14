
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Glass : Interactable {
	private float cleaningOffsetX = -0.25f;
	private Vector3 handPos;
	private Vector3 tablePos;
	public bool tweenToHandIsDone;
	public bool tweenToTableIsDone;
	public GameObject[] stains;
	Interactable interactable;
	public bool isDirty = false;
	public GameObject stainHolder;
	// Use this for initialization
	public override void Start () {
		tweenToHandIsDone = false;
		onTablePos = transform.position;
		onTableRot = transform.localEulerAngles;		
		interactable = GetComponent<Interactable>();
	}
	
	// Update is called once per frame
	public override void Update () {
		if(isDirty){
			ChangeStainAlpha();
			ShrinkLiquid();
			isDirty = true;
		} 

		if(Vector3.Distance(transform.localPosition, handPos) <= 0.1f){
			tweenToHandIsDone = true;
			// Debug.Log("tween to hand is done is " + tweenToHandIsDone);
		}

		if (Vector3.Distance(transform.localPosition, tablePos) <= 0.1f){
			tweenToTableIsDone = true;
			// Debug.Log("Tween to table is done is " + tweenToTableIsDone);
		}
	}

	public void EmptyGlass(){
		transform.localEulerAngles = new Vector3(0, 25, 90f);
		GetComponentInChildren<PourSimulator>().Empty();
	}

	public void StopEmptyGlass(){
		transform.localEulerAngles = onTableRot;
	}

	float newAlpha;
	public void ChangeStainAlpha(){
 		foreach (GameObject stain in stains){
			// Debug.Log(stainMesh.material);
			Material mat = stain.GetComponent<MeshRenderer>().material;
			Color color = new Vector4 (0.5f, 0.5f, 0.5f, newAlpha);
			mat.SetColor("_TintColor", color);
			// Color color = stainMesh.
			// Color color = mat.color;
			// Debug.Log(color.r);
			// Debug.Log(mat.name);
			newAlpha += 0.01f * Time.deltaTime;
			
			// stainMesh.material.color.a 
		}
	}

	public void ShrinkLiquid(){
		PourSimulator liquid = GetComponentInChildren<PourSimulator>();
		liquid.Empty();
	}
	public void ShowStains(){
		// stains.SetActive(true);
		isDirty = true;
	}

	public override void TwoHandedContextualAction(ObjectInHand _objectInOtherHand){
		//all right handed actions
		Debug.Log("Rotating glass!");
		if(this.tag == "RightHand"){
			if(_objectInOtherHand == ObjectInHand.Bottle){
				transform.DOLocalMove(posRightHandedAction, 0.75f, false);
			}
			//Rag
			else if (_objectInOtherHand == ObjectInHand.Rag){
				RightHandToRagTween();
			}
		}
		//all left-handed actions
		else if (this.tag == "LeftHand"){
			//Bottle
			if(_objectInOtherHand == ObjectInHand.Bottle){
				transform.DOLocalMove(posLeftHandedAction, 0.75f, false);
			}
				// transform.DOLocalRotate(myLeftHandedPourRotation, 0.75f, RotateMode.Fast);
			//Rag
			else if (_objectInOtherHand == ObjectInHand.Rag){
				LeftHandToRagTween();
			}
		}
	} 

	// private void RightHandCleaningTween(){
	// 	Sequence glassToRagSequence = DOTween.Sequence();
	// 	glassToRagSequence.Append(transform.DOLocalMove((Vector3.left*cleaningOffsetX) + Vector3.forward + (Vector3.down * 0.5f), 0.25f, false));
	// }

	// private void LeftHandCleaningTween(){
	// 	Sequence glassToRagSequence = DOTween.Sequence();
	// 	glassToRagSequence.Append(transform.DOLocalMove((Vector3.right*cleaningOffsetX) + Vector3.forward + (Vector3.down * 0.5f), 0.25f, false));
	// }

	private void RightHandToRagTween(){
		Sequence cleanSequence = DOTween.Sequence();
		cleanSequence.Append(transform.DOLocalMove((Vector3.left*cleaningOffsetX) + Vector3.forward + (Vector3.down * 0.5f), 0.25f, false));
		// cleanSequence.OnComplete(()=>ReturnToInitHandPos(myInitHandPos, onTableRot));
	}
	private void LeftHandToRagTween(){
		Sequence cleanSequence = DOTween.Sequence();
 		cleanSequence.Append(transform.DOLocalMove((Vector3.right*cleaningOffsetX) + Vector3.forward + (Vector3.down * 0.5f), 0.25f, false));
		// cleanSequence.OnComplete(()=>ReturnToInitHandPos(myInitHandPos, onTableRot));
	}

	// public override void ReturnToInitHandPos(Vector3 _initHandPos, Vector3 _initHandRot){
	// 	transform.DOLocalMove(_initHandPos, 0.5f, false).SetDelay(2.6f);
	// 	transform.DOLocalRotate(_initHandRot, 0.5f, RotateMode.Fast).SetDelay(2.6f);
	// }

	public override void TweenToHand(Vector3 _handPos){
 		myInitHandPos = _handPos;
		myInitHandRot = onTableRot;
		tweensAreActive = true;
		Sequence myTweenToHandSeq = DOTween.Sequence();
		myTweenToHandSeq.Append(transform.DOLocalRotate(onTableRot, 1f, RotateMode.Fast)).OnComplete(()=>SetTweenToInactive());
		transform.DOLocalMove(_handPos, 1f, false);
 	}
}


//-----ORIGINAL------

// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using DG.Tweening;

// public class Glass : Interactable {
// 	private Vector3 handPos;
// 	private Vector3 tablePos;
// 	public bool tweenToHandIsDone;
// 	public bool tweenToTableIsDone;
// 	public GameObject[] stains;
// 	Interactable interactable;
// 	private Vector3 startRot;
// 	public Vector3 startPos;

// 	public bool isDirty = false;

// 	public GameObject stainHolder;
// 	public string glassName = "glass";
// 	// Use this for initialization
// 	void Start () {
// 		tweenToHandIsDone = false;
// 		startPos = transform.position;
// 		startRot = transform.localEulerAngles;		
// 		interactable = GetComponent<Interactable>();
// 	}
	
// 	// Update is called once per frame
// 	void Update () {
// 		if(isDirty){
// 			ChangeStainAlpha();
// 			ShrinkLiquid();
// 			isDirty = true;
// 		} 

// 		if(Vector3.Distance(transform.localPosition, handPos) <= 0.1f){
// 			tweenToHandIsDone = true;
// 			Debug.Log("tween to hand is done is " + tweenToHandIsDone);
// 		}

// 		if (Vector3.Distance(transform.localPosition, tablePos) <= 0.1f){
// 			tweenToTableIsDone = true;
// 			Debug.Log("Tween to table is done is " + tweenToTableIsDone);
// 		}
// 	}

// 	public void EmptyGlass(){
// 		transform.localEulerAngles = new Vector3(0, 25, 90f);
// 		GetComponentInChildren<PourSimulator>().Empty();
// 	}

// 	public void StopEmptyGlass(){
// 		transform.localEulerAngles = startRot;
// 	}

// 	float newAlpha;
// 	public void ChangeStainAlpha(){
//  		foreach (GameObject stain in stains){
// 			// Debug.Log(stainMesh.material);
// 			Material mat = stain.GetComponent<MeshRenderer>().material;
// 			Color color = new Vector4 (0.5f, 0.5f, 0.5f, newAlpha);
// 			mat.SetColor("_TintColor", color);
// 			// Color color = stainMesh.
// 			// Color color = mat.color;
// 			// Debug.Log(color.r);
// 			// Debug.Log(mat.name);
// 			newAlpha += 0.01f * Time.deltaTime;
			
// 			// stainMesh.material.color.a 
// 		}
// 	}

// 	public void ShrinkLiquid(){
// 		PourSimulator liquid = GetComponentInChildren<PourSimulator>();
// 		liquid.Empty();
// 	}
// 	public void ShowStains(){
// 		// stains.SetActive(true);
// 		isDirty = true;
// 	}

// 	public void TweenToHand(Vector3 _handPos){
// 		handPos = _handPos;
// 		transform.DOLocalMove(_handPos, 1f, false);
// 	}

// 	public void TweenToTable(Vector3 _tablePos){
// 		tablePos = _tablePos;
// 		transform.DOMove(_tablePos, 1f, false);
// 	}

// 	public void KillHandTween(){
// 		DOTween.KillAll();
// 	}
// }
