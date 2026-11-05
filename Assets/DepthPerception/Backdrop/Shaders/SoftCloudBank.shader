Shader "DepthPerception/SoftCloudBank"
{
 Properties
 {
  _BaseColor("Tint", Color) = (0.45,0.55,0.68,0.24)
  _Speed("UV Drift", Float) = 0.01
 }
 SubShader
 {
  // Draw before the world and pin visible cloud pixels to the far background.
  // Opaque level geometry will therefore always cover the clouds, regardless
  // of which direction a gameplay camera is facing.
  Tags { "Queue"="Background+50" "RenderType"="Transparent" "RenderPipeline"="UniversalPipeline" }
  Pass
  {
   Blend SrcAlpha OneMinusSrcAlpha
   ZWrite On
   ZTest Always
   Cull Off
   HLSLPROGRAM
   #pragma vertex vert
   #pragma fragment frag
   #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
   struct A { float4 positionOS : POSITION; float2 uv : TEXCOORD0; };
   struct V { float4 positionHCS : SV_POSITION; float2 uv : TEXCOORD0; };
   CBUFFER_START(UnityPerMaterial)
   float4 _BaseColor;
   float _Speed;
   CBUFFER_END
   V vert(A i)
   {
    V o;
    o.positionHCS=TransformObjectToHClip(i.positionOS.xyz);
    // Keep the card's screen-space shape but render it just in front of the
    // far plane. A slightly-nearer value prevents the later skybox pass from
    // painting over it, while every piece of level geometry remains nearer.
    #if UNITY_REVERSED_Z
     o.positionHCS.z = 0.0001 * o.positionHCS.w;
    #else
     o.positionHCS.z = 0.9999 * o.positionHCS.w;
    #endif
    o.uv=i.uv;
    return o;
   }
   half4 frag(V i) : SV_Target
   {
    float2 uv=i.uv; uv.x += _Time.y*_Speed;
    float edge=smoothstep(0.0,0.13,uv.x)*smoothstep(0.0,0.13,1.0-uv.x)*smoothstep(0.0,0.18,uv.y)*smoothstep(0.0,0.22,1.0-uv.y);
    float n=sin((uv.x*5.7+uv.y*2.1)*6.283)+sin((uv.x*2.3-uv.y*4.7)*6.283)*0.55;
    half alpha=edge*saturate(0.74+n*0.08)*_BaseColor.a;
    // Do not write far depth for the transparent portion of the card; this
    // prevents any rectangular card boundary from masking the skybox.
    clip(alpha-0.002h);
    return half4(_BaseColor.rgb,alpha);
   }
   ENDHLSL
  }
 }
}
