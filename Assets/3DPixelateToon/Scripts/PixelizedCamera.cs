using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PixelizedCamera : MonoBehaviour
{
    const int MAX = 32;
    [SerializeField]
    [Range(1, 16)]
    private int pixelSize = 9;
    private int prevPixelSize;
    private float aspectRatio = (float)Screen.width / (float)Screen.height;
    private float prevAspectRatio;
    private RenderTexture cameraViewTexture;
    private Camera cam;
    [SerializeField]
    private Shader shader;
    void Awake()
    {
        prevPixelSize = pixelSize;
        prevAspectRatio = aspectRatio;

        cam = Camera.main;
        cameraViewTexture = new RenderTexture((int)(100 * aspectRatio) * (MAX / pixelSize), 100 * (MAX / pixelSize), 16);
        cameraViewTexture.filterMode = FilterMode.Point;
        cam.targetTexture = cameraViewTexture;

        GameObject newCanvas = new GameObject("Canvas");
        Canvas canvas = newCanvas.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.pixelPerfect = true;

        GameObject newRawImage = new GameObject("RawImage");
        newRawImage.AddComponent<CanvasRenderer>();
        RawImage rawImage = newRawImage.AddComponent<RawImage>();
        rawImage.transform.SetParent(newCanvas.transform, false);
        rawImage.texture = cameraViewTexture;
        rawImage.rectTransform.anchorMin = new Vector2(0, 0);
        rawImage.rectTransform.anchorMax = new Vector2(1, 1);
        rawImage.rectTransform.anchoredPosition3D = new Vector3(0, 0, 0);
        rawImage.rectTransform.sizeDelta = new Vector2(0, 0);
    }
    void Update()
    {
        aspectRatio = (float)Screen.width / (float)Screen.height;
        //Update if pixel size is updated
        if (prevPixelSize != pixelSize || prevAspectRatio != aspectRatio)
            Resize(cameraViewTexture, (int)(100 * aspectRatio) * (MAX / pixelSize), 100 * (MAX/pixelSize));
    }

    void Resize(RenderTexture renderTexture, int width, int height)
    {
        if (renderTexture)
        {
            renderTexture.Release();
            renderTexture.width = width;
            renderTexture.height = height;
        }
        prevPixelSize = pixelSize;
    }
}
