using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dilute : MonoBehaviour {

	public string diluteName;
	public Ingredients.DiluteType diluteType;

	void Start(){
		switch (diluteType){
			case Ingredients.DiluteType.TONIC_WATER:
			diluteName = "Tonic Water";
			break;

			case Ingredients.DiluteType.SODA:
			diluteName = "Soda";
			break;

			case Ingredients.DiluteType.JUICE:
			diluteName = "Juice";
			break;

			default:
			break;
		}
	}

	

}
