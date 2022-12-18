using System;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;

[CreateAssetMenu]
public class MapTile : MonoBehaviour
{
    [HorizontalGroup("Split", Width = 50), HideLabel, PreviewField(50)]
    //[PreviewField]
    public Texture2D preview;// { get { return null; } }

    [VerticalGroup("Split/Properties")]
    public bool isWalkable;

    [VerticalGroup("Split/Properties")]
    public Type type;


    [VerticalGroup("Split/Properties")]
    [PreviewField]
    public Mesh mesh;

    public enum Type
    {
        Grass, Rock, Desert, Water, EditorOnly
    }
}

// https://odininspector.com/attributes/table-list-attribute
[Serializable]
public class MapTile2 //: SerializedScriptableObject
{
    
    [TableColumnWidth(57, resizable: false), PreviewField(50)]
    //[HorizontalGroup("Split", Width = 50), HideLabel, PreviewField(50)]
    [HideLabel]
    public Texture icon;

    /*  [TextArea(2, 10)]
      public string description;*/

    //[VerticalGroup("Split/Properties")]
    [VerticalGroup("combinedColumn")]
    public bool isWalkable;

    [VerticalGroup("combinedColumn")]
    public Type type;


    [VerticalGroup("Mesh preview")]
    [PreviewField]
    public Mesh mesh;

    public enum Type
    {
        Grass, Rock, Desert, Water, EditorOnly
    }
}
/*
 * [HorizontalGroup("Split", Width = 50), HideLabel, PreviewField(50)]
public Texture2D Icon;

[VerticalGroup("Split/Properties")]
public string MinionName;

[VerticalGroup("Split/Properties")]
public float Health;

[VerticalGroup("Split/Properties")]
public float Damage;
 * */