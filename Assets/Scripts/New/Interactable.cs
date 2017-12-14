using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Yarn.Unity;
public class Interactable : MonoBehaviour {
	protected DialogueRunner dialogueRunner;
	public string myName;
	//exposed these to the Inspector because of tween weirdness.
	public Vector3 posRightHandedAction;
	public Vector3 posLeftHandedAction;
	public Vector3 rotRightHandedAction;
	public Vector3 rotLeftHandedAction;

	public Vector3 rotLeftHandOnlyAction;
	public Vector3 rotRightHandOnlyAction;

	public Vector3 handOffset;
	public enum PourState{
		Pouring,
		Not_pouring
	}

	public PourState pourState;
	public enum HighlightState {
		Highlighted,
		Not_highlighted
	}
	public HighlightState highlightState;
	public Vector3 dropOffset;
	public Vector3 rotOffset;

	private Rigidbody rb;
	public bool isHeld;
	public bool tweensAreActive = false;
	// public Transform child;
	// public Ingredients.BaseType baseType;
	// public Ingredients.MixerType mixerType;
	public Vector3 onTableRot;
	public Vector3 onTablePos;

	public Vector3 myInitHandPos;
	public Vector3 myInitHandRot;
	// Use this for initialization
	public virtual void Start () {
		// child = gameObject.transform.Find("MouseOverBottle");
		if(GetComponent<Rigidbody>() != null){
			rb = GetComponent<Rigidbody>();
		}
 		// child = transform.GetChild(0);
		dialogueRunner = FindObjectOfType<DialogueRunner>();
		onTablePos = transform.position;
 		onTableRot = transform.eulerAngles;
		highlightState = HighlightState.Not_highlighted;
	}
	
	// Update is called once per frame
	public virtual void Update () {

		//enable or disable the highlight mesh
		switch(highlightState){
			case HighlightState.Highlighted:
				// child.GetComponent<MeshRenderer>().enabled = true;
  				break;
			case HighlightState.Not_highlighted:
				// child.GetComponent<MeshRenderer>().enabled = false;			
 				break;
			default:
				break;
		}

		DisableHighlightMesh();

		//teleport to original position if off-map
		if(transform.position.y <= -5f){
			if(rb != null){
				rb.isKinematic = true;
			}
			transform.position = onTablePos;
			transform.eulerAngles = onTableRot;
		}

 	}
	public virtual void TwoHandedContextualAction(ObjectInHand _objectInOtherHand){
		// if(_objectInOtherHand == ObjectInHand.Bottle){
		// 	//do left handed animation
		// 	transform.DOLocalRotate(myLeftHandedPourRotation, 0.75f, RotateMode.Fast);
		// } else if (_objectInOtherHand.tag == "LeftHand") {
		// 	//Do right-handed animation
		// 	transform.DOLocalRotate(myRightHandedPourRotation, 0.75f, RotateMode.Fast);
		// }
	}

	public virtual void OneHandedContextualAction(){
		
	} 

	public virtual void StopTwoHandedContextualAction(){
		transform.DOLocalRotate(onTableRot, 0.3f, RotateMode.Fast);
	}
	public virtual void EnableHighlightMesh(){
		highlightState = HighlightState.Highlighted;
	}

	public virtual void DisableHighlightMesh(){
		highlightState = HighlightState.Not_highlighted;
 	}
	public virtual void SetTweenToInactive(){
		tweensAreActive = false;
	}
	//all Interactables can get picked up
	Sequence myTweenToHandSeq;
	public virtual void TweenToHand(Vector3 _handPos){
		// handPos = _handPos;
		tweensAreActive = true;
		myTweenToHandSeq = DOTween.Sequence();
		myTweenToHandSeq.Append(transform.DOLocalRotate(onTableRot, 1f, RotateMode.Fast)).OnComplete(()=>SetTweenToInactive());
		myInitHandPos = _handPos + handOffset;
		myInitHandRot = onTableRot;
		transform.DOLocalMove(_handPos + handOffset, 1f, false);
	
		// transform.DOLocalRotate(onTableRot, 1f, RotateMode.Fast);
		
		// GetComponent<Collider>().enabled = false;
		// StartCoroutine(DisableColliderForTweenToHand(0.5f));
		// Debug.Log("tweening " + this.gameObject.name + " to hand!");
	}

	
	Sequence myTweenToTableSeq;
	//all interactables can be given away, or "unequipped"/dropped.
	public virtual void TweenToTable(Vector3 _tablePos){
		// tablePos = _tablePos;
		myTweenToTableSeq = DOTween.Sequence();
		tweensAreActive = true;
		myTweenToTableSeq.Append(transform.DOMove(_tablePos + dropOffset, 0.5f, false)).OnComplete(()=>SetTweenToInactive());
		transform.DORotate(onTableRot, 0.5f, RotateMode.Fast);
		// GetComponent<Collider>().enabled = true;
		// StartCoroutine(EnableColliderAfterTweenToTable(1f));
 	}

	public virtual void OnTriggerStay(Collider coll){
		if(coll.gameObject.layer == 17 && dialogueRunner.isDialogueRunning){
			this.gameObject.layer = 2;
		} else {
			this.gameObject.layer = 0;
		}
	}
	public virtual void OnTriggerEnter(Collider coll){
		if(coll.gameObject.layer == 17){

		}
	}

	


	//return object to table if dropped onto the floor.
	// public virtual void OnCollisionEnter(Collision coll){
	// 	if(coll.transform.tag == "Floor"){
	// 		rb.isKinematic = true;
	// 		transform.position = onTablePos;
	// 		transform.eulerAngles = onTableRot;
	// 		rb.isKinematic = false;
	// 	}
	// }
	// public virtual void DisableCollider(){
	// 	GetComponent<Collider>().enabled =  false;
	// }

	public virtual IEnumerator DisableColliderForTweenToHand(float delay){
		yield return new WaitForSeconds(delay);
		GetComponent<Collider>().enabled = false;
	}

	public virtual IEnumerator EnableColliderAfterTweenToTable(float delay){
		yield return new WaitForSeconds(delay);
		GetComponent<Collider>().enabled =  true;
	}

	Sequence returnSequence;
	public virtual void ReturnToInitHandPos(Vector3 _initHandPos, Vector3 _initHandRot){
		if(!tweensAreActive){
 			returnSequence = DOTween.Sequence();
			returnSequence.Append(transform.DOLocalMove(_initHandPos, 0.5f, false)).OnComplete(()=>SetTweenToInactive());
			transform.DOLocalRotate(_initHandRot, 0.5f, RotateMode.Fast);
		}
 	}

	public virtual void KillAllTweens(){
		DOTween.KillAll();
		tweensAreActive = false;
		// transform.DOKill(this.transform);
	}

	public virtual void KillTweenOnThis(){
		// DOTween.Kill(this);
		
		tweensAreActive = false;
	}



}


// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using DG.Tweening;

// public class Interactable : MonoBehaviour {

// 	// public Material[] materials;

// 	//offset for when Interactable is dropped onto a SnapTriggerArea. This will vary from model to model.
// 	[SerializeField] Vector3 myPourRotation;
// 	[SerializeField]Vector3 myStopPourRotation;

// 	public enum PourState{
// 		Pouring,
// 		Not_pouring
// 	}

// 	public PourState pourState;
// 	private Glass glass;
// 	public Vector3 dropOffset;
// 	public Vector3 rotOffset;

// 	private Rigidbody rb;
// 	public bool isHeld;
// 	public Transform child;
// 	public enum IsBeingLookedAtState {
// 		LOOKED_AT,
// 		NOT_LOOKED_AT
// 	}

// 	public Ingredients.BaseType baseType;
 
// 	public IsBeingLookedAtState isBeingLookedAtState;

// 	public Vector3 startRot;
// 	public Vector3 startPos;
// 	// Use this for initialization
// 	void Start () {
// 		// child = gameObject.transform.Find("MouseOverBottle");
// 		rb = GetComponent<Rigidbody>();
//  		child = transform.GetChild(0);
// 		startPos = transform.position;
//  		startRot = transform.eulerAngles;
// 		isBeingLookedAtState = IsBeingLookedAtState.NOT_LOOKED_AT;
// 	}
	
// 	// Update is called once per frame
// 	void Update () {

// 		//for the mouse-over texture
// 		switch(isBeingLookedAtState){
// 			case IsBeingLookedAtState.LOOKED_AT:
// 				child.GetComponent<MeshRenderer>().enabled = true;
//   				break;
// 			case IsBeingLookedAtState.NOT_LOOKED_AT:
// 				child.GetComponent<MeshRenderer>().enabled = false;
//  				break;
// 			default:
// 				break;
// 		}

// 		ChangeMaterialOnRaycastMiss();

// 		//teleport to original position if off-map
// 		if(transform.position.y <= -5f){
// 			rb.isKinematic = true;
// 			transform.position = startPos;
// 			transform.eulerAngles = startRot;
// 			rb.isKinematic = false;
// 		}
//  	}

// 	 public void CheckForTween(){
// 		 if(glass != null){
// 		 }
// 	 }

// 	public void Pour(){
// 		transform.DOLocalRotate(myPourRotation, 0.75f, RotateMode.Fast);
// 	} 

// 	public void StopPour(){
// 		// transform.localEulerAngles = startRot;
// 		// StartCoroutine(StopPourDelayed(0.76f));		
// 		transform.DOLocalRotate(startRot, 0.3f, RotateMode.Fast);
// 	}
// 	IEnumerator StopPourDelayed(float delay){
// 		yield return new WaitForSeconds(delay);
// 		// transform.localEulerAngles = startRot;
// 		transform.DOLocalRotate(startRot, 0.3f, RotateMode.Fast);
// 	}
// 	public virtual void ChangeMaterialOnRaycastHit(){
// 		isBeingLookedAtState = IsBeingLookedAtState.LOOKED_AT;
// 	}

// 	public virtual void ChangeMaterialOnRaycastMiss(){
// 		isBeingLookedAtState = IsBeingLookedAtState.NOT_LOOKED_AT;
//  	}

// 	public virtual void TweenToHand(Vector3 _handPos){
// 		// handPos = _handPos;
// 		transform.DOLocalMove(_handPos, 1f, false);
// 	}

// 	public void TweenToTable(Vector3 _tablePos){
// 		// tablePos = _tablePos;
// 		transform.DOMove(_tablePos, 1f, false);
// 	}

// 	void OnTriggerEnter(Collider coll){
// 		//check if Trigger is a SnapTriggerArea
// 		if(coll.GetComponent<SnapTriggerArea>()!= null){
// 			coll.GetComponent<SnapTriggerArea>().posOffset = dropOffset;
// 		}
// 		//check if trigger is the floor
		
// 	}

// 	void OnCollisionEnter(Collision coll){
// 		if(coll.transform.tag == "Floor"){
// 			rb.isKinematic = true;
// 			transform.position = startPos;
// 			transform.eulerAngles = startRot;
// 			rb.isKinematic = false;
// 		}
// 	}
// }
