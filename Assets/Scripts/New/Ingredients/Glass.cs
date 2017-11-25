using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glass : MonoBehaviour {

	public GameObject[] stains;
	Interactable interactable;
	private Vector3 startRot;

	public bool isDirty = false;

	public GameObject stainHolder;
	public string glassName = "glass";
	// Use this for initialization
	void Start () {
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

}
