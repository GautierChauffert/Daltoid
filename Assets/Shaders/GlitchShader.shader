Shader "Hidden/GlitchShader"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_DispTex ("Base (RGB)", 2D) = "bump" {}
		_Intensity ("Glitch Intensity", Range(0.1, 1.0)) = 1
	}
	
	SubShader
	{
		Pass
		{
			ZTest Always Cull Off ZWrite Off
			Fog { Mode off }

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma fragmentoption ARB_precision_hint_fastest

			#include "UnityCG.cginc"



			uniform sampler2D _MainTex;
			uniform sampler2D _DispTex;
			float2 _MainTex_TexelSize;

			float2 _ScanLineJitter;
			float2 _VerticalJump;
			float _HorizontalShake;
			float2 _ColorDrift;

			
			float _Intensity;
			float filter_radius;
			float flip_up, flip_down;
			float displace;
			float scale;




			float nrand(float x, float y) {
				return frac(sin(dot(float2(x, y), float2(12.9898, 78.233))) * 43758.5453);
			}

			struct v2f {
				float4 pos : POSITION;
				float2 uv : TEXCOORD0;
			};

			


			v2f vert(appdata_img v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = v.texcoord.xy;
				
				return o;
			}

			half4 frag(v2f i) : COLOR
			{
				half4 normal = tex2D(_DispTex, i.uv.xy * scale);
				
				if(i.uv.y < flip_up) {
					i.uv.y = 1 - (i.uv.y + flip_up);
				}
				
				if(i.uv.y > flip_down) {
					i.uv.y = 1 - (i.uv.y - flip_down);
				}
				
				i.uv.xy += (normal.xy - 0.5) * displace * _Intensity;






				float u = i.uv.x;
				float v = i.uv.y;

				// Scan line jitter
				float jitter = nrand(v, _Time.x) * 2 - 1;
				jitter *= step(_ScanLineJitter.y, abs(jitter)) * _ScanLineJitter.x;

				// Vertical jump
				float jump = lerp(v, frac(v + _VerticalJump.y), _VerticalJump.x);

				// Horizontal shake
				float shake = (nrand(_Time.x, 2) - 0.5) * _HorizontalShake;

				// Color drift
				float drift = sin(jump + _ColorDrift.y) * _ColorDrift.x;

				half4 src1 = tex2D(_MainTex, frac(float2(u + jitter + shake, jump)));
				half4 src2 = tex2D(_MainTex, frac(float2(u + jitter + shake + drift, jump)));

				return half4(src1.r, src2.g, src1.b, 1);
			}
			ENDCG
		}
	}

	Fallback off
}
