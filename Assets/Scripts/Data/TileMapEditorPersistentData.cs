using UnityEngine;
using UnityEditor;

[System.Serializable]
public class TileMapEditorPersistentData : ScriptableObject
{
    [SerializeField]
    public GameObject cursorPrefab;

    [SerializeField]
    public bool isEnabled = true;

    [SerializeField]
    public Material tileTexture;

    [SerializeField]
    public Vector2 planeDimensions = new Vector2(16, 16);

    [SerializeField]
    public Vector3 scale = new Vector3(1, 1, 1);

    [SerializeField]
    public string lastWorkingMapFile;

    // possibly moved into another ScriptableObject
    [SerializeField] TileGroupData[] tilePlacementGroups;
}
