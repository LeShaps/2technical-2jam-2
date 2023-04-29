#ifndef CELSHADER
#define CELSHADER
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

// This is a neat trick to work around a bug in the shader graph when
// enabling shadow keywords. Created by @cyanilux
// https://github.com/Cyanilux/URP_ShaderGraphCustomLighting
#ifndef SHADERGRAPH_PREVIEW
    #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
    #if (SHADERPASS != SHADERPASS_FORWARD)
        #undef REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR
    #endif
#endif

struct CelShadeData
{
    // Surface attributes
    float3 albedo;
    // Position and orientation
    float3 normalWS;
    float3 viewDirectionWS;
    float3 specCol;
    float4 shadowCoord;
    float3 positionWS;
    float fogFactor;
    float3 ShadowColor;
    
    UnityTexture2D celshadeGradient;
    UnitySamplerState celshadeGradientSS;
};
float Greyscale(float3 color)
{
    return 0.2989 * color.r + 0.5870 * color.g + 0.1140 * color.b;
}

float BlendMode_Overlay(float base, float blend)
{
    return (base <= 0.5) ? 2*base*blend : 1 - 2*(1-base)*(1-blend);
}

float3 BlendMode_Overlay(float3 base, float3 blend)
{
    return float3(  BlendMode_Overlay(base.r, blend.r), 
                    BlendMode_Overlay(base.g, blend.g), 
                    BlendMode_Overlay(base.b, blend.b) );
}
float3 Celshade_Lighting(CelShadeData d, Light light) {
    
    //saturate(dot(d.normalWS, light.direction));
    float diffuse = saturate(dot(normalize(d.normalWS), normalize(light.direction)));
    float shadow = light.shadowAttenuation * light.distanceAttenuation;
    diffuse *= shadow;

    float celshading = SAMPLE_TEXTURE2D(d.celshadeGradient, d.celshadeGradientSS, half2(diffuse, 0)).r;
    
    float3 color = lerp(0, BlendMode_Overlay(light.color,d.albedo), celshading);

    return color;
}
float3 Celshade(CelShadeData d)
{
    #ifdef SHADERGRAPH_PREVIEW
    return d.albedo;
    #else
    
    Light mainLight = GetMainLight(d.shadowCoord, d.positionWS, 1);

    float3 color = 0;
    // Shade the main light
    color += Celshade_Lighting(d, mainLight);

    #ifdef _ADDITIONAL_LIGHTS
    // Shade additional cone and point lights. Functions in URP/ShaderLibrary/Lighting.hlsl
    uint numAdditionalLights = GetAdditionalLightsCount();
    for (uint lightI = 0; lightI < numAdditionalLights; lightI++) {
        Light light = GetAdditionalLight(lightI, d.positionWS, 1);
        color += Celshade_Lighting(d, light);
    }
    #endif

    color = lerp(d.ShadowColor, color, Greyscale(color));
    
    color = MixFog(color, d.fogFactor);
    
    return color;
    
    #endif
}

void Celshade_float(float3 Albedo, float3 Normal, float3 ViewDirection,
UnityTexture2D celshadeGradient, UnitySamplerState celshadeGradientSS, float3 Position,
float3 ShadowColor,
                                   out float3 Color)
{
    CelShadeData d;
    d.albedo = Albedo;
    d.normalWS = Normal;
    d.viewDirectionWS = ViewDirection;
    d.celshadeGradient = celshadeGradient;
    d.celshadeGradientSS = celshadeGradientSS;
    d.positionWS = Position;
    d.ShadowColor = ShadowColor;
    
    #ifdef SHADERGRAPH_PREVIEW
    // In preview, there's no shadows or bakedGI
    d.shadowCoord = 0;
    d.fogFactor = 0;
    #else
    // Calculate the main light shadow coord
    // There are two types depending on if cascades are enabled
    float4 positionCS = TransformWorldToHClip(Position);
    #if SHADOWS_SCREEN
    d.shadowCoord = ComputeScreenPos(positionCS);
    #else
    d.shadowCoord = TransformWorldToShadowCoord(Position);
    d.fogFactor = ComputeFogFactor(positionCS.z);
    #endif
    #endif

    Color = Celshade(d);
}

#endif
