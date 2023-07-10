using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

public class PixelizeFeature : ScriptableRendererFeature
{
    [System.Serializable]
    public class PixelizePassSettings
    {
        [Header("General Pixelize Settings")]
        public RenderPassEvent renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
        public int screenHeight = 144;
    }

    [SerializeField] private PixelizePassSettings pixelizeSettings;

    private PixelizePass pixelizePass;

    public override void Create()
    {
        pixelizePass = new PixelizePass(pixelizeSettings);
    }
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {

#if UNITY_EDITOR
        if (renderingData.cameraData.isSceneViewCamera) return;
#endif
        renderer.EnqueuePass(pixelizePass);
    }
}