using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PourSimulator : MonoBehaviour {

	private Vector3 startPos;
	private Vector3 startScale;
	float pourRate = 0.5f;
	float scaleGrowthRate = 0.1f;
	float maxDrinkLevel = 0.6f;

	private float scale; 
	private float drinkLevel;
	private Color baseColor;
	private Color diluteColor;

	//add quads for the toppings?

	private MeshRenderer myMesh;
	// Use this for initialization
	void Start () {
		startPos = transform.localPosition;
		startScale = new Vector3 (0,transform.localScale.y,0);
		scale = 0;
		drinkLevel = transform.localPosition.z;
		myMesh = GetComponent<MeshRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void FillUpWithBase(Ingredients.BaseType baseType){
		// scale += scaleGrowthRate * Time.deltaTime;
		
		switch (baseType){
			case Ingredients.BaseType.GIN:
 				myMesh.material.color = Color.white;
				//  Debug.Log("Pouring gin!");
			break;
			case Ingredients.BaseType.WHISKY:
 				myMesh.material.color = Color.yellow;
				//  Debug.Log("Pouring whisky!");
			break;
			case Ingredients.BaseType.RUM:
 				myMesh.material.color = Color.red;
				//  Debug.Log("Pouring rum!");
			break;
			default:
			break;
		}

		if(drinkLevel <= maxDrinkLevel){
			drinkLevel += pourRate * Time.deltaTime;
		}
 		transform.localPosition = new Vector3 (transform.localPosition.x, transform.localPosition.y, drinkLevel);
	}

	public void FillUpWithDilute(Ingredients.DiluteType diluteType){

		switch (diluteType){
			case Ingredients.DiluteType.SODA:
 				myMesh.material.color = Color.red;
				// Debug.Log("pouring soda!");
			break;
			
			case Ingredients.DiluteType.JUICE:
 				myMesh.material.color = Color.yellow;
				// Debug.Log("pouring juice!");
			break;
			
			case Ingredients.DiluteType.TONIC_WATER:
 				myMesh.material.color = Color.blue;
				// Debug.Log("pouring tonic water!");
			break;
			
			default:
			break;
		}
		// scale += scaleGrowthRate * Time.deltaTime;
		if(drinkLevel <= maxDrinkLevel){
			drinkLevel += pourRate * Time.deltaTime;
		}
 		transform.localPosition = new Vector3 (transform.localPosition.x, transform.localPosition.y, drinkLevel);
	}

	public void Empty(){
		// Debug.Log("Emptying glass!");
		if(GetComponent<Base>() != null){
			Destroy(GetComponentInParent<Base>());
		}
		if(GetComponent<Dilute>() != null){
			Destroy(GetComponentInParent<Dilute>());
		}
		transform.localPosition = startPos;
	}
}
