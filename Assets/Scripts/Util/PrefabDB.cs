using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Prefab DB")]
public class PrefabDB : ScriptableObject {
    [SerializeField]
    private GameObject[] scenes;
    public GameObject[] Scenes { get { return scenes; } }

    [SerializeField]
    private GameObject tile;
    public GameObject Tile { get { return tile; }}

    [SerializeField]
    private GameObject whiskeyGlass;
    public GameObject WhiskeyGlass { get { return WhiskeyGlass; }}

    [SerializeField]
    private GameObject dropIndicator;
    public GameObject DropIndicator { get { return dropIndicator; }}

    [SerializeField]
    private GameObject player;
    public GameObject Player { get { return player; } }
}
