using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour {

 
    public int mapWidth;
    public int mapLength;
    public Tile[,] map;

    [SerializeField]
    private int numResources;
    [SerializeField]
    private int minResourceDist;
    [SerializeField]
    private int maxTriesProcGen;

    private GameObject selectedTile;
    private float mouseOffset;

    // Use this for initialization
    void Start () {

    }
    
    // Update is called once per frame
    void Update () {
        SelectTile();
    }


    public void GenerateMap()
    {
        map = new Tile[mapWidth, mapLength];
        for (int i = 0; i < mapWidth; i++)
        {
            for (int j = 0; j < mapLength; j++)
            {
                Tile tile = Instantiate(Services.Prefabs.Tile, Services.Main.transform)
                    .GetComponent<Tile>();
                tile.Init(new Coord(i, j));
                map[i, j] = tile;
            }
        }
    }

    void SelectTile(){
        Ray ray = Services.GameManager.currentCamera.ScreenPointToRay(Input.mousePosition);
        if(Input.GetMouseButton(0)){
            if (selectedTile == null)
            {
                RaycastHit hit = new RaycastHit();
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, Services.Main.tileLayer))
                {
                    selectedTile = hit.collider.transform.gameObject;
                    if (selectedTile.GetComponent<Tile>() == null)
                    {
                        selectedTile = selectedTile.transform.parent.gameObject;
                    }

					mouseOffset = Services.GameManager.currentCamera.ScreenToWorldPoint(Input.mousePosition).y - selectedTile.transform.position.y;
                }

            } else{
                selectedTile.transform.position =
                    new Vector3(selectedTile.transform.position.x,
                                Mathf.Clamp(Services.GameManager.currentCamera.ScreenToWorldPoint(Input.mousePosition).y - mouseOffset, 0, Mathf.Infinity),
                                selectedTile.transform.position.z);
            }
        } else{
            selectedTile = null;
        }

    }




   /* public IntVector2 CenterIndexOfGrid(){

      
        return new IntVector2 (mapWidth/2, mapLength/2);
    }
*/


}
