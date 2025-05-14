Shader "Custom/SplitUVShader"
{
    Properties
    {
        _MyBaseMap("Base Color (UV2)", 2D) = "white" {}
        _AOMap("Ambient Occlusion (UV0)", 2D) = "white" {}
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 300

        // FORWARD PASS
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
                float3x3 TBN : TEXCOORD1;
                float2 uv0 : TEXCOORD4;
                float2 uv2 : TEXCOORD5;
            };

            TEXTURE2D(_MyBaseMap); SAMPLER(sampler_MyBaseMap);
            TEXTURE2D(_AOMap);     SAMPLER(sampler_AOMap);

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                float3 positionWS = TransformObjectToWorld(IN.positionOS.xyz);
                float3 normalWS = TransformObjectToWorldNormal(IN.normalOS);
                float3 tangentWS = TransformObjectToWorldDir(IN.tangentOS.xyz);
                float3 bitangentWS = cross(normalWS, tangentWS) * IN.tangentOS.w;

                OUT.positionWS = positionWS;
                OUT.positionHCS = TransformWorldToHClip(positionWS);
                OUT.TBN = float3x3(tangentWS, bitangentWS, normalWS);
                OUT.uv0 = IN.uv0;
                OUT.uv2 = IN.uv2;
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                float4 baseColor = SAMPLE_TEXTURE2D(_MyBaseMap, sampler_MyBaseMap, IN.uv2);
                float ao = SAMPLE_TEXTURE2D(_AOMap, sampler_AOMap, IN.uv0).r;

                float3 normalTS = float3(0.0, 0.0, 1.0);
                float3 normalWS = normalize(mul(normalTS, IN.TBN));

                InputData lightingInput = (InputData)0;
                lightingInput.positionWS = IN.positionWS;
                lightingInput.normalWS = normalWS;
                lightingInput.viewDirectionWS = GetCameraPositionWS() - IN.positionWS;
                lightingInput.shadowCoord = TransformWorldToShadowCoord(IN.positionWS);
                lightingInput.fogCoord = 0;

                SurfaceData surface = (SurfaceData)0;
                surface.albedo = baseColor.rgb * ao;
                surface.alpha = baseColor.a;
                surface.normalTS = normalTS;
                surface.occlusion = ao;

                return UniversalFragmentPBR(lightingInput, surface);
            }

            ENDHLSL
        }

        // SHADOW CASTER PASS (eigene Implementierung)
        Pass
        {
            Name "ShadowCaster"
            Tags { "LightMode" = "ShadowCaster" }

            Cull Back
            ZWrite On
            ZTest LEqual

            HLSLPROGRAM
            #pragma vertex ShadowVert
            #pragma fragment ShadowFrag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderVariablesFunctions.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
            };

            Varyings ShadowVert(Attributes IN)
            {
                Varyings OUT;
                float3 positionWS = TransformObjectToWorld(IN.positionOS.xyz);
                float3 normalWS = TransformObjectToWorldNormal(IN.normalOS);

                OUT.positionHCS = TransformWorldToHClip(positionWS);
                return OUT;
            }

            float4 ShadowFrag(Varyings IN) : SV_Target
            {
                return 0; // Nur Tiefe, keine Farbe
            }
            ENDHLSL
        }
    }

    FallBack "Hidden/Universal Render Pipeline/FallbackError"
}
