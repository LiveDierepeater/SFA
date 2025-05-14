Shader "Custom/SplitUVShader"
{
Properties
    {
        _MyBaseMap("Base Color (UV2)", 2D) = "white" {}
        _NormalMap("Normal Map (UV0)", 2D) = "bump" {}
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
                float3 positionWS : TEXCOORD0;
                float3 normalWS : TEXCOORD1;
                float3x3 TBN : TEXCOORD2;
                float2 uv0 : TEXCOORD5;
                float2 uv2 : TEXCOORD6;
            };

            TEXTURE2D(_MyBaseMap); SAMPLER(sampler_MyBaseMap);
            TEXTURE2D(_NormalMap); SAMPLER(sampler_NormalMap);
            float _NormalStrength;

            Varyings vert(Attributes IN)
            {
                Varyings OUT;

                OUT.positionWS = TransformObjectToWorld(IN.positionOS.xyz);
                OUT.positionHCS = TransformWorldToHClip(OUT.positionWS);
                OUT.normalWS = TransformObjectToWorldNormal(IN.normalOS);

                float3 tangentWS = TransformObjectToWorldDir(IN.tangentOS.xyz);
                float3 bitangentWS = cross(OUT.normalWS, tangentWS) * IN.tangentOS.w;

                OUT.TBN = float3x3(tangentWS, bitangentWS, OUT.normalWS);

                OUT.uv0 = IN.uv0;
                OUT.uv2 = IN.uv2;

                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                float4 baseColor = SAMPLE_TEXTURE2D(_MyBaseMap, sampler_MyBaseMap, IN.uv2);

                // Sample and unpack normal
                float4 normalSample = SAMPLE_TEXTURE2D(_NormalMap, sampler_NormalMap, IN.uv0);
                float3 normalTS = UnpackNormal(normalSample);
                normalTS.xy *= _NormalStrength;
                normalTS = normalize(normalTS);

                // Pass to URP: surface.normalTS must be in Tangent Space
                InputData lightingInput = (InputData)0;
                lightingInput.positionWS = IN.positionWS;
                lightingInput.normalWS = normalize(mul(normalTS, IN.TBN)); // used only for lighting
                lightingInput.viewDirectionWS = GetCameraPositionWS() - IN.positionWS;
                lightingInput.shadowCoord = TransformWorldToShadowCoord(IN.positionWS);
                lightingInput.fogCoord = 0;

                SurfaceData surface = (SurfaceData)0;
                surface.albedo = baseColor.rgb;
                surface.alpha = baseColor.a;
                surface.normalTS = normalTS;
                surface.occlusion = 1;

                return half4(surface.albedo, 1.0); // Nur Base Color zeigen
                
            }

            ENDHLSL
        }
    }

    FallBack "Hidden/Universal Render Pipeline/FallbackError"
}
