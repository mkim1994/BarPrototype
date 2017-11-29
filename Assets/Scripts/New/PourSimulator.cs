using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PourSimulator : MonoBehaviour {
	
	private Cocktail cocktail;
	private Vector3 startPos;
	private Vector3 startScale;
	float pourRate = 1f;
	float drainRate = 1f;
	float maxDrinkZ = 1.09f;
	float maxDrinkY = 0.65f;
	float maxDrinkX = 0.65f;


	private float scale; 
	private float drinkZ;
	private float drinkY;
	private float drinkX;
	private Color baseColor;
	private Color diluteColor;

	//add quads for the toppings?

	private MeshRenderer myMesh;
	// Use this for initialization
	void Start () {
		cocktail = GetComponentInParent<Cocktail>();
		startPos = transform.localPosition;
		startScale = transform.localScale;
		drinkZ = transform.localScale.z;
		startScale = new Vector3(startScale.x, startScale.y, drinkZ);
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

		if(drinkZ < maxDrinkZ){
			FillUp();
			cocktail.AddBase(baseType);
		}
		else if(drinkZ >= maxDrinkZ){
			Debug.Log("Drink is full!");
		}
	}

	public void FillUpWithDilute(Ingredients.MixerType mixerType){

		switch (mixerType){
			case Ingredients.MixerType.SODA:
 				myMesh.material.color = Color.red;
				// Debug.Log("pouring soda!");
			break;
			
			case Ingredients.MixerType.JUICE:
 				myMesh.material.color = Color.yellow;
				// Debug.Log("pouring juice!");
			break;
			
			case Ingredients.MixerType.TONIC_WATER:
 				myMesh.material.color = Color.blue;
				// Debug.Log("pouring tonic water!");
			break;
			
			default:
			break;
		}
		// scale += scaleGrowthRate * Time.deltaTime;
		FillUp();
		if(drinkZ < maxDrinkZ){
			FillUp();
			cocktail.AddMixer(mixerType);
		}
		else if(drinkZ >= maxDrinkZ){
			Debug.Log("Drink is full!");
		}
	}

	public void Empty(){
		// Debug.Log("Emptying glass!");
		if(GetComponent<Base>() != null){
			Destroy(GetComponentInParent<Base>());
		}
		if(GetComponent<Mixer>() != null){
			Destroy(GetComponentInParent<Mixer>());
		}
		// transform.localPosition = startPos;
		// if(drinkZ >= 0){
		// 	drinkZ -= drainRate * Time.deltaTime;
		// }

		// if(drinkZ <= 0.01f && drinkY >= 0){
		// 	drinkY -= drainRate * Time.deltaTime;
		// }

		// if(drinkZ <= 0.01f && drinkX >= 0){
		// 	drinkX -= drainRate * Time.deltaTime;
		// }
 		// transform.localScale = new Vector3 (drinkX, drinkY, drinkZ);
		transform.localScale = new Vector3 (0, 0, 0);
	}

	private void FillUp(){
		if(drinkZ <= maxDrinkZ && (maxDrinkY - drinkY) <= 0.01f && (maxDrinkX - drinkX) <= 0.01f){
			drinkZ += pourRate * Time.deltaTime;
		} 

		if(drinkY <= maxDrinkY){
			drinkY += pourRate * Time.deltaTime;
		}

		if(drinkX <= maxDrinkX){
			drinkX += pourRate * Time.deltaTime;
		}

 		transform.localScale = new Vector3 (drinkX, drinkY, drinkZ);
	}
}
