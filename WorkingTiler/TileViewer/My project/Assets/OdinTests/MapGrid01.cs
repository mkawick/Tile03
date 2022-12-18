
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor.Examples;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class MapGrid01
{
    public bool test;
    
    //public MeshRenderer mapBlock;
 /*   [PreviewField]
    public Object RegularPreviewField;*/

    public GameObject tile;
    private Texture2D cachedImage;
    
    [OnInspectorInit]
    public void CreateData(GameObject newTile = null)
    {
        if (newTile != null)
        {
            tile = newTile;
        }
        cachedImage = AssetPreview.GetAssetPreview(tile);
       // cachedImage = ExampleHelper.GetTexture();
    /*    D = ExampleHelper.GetTexture();
        E = ExampleHelper.GetTexture();*/
    }
    public Texture2D GetTexture()
    {
        //Texture2D GetMiniThumbnail(mapBlock);
        //GUI.DrawTexture(new Rect(10, 10, 60, 60), aTexture, ScaleMode.ScaleToFit, true, 10.0F);

        return cachedImage;

        //return null;
    }
    // public Texture2D texture;
}
