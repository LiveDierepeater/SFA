Shader "Custom/FoliageWithExternalAlpha"
{
    Properties
    {
        _MainTex ("Main Texture (RGB)", 2D) = "white" {}
        _AlphaTex ("Alpha Texture (A only)", 2D) = "white" {}
        _Cutoff ("Alpha Cutoff", Range(0,1)) = 0.5
    }
    SubShader
    {
        Tags { "RenderType"="TransparentCutout" "Queue"="AlphaTest" }

        Cull Off           // Double-sided for foliage
        ZWrite On
        Blend Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            sampler2D _AlphaTex;
            float4 _MainTex_ST;
            float _Cutoff;

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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                fixed alpha = tex2D(_AlphaTex, i.uv).r;

                clip(alpha - _Cutoff); // Discard fragment if alpha too low
                col.a = alpha;

                return col;
            }
            ENDCG
        }
    }
    FallBack "Transparent/Cutout/Diffuse"
}
