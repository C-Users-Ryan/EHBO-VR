Shader "Custom/MenuSkybox"
{
    Properties
    {
        
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct MeshData
            {
                float4 oPos : POSITION;
            };

            struct Interpolators
            {
                float4 cPos : SV_POSITION;
                float3 oPos : TEXCOORD0;
            };

            Interpolators vert (MeshData i)
            {
                Interpolators o;

                o.oPos = i.oPos;
                o.cPos = UnityObjectToClipPos(i.oPos);
                
                return o;
            }

            fixed4 frag (Interpolators i) : SV_Target
            {
                float3 n = normalize( i.oPos );
                float depth = pow( 1 - abs( n.y ), 6 );
                float col = n.y > 0 ? .1 + .9 * depth : pow( depth, 6 ) + 0.05;
                
                return float4( col, col, col, 0 );
            }
            ENDCG
        }
    }
}
