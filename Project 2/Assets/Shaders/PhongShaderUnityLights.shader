// Note we are using a Unity "directional" light here. This means the 
// incoming light "position" data is actually a vector representing the
// light direction rather than an actual world-space position. As an 
// experiment, try moving the UnityLight object in the game and notice
// how it has no effect on the lighting for the cube using this shader. 
// (Only *rotating* it will have an effect.)

//UNITY_SHADER_NO_UPGRADE

Shader "Unlit/PhongShaderUnityLights"
{
	Properties
	{
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
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.color = v.color;

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

				// float dist = distance(worldVertex,_WorldSpaceLightPos0);
				// o.emission = (dist - 300) / 1400;

				// Our interpolated normal might not be of length 1
				float3 interpolatedNormal = normalize(v.worldNormal); // * (2.0f*normalize(normalMap.rgb)-1.0f)

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
	}
}
