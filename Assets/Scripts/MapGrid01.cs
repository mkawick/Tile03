
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor.Examples;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class MapGrid01
{
    public bool test;


    public GameObject tile;
    private Texture2D cachedImage;

    [OnInspectorInit]
    public void CreateData(GameObject newTile = null)
    {
        if (newTile != null)
        {
            tile = newTile;
        }
        //cachedImage = AssetPreview.GetAssetPreview(tile);
        cachedImage = CopiedCode();
    }
    public Texture2D GetTexture()
    {
        return cachedImage;
    }

    // https://csharp.hotexamples.com/examples/UnityEngine/Camera/CopyFrom/php-camera-copyfrom-method-examples.html
    public Texture2D CopiedCode()
    {
        GameObject temp = GameObject.Instantiate(tile);
        temp.gameObject.transform.Rotate(0, 180, 0);
        var tempCamera = new GameObject("2d_camera");
        var camera = tempCamera.AddComponent<Camera>();
        camera.CopyFrom(Camera.main);
        camera.depth = 2;

        //Then create a render target with desired dimension. I used 512 here, for testing.
        var renderTarget = RenderTexture.GetTemporary(128, 128);
        renderTarget.name = "output_texture";

        tempCamera.transform.position = temp.transform.position + Vector3.up * 1.9f;
        tempCamera.transform.LookAt(temp.transform);
        //tempCamera.transform.Rotate(90, 0, 0);

        camera.targetTexture = renderTarget;
        camera.transform.LookAt(temp.transform);

        camera.cullingMask = ~(0) - (1<<20);
        camera.backgroundColor = Color.black;
        camera.clearFlags = CameraClearFlags.SolidColor;
        camera.cameraType = CameraType.Preview;

        //After that you need to set an aspect ratio to the camera. It's calculated as width divided by height.
        camera.aspect = (float)renderTarget.width / renderTarget.height;
        camera.fieldOfView = 60.0f;

        //Set your render target as a target texture of the unity camera.
        camera.targetTexture = renderTarget;
        camera.Render();

        //After the camera rendered an image you need to set it as a foreground to get data from it.
        RenderTexture.active = renderTarget;

        //Create a texture which you will use to save a screenshot, remember that it should have same resolution as
        //render target.
        Texture2D screenshot = new Texture2D(renderTarget.width, renderTarget.height);
        //Read pixels from the screen.
        screenshot.ReadPixels(new Rect(0, 0, renderTarget.width, renderTarget.height), 0, 0);

        screenshot.Apply();//<< really important

        GameObject.DestroyImmediate(tempCamera);
        GameObject.DestroyImmediate(temp);
        return screenshot;
    }
    // public Texture2D texture;
}
