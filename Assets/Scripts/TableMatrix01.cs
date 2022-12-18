using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;

public class TableMatrix01 : SerializedMonoBehaviour
{
    public static event System.Action<GameObject> TileSelected;
   // public TileSelected tileSelected;

    // [BoxGroup("ReadOnly table")]
    [TableMatrix(IsReadOnly = false, ResizableColumns = false, 
        HideColumnIndices = true, HideRowIndices = true, 
        DrawElementMethod = "DrawCell", HorizontalTitle = "Tile selector", 
        SquareCells = true)]
    public MapGrid01[,] ReadOnlyTable;

    [TableMatrix(HorizontalTitle = "X axis")]
    public GameObject[,] blocks = new GameObject[5,3];

    private static Vector2 selectedTile = new Vector2(-1, -1);

    [OnInspectorInit]
    private void CreateData()
    {
        //TileSelected.
        // https://odininspector.com/attributes/table-matrix-attribute
        //Debug.Log("OnInspectorInit");
        const int height = 3;
        const int width = 5;
        ReadOnlyTable = new MapGrid01[width, height];

        for (int w = 0; w < width; w++)
        {
            for (int h = 0; h < height; h++)
            {
                ReadOnlyTable[w, h] = new MapGrid01();
                var bloc = blocks[w, h];
                if (bloc == null)
                    bloc = blocks[0, 0];
                ReadOnlyTable[w, h].tile = bloc;
                ReadOnlyTable[w, h].CreateData();
            }
        }
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

    private MapGrid01 DrawCell(Rect rect, MapGrid01 grid, int x, int y)
    {
        if (Event.current.type == EventType.MouseDown &&
            rect.Contains(Event.current.mousePosition))
        {
           // Debug.Log($"Mouse stuff {x}, {y}");//  ReadOnlyTable2[w, h]
            selectedTile = new Vector2(x, y);
            if (TileSelected != null)
            {
                TileSelected.Invoke(grid.tile);
            }
            else
            {
                Debug.Log("TileSelected was null");
            }

            //TileSelected.Invoke(ReadOnlyTable[x, y].tile);
        }
        Texture2D texture = grid.GetTexture();
        EditorGUI.DrawPreviewTexture(rect, texture);

        if (selectedTile.x == x && selectedTile.y == y)
        {
            EditorGUI.DrawRect(rect, new Color(0.2f, 0.6f, 0.9f, 0.2f));

            DrawDropShadow(rect);
        }
        return grid;
    }

    static void DrawDropShadow(Rect rect)
    {
        // drop shadow... really inefficient
        Color dropShadow = new Color(0.4f, 0.4f, 0.4f, 0.4f);
        float shadowDepth = 2;
        float indent = 2;
        Rect right = rect;
        right.Set(right.x + right.width - shadowDepth - 1, right.y + indent, shadowDepth, right.height - indent - 1);

        EditorGUI.DrawRect(right, dropShadow);

        Rect bottom = rect;
        bottom.Set(bottom.x + indent, bottom.y + bottom.height - shadowDepth - 1, bottom.width - indent - 1, shadowDepth);
        EditorGUI.DrawRect(bottom, dropShadow);
    }
}


