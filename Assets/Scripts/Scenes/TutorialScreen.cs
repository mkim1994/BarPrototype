using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialScreen : MonoBehaviour {

    public Material tutmat;
    public Renderer tutrenderer;
	// Use this for initialization
	void Start () {
        Cursor.lockState = CursorLockMode.Locked;
	}
	
	// Update is called once per frame
	void Update () {
        if(Input.GetKeyDown(KeyCode.JoystickButton1)){
            tutrenderer.material = tutmat;
            GetComponent<Animator>().SetTrigger("triggerIntroAnim");
        }
	}

    public void PlayGame(){
        SceneManager.LoadScene("drinkmix_main2");
    }
}
