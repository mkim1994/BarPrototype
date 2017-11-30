using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Glass : MonoBehaviour {
	private Vector3 handPos;
	private Vector3 tablePos;
	public bool tweenToHandIsDone;
	public bool tweenToTableIsDone;
	public GameObject[] stains;
	Interactable interactable;
	private Vector3 startRot;
	public Vector3 startPos;

	public bool isDirty = false;

	public GameObject stainHolder;
	public string glassName = "glass";
	// Use this for initialization
	void Start () {
		tweenToHandIsDone = false;
		startPos = transform.position;
		startRot = transform.localEulerAngles;		
		interactable = GetComponent<Interactable>();
	}
	
	// Update is called once per frame
	void Update () {
		if(isDirty){
			ChangeStainAlpha();
			ShrinkLiquid();
			isDirty = true;
		} 

		if(Vector3.Distance(transform.localPosition, handPos) <= 0.1f){
			tweenToHandIsDone = true;
			Debug.Log("tween to hand is done is " + tweenToHandIsDone);
		}

		if (Vector3.Distance(transform.localPosition, tablePos) <= 0.1f){
			tweenToTableIsDone = true;
			Debug.Log("Tween to table is done is " + tweenToTableIsDone);
		}
	}

	public void EmptyGlass(){
		transform.localEulerAngles = new Vector3(0, 25, 90f);
		GetComponentInChildren<PourSimulator>().Empty();
	}

	public void StopEmptyGlass(){
		transform.localEulerAngles = startRot;
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

	public void TweenToHand(Vector3 _handPos){
		handPos = _handPos;
		transform.DOLocalMove(_handPos, 1f, false);
	}

	public void TweenToTable(Vector3 _tablePos){
		tablePos = _tablePos;
		transform.DOMove(_tablePos, 1f, false);
	}

	public void KillHandTween(){
		DOTween.KillAll();
	}
}
