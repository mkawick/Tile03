using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;

public class TableMatrix01 : SerializedMonoBehaviour
{
   // [BoxGroup("ReadOnly table")]
    [TableMatrix(IsReadOnly = true, DrawElementMethod = "DrawCell", HorizontalTitle = "table of colors", SquareCells = true)]
    public MapGrid01[,] ReadOnlyTable2;

    public GameObject testBlock;
    
    //[InfoBox("Right-click and drag column and row labels in order to modify the tables."), PropertyOrder(-10), OnInspectorGUI]
   // private void ShowMessageAtOP() { }

 /*   [BoxGroup("Two Dimensional array without the TableMatrix attribute.")]
    public bool[,] BooleanTable = new bool[15, 6];

    [BoxGroup("ReadOnly table")]
    [TableMatrix(IsReadOnly = true)]
    public int[,] ReadOnlyTable = new int[5, 5];

    [BoxGroup("Labled table")]
    [TableMatrix(HorizontalTitle = "X axis", VerticalTitle = "Y axis")]
    public GameObject[,] LabledTable = new GameObject[15, 10];

    [BoxGroup("Enum table")]
    [TableMatrix(HorizontalTitle = "X axis")]
    public InfoMessageType[,] EnumTable = new InfoMessageType[4,4];

    [BoxGroup("Custom table")]
    [TableMatrix(DrawElementMethod = "DrawColoredEnumElement", ResizableColumns = false)]
    public bool[,] CustomCellDrawing = new bool[30,30];*/
    
    [OnInspectorInit]
    private void CreateData()
    {
        // https://odininspector.com/attributes/table-matrix-attribute
        //Debug.Log("OnInspectorInit");
        const int height = 3;
        const int width = 5;
        ReadOnlyTable2 = new MapGrid01[width, height];
     /*   {
            {new MapGrid01(), new MapGrid01(), new MapGrid01()},
            {new MapGrid01(), new MapGrid01(), new MapGrid01()}/
            {null, null, null},
            {null, null, null},
            {null, null, null}, 
            {null, null, null}
        };*/

        for (int w = 0; w < width; w++)
        {
            for (int h = 0; h < height; h++)
            {
                ReadOnlyTable2[w, h] = new MapGrid01();
                ReadOnlyTable2[w, h].tile = testBlock;
                ReadOnlyTable2[w, h].CreateData();
            }
        }
        /* SquareCelledMatrix = new Texture2D[8, 4]
         {
             { ExampleHelper.GetTexture(), null, null, null },
             { null, ExampleHelper.GetTexture(), null, null },
             { null, null, ExampleHelper.GetTexture(), null },
             { null, null, null, ExampleHelper.GetTexture() },
             { ExampleHelper.GetTexture(), null, null, null },
             { null, ExampleHelper.GetTexture(), null, null },
             { null, null, ExampleHelper.GetTexture(), null },
             { null, null, null, ExampleHelper.GetTexture() },
         };
 
         PrefabMatrix = new Mesh[8, 4]
         {
             { ExampleHelper.GetMesh(), null, null, null },
             { null, ExampleHelper.GetMesh(), null, null },
             { null, null, ExampleHelper.GetMesh(), null },
             { null, null, null, ExampleHelper.GetMesh() },
             { null, null, null, ExampleHelper.GetMesh() },
             { null, null, ExampleHelper.GetMesh(), null },
             { null, ExampleHelper.GetMesh(), null, null },
             { ExampleHelper.GetMesh(), null, null, null },
         };*/
    }

    #if UNITY_EDITOR

        private static bool DrawColoredEnumElement(Rect rect, bool value)
        {
            if (Event.current.type == EventType.MouseDown && rect.Contains(Event.current.mousePosition))
            {
                value = !value;
                GUI.changed = true;
                Event.current.Use();
            }

            UnityEditor.EditorGUI.DrawRect(rect.Padding(1), value ? new Color(0.1f, 0.8f, 0.2f) : new Color(0, 0, 0, 0.5f));

            return value;
        }

    #endif

    private static MapGrid01 DrawCell(Rect rect, MapGrid01 grid)
    {
        // Event.current.type == EventType.MouseDown &&
        // rect.Contains(Event.current.mousePosition)

       // EditorGUI.DrawRect(rect.Padding(2), new Color(0.5f, 0.7f, 0.5f));

        Texture2D texture = grid.GetTexture();
        //GUI.DrawTexture(new Rect(10, 10, 60, 60), texture, ScaleMode.ScaleToFit, true, 10.0F);
        EditorGUI.DrawPreviewTexture (rect, texture);

        return grid;
    }
}


