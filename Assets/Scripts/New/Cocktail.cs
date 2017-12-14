using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cocktail : MonoBehaviour{

	private Ingredients.BaseType baseType;
	private Ingredients.MixerType mixerType;

	private float errorMargin = 5f;
	public float whiskyVolume;
	public float ginVolume;
	public float rumVolume;
	public float juiceVolume;
	public float sodaVolume;
	public float tonicVolume;
	
	public float totalVolume;
	public float requestedAlcoholPercentage;
	public float requestedWhiskeyPercentage;
	public float requestedGinPercentage;
	public float requestedRumPercentage;
	public float requestedJuicePercentage;
	public float requestedSodaPercentage;
	public float requestedTonicPercentage;

	public Ingredients.BaseType requestedBaseType;
	public Ingredients.MixerType requestedMixerType;

	public float alcoholPercentage;
	private float drinkVolume;

	PourSimulator pourSim;

	void Start(){
		pourSim = GetComponentInChildren<PourSimulator>();
		requestedBaseType = Ingredients.BaseType.NO_BASE;
		requestedMixerType = Ingredients.MixerType.NO_MIXER;
		requestedAlcoholPercentage = -1f;
	}
	void Update(){
		totalVolume = whiskyVolume + ginVolume + rumVolume + juiceVolume + sodaVolume + tonicVolume;
		totalVolume = Mathf.Clamp (totalVolume, 0, 26f);
		alcoholPercentage = ((ginVolume + rumVolume + whiskyVolume)*0.35f/totalVolume) * 100;
		if(Input.GetKeyDown(KeyCode.E)){
			Debug.Log(DrinkStrength());
			// isRightDrink(alcoholPercentage, baseType, mixerType);
		}
	}
	//for future use in evaluating cocktails for narrative purposes
	// public static void EvaluateCocktail(List<Ingredients.BaseType> base_, List<Ingredients.DiluteType> dilute_, 
	// 									List<Ingredients.ToppingType> topping_, float alcoholLevel_, 
	// 									float satisfactionLevel_){

	// }
	public void AddBase(Ingredients.BaseType baseType_){
		switch(baseType_){
			case Ingredients.BaseType.GIN:
			ginVolume = ginVolume + pourSim.drinkZ;
			ginVolume = Mathf.Clamp(ginVolume, 0, 26f);
			break;
			case Ingredients.BaseType.WHISKY:
			whiskyVolume = whiskyVolume + pourSim.drinkZ;
			whiskyVolume = Mathf.Clamp(whiskyVolume, 0, 26f);
			break;
			case Ingredients.BaseType.RUM:
			rumVolume = rumVolume + pourSim.drinkZ;
			rumVolume = Mathf.Clamp(rumVolume, 0, 26f);
			break;
			default:
			break;
		}
	}

	public void AddMixer(Ingredients.MixerType mixerType_){
		switch (mixerType_)
		{
			case Ingredients.MixerType.JUICE:
			juiceVolume = juiceVolume + pourSim.drinkZ;
			break;
			case Ingredients.MixerType.SODA:
			sodaVolume = sodaVolume + pourSim.drinkZ;
			break;
			case Ingredients.MixerType.TONIC_WATER:
			tonicVolume = tonicVolume + pourSim.drinkZ;
			break;
			default:
			break;
		}
	}

	public void AddTopping(){
		
	}

	public void EmptyCocktail(){
		whiskyVolume = 0;
		rumVolume = 0;
		ginVolume = 0;
		tonicVolume = 0;
		sodaVolume = 0;
		juiceVolume = 0;
		totalVolume = 0;
	}

	public int DrinkStrength(){
		int drinkStrength = -1;
		if(alcoholPercentage >= 25f){
			Debug.Log("strong drink you've got there!");
			return 0;
		} else if (alcoholPercentage >= 15f && alcoholPercentage < 25f){
			Debug.Log("this is just right!");
			return 1;
		} else if (alcoholPercentage >= 0 && alcoholPercentage < 15f){
			Debug.Log("ahh easy drinking");
			return 2;
		}
		return drinkStrength;
	}

	public bool isRightDrink(float alcoholPercentage, Ingredients.BaseType baseType, Ingredients.MixerType mixerType){
		
		//first check what kind of request it was.
		bool isRightDrink_ = false;
		//customer doesn't care how much alcohol is in drink, but cares about BOTH ingredients
		if(requestedAlcoholPercentage == -1f){
			//then check for requested ingredients
			if(baseType == requestedBaseType && mixerType == requestedMixerType)
				Debug.Log("Drink is correct!");
				return true;
		}
		//customer doesn't care about how much alcohol is in drink, but cares about the base
		if(requestedAlcoholPercentage == -1f && mixerType == requestedMixerType){
			if(baseType == requestedBaseType)
				Debug.Log("Drink is correct!");
				return true;
		}
		//customer cares how much alcohol is in drink, doesn't care about ingredients
	 	if (requestedAlcoholPercentage > 0){
			if(Mathf.Abs(requestedAlcoholPercentage-alcoholPercentage)<= errorMargin)
				Debug.Log("Drink is correct!");
				return true;
		} 
		
		return isRightDrink_;
	}

	



}
