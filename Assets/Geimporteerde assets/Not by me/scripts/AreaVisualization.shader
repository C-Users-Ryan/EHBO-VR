Shader "Custom/AreaVisualization"
{
    Properties
    {
        _Color ("Grid Color", Color) = ( 1, 1, 1, 1 )
        _LineWidth ( "Gridline Width" , Range ( 0 ,1 )) = 0.1
        _GridSize ( "Grid Size", Float ) = 0.5
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" }

        Pass
        {
            Cull Back
            Blend One Zero
            
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
                float2 oCoords : TEXCOORD0;
                float2 wCoords : TEXCOORD1;
            };

            float4 _Color;
            float _LineWidth;
            float _GridSize;

            Interpolators vert ( MeshData i )
            {
                Interpolators o;
                o.oCoords = i.oPos.xy;
                o.wCoords = mul( unity_ObjectToWorld, i.oPos ).xz;
                o.cPos = UnityObjectToClipPos( i.oPos );
                
                return o;
            }

            inline float decimals( float subject )
            {
                return abs( subject - int( subject ));
            }

            fixed4 frag ( Interpolators i ) : SV_Target
            {
                // sample the texture
                float xMod = decimals( i.wCoords.x / _GridSize );
                float yMod = decimals( i.wCoords.y / _GridSize );

                if (
                    decimals( xMod + _LineWidth * .5 ) > _LineWidth &&
                    decimals( yMod + _LineWidth * .5 ) > _LineWidth &&
                    ( .5 - abs( i.oCoords.x ) ) * abs( i.wCoords.x ) * 5  > _LineWidth && 
                    ( .5 - abs( i.oCoords.y ) ) * abs( i.wCoords.y ) * 5  > _LineWidth
                    ) 
                    discard;
                
                return _Color;
            }
            ENDCG
        }
    }
}
