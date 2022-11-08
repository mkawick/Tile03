using UnityEngine;

[CreateAssetMenu(fileName = "TileGroup", menuName = "Data/TileGroup", order = 1)]
public class TileGroupData : ScriptableObject
{
    public string prefabName;

    public GameObject [] tiles;
}