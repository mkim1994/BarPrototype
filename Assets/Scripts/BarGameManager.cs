using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class BarGameManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if(Input.GetKeyUp(KeyCode.R) && SceneManager.GetActiveScene().name == "drinkmix_main"){
            SceneManager.LoadScene("drinkmix_titlescreen");
        }
        if(Input.GetKeyUp(KeyCode.Escape)){
            Application.Quit();
        }
	}
}
