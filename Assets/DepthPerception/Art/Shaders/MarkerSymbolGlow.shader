Shader "DepthPerception/MarkerSymbolGlow"
{
 Properties { _BaseColor("Tint",Color)=(0.5,0.75,0.85,0) _EmissionColor("Emission",Color)=(0,0,0,0) }
 SubShader
 {
  Tags { "Queue"="Transparent+20" "RenderType"="Transparent" "RenderPipeline"="UniversalPipeline" }
  Pass
  {
   Blend SrcAlpha One
   ZWrite Off
   Cull Off
   HLSLPROGRAM
   #pragma vertex vert
   #pragma fragment frag
   #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
   struct A { float4 positionOS : POSITION; };
   struct V { float4 positionHCS : SV_POSITION; };
   CBUFFER_START(UnityPerMaterial)
   float4 _BaseColor;
   float4 _EmissionColor;
   CBUFFER_END
   V vert(A i){V o;o.positionHCS=TransformObjectToHClip(i.positionOS.xyz);return o;}
   half4 frag(V i):SV_Target{return half4(_BaseColor.rgb+_EmissionColor.rgb,_BaseColor.a);}
   ENDHLSL
  }
 }
}