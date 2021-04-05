﻿Shader "Unlit/DrawTracks"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Coordinate("Coordinate", Vector) = (0,0,0,0)  
		_Color("Draw Color",Color) = (1,0,0,0) // red
		_Size("Brush size",Range(1,500)) = 150
		_Strength("Brush Strength",Range(0,1)) = 1
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

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

			sampler2D _MainTex;
			float4 _MainTex_ST;
			fixed4 _Coordinate,_Color;
			half _Size;
			half _Strength;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture , UV coordinates are alwys between 0 and 1 so we staturate it like clamp in C#
				fixed4 col = tex2D(_MainTex, i.uv);
				// make brush size by pow hugher the number smaller the brush
				float draw = pow( saturate(1-distance(i.uv, _Coordinate.xy)),500/_Size);
				fixed4 drawcol = _Color * (draw *_Strength); // draw is multiplued by strength

				return saturate(col + drawcol);
			}
			ENDCG
		}
	}
}
