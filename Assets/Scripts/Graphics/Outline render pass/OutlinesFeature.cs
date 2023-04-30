using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[ System.Serializable ]
public class OutlinesFeature : ScriptableRendererFeature
{
    OutlinesPass pass;

    public Material overrideMat;
    // Corresponds to `Tags { "LightMode" = "MyCustomPass" }` in the shaders.
    // You have to add this tag for the corresponding shaders to associate them with this pass.

    public override void Create()
    {
        ShaderTagId shaderTag = new ShaderTagId( nameof(OutlinesPass) );
        pass = new OutlinesPass( RenderPassEvent.AfterRenderingTransparents, overrideMat );
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(pass); // letting the renderer know which passes will be used before allocation
    }

    public override void SetupRenderPasses(ScriptableRenderer renderer, in RenderingData renderingData)
    {
        pass.Setup(renderer.cameraColorTargetHandle, renderer.cameraDepthTargetHandle);  // use of target after allocation
    }
}