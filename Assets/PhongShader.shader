// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/PhongShader"
{
	Properties
	{
		_MainColor("Color", Color) = (1,1,1,1)
		_LightPos("LightPosition", Vector) = (-0.5, 5.5, -0.5, 1)
		_Intensity("Intensity", float) = 1
	}
		SubShader
	{
		Tags { "RenderType" = "Opaque" }
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
				float3 worldPos : TEXCOODR0;
				float3 worldNor : TEXCOODR1;
			};

			float4 _MainColor;
			float4 _LightPos;
			float _Intensity;

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				
				o.worldPos = mul(UNITY_MATRIX_M, v.vertex);
				o.worldNor = mul(UNITY_MATRIX_M, v.normal);

				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				//光的方向
				float3 lightDir = normalize(_LightPos - i.worldPos);
				//距离，计算光的衰减用的
				float3 dist = distance(_LightPos, i.worldPos);

				//Diffuse Light的计算。
				float lightPor = max(0, dot(i.worldNor, lightDir));
				//衰减系数，用2除是个神秘的原因
				float atten = 2 / pow(dist, 2);

				//返回最终的计算结果
				return _MainColor * lightPor * atten * _Intensity;
			}
			ENDCG
		}
	}
}
