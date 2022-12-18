using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TileList", menuName = "Data/TileList", order = 1)]
public class TileList : ScriptableObject
{
    [TableList]//(ShowIndexLabels = true, DrawScrollView = true, AlwaysExpanded = true)]
    [BoxGroup("Tiles 2")]
    public MapTile2[] tiles;
}
