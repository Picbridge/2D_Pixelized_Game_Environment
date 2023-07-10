using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

public class CelShaderPass : ScriptableRenderPass
{
    private readonly Material toonMaterial;

    private RenderTargetIdentifier cameraColorTarget;

    private RenderTargetIdentifier toonBuffer;

    private int toonBufferID = Shader.PropertyToID("_ToonBuffer");

    public CelShaderPass(RenderPassEvent renderPassEvent, ToonFeature.CelShaderSettings settings)
    {
        this.renderPassEvent = renderPassEvent;

        toonMaterial = new Material(Shader.Find("Hidden/ToonShader"));
        toonMaterial.SetFloat("_Shades", settings.numShades);
        toonMaterial.SetFloat("_Smoothness", settings.smoothness);
        toonMaterial.SetFloat("_RimThreshold", settings.rimThreshold);
        toonMaterial.SetFloat("_EdgeDiffuse", settings.edgeDiffuse);
        toonMaterial.SetFloat("_EdgeSpecular", settings.edgeSpecular);
        toonMaterial.SetFloat("_EdgeSpecularOffset", settings.edgeSpecularOffset);
        toonMaterial.SetFloat("_EdgeDistanceAttenuation", settings.edgeDistanceAttenuation);
        toonMaterial.SetFloat("_EdgeShadowAttenuation", settings.edgeShadowAttenuation);
        toonMaterial.SetFloat("_EdgeRim", settings.edgeRim);
        toonMaterial.SetFloat("_EdgeRimOffset", settings.edgeRimOffset);
    }

    public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
    {
#pragma warning disable CS0618 // Type or member is obsolete
        cameraColorTarget = renderingData.cameraData.renderer.cameraColorTarget;
#pragma warning restore CS0618 // Type or member is obsolete
        RenderTextureDescriptor descriptor = renderingData.cameraData.cameraTargetDescriptor;

        cmd.GetTemporaryRT(toonBufferID, descriptor, FilterMode.Point);
        toonBuffer = new RenderTargetIdentifier(toonBufferID);
    }

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        if (!toonMaterial)
            return;
        CommandBuffer cmd = CommandBufferPool.Get();
        using (new ProfilingScope(cmd, new ProfilingSampler("Pixelize Pass")))
        {

#pragma warning disable CS0618 // Type or member is obsolete
            Blit(cmd, cameraColorTarget, toonBuffer);
            Blit(cmd, toonBuffer, cameraColorTarget, toonMaterial);
#pragma warning restore CS0618 // Type or member is obsolete
        }

        context.ExecuteCommandBuffer(cmd);
        CommandBufferPool.Release(cmd);
    }

    public override void OnCameraCleanup(CommandBuffer cmd)
    {
        if (cmd == null) throw new System.ArgumentNullException("cmd");
        cmd.ReleaseTemporaryRT(toonBufferID);
    }

}