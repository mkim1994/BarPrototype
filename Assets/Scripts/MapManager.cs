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

    // Use this for initialization
    void Start () {

    }
    
    // Update is called once per frame
    void Update () {
        
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






   /* public IntVector2 CenterIndexOfGrid(){

      
        return new IntVector2 (mapWidth/2, mapLength/2);
    }
*/


}
