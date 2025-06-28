Shader "Custom/RadialProgress"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Progress ("Progress", Range(0,1)) = 0
        _StartAngle ("Start Angle (Degrees)", Range(0,360)) = 0
        _Antialiasing ("Antialiasing", Range(0.005,0.1)) = 0.05
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float2 centerPos : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Progress;
            float _StartAngle;
            float _Antialiasing;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.centerPos = float2(0.5, 0.5);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // 计算到中心的距离和角度
                float2 dir = i.uv - i.centerPos;
                float dist = length(dir);
                float angle = atan2(dir.y, dir.x);
                
                // 角度归一化到[0, 2π]
                angle = angle < 0 ? angle + 6.283185 : angle;
                
                // 将起始角度转换为弧度
                float startRadians = radians(_StartAngle);
                
                // 调整角度坐标系 (考虑起始角度)
                float adjustedAngle = angle - startRadians;
                adjustedAngle = adjustedAngle < 0 ? adjustedAngle + 6.283185 : adjustedAngle;
                
                // 进度对应的目标角度
                float targetAngle = _Progress * 6.283185;
                
                // 边缘抗锯齿处理
                float aa = smoothstep(targetAngle + _Antialiasing, 
                                     targetAngle, 
                                     adjustedAngle + 1e-5);
                
                // 组合最终效果
                fixed4 col = tex2D(_MainTex, i.uv);
                col.a *= saturate(aa + (1 - step(targetAngle + 0.0001, adjustedAngle + 1e-5)));
                
                // 圆形遮罩
                float circle = smoothstep(0.5 + _Antialiasing, 
                                        0.5 - _Antialiasing, 
                                        dist);
                col.a *= circle;
                
                return col;
            }
            ENDCG
        }
    }
}