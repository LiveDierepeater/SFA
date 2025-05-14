Shader "Custom/SplitUVShader"
{
    Properties
    {
        _MyBaseMap("Base Color (UV2)", 2D) = "white" {}
        _NormalMap("Normal Map (UV0)", 2D) = "bump" {}
        _AOMap("AO Map (UV0)", 2D) = "white" {}
        _NormalStrength("Normal Strength", Range(0, 2)) = 1.0
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 300

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode" = "UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _ADDITIONAL_LIGHTS
            #pragma multi_compile_fragment _ _SHADOWS_SOFT
            #pragma multi_compile_fog
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/SurfaceInput.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
                float4 tangentOS : TANGENT;
                float2 uv0 : TEXCOORD0;
                float2 uv2 : TEXCOORD2;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float3 normalWS : TEXCOORD1;
                float3 tangentWS : TEXCOORD2;
                float3 bitangentWS : TEXCOORD3;
                float3 positionWS : TEXCOORD4;
                float2 uv0 : TEXCOORD5;
                float2 uv2 : TEXCOORD6;
            };

            sampler2D _MyBaseMap;
            sampler2D _NormalMap;
            sampler2D _AOMap;
            float _NormalStrength;

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.uv0 = IN.uv0;
                OUT.uv2 = IN.uv2;

                float3 normalWS = TransformObjectToWorldNormal(IN.normalOS);
                float3 tangentWS = TransformObjectToWorldDir(IN.tangentOS.xyz);
                float3 bitangentWS = cross(normalWS, tangentWS) * IN.tangentOS.w;

                OUT.normalWS = normalWS;
                OUT.tangentWS = tangentWS;
                OUT.bitangentWS = bitangentWS;
                OUT.positionWS = TransformObjectToWorld(IN.positionOS.xyz);
                OUT.positionHCS = TransformWorldToHClip(OUT.positionWS);
                return OUT;
            }

            float3 UnpackNormalCustom(float4 packedNormal, float3 normalWS, float3 tangentWS, float3 bitangentWS)
            {
                float3 normalTS = UnpackNormal(packedNormal);
                normalTS.xy *= _NormalStrength;
                float3x3 TBN = float3x3(tangentWS, bitangentWS, normalWS);
                return normalize(mul(normalTS, TBN));
            }

            half4 frag(Varyings IN) : SV_Target
            {
                float4 baseColor = tex2D(_MyBaseMap, IN.uv2);

                float4 packedNormal = tex2D(_NormalMap, IN.uv0);
                float3 normalWS = UnpackNormalCustom(packedNormal, IN.normalWS, IN.tangentWS, IN.bitangentWS);

                float ao = tex2D(_AOMap, IN.uv0).r;

                InputData lightingInput = (InputData)0;
                lightingInput.positionWS = IN.positionWS;
                lightingInput.normalWS = normalWS;
                lightingInput.viewDirectionWS = GetCameraPositionWS() - IN.positionWS;
                lightingInput.shadowCoord = TransformWorldToShadowCoord(IN.positionWS);
                lightingInput.fogCoord = 0;

                SurfaceData surface = (SurfaceData)0;
                surface.albedo = baseColor.rgb;
                surface.alpha = baseColor.a;
                surface.normalTS = normalWS;
                surface.occlusion = ao;

                half4 finalColor = UniversalFragmentPBR(lightingInput, surface);
                return finalColor;
            }

            ENDHLSL
        }
    }
    FallBack "Hidden/Universal Render Pipeline/FallbackError"
}
