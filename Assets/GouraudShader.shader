// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/GouraudShader"
{
	Properties
	{
		_MainColor ("Color", Color) = (1,1,1,1)
		_LightPos("LightPosition", Vector) = (-0.5, 5.5, -0.5, 1)
		_Intensity("Intensity", float) = 1
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
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float4 color: TEXCOORD0;
			};

			float4 _MainColor;
			float4 _LightPos;
			float _Intensity;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);

				//将我们之前手写的点的坐标信息还有法线信息转换到世界坐标系计算。
				float3 worldPos = mul(UNITY_MATRIX_M, v.vertex);
				float3 worldNor = mul(UNITY_MATRIX_M, v.normal);

				//光的方向
				float3 lightDir = normalize(_LightPos - worldPos);
				//距离，计算光的衰减用的
				float3 dist = distance(_LightPos, worldPos);

				//Diffuse Light的计算。
				float lightPor = max(0, dot(worldNor, lightDir));
				//衰减系数，用2除是个神秘的原因
				float atten = 2 / pow(dist, 2);

				//最终展现在屏幕中的颜色，插值后传递到fragment函数里显示颜色。
				o.color = _MainColor *lightPor * atten * _Intensity;

				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				return i.color;
			}
			ENDCG
		}
	}
}
