using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Collections.Generic;

[ Serializable ]
public class OutlinesPass : ScriptableRenderPass
{
	RTHandle m_Handle;
	// RTHandles don't combine color and dpeth
	RTHandle m_DestinationColor;
	RTHandle m_DestinationDepth;
	private Material _overrideMat;
	int tmpId1;

	RenderTargetIdentifier tmpRT1;

	void Dispose()
	{
		m_Handle?.Release();
	}

	public OutlinesPass( RenderPassEvent evt, Material material )
	{
		renderPassEvent = evt;
		_overrideMat = material;
	}
	
	public override void Configure( CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor )
	{
		var width = cameraTextureDescriptor.width;
		var height = cameraTextureDescriptor.height;

		tmpId1 = Shader.PropertyToID( "tmpGlobalNoiseRT1" );
		cmd.GetTemporaryRT( tmpId1, width, height, 16, FilterMode.Bilinear, RenderTextureFormat.ARGB32 );
		tmpRT1 = new RenderTargetIdentifier( tmpId1 );
		ConfigureTarget( tmpRT1 );
		ConfigureClear( ClearFlag.All, Color.clear );
	}
	
	public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
	{
		var desc = renderingData.cameraData.cameraTargetDescriptor;
		desc.depthBufferBits = 0; // Color and depth cannot be combined in RTHandles
		RenderingUtils.ReAllocateIfNeeded(ref m_Handle, desc, FilterMode.Point, TextureWrapMode.Clamp, name: "_CustomPassHandle");
	}

	public override void OnCameraCleanup(CommandBuffer cmd)
	{
		m_DestinationColor = null;
		m_DestinationDepth = null;
	}

	public void Setup(RTHandle destinationColor, RTHandle destinationDepth)
	{
		m_DestinationColor = destinationColor;
		m_DestinationDepth = destinationDepth;
	}

	public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
	{
		CommandBuffer cmd = CommandBufferPool.Get();
		
		cmd.Blit( m_DestinationColor, tmpId1, _overrideMat, 0 );
		cmd.Blit( tmpId1, m_DestinationColor);
	
		context.ExecuteCommandBuffer(cmd);
		CommandBufferPool.Release(cmd);
	}
}
