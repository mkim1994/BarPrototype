using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mixer : MonoBehaviour {

	public string mixerName;
	public Ingredients.MixerType mixerType;

	void Start(){
		switch (mixerType){
			case Ingredients.MixerType.TONIC_WATER:
			mixerName = "Tonic Water";
			break;

			case Ingredients.MixerType.SODA:
			mixerName = "Soda";
			break;

			case Ingredients.MixerType.JUICE:
			mixerName = "Juice";
			break;

			default:
			break;
		}
	}

	

}
