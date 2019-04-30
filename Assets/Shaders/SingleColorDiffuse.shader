Shader "Hidden/SingleColorDiffuse"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_threshold ("Color threshold", Range (0, 1)) = 0.25
		_targetColor ("Color target", Color) = (1, 0, 0, 1)
	}

	SubShader
	{
		Pass
		{
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag

			#include "UnityCG.cginc"

			float3 rgb2hsv(float3 c)
			{
				float4 K = float4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
				float4 p = lerp(float4(c.bg, K.wz), float4(c.gb, K.xy), step(c.b, c.g));
				float4 q = lerp(float4(p.xyw, c.r), float4(c.r, p.yzx), step(p.x, c.r));

				float d = q.x - min(q.w, q.y);
				float e = 1.0e-10;
				return float3(abs(q.z + (q.w - q.y) / (6.0 * d + e)), d / (q.x + e), q.x);
			}

			float3 hsv2rgb(float3 c)
			{
				float4 K = float4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
				float3 p = abs(frac(c.xxx + K.xyz) * 6.0 - K.www);
				return c.z * lerp(K.xxx, clamp(p - K.xxx, 0.0, 1.0), c.y);
			}

			float color_dist(float3 hsv1, float3 hsv2)
			{
				// 1 -> different, 0 -> same
				return 2.0 * min(abs(hsv1.x - hsv2.x), 1.0 - abs(hsv1.x - hsv2.x));
			}


			uniform sampler2D _MainTex;
			uniform float _threshold;
			uniform fixed4 _targetColor;

			float4 frag(v2f_img i) : COLOR
			{
				float4 c = tex2D(_MainTex, i.uv);
				float4 result = c;

				_targetColor.gb = _targetColor.bg;

				float lum = c.r*.3 + c.g*.59 + c.b*.11;

				result.rgb = (color_dist(rgb2hsv(c.rbg), rgb2hsv(_targetColor.rgb)) < _threshold) ? c.rgb : float3(lum, lum, lum);

				return result;
			}
			ENDCG
		}
	}
}
