using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour {
	public Ingredients.BaseType baseType;
	public string baseName;
	void Start(){
		switch (baseType){
			case Ingredients.BaseType.GIN:
			baseName = "Gin";
			break;

			case Ingredients.BaseType.WHISKY:
			baseName = "Whisky";
			break;

			case Ingredients.BaseType.RUM:
			baseName = "Rum";
			break;

			default:
			break;
		}
	}
}
