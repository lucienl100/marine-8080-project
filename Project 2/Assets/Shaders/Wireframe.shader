// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Geometry/Wireframe"
{
	Properties
	{
		[PowerSlider(3.0)]
		_WireframeVal ("Wireframe width", Range(0., 0.5)) = 0.05
		_FrontColor ("Front color", color) = (1., 1., 1., 1.)
		_BackColor ("Back color", color) = (1., 1., 1., 1.)
		[Toggle] _RemoveDiag("Remove diagonals?", Float) = 0.

		_SurfaceColor ("Surface Color", Color) = (0, 0, 0, 1)
		_fAtt ("fAtt", Range(0,5)) = 1
        _Ka ("Ambient reflection constant",Range(0,5)) = 1.5
        _Kd ("Diffuse reflection constant",Range(0,5)) = 1
        _Ks ("Specular reflection constant", Range(0,5)) = 0.15
        _specN ("SpecularN", Range(1,20)) = 1
		_PointLightReds ("Point Light Color 1", Vector) = (1, 1, 1, 1)
		_PointLightBlues ("Point Light Color 2", Vector) = (1, 1, 1, 1)
		_PointLightGreens ("Point Light Color 3", Vector) = (1, 1, 1, 1)
        _PointLightPositionX ("Point Light Position X", Vector) = (0.0, 0.0, 0.0, 0.0)
		_PointLightPositionY ("Point Light Position Y", Vector) = (0.0, 0.0, 0.0, 0.0)
		_PointLightPositionZ ("Point Light Position Z", Vector) = (0.0, 0.0, 0.0, 0.0)
		_mainTexture("Main texture", 2D) = "white" {}
		_normalMap("Normal map", 2D) = "black" {}

		_OutlineColor ("Outline Color", Color) = (1,1,1,1)
		_Outline ("Outline width", Range (0, 1)) = .5
	}
	SubShader
	{
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"
			#include "UnityLightingCommon.cginc"

			struct vertIn
			{
				float4 vertex : POSITION;
				float4 normal : NORMAL;
				float4 color : COLOR;
				float2 uv : TEXCOORD0;
			};

			struct vertOut
			{
				float4 vertex : SV_POSITION;
				float4 color : COLOR;
				float2 uv : TEXCOORD0;
				float4 worldVertex : TEXCOORD1;
				float3 worldNormal : TEXCOORD2;
				float3 viewDir : TEXCOORD3;
			};

			sampler2D _mainTexture;
			float4 _mainTexture_ST;
			sampler2D _normalMap;
			float4 _normalMap_ST;
			float4 _SurfaceColor;
			float _fAtt;
			float _Ka;
			float _Kd;
			float _Ks;
			float _specN;
			float4 _PointLightReds;
			float4 _PointLightBlues;
			float4 _PointLightGreens;
			float4 _PointLightPositionX;
			float4 _PointLightPositionY;
			float4 _PointLightPositionZ;

			float _Outline;
 			float4 _OutlineColor;
			// Implementation of the vertex shader
			vertOut vert(vertIn v)
			{
				vertOut o;

				// Convert Vertex position and corresponding normal into world coords.
				// Note that we have to multiply the normal by the transposed inverse of the world 
				// transformation matrix (for cases where we have non-uniform scaling; we also don't
				// care about the "fourth" dimension, because translations don't affect the normal) 
				float4 worldVertex = mul(unity_ObjectToWorld, v.vertex);
				float3 worldNormal = normalize(mul(transpose((float3x3)unity_WorldToObject), v.normal.xyz));

				// Transform vertex in world coordinates to camera coordinates, and pass colour
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.color = _SurfaceColor;

				// Pass out the world vertex position and world normal to be interpolated
				// in the fragment shader (and utilised)
				o.worldVertex = worldVertex;
				o.worldNormal = worldNormal;
				o.uv = TRANSFORM_TEX(v.uv, _mainTexture);

				o.viewDir = WorldSpaceViewDir(v.vertex);
				return o;
			}

			// Implementation of the fragment shader
			fixed4 frag(vertOut v) : SV_Target
			{				
				fixed4 mainTexture = tex2D(_mainTexture, v.uv);
				fixed4 normalMap = tex2D(_normalMap, v.uv);
                float4 color;

				// float dist = distance(worldVertex,_WorldSpaceLightPos0);
				// o.emission = (dist - 300) / 1400;

				// Our interpolated normal might not be of length 1
				float3 interpolatedNormal = normalize(v.worldNormal); // * (2.0f*normalize(normalMap.rgb)-1.0f)

				float3 viewDir = normalize(v.viewDir);
				float4 rimDot = 1 - dot(viewDir, interpolatedNormal);
				float rimIntensity = smoothstep(_Outline - 0.01, _Outline + 0.01, rimDot);
				float4 rim = rimIntensity * _OutlineColor;

				 // Calculate ambient RGB intensities
                float3 amb = v.color.rgb * UNITY_LIGHTMODEL_AMBIENT.rgb * _Ka;
				color.rgb =  amb.rgb * mainTexture.rgb;

                // Calculate diffuse RBG reflections, we save the results of L.N because we will use it again for specular
                float3 L;
				float lLength;
                float LdotN;
                float3 dif;

                float3 V;
                float3 H;

				// Calculate specular
                float3 spe  ;
				float4 lightPosition;
				float4 lightColour;

				for (int index = 0; index < 4; index++) {
					// Calculate diffuse RBG reflections, we save the results of L.N because we will use it again for specular
					lightPosition = float4(_PointLightPositionX[index], _PointLightPositionY[index], _PointLightPositionZ[index], 1.0);
					lightColour = float4(_PointLightReds[index], _PointLightBlues[index], _PointLightGreens[index], 1.0);
					L = normalize(lightPosition - v.worldVertex.xyz);
					lLength = clamp(5/length(lightPosition - v.worldVertex.xyz), 0, 0.45);
					LdotN = dot(L, interpolatedNormal);
					dif = _fAtt * lightColour.rgb * _Kd * v.color.rgb * saturate(LdotN);

					V = normalize(_WorldSpaceCameraPos - v.worldVertex.xyz);
					H = normalize(V + L);

					// Calculate specular
					spe = _fAtt * lightColour.rgb * _Ks * pow(saturate(dot(interpolatedNormal, H)), _specN);

					// Combine Phong illumination model components
					color.rgb += lLength * (dif.rgb + spe.rgb);
				}

                color.a = 1.0f;
                return color;
			}
			ENDCG
		}

		Tags { "Queue"="Geometry" "RenderType"="Opaque" }

		Pass
		{
			Cull Back
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma geometry geom

			// Change "shader_feature" with "pragma_compile" if you want set this keyword from c# code
			#pragma shader_feature __ _REMOVEDIAG_ON

			#include "UnityCG.cginc"

			struct v2g {
				float4 worldPos : SV_POSITION;
			};

			struct g2f {
				float4 pos : SV_POSITION;
				float3 bary : TEXCOORD0;
			};

			v2g vert(appdata_base v) {
				v2g o;
				o.worldPos = mul(unity_ObjectToWorld, v.vertex);
				return o;
			}

			[maxvertexcount(3)]
			void geom(triangle v2g IN[3], inout TriangleStream<g2f> triStream) {
				float3 param = float3(0., 0., 0.);

				#if _REMOVEDIAG_ON
				float EdgeA = length(IN[0].worldPos - IN[1].worldPos);
				float EdgeB = length(IN[1].worldPos - IN[2].worldPos);
				float EdgeC = length(IN[2].worldPos - IN[0].worldPos);
				
				if(EdgeA > EdgeB && EdgeA > EdgeC)
					param.y = 1.;
				else if (EdgeB > EdgeC && EdgeB > EdgeA)
					param.x = 1.;
				else
					param.z = 1.;
				#endif

				g2f o;
				o.pos = mul(UNITY_MATRIX_VP, IN[0].worldPos);
				o.bary = float3(1., 0., 0.) + param;
				triStream.Append(o);
				o.pos = mul(UNITY_MATRIX_VP, IN[1].worldPos);
				o.bary = float3(0., 0., 1.) + param;
				triStream.Append(o);
				o.pos = mul(UNITY_MATRIX_VP, IN[2].worldPos);
				o.bary = float3(0., 1., 0.) + param;
				triStream.Append(o);
			}

			float _WireframeVal;
			fixed4 _FrontColor;

			fixed4 frag(g2f i) : SV_Target {
			if(!any(bool3(i.bary.x <= _WireframeVal, i.bary.y <= _WireframeVal, i.bary.z <= _WireframeVal)))
				 discard;

				return _FrontColor;
			}

			ENDCG
		}

	}
}