Shader "Custom/VertexColor" {
    Properties {
        _PointSize ("Point Size", Float) = 1.0
    }
    SubShader {
        Pass {
            LOD 200

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0 // PSIZEセマンティクスを使うため

            struct VertexInput {
                float4 v : POSITION;
                float4 color: COLOR;
            };

            struct VertexOutput {
                float4 pos : SV_POSITION;
                float4 col : COLOR;
                float pointSize : PSIZE; // PSIZEセマンティクスを追加
            };

            float _PointSize; // 点のサイズを設定する変数

            VertexOutput vert(VertexInput v) {
                VertexOutput o;
                o.pos = UnityObjectToClipPos(v.v);
                o.col = v.color;
                o.pointSize = _PointSize; // 点のサイズを設定
                return o;
            }

            float4 frag(VertexOutput o) : SV_Target {
                return o.col;
            }

            ENDCG
        } 
    }
}
