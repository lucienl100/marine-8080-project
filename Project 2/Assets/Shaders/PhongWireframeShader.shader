//UNITY_SHADER_NO_UPGRADE

Shader "Custom/Geometry/PhongWireframeShader"
{
	Properties
	{
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

		
		_WireframeVal ("Wireframe width", Range(0., 0.5)) = 0.09
		_WireframeColor ("Wireframe color", color) = (1., 1., 1., 1.)
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
				return o;
			}

			// Implementation of the fragment shader
			fixed4 frag(vertOut v) : SV_Target
			{				
				fixed4 mainTexture = tex2D(_mainTexture, v.uv);
				fixed4 normalMap = tex2D(_normalMap, v.uv);
                float4 color;

				// Our interpolated normal might not be of length 1
				float3 interpolatedNormal = normalize(v.worldNormal); // * (2.0f*normalize(normalMap.rgb)-1.0f)

				 // Calculate ambient RGB intensities
                float3 amb = v.color.rgb * UNITY_LIGHTMODEL_AMBIENT.rgb * _Ka;
				color.rgb =  amb.rgb * mainTexture.rgb;

                float3 L;
				float lLength;
                float LdotN;
                float3 dif;
                float3 V;
                float3 H;
                float3 spe;
				float4 lightPosition;
				float4 lightColour;

				for (int index = 0; index < 4; index++) {
					// Set light position and colour based on 
					lightPosition = float4(_PointLightPositionX[index], _PointLightPositionY[index], _PointLightPositionZ[index], 1.0);
					lightColour = float4(_PointLightReds[index], _PointLightBlues[index], _PointLightGreens[index], 1.0);
					
					// Calculate diffuse RBG reflections, we save the results of L.N because we will use it again for specular
					L = normalize(lightPosition - v.worldVertex.xyz);
					LdotN = dot(L, interpolatedNormal);
					dif = _fAtt * lightColour.rgb * _Kd * v.color.rgb * saturate(LdotN);

					// clamp power of light based on distance
					lLength = clamp(5/length(lightPosition - v.worldVertex.xyz), 0, 0.45);

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

		// Wireframe pass
		Pass
		{
			Cull Back
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma geometry geom

			#include "UnityCG.cginc"

			struct vertIn {
				float4 worldPos : SV_POSITION;
			};

			struct vertOut {
				float4 pos : SV_POSITION;
				float3 bary : TEXCOORD0;
			};

			// Implementation of the vertex shader
			vertIn vert(appdata_base v) {
				vertIn o;
				o.worldPos = mul(unity_ObjectToWorld, v.vertex);
				return o;
			}

			// Implementation of the geometry shader
			[maxvertexcount(3)]
			void geom(triangle vertIn triIn[3], inout TriangleStream<vertOut> triStream) {
				float3 noWire = float3(0., 0., 0.);

				// Define edges length
				float EdgeALength = length(triIn[0].worldPos - triIn[1].worldPos);
				float EdgeBLength = length(triIn[1].worldPos - triIn[2].worldPos);
				float EdgeCLength = length(triIn[2].worldPos - triIn[0].worldPos);
				
				// Find diagonal line (longest line in triangle)
				if(EdgeALength > EdgeBLength && EdgeALength > EdgeCLength)
					noWire.y = 1.; // edge A
				else if (EdgeBLength > EdgeCLength && EdgeBLength > EdgeALength)
					noWire.x = 1.; // edge B
				else
					noWire.z = 1.; // edge C

				// Remove diagonal wires by setting bary to 1 for each vertex
				vertOut o;
				o.pos = mul(UNITY_MATRIX_VP, triIn[0].worldPos);
				o.bary = float3(1., 0., 0.) + noWire;
				triStream.Append(o);

				o.pos = mul(UNITY_MATRIX_VP, triIn[1].worldPos);
				o.bary = float3(0., 0., 1.) + noWire;
				triStream.Append(o);

				o.pos = mul(UNITY_MATRIX_VP, triIn[2].worldPos);
				o.bary = float3(0., 1., 0.) + noWire;
				triStream.Append(o);
			}

			float _WireframeVal;
			fixed4 _WireframeColor;

			// Implementation of the fragment shader
			fixed4 frag(vertOut v) : SV_Target {
				// Discard if too far from edge or diagonal
				if(!(v.bary.x <= _WireframeVal || v.bary.y <= _WireframeVal || v.bary.z <= _WireframeVal)) {
					discard;
				}
				
				// Return wireframe colour
				return _WireframeColor;
			}
			ENDCG
		}

	}
}