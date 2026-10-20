Shader "Custom/MarchingShader"
{
    Properties
    {
        _GrassColor ("Grass Color", Color) = (0.2, 0.6, 0.1, 1)
        _DirtColor ("Dirt Color", Color) = (0.4, 0.25, 0.12, 1)

        _LightDirection ("Light Direction", Vector) = (0, 1, 0, 0)
        _AmbientStrength ("Ambient Strength", Range(0, 1)) = 0.3
    }

    SubShader
    {
        Tags
        {
            "RenderType" = "Opaque"
        }

        Pass
        {
            Tags { "LightMode" = "ForwardBase" }

            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fwdbase

            #include "UnityCG.cginc"
            #include "Lighting.cginc"

            fixed4 _GrassColor;
            fixed4 _DirtColor;
            float4 _LightDirection;
            float _AmbientStrength;

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 position : SV_POSITION;
                float3 worldNormal : TEXCOORD0;
                float3 worldPos : TEXCOORD1;
            };

            v2f vert(appdata v)
            {
                v2f o;

                o.position = UnityObjectToClipPos(v.vertex);
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;

                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float3 normal = normalize(i.worldNormal);

                // 1. Determine whether the surface faces upward.
                float upDot = dot(normal, float3(0, 1, 0));

                // Green on top, dirt on sides and bottom.
                float grassMask = step(0.7, upDot);

                fixed3 surfaceColor =
                    lerp(_DirtColor.rgb, _GrassColor.rgb, grassMask);

                // 2. Calculate directional lighting.
                float3 lightDir = normalize(_WorldSpaceLightPos0.xyz);

                float NdotL = saturate(dot(normal, lightDir));

                // 3. Combine ambient and direct light.
                fixed3 ambient = ShadeSH9(float4(normal, 1));

                fixed3 lighting =
                    ambient * _AmbientStrength +
                    _LightColor0.rgb * NdotL;

                // 4. Apply lighting to the surface color.
                fixed3 finalColor = surfaceColor * lighting;

                return fixed4(finalColor, 1);
            }

            ENDCG
        }
    }

    FallBack "Diffuse"
}
