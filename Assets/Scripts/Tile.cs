using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {
	public Coord coord { get; private set; }
    public BoxCollider boxCol { get; private set; }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	public void Init(Coord coord_)
	{
		coord = coord_;
		transform.position = new Vector3(coord.x, 0, coord.y);
		Material mat = GetComponent<MeshRenderer>().material;
		if ((coord.x + coord.y) % 2 == 0)
		{
			mat.color = Color.white;
		}
		else
		{
			mat.color = new Color(0.95f, 0.95f, 0.95f, 1f);
		}
		boxCol = GetComponent<BoxCollider>();
		//navQuads = new List<NavQuad>();
	}
}
