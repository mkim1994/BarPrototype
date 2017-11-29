using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour {
	public Ingredients.BaseType baseType;
	public string baseName;

	void Start(){
		switch (baseType){
			case Ingredients.BaseType.GIN:
			baseName = "gin";
			break;

			case Ingredients.BaseType.WHISKY:
			baseName = "whiskey";
			break;

			case Ingredients.BaseType.RUM:
			baseName = "rum";
			break;

			default:
			break;
		}
	}


}
