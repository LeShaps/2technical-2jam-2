#ifndef CUSTOM_LIGHTING_INCLUDED
#define CUSTOM_LIGHTING_INCLUDED
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
struct CustomLightingData
{
    // Surface attributes
    float3 albedo;
    // Position and orientation
    float3 normalWS;
    float3 viewDirectionWS;
    float smoothness;
    float3 specCol;
};
float GetSmoothnessPower(float rawSmoothness) {
    return exp2(10 * rawSmoothness + 1);
}
float D_GGX(float roughness, float NoH, const float3 n, const float3 h) {
    float a = NoH * roughness;
    float k = roughness / (1.0 - NoH * NoH + a * a);
    return k * k * (1.0 / PI);
}
float4 CustomLightHandling(CustomLightingData d, Light light) {
    
    //saturate(dot(d.normalWS, light.direction));
    float diffuse = saturate(dot(d.normalWS, light.direction));
    float specularDot = saturate(dot(d.normalWS, normalize(light.direction + d.viewDirectionWS)));
    float specular = smoothstep(0.3,0.35,pow(specularDot, GetSmoothnessPower(d.smoothness)) * diffuse);

    float4 color = float4(d.albedo + specular*d.specCol, specular);

    return color;
}
float4 CalculateCustomLighting(CustomLightingData d)
{
    #ifdef SHADERGRAPH_PREVIEW
    return d.albedo;
    #else
    
    Light mainLight = GetMainLight();

    float4 color = 0;
    // Shade the main light
    color += CustomLightHandling(d, mainLight);
    return color;
    
    #endif
}

void CalculateCustomLighting_float(float3 Albedo, float3 Normal, float3 ViewDirection, float Smoothness,
                                   float3 SpecColor, out float4 Color)
{
    CustomLightingData d;
    d.albedo = Albedo;
    d.normalWS = Normal;
    d.viewDirectionWS = ViewDirection;
    d.smoothness = Smoothness;
    d.specCol = SpecColor;
    Color = CalculateCustomLighting(d);
}

#endif
