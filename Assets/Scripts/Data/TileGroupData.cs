using UnityEngine;

[CreateAssetMenu(fileName = "TileGroup", menuName = "Data/TileGroupData", order = 1)]
public class TileGroupData : ScriptableObject
{
    public string prefabName;

    public TileList tileList;
}