using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialScreen : MonoBehaviour {

    public Material tutmat;
    public Renderer tutrenderer;
    bool called;
	// Use this for initialization
	void Start () {
        Cursor.lockState = CursorLockMode.Locked;
        called = false;
	}
	
	// Update is called once per frame
	void Update () {
        if(!called && Input.GetKeyDown(KeyCode.JoystickButton1)){
            GetComponent<AudioSource>().Play();
            called = true;
            tutrenderer.material = tutmat;
            GetComponent<Animator>().SetTrigger("triggerIntroAnim");
        }
	}

    public void PlayGame(){
        SceneManager.LoadScene("drinkmix_main2");
    }
}
