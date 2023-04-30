Shader "PP/Outlines"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _SobelStep("SobelStep", float) = 0
        _Thickness("sThickness", float) = 0
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }


            half sobelDepth(half2 uv, sampler2D _Depth, half2 texelSize)
            {
                half x = 0;
                half y = 0;

                x += tex2D(_Depth, uv + float2(-texelSize.x, -texelSize.y)) * -1.0;
                x += tex2D(_Depth, uv + float2(-texelSize.x, 0)) * -2.0;
                x += tex2D(_Depth, uv + float2(-texelSize.x, texelSize.y)) * -1.0;

                x += tex2D(_Depth, uv + float2( texelSize.x, -texelSize.y)) * 1.0;
                x += tex2D(_Depth, uv + float2( texelSize.x, 0)) * 2.0;
                x += tex2D(_Depth, uv + float2( texelSize.x, texelSize.y)) * 1.0;

                y += tex2D(_Depth, uv + float2(-texelSize.x, -texelSize.y)) * -1.0;
                y += tex2D(_Depth, uv + float2( 0, -texelSize.y)) * -2.0;
                y += tex2D(_Depth, uv + float2( texelSize.x, -texelSize.y)) * -1.0;

                y += tex2D(_Depth, uv + float2(-texelSize.x, texelSize.y)) * 1.0;
                y += tex2D(_Depth, uv + float2( 0, texelSize.y)) * 2.0;
                y += tex2D(_Depth, uv + float2( texelSize.x, texelSize.y)) * 1.0;

                float depth = Linear01Depth(sqrt(x * x + y * y));
                depth = depth * _ProjectionParams.z;
                
                return depth;
            }

            half sobel(half2 uv, sampler2D _MainTex, half2 texelSize)
            {
                half x = 0;
                half y = 0;

                x += tex2D(_MainTex, uv + float2(-texelSize.x, -texelSize.y)) * -1.0;
                x += tex2D(_MainTex, uv + float2(-texelSize.x, 0)) * -2.0;
                x += tex2D(_MainTex, uv + float2(-texelSize.x, texelSize.y)) * -1.0;

                x += tex2D(_MainTex, uv + float2(texelSize.x, -texelSize.y)) * 1.0;
                x += tex2D(_MainTex, uv + float2(texelSize.x, 0)) * 2.0;
                x += tex2D(_MainTex, uv + float2(texelSize.x, texelSize.y)) * 1.0;

                y += tex2D(_MainTex, uv + float2(-texelSize.x, -texelSize.y)) * -1.0;
                y += tex2D(_MainTex, uv + float2(0, -texelSize.y)) * -2.0;
                y += tex2D(_MainTex, uv + float2(texelSize.x, -texelSize.y)) * -1.0;

                y += tex2D(_MainTex, uv + float2(-texelSize.x, texelSize.y)) * 1.0;
                y += tex2D(_MainTex, uv + float2(0, texelSize.y)) * 2.0;
                y += tex2D(_MainTex, uv + float2(texelSize.x, texelSize.y)) * 1.0;

                return sqrt(x * x + y * y);
            }


            sampler2D _MainTex;
            sampler2D _CameraDepthTexture;

            float4 _MainTex_TexelSize;
            float _SobelStep;
            float _Thickness;

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);

                float3 edge = smoothstep(_SobelStep - 0.0001, _SobelStep,
                                         sobelDepth(i.uv, _CameraDepthTexture, _MainTex_TexelSize.xy * _Thickness));
               //float colorEdge = smoothstep(1 - 0.0001, 1,sobel(i.uv, _MainTex, _MainTex_TexelSize.xy * _Thickness));
                
                // just invert the colors
                col.rgb = lerp(0, col.rgb, edge);
                return col;
            }
            ENDCG
        }
    }
}