Shader "Depth Perception/Panorama State"
{
    Properties
    {
        [MainTexture] _BaseMap("Valley Panorama", 2D) = "white" {}
        [MainColor] _BaseColor("Reality Tint", Color) = (1, 1, 1, 1)
        _PerceptionState("Perception State", Range(0, 2)) = 0
        _HallucinationStrength("Hallucination Strength", Range(0, 1.5)) = 1
        _LostRealityStrength("Lost Reality Strength", Range(0, 1.5)) = 1
        _MotionSpeed("Motion Speed", Range(0, 1)) = 0.12
    }

    SubShader
    {
        Tags
        {
            "RenderType" = "Opaque"
            "Queue" = "Background"
            "RenderPipeline" = "UniversalPipeline"
        }

        Pass
        {
            Name "PanoramaState"
            Cull Off
            ZWrite On
            ZTest LEqual

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);

            CBUFFER_START(UnityPerMaterial)
                float4 _BaseMap_ST;
                half4 _BaseColor;
                float _PerceptionState;
                float _HallucinationStrength;
                float _LostRealityStrength;
                float _MotionSpeed;
            CBUFFER_END

            Varyings Vert(Attributes input)
            {
                Varyings output;
                output.positionCS = TransformObjectToHClip(input.positionOS.xyz);
                output.uv = TRANSFORM_TEX(input.uv, _BaseMap);
                return output;
            }

            half4 Frag(Varyings input) : SV_Target
            {
                float hall = saturate(_PerceptionState) * _HallucinationStrength;
                float lost = saturate(_PerceptionState - 1.0) * _LostRealityStrength;
                float time = _Time.y * _MotionSpeed;

                float2 uv = input.uv;
                float horizonBend = sin((uv.x * 6.2831853) + time * 0.7) * (0.004 * hall + 0.012 * lost);
                uv.y += horizonBend;

                float2 centered = uv - 0.5;
                float radius = length(centered);
                float swirl = sin(radius * 18.0 - time) * (0.002 * hall + 0.005 * lost);
                float sineValue = sin(swirl);
                float cosineValue = cos(swirl);
                centered = float2(
                    centered.x * cosineValue - centered.y * sineValue,
                    centered.x * sineValue + centered.y * cosineValue);
                uv = centered + 0.5;

                uv.x += sin(uv.y * 13.0 + time * 1.2) * (0.0015 * hall + 0.004 * lost);
                uv.y += sin(uv.x * 10.0 - time * 0.8) * (0.001 * hall + 0.003 * lost);
                uv.x = frac(uv.x);
                uv.y = saturate(uv.y);

                half3 baseColor = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, uv).rgb * _BaseColor.rgb;

                float2 mirrorUv = float2(frac(1.0 - uv.x + sin(time * 0.4) * 0.006), uv.y);
                half3 mirrorColor = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, mirrorUv).rgb * _BaseColor.rgb;
                baseColor = lerp(baseColor, mirrorColor, 0.18 * lost);

                float2 echoUv = float2(frac(uv.x + 0.010 + sin(time * 0.5) * 0.004), saturate(uv.y + 0.003));
                half3 echoColor = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, echoUv).rgb * _BaseColor.rgb;
                baseColor = lerp(baseColor, echoColor, 0.08 * hall + 0.14 * lost);

                const half3 signal = half3(0.5098, 0.9137, 0.9333);
                const half3 drift = half3(0.6784, 0.5490, 1.0000);
                const half3 doubt = half3(0.9059, 0.5647, 0.7412);
                float gradientWave = 0.5 + 0.5 * sin(uv.x * 11.0 + uv.y * 5.0 + time * 0.65);
                half3 dreamTint = lerp(signal, drift, smoothstep(0.15, 0.8, gradientWave));
                dreamTint = lerp(
                    dreamTint,
                    doubt,
                    smoothstep(0.62, 1.0, gradientWave) * (0.18 * hall + 0.48 * lost));

                half luminance = dot(baseColor, half3(0.2126, 0.7152, 0.0722));
                half3 treatedColor = baseColor * (0.62 + dreamTint * 0.52) + dreamTint * (luminance * 0.30 + 0.025);
                float treatment = saturate(0.24 * hall + 0.28 * lost);
                half3 finalColor = lerp(baseColor, treatedColor, treatment);
                finalColor += dreamTint * (0.015 * hall + 0.035 * lost) * smoothstep(0.35, 1.0, gradientWave);
                finalColor = lerp(finalColor, finalColor * half3(0.80, 0.88, 1.10), lost * 0.10);

                return half4(finalColor, 1.0);
            }
            ENDHLSL
        }
    }
}
