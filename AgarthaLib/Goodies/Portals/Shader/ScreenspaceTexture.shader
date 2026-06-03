Shader "AgarthaLib / ScreenspaceTexture"
{
	Properties
	{
		_Color("Tint", Color) = (1, 1, 1, 1)
        _MainTex("Texture", 2D) = "white" {}
		_Blend("Blend", Range(0, 1)) = 0
	}
	SubShader
	{
		Tags
		{
			"Queue" = "Geometry"
			"RenderType" = "Opaque"
		}

		Pass
		{
			Blend SrcAlpha OneMinusSrcAlpha
			ZWrite On
			ZTest LEqual
			Lighting Off

			CGPROGRAM

			#include "UnityCG.cginc"

			#pragma multi_compile SAMPLE_DEFAULT SAMPLE_PREVIOUS

			#pragma vertex vert
			#pragma fragment frag

			sampler2D _MainTex;
            float4 _MainTex_ST;
			fixed4 _Color;
			float _Blend;

			#ifdef SAMPLE_PREVIOUS
				fixed4 MATRIX_VP;
			#endif

			struct appdata
			{
				float4 vertex : POSITION;
			};

			struct v2f
			{
				float4 position : SV_POSITION;
                float4 screenPos : TEXCOORD0;
			};

            v2f vert(appdata v)
			{
                v2f o;
                o.position = UnityObjectToClipPos(v.vertex);

				#ifdef SAMPLE_PREVIOUS
					float4 clipPos = mul(MATRIX_VP, mul(unity_ObjectToWorld, float4(v.vertex.xyz, 1.0)));
					clipPos.y *= _ProjectionParams.x;
					o.screenPos = ComputeNonStereoScreenPos(clipPos);
				#else
					o.screenPos = ComputeNonStereoScreenPos(o.position);
				#endif

                return o;
            }

            fixed4 frag(v2f i) : SV_Target
			{
                float2 textureCoordinate = i.screenPos.xy / i.screenPos.w;
				float4 tex = tex2D(_MainTex, textureCoordinate);
                fixed4 col = lerp(tex, _Color, _Blend);
                return col;
            }

			ENDCG
		}
	}
}