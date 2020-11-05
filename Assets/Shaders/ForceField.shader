Shader "Unlit/Forcefield"
{
    Properties
    {
        _HexagonTex ("Hexagon Texture", 2D) = "white" {}
        _Color ("Color", Vector) = (0.3, 0.3, 0.8, 0.5)
        _PulseSpeed ("Pulse Speed", float) = 1.0
        _PulseIntensity ("Pulse Intensity", float) = 2.0
        _PulseObjScale ("Pulse Object Scale", float) = 3.0
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent"}
        LOD 100
        Cull Off
        Blend SrcAlpha OneMinusSrcAlpha
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct vertexIn
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct vertexOut
            {
                float4 vertex : SV_POSITION;
                float4 objPosition : TEXCOORD1;
                float2 uv : TEXCOORD0;
            };

            sampler2D _HexagonTex;
            float4 _HexagonTex_ST;
            float4 _Color;
            float _PulseSpeed;
            float _PulseIntensity;
            float _PulseObjScale;

            vertexOut vert (vertexIn v)
            {
                vertexOut o;
                o.objPosition = v.vertex;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _HexagonTex);
                return o;
            }

            fixed4 frag (vertexOut i) : COLOR
            {
                // sample the texture
                float horizontalDist = abs(i.objPosition.x);
                fixed4 hexagons = tex2D(_HexagonTex, i.uv);
                float noise = abs(sin(_Time.y*_PulseSpeed - horizontalDist.x*_PulseObjScale + hexagons.r));
                fixed4 hexagonPulse = hexagons * _PulseIntensity * noise;
                fixed4 color;
                color.rgb = _Color.rgb + hexagonPulse.rgb*noise;
                color.a = _Color.a;
                
                return color;
            }
            ENDCG
        }
    }
}
