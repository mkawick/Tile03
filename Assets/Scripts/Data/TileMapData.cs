using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public struct IntVector
{
    public int x, y, z;
}

[System.Serializable]
public class TileMapData : ScriptableObject
{
    [SerializeField]
    public string mapName;

    [SerializeField]
    public Dictionary<IntVector, GameObject> mapStorageBlocks;

   /* [SerializeField]
    public GameObject cursorPrefab;

    [SerializeField]
    public bool isEnabled = true;

    [SerializeField]
    public Material tileTexture;

    [SerializeField]
    public Vector2 planeDimensions;

    [SerializeField]
    public string lastWorkingMapFile;*/
}
