using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Main : Scene<TransitionData>
{


	private Text resourceText;

    public Light Spotlight;

    public LayerMask groundLayer;

	// Use this for initialization
	void Start()
	{
//		resourceText = GameObject.Find("ResourceText").GetComponent<Text>();
		Services.MapManager.GenerateMap();


	}

	// Update is called once per frame
	void Update()
	{
        Services.LightManager.Update();

	}

	void InitializeServices()
	{
		Services.Main = this;
		Services.MapManager = GetComponentInChildren<MapManager>();

	}


	internal override void OnEnter(TransitionData data)
	{
		InitializeServices();
		Services.GameManager.currentCamera = GetComponentInChildren<Camera>();
	}
}