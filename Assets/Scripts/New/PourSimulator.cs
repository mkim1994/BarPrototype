using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PourSimulator : MonoBehaviour {
	
	public enum PourState {
		Pouring_Base,
		Pouring_Mixer,
		Not_pouring
	}

	private Ingredients.BaseType myBaseType;
	private Ingredients.MixerType myMixerType;
	public PourState pourState;
	private Cocktail cocktail;
	private Vector3 startPos;
	private Vector3 startScale;
	float pourRate = 1f;
	float drainRate = 1f;
	float maxDrinkZ = 0.5f;
	float maxDrinkY = 0.65f;
	float maxDrinkX = 0.65f;
	float posZ;
	float startPosZ;
	float maxPosZ = 0.5f;

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
		startPosZ = transform.localPosition.z;
		posZ = startPosZ;
		Debug.Log(startPosZ);
		pourState = PourState.Not_pouring;
		cocktail = GetComponentInParent<Cocktail>();
		startPos = transform.localPosition;
		startScale = transform.localScale;
		drinkZ = transform.localScale.z;
		startScale = new Vector3(startScale.x, startScale.y, drinkZ);
		myMesh = GetComponent<MeshRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		switch (pourState)
		{
			case PourState.Pouring_Base:
				// if(drinkZ < maxDrinkZ && GetComponentInParent<Glass>().tweenToHandIsDone){
				if(drinkZ < maxDrinkZ){	
					FillUp();
 					cocktail.AddBase(myBaseType);
				}
			break;
			case PourState.Pouring_Mixer:
			if(drinkZ < maxDrinkZ){
					FillUp();
 					cocktail.AddMixer(myMixerType);
				}
			break;
			case PourState.Not_pouring:
				myMixerType = Ingredients.MixerType.NO_MIXER;
				myBaseType = Ingredients.BaseType.NO_BASE;
			break;
			default:
			break;
		}

	}

	public void StopFillingUp(){
		pourState = PourState.Not_pouring;
	}
	public void FillUpWithBase(Ingredients.BaseType _baseType){
		pourState = PourState.Pouring_Base;
		// scale += scaleGrowthRate * Time.deltaTime;
		// Debug.Log("Filling up drink with base!");
		myBaseType = _baseType;
		switch (_baseType){
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
 			cocktail.AddBase(_baseType);
		}
		else if(drinkZ >= maxDrinkZ){
			Debug.Log("Drink is full!");
		}
	}

	public void FillUpWithMixer(Ingredients.MixerType _mixerType){
		pourState = PourState.Pouring_Mixer;
		myMixerType = _mixerType;
		Debug.Log("Filling up drink with mixer!");
		switch (_mixerType){
			case Ingredients.MixerType.SODA:
 				myMesh.material.color = Color.red;
				Debug.Log("pouring soda!");
			break;
			
			case Ingredients.MixerType.JUICE:
 				myMesh.material.color = Color.yellow;
				Debug.Log("pouring juice!");
			break;
			
			case Ingredients.MixerType.TONIC_WATER:
 				myMesh.material.color = Color.blue;
				Debug.Log("pouring tonic water!");
			break;
			
			default:
			break;
		}
		// scale += scaleGrowthRate * Time.deltaTime;
		// FillUp();
		if(drinkZ < maxDrinkZ){
			FillUp();
			cocktail.AddMixer(_mixerType);
		}
		else if(drinkZ >= maxDrinkZ){
			Debug.Log("Drink is full!");
		}
	}

	public void Empty(){
		// transform
		// Debug.Log("Emptying glass!");
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
		posZ = startPosZ;
		transform.localScale = startScale;
	}



	private void FillUp(){
		if(drinkZ <= maxDrinkZ && (maxDrinkY - drinkY) <= 0.01f && (maxDrinkX - drinkX) <= 0.01f){
			drinkZ += pourRate * Time.deltaTime;
			if(posZ < maxPosZ){
				posZ += (pourRate*0.35f) * Time.deltaTime;
			}
		} 

		if(drinkY <= maxDrinkY){
			drinkY += pourRate * Time.deltaTime;
		}

		if(drinkX <= maxDrinkX){
			drinkX += pourRate * Time.deltaTime;
		}

		transform.localPosition = new Vector3 (0, 0, posZ);
 		transform.localScale = new Vector3 (drinkX, drinkY, drinkZ);
	}
}
