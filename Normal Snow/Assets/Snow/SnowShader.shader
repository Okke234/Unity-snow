Shader "SnowShader"
{
    Properties
    {
        _Tess ("Tessellation", Range(1,32)) = 4
        _SnowColor ("Snow Color", color) = (1,1,1,0)
        _SnowTex ("Snow", 2D) = "white" {}
        _GroundColor ("Ground Color", color) = (1,1,1,0)
        _GroundTex ("Ground", 2D) = "white" {}
        _DispTex ("Disp Texture", 2D) = "black" {}
        _Displacement ("Displacement", Range(0, 1.0)) = 0.3
    }
    SubShader
    {
        Tags
        {
            "RenderType"="Opaque"
        }
        LOD 300

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows vertex:disp tessellate:tessDistance
        #pragma target 4.6
        #include "Tessellation.cginc"

        struct appdata
        {
            float4 vertex : POSITION;
            float4 tangent : TANGENT;
            float3 normal : NORMAL;
            float2 texcoord : TEXCOORD0;
        };

        float _Tess;

        float4 tessDistance(appdata v0, appdata v1, appdata v2)
        {
            float minDist = 10.0;
            float maxDist = 25.0;
            return UnityDistanceBasedTess(v0.vertex, v1.vertex, v2.vertex, minDist, maxDist, _Tess);
        }

        sampler2D _DispTex;
        float _Displacement;

        void disp(inout appdata v)
        {
            float d = tex2Dlod(_DispTex, float4(v.texcoord.xy, 0, 0)) * _Displacement;
            v.vertex.xyz -= v.normal * d;
            v.vertex.xyz += v.normal * _Displacement;
        }

        sampler2D _SnowTex;
        fixed4 _SnowColor;
        sampler2D _GroundTex;
        fixed4 _GroundColor;

        struct Input
        {
            float2 uv_SnowTex;
            float2 uv_GroundTex;
            float2 uv_DispTex;
        };

        half _Glossiness;
        half _Metallic;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            half amount = tex2Dlod(_DispTex, float4(IN.uv_DispTex, 0, 0));
            fixed4 c = lerp(tex2D (_SnowTex, IN.uv_SnowTex) * _SnowColor, tex2D (_GroundTex, IN.uv_GroundTex) * _GroundColor, amount);
            
            o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}