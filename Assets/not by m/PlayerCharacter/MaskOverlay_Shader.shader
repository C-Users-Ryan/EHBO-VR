Shader "CustomVR/MaskOverlay"
{
    Properties
    {
        _Color ("Color", Color) = ( 1, 1, 1, 0 )
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" }
        LOD 100

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct MeshData
            {
                float4 oPos : POSITION;
                float2 uv : TEXCOORD0;

                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Interpolators
            {
                float4 cPos : SV_POSITION;
                float2 uv : TEXCOORD0;

                UNITY_VERTEX_OUTPUT_STEREO
            };

            float4 _Color;

            Interpolators vert (MeshData i)
            {
                Interpolators o;

                UNITY_SETUP_INSTANCE_ID(i);
                UNITY_INITIALIZE_OUTPUT(Interpolators, o);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

                o.cPos = float4( i.uv * 2. - 1. , 1, 1);
                return o;
            }

            fixed4 frag (Interpolators i) : SV_Target
            {
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);
                
                return _Color;
            }
            ENDCG
        }
    }
}
