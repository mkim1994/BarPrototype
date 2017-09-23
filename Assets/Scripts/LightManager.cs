using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManager {

    private Vector3 goal;
    private Vector3 spotlightDirForTheLevel;
    private Vector3 lastRandomTileDir;

    private KeyCode randomizeTileKey = KeyCode.R;

    

	// Use this for initialization
	public void Start () {
        spotlightDirForTheLevel = GetRandomTile();
        Debug.Log(spotlightDirForTheLevel);
    }

    // Update is called once per frame
    public void Update () {
        //Ray ray = Services.GameManager.currentCamera.ScreenPointToRay(Input.mousePosition);

        RandomizeTileToBeLookedAt(randomizeTileKey);

        Ray ray = new Ray(Services.Main.Spotlight.transform.position, spotlightDirForTheLevel);
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, Services.Main.groundLayer))
		{
            Vector3 target = hit.point;
            Services.Main.Spotlight.transform.LookAt(target);
		}

	}


    Vector3 GetRandomTile() {
        Vector3 randomTileDir;
        Vector3 randomTilePos = Services.MapManager.map[Random.Range(0, Services.MapManager.mapWidth - 1), Random.Range(0, Services.MapManager.mapLength - 1)].transform.position;
        Vector3 spotlightPos = Services.Main.Spotlight.transform.position;
        randomTileDir = randomTilePos - spotlightPos;
        return randomTileDir;
    }

    void RandomizeTileToBeLookedAt(KeyCode key) {
        if (Input.GetKeyDown(key))
        {
            spotlightDirForTheLevel = GetRandomTile();
            if(spotlightDirForTheLevel == lastRandomTileDir){
                spotlightDirForTheLevel = GetRandomTile();                
            }
        }
    }

    

}
