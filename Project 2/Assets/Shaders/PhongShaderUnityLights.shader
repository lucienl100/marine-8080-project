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
        _PointLightColor1 ("Point Light Color 1", Color) = (1, 1, 1)
        _PointLightPosition1 ("Point Light Position 1", Vector) = (0.0, 0.0, 0.0)
		_PointLightColor2 ("Point Light Color 2", Color) = (1, 1, 1)
        _PointLightPosition2 ("Point Light Position 2", Vector) = (0.0, 0.0, 0.0)
		_PointLightColor3 ("Point Light Color 3", Color) = (1, 1, 1)
        _PointLightPosition3 ("Point Light Position 3", Vector) = (0.0, 0.0, 0.0)
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
			float3 _PointLightColor1;
			float3 _PointLightPosition1;
			float3 _PointLightColor2;
			float3 _PointLightPosition2;
			float3 _PointLightColor3;
			float3 _PointLightPosition3;
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
				float3 interpolatedNormal = normalize(v.worldNormal ); // * (2.0f*normalize(normalMap.rgb)-1.0f)

				 // Calculate ambient RGB intensities
                float3 amb = v.color.rgb * UNITY_LIGHTMODEL_AMBIENT.rgb * _Ka;
				color.rgb = amb.rgb * mainTexture.rgb;

                // Calculate diffuse RBG reflections, we save the results of L.N because we will use it again for specular
                float3 L = normalize(_PointLightPosition1 - v.worldVertex.xyz);
				float lLength = clamp(5/length(_PointLightPosition1 - v.worldVertex.xyz), 0, 0.5);
                float LdotN = dot(L, interpolatedNormal);
                float3 dif = _fAtt * _PointLightColor1.rgb * _Kd * v.color.rgb * saturate(LdotN);

                float3 V = normalize(_WorldSpaceCameraPos - v.worldVertex.xyz);
                float3 H = normalize(V + L);

				// Calculate specular
                float3 spe = _fAtt * _PointLightColor1.rgb * _Ks * pow(saturate(dot(interpolatedNormal, H)), _specN);

                // Combine Phong illumination model components
                color.rgb += lLength * (dif.rgb + spe.rgb);




                // Calculate diffuse RBG reflections, we save the results of L.N because we will use it again for specular
                L = normalize(_PointLightPosition2 - v.worldVertex.xyz);
				lLength = clamp(10/length(_PointLightPosition2 - v.worldVertex.xyz), 0, 0.5);
                LdotN = dot(L, interpolatedNormal);
                dif = _fAtt * _PointLightColor2.rgb * _Kd * v.color.rgb * saturate(LdotN);

                V = normalize(_WorldSpaceCameraPos - v.worldVertex.xyz);
                H = normalize(V + L);

				// Calculate specular
                spe = _fAtt * _PointLightColor2.rgb * _Ks * pow(saturate(dot(interpolatedNormal, H)), _specN);

                // Combine Phong illumination model components
                // color.rgb += lLength*(dif.rgb + spe.rgb);





                // Calculate diffuse RBG reflections, we save the results of L.N because we will use it again for specular
                L = normalize(_PointLightPosition3 - v.worldVertex.xyz);
				lLength = clamp(20/length(_PointLightPosition3 - v.worldVertex.xyz), 0, 0.2);
                LdotN = dot(L, interpolatedNormal);
                dif = _fAtt * _PointLightColor3.rgb * _Kd * v.color.rgb * saturate(LdotN);

                V = normalize(_WorldSpaceCameraPos - v.worldVertex.xyz);
                H = normalize(V + L);

				// Calculate specular
                spe = _fAtt * _PointLightColor3.rgb * _Ks * pow(saturate(dot(interpolatedNormal, H)), _specN);

                // Combine Phong illumination model components
                // color.rgb += lLength*(dif.rgb + spe.rgb);






                color.a = 1.0f;
                return color;
			}
			ENDCG
		}
	}
}
