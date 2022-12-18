using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;


// This is the main tile editor and works well.

[InitializeOnLoad]
public class TilemapEditor : EditorWindow
{
    TileMapEditorPersistentData persistentData;
    GameObject cursorObject;
    [SerializeField] GameObject plane;
    [SerializeField] GameObject placedObjectsRoot;
    TileMapUtils mapper;

    TableMatrix01 simpleMap; // needs to be a list

    public void Awake()
    {
        InitAllData();
    }

    //public Plane plane;
    void OnGUI()
    {
        GUILayout.Label("testing",
            EditorStyles.boldLabel);

        bool isEnabled = EditorGUILayout.Toggle("Is enabled", persistentData.isEnabled);
        if (persistentData.isEnabled != isEnabled)
        {
            persistentData.isEnabled = isEnabled;
            cursorObject.gameObject.SetActive(isEnabled);
            /* if(isEnabled)
             {
                 plane.gameObject.SetActive(true);
             }*/
            plane.gameObject.SetActive(isEnabled);
            SavePersistentData();
        }

        EditorGUILayout.BeginHorizontal();
        GameObject changedObj = (GameObject)EditorGUILayout.ObjectField("Cursor", persistentData.cursorPrefab, typeof(GameObject), true);//, EditorStyles.objectField);

        if (persistentData.cursorPrefab != changedObj)
        {
            persistentData.cursorPrefab = changedObj;
            SavePersistentData();
            CreateCursor(changedObj);
        }

       

        EditorGUILayout.EndHorizontal();

        GUILayout.Space(40);
        if (GUILayout.Button("reset"))
        {
            DestroyImmediate(placedObjectsRoot);
            DestroyImmediate(plane);
            DestroyImmediate(cursorObject);
            InitAllData();
        }
    }

    [MenuItem("Window/TilemapEditor")]
    public static void ShowWindow()
    {
        var window = GetWindow<TilemapEditor>("TilemapEditor");
        window.autoRepaintOnSceneChange = true;
    }

    private void OnSceneGUI(SceneView sceneView)
    {
        Handles.BeginGUI();

        if (cursorObject != null)
        {
            cursorObject.gameObject.SetActive(true);

            if (persistentData.isEnabled)
            {
                if (mapper.Update() == false)
                {
                    // this is the default plane when the mapper fails to raycast
                    Vector3 distanceFromCam = new Vector3(0, -0.5f, 0);
                    Plane plane = new Plane(Vector3.up, distanceFromCam);

                    Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
                    float enter = 0.0f;

                    if (plane.Raycast(ray, out enter))
                    {
                        var worldPosition = ray.GetPoint(enter);
                        //Debug.Log(worldPosition);
                        cursorObject.transform.position = worldPosition;
                    }
                }
            }
            else
            {
                cursorObject.gameObject.SetActive(false);
            }
        }
        else
        {
            InitAllData();
        }
        Handles.EndGUI();


    }

    void OnEnable()
    {
        InitAllData();
    }

    private void InitAllData()
    {
        CreateRootForPlacedObjects();

        LoadPersistentData();

        CreateCursor();

        CreatePlane();

        CreateMap();

        TableMatrix01.TileSelected += SelectTile;
    }

    private void SelectTile(GameObject go)
    {
        Debug.Log($"TilemapEditor... callback enabled {go.name}");
        if (go != null)
            CreateCursor(go);
    }

    private void CreateMap()
    {
        if (mapper == null)
        {
            mapper = new TileMapUtils();
            mapper.Init(plane, cursorObject, placedObjectsRoot, persistentData.scale.y);
        }
    }

    private void CreatePlane()
    {
        if (plane == null)
        {
            plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
            plane.transform.position = new Vector3(0.51f, -0.5f, 0.51f);
            plane.transform.localScale = new Vector3(persistentData.planeDimensions.x, 1, persistentData.planeDimensions.y);
            LayerMask mask = LayerMask.NameToLayer("LevelAuthoring");
            plane.layer = mask;
            var meshCollider = plane.GetComponent<MeshCollider>();
            meshCollider.enabled = true;

            var mr = plane.GetComponent<MeshRenderer>();
            mr.material = persistentData.tileTexture;

            plane.gameObject.transform.parent = placedObjectsRoot.transform;
        }
    }

    void LoadPersistentData()
    {
        string savedAssetPath = "Assets/Resources/TileMapEditorPersistentence.asset";
        persistentData = (TileMapEditorPersistentData)AssetDatabase.LoadAssetAtPath(savedAssetPath, typeof(TileMapEditorPersistentData));
        if (persistentData == null)
        {
            persistentData = CreateInstance<TileMapEditorPersistentData>();
            if (AssetDatabase.IsValidFolder("Assets/Resources") == false)
            {
                AssetDatabase.CreateFolder("Assets", "Resources");
            }
            AssetDatabase.CreateAsset(persistentData, savedAssetPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }

    void CreateRootForPlacedObjects()
    {
        if (placedObjectsRoot == null)
        {
            placedObjectsRoot = new GameObject("HierarchyRoot");
            placedObjectsRoot.transform.position = Vector3.zero;
        }
    }

    void CreateCursor(GameObject replacement = null)
    {
        if (persistentData == null || persistentData.cursorPrefab == null)
            return;

        if (cursorObject == null)// || typeof(cursorObject) != typeof(persistentData.cursorPrefab))
        {
            cursorObject = Instantiate(persistentData.cursorPrefab);
        }
        if (replacement != null)
        {
            DestroyImmediate(cursorObject);
            cursorObject = Instantiate(replacement);
        }

        cursorObject.transform.parent = placedObjectsRoot.transform;
    }

    void SavePersistentData()
    {
        EditorUtility.SetDirty(persistentData);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private void OnDisable()
    {
        //AssetDatabase.SaveAssetIfDirty(persistentData);
    }

    void OnFocus()
    {
        SceneView.duringSceneGui -= OnSceneGUI; // Just in case
        SceneView.duringSceneGui += OnSceneGUI;
        InitAllData();
    }

    private void OnLostFocus()
    {
        SavePersistentData();
    }

    void OnDestroy()
    {
        TableMatrix01.TileSelected -= SelectTile;
        SceneView.duringSceneGui -= OnSceneGUI;
        DestroyImmediate(cursorObject);
        DestroyImmediate(placedObjectsRoot);
        DestroyImmediate(plane);
        persistentData = null;//  DestroyImmediate(persistentData);

        mapper = null;

    }

    
}
