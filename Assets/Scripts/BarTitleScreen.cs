using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using DG.Tweening;

public class BarTitleScreen : MonoBehaviour {

    private bool loaded;
    public LayerMask dialogueOption1;
    public GameObject button;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (!loaded)
        {
            Ray ray = new Ray(transform.position, transform.forward);
            float rayDist = Mathf.Infinity;

            RaycastHit hit = new RaycastHit();
            if (Physics.Raycast(ray, out hit, rayDist, dialogueOption1))
            {

                hit.collider.transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.5f);
                if (Input.GetMouseButtonUp(0))
                {
                    loaded = true;
                    Invoke("LoadNextScene", 1f);
                }
            }
            else
            {

                button.transform.DOScale(new Vector3(1f, 1f, 1f), 0.2f);

            }
        }

	}

    void LoadNextScene(){
        SceneManager.LoadScene("drinkmix_main");
    }
}
