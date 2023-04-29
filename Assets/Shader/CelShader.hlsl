#ifndef CELSHADER
#define CELSHADER
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
struct CelShadeData
{
    // Surface attributes
    float3 albedo;
    // Position and orientation
    float3 normalWS;
    float3 viewDirectionWS;
    float3 specCol;
    
    UnityTexture2D celshadeGradient;
    UnitySamplerState celshadeGradientSS;
};

float3 Celshade_Lighting(CelShadeData d, Light light) {
    
    //saturate(dot(d.normalWS, light.direction));
    float diffuse = dot(normalize(d.normalWS), normalize(light.direction));
    diffuse = saturate(diffuse * 0.5 + 0.5);
    float celshade = 
    float3 color = d.albedo;

    return diffuse;
}
float3 Celshade(CelShadeData d)
{
    #ifdef SHADERGRAPH_PREVIEW
    return d.albedo;
    #else
    
    Light mainLight = GetMainLight();

    float3 color = 0;
    // Shade the main light
    color += Celshade_Lighting(d, mainLight);
    return color;
    
    #endif
}

void Celshade_float(float3 Albedo, float3 Normal, float3 ViewDirection,
UnityTexture2D celshadeGradient, UnitySamplerState celshadeGradientSS,
                                   out float3 Color)
{
    CelShadeData d;
    d.albedo = Albedo;
    d.normalWS = Normal;
    d.viewDirectionWS = ViewDirection;
    d.celshadeGradient = celshadeGradient;
    d.celshadeGradientSS = celshadeGradientSS;

    Color = Celshade(d);
}

#endif
