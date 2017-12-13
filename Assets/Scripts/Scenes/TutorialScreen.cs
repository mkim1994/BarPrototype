using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialScreen : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if(Input.GetKeyDown(KeyCode.JoystickButton1)){
            GetComponent<Animator>().SetTrigger("triggerIntroAnim");
        }
	}

    public void PlayGame(){
        SceneManager.LoadScene("drinkmix_main2");
    }
}
