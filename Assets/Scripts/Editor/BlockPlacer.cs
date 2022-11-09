using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TileMapUtils
{
    public TileMapData tileData;

    [SerializeField] GameObject mouseCursor;
    [SerializeField] GameObject placedObjectsRoot;
    [SerializeField] GameObject plane;
    [SerializeField] TileGroupData[] tilePlacementGroups;
    [SerializeField] float gridSize = 1;

    float elevation = 0;
    Vector3 cursorPosition;
    bool paintEnabled = false;
    bool eraseEnabled = false;

    GameObject selectedBlockCursor;

    int blockIndex;
    int bankIndex;

    public void Init(GameObject plane, GameObject mouseCursor, GameObject placedObjectsRoot, float gridSize)
    {
        this.mouseCursor = mouseCursor;
        this.placedObjectsRoot = placedObjectsRoot;
        this.plane = plane;
        this.gridSize = gridSize;

        tileData = new TileMapData();// TODO, load from file
        tileData.mapStorageBlocks = new Dictionary<IntVector, GameObject>();// TODO, create an initializer

        LoadAllTilePlacementGroups();
        SelectNextBankIndex();
    }

    public bool Update()
    {
        bool mouseMoved = false;
        if (Event.current != null && Event.current.isMouse)
        {
            mouseMoved = UpdateCurorPosition();
            MoveBlockCursor();
        }
        PlaceBlock();
        RemoveBlock();

        if (Event.current != null && Event.current.isKey)
        {
            if (Event.current.type == EventType.KeyUp)
            {
                if (Event.current.keyCode == KeyCode.U)
                {
                    plane.transform.position += new Vector3(0, gridSize, 0);
                    elevation++;
                }
                if (Event.current.keyCode == KeyCode.J)
                {
                    plane.transform.position += new Vector3(0, -gridSize, 0);
                    elevation--;
                }
                if (Event.current.keyCode == KeyCode.N)
                {
                    SelectNextBlockCursor();
                }
                if (Event.current.keyCode == KeyCode.B)
                {
                    SelectNextBankIndex();
                }
                if (Event.current.keyCode == KeyCode.P)
                {
                    paintEnabled = false;
                    EnableBlockCursor(paintEnabled);
                }

                if (Event.current.keyCode == KeyCode.O)// erase
                {
                    eraseEnabled = false;
                    EnableBlockCursor(paintEnabled);// if paint was enabled
                }
            }
            else if (Event.current.type == EventType.KeyDown)
            {
                if (Event.current.keyCode == KeyCode.P)
                {
                    paintEnabled = true;
                    EnableBlockCursor(paintEnabled);
                }
                if (Event.current.keyCode == KeyCode.O)// erase
                {
                    eraseEnabled = true;
                    EnableBlockCursor(false);
                }
            }
            

        }
        
        return mouseMoved;
    }

    void EnableBlockCursor(bool isEnabled)
    {
        if (selectedBlockCursor)
            selectedBlockCursor.gameObject.SetActive(paintEnabled);
    }

    bool UpdateCurorPosition()
    {
        Vector3 distanceFromCam = new Vector3(0, -0.5f + elevation* gridSize, 0);
        Plane plane = new Plane(Vector3.up, distanceFromCam);

        Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
        float enter = 0.0f;

        if (plane.Raycast(ray, out enter))
        {
            var worldPosition = ray.GetPoint(enter);
            int x = (int)(Mathf.Round(worldPosition.x) / gridSize * gridSize);// * Mathf.Sign(worldPosition.x)); ; ; ;// ;// ;// ;// on the 1x1 grid
            int z = (int)(Mathf.Round(worldPosition.z) / gridSize * gridSize);// on the 1x1 grid
            float y = worldPosition.y + 0.2f;

            Vector3 constrainedPosition = new Vector3(x, y, z);
            cursorPosition = constrainedPosition;

            if (mouseCursor != null)
            {
                mouseCursor.transform.position = constrainedPosition;
            }

            return true;
        }
        return false;

     /*   Vector3 distanceFromCam = new Vector3(0, -0.5f, 0);
        Plane plane = new Plane(Vector3.up, distanceFromCam);

        Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
        float enter = 0.0f;

        if (plane.Raycast(ray, out enter))
        {
            var worldPosition = ray.GetPoint(enter);
            //Debug.Log(worldPosition);
            cursorObject.transform.position = worldPosition;
        }*/
    }

    private void LoadAllTilePlacementGroups()
    {
        TileGroupData[] test = Resources.LoadAll<TileGroupData>("TileGroupData");
                //Resources.LoadAll("TileGroupData", typeof(TileGroupData));

        if (test.Length == 0)
        {
            Debug.LogError("no file");
        }
        foreach (var t in test)
        {
            Debug.Log(t.name);
        }

        tilePlacementGroups = test;
    }

    void SelectNextBlockCursor()
    {
        if (tilePlacementGroups != null)
        {
            blockIndex++;
            LimitRange(ref bankIndex, tilePlacementGroups.Length);
            var currentBank = tilePlacementGroups[bankIndex];
            if (currentBank != null)
            {
                LimitRange(ref blockIndex, currentBank.tiles.Length);

                var position = Vector3.one;
                if (selectedBlockCursor != null)
                {
                    position = selectedBlockCursor.transform.position;
                    GameObject.DestroyImmediate(selectedBlockCursor);                    
                }
                selectedBlockCursor = GameObject.Instantiate(currentBank.tiles[blockIndex]);
                selectedBlockCursor.transform.position = position;
            }
        }
    }

    void SelectNextBankIndex()
    {
        if (tilePlacementGroups != null)
        {
            bankIndex++;
            LimitRange(ref bankIndex, tilePlacementGroups.Length);
            var currentBank = tilePlacementGroups[bankIndex];
            if (currentBank != null)
            {
                blockIndex = -1;
                SelectNextBlockCursor();
            }
        }
    }

    void LimitRange(ref int value, int highValue)
    {
        if (value < 0)
            value = 0;
        else if (value >= highValue)
            value = 0;
        return;
    }

    void PlaceBlock()
    {
        if (paintEnabled)
        {
            Debug.Log("PlaceBlock");
            if (DoesSpotAlreadyContainBlock(cursorPosition) == false)
            {
                Debug.Log("Add block");
                var obj = AddBlock(cursorPosition);
                if (obj)
                {
                    obj.transform.parent = placedObjectsRoot.transform;
                }
            }
            else
            {
                Debug.Log("PlaceBlock: block already there");
            }
        }
    }

    private void RemoveBlock()
    {
        if (eraseEnabled)
        {
            if (DoesSpotAlreadyContainBlock(cursorPosition) == false)
            {
                return;
            }

            IntVector vec = Convert(cursorPosition);
            if (tileData.mapStorageBlocks.ContainsKey(vec))
            {
                var block = tileData.mapStorageBlocks[vec];
                tileData.mapStorageBlocks.Remove(vec);

                GameObject.DestroyImmediate(block);
            }
        }
    }

    void MoveBlockCursor()
    {
        if (selectedBlockCursor)
        {
            selectedBlockCursor.transform.position = cursorPosition;
        }
    }

    bool DoesSpotAlreadyContainBlock(Vector3 position)
    {
        IntVector vec = Convert(position);
        return tileData.mapStorageBlocks.ContainsKey(vec);
    }

    GameObject AddBlock(Vector3 position)
    {
        if (selectedBlockCursor == null)
            return null;

        var newBlock = GameObject.Instantiate(selectedBlockCursor, position, Quaternion.identity);
        newBlock.layer = LayerMask.GetMask("default");
        IntVector vec = Convert(position);
        tileData.mapStorageBlocks.Add(vec, newBlock);
        return newBlock;
    }

    IntVector Convert(Vector3 position)
    {
        IntVector vec = new IntVector();
        vec.x = (int)(Mathf.Round(position.x) / gridSize);
        vec.y = (int)(Mathf.Round(position.y) / gridSize);
        vec.z = (int)(Mathf.Round(position.z) / gridSize);
        return vec;
    }
}
