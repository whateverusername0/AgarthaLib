Shader "Screenspace/Texture"
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
			CGPROGRAM
			#include "UnityCG.cginc"

			#pragma vertex vert
			#pragma fragment frag

			sampler2D _MainTex;
            float4 _MainTex_ST;
			fixed4 _Color;
			float _Blend;

			struct appdata
			{
				float4 vertex : POSITION;
			};

			struct v2f
			{
				float4 position : SV_POSITION;
                float4 screenPosition : TEXCOORD0;
			};

			struct fragOut
            {
                fixed4 color : SV_TARGET;
            };

            v2f vert(appdata v)
			{
                v2f o;
                o.position = UnityObjectToClipPos(v.vertex);
                o.screenPosition = ComputeScreenPos(o.position);
                return o;
            }

            fragOut frag(v2f i)
			{
                fragOut o;
                float2 textureCoordinate = i.screenPosition.xy / i.screenPosition.w;
				float4 tex = tex2D(_MainTex, textureCoordinate);
                fixed4 col = lerp(tex, _Color, _Blend);
                o.color = col;
                return o;
            }

			ENDCG
		}
	}
}