using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

public class ToonFeature : ScriptableRendererFeature
{
    [System.Serializable]
    public class CelShaderSettings
    {
        [Header("General Shadow Settings")]
        [Range(0.0f, 20.0f)]
        public float numShades = 4.3f;
        public bool Shadows = true;
        public bool ShadowsCascade = true;
        public bool SoftShadows = true;

        [Header("Light Settings")]
        [Range(0.0f, 10.0f)]
        public float smoothness = 0.7f;
        [Range(0.0f, 5.0f)]
        public float rimThreshold = 2.8f;

        [Header("Edge Settings")]
        [Range(0.0f, 5.0f)]
        public float edgeDiffuse = 1;
        [Range(0.0f, 1.0f)]
        public float edgeSpecular = 0;
        [Range(0.0f, 1.0f)]
        public float edgeSpecularOffset = 0.24f;
        [Range(0.0f, 1.0f)]
        public float edgeDistanceAttenuation = 0.3f;
        [Range(0.0f, 5.0f)]
        public float edgeShadowAttenuation = 0.5f;
        [Range(0.0f, 1.0f)]
        public float edgeRim = 0.31f;
        [Range(0.0f, 1.0f)]
        public float edgeRimOffset = 0.67f;

    }

    [SerializeField] private CelShaderSettings celShaderSettings = new CelShaderSettings();
    [SerializeField] private RenderPassEvent renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
    private CelShaderPass celShaderePass;

    public override void Create()
    {
        celShaderePass = new CelShaderPass(renderPassEvent, celShaderSettings);
    }
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {

#if UNITY_EDITOR
        if (renderingData.cameraData.isSceneViewCamera) return;
#endif
        renderer.EnqueuePass(celShaderePass);
    }
}