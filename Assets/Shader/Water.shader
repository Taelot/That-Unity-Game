Shader "Water"
{

	Properties
	{
		[HDR]
		_Color("Color", Color) = (0.5,0.5,0.5,0.5)
		//how transparent the object will be when camera is near the object
        _Transparent ("Transparent", float) =  0
		
		// Noise texture used to generate waves.
		_WaveNoise("Wave Noise", 2D) = "white" {}

		// Speed, in UVs per second the noise will scroll. Only the xy components are used.
		_WaveScroll("Wave Scroll", Vector) = (0.03, 0.03, 0, 0)

		//how transparent the wave will be when camera is far from the object
		_WaveTransparent("Wave Transparent",float) = 0.27

		_WaveSize("Wave Size",float) =1

		_WaveLength("Wave Lenght",float) =1

	    // Values in the noise texture above this cutoff are rendered on the surface.
		_WaveNoiseLimit("Wave Noise Limit", float) = 0.777
        
		[HDR]
		_WaveColor("Wave Color", Color) = (1,1,1,1)

		_WaveHighLightNoiseLimit("Wave High Light Noise Limit", float) = 0.777

        [HDR]
		_WaveHighLightColor("Wave High Light Color", Color) = (1,1,1,1)


	}
	SubShader
	{
	Tags { "Queue"="Transparent" "RenderType"="Transparent" }

	Pass{
    Blend SrcAlpha OneMinusSrcAlpha
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			//for this shader to interact with other shadow
			#pragma multi_compile_fwdbase
			
			#include "UnityCG.cginc"
			#include "AutoLight.cginc"

			struct vertIn 
			{
				float4 vertex : POSITION;				
				float4 uv : TEXCOORD0;
				float3 normal : NORMAL;
			};

			struct vertOut 
			{
				float4 pos : SV_POSITION;
				float3 worldPos : TEXCOORD2;
				float3 cameraLength :TEXCOORD3;
				float2 noiseUV : TEXCOORD4;
				//4d value for storing shadow coord
				SHADOW_COORDS(2)
			};


			sampler2D _WaveNoise;
			float4 _WaveNoise_ST;

			float _WaveSize;
			float _WaveLength;

			vertOut  vert (vertIn  v)
			{
				float4 displacement = float4(0.0f, sin((v.vertex.x + _Time.y)*_WaveLength )/_WaveSize, 0.0f, 0.0f);
				v.vertex += displacement;
				
				vertOut  o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.worldPos=mul (unity_ObjectToWorld, v.vertex);
				o.cameraLength =length(_WorldSpaceCameraPos - o.worldPos.xyz);
				o.noiseUV = TRANSFORM_TEX(v.uv, _WaveNoise);

				return o;
			}
			
			float4 _Color;
			float _Transparent;
			float2 _WaveScroll;
			float _WaveTransparent;
			float _WaveNoiseLimit;
			float4 _WaveColor;
            float _WaveHighLightNoiseLimit;
			float4 _WaveHighLightColor;

			float4 frag (vertOut  i) : SV_Target
			{

				
				float4 color =  _Color ;

				if(_Transparent > 0){
					_Transparent=-_Transparent;
				}

				if(_Transparent!=0){
					color.a =saturate(-i.cameraLength/_Transparent)*1;
				}else{
					color.a=1;
				}
				float2 noiseUV = float2(i.noiseUV.x + _Time.y * _WaveScroll.x, i.noiseUV.y + _Time.y * _WaveScroll.y);
                // apply noise as combination of two scrolling noise textures with oppsite scrolling direction
                float combinedSurfaceNoise = 0;
                float2 noiseScroll1  = float2( frac(_Time.x * _WaveScroll.x), frac(_Time.x * _WaveScroll.y));
                float4 surfaceNoise1 = tex2D(_WaveNoise, i.noiseUV + noiseScroll1);

                float2 noiseScroll2  = float2( frac(_Time.x * _WaveScroll.x), frac(_Time.x * -_WaveScroll.y));
                float4 surfaceNoise2 = tex2D(_WaveNoise, i.noiseUV + noiseScroll2);

                combinedSurfaceNoise = surfaceNoise1 + surfaceNoise2; 
				float surfaceNoise = combinedSurfaceNoise > _WaveNoiseLimit ? 1 : 0;
                    

				if(_WaveTransparent > 0){
					_WaveTransparent=-_WaveTransparent;
				}
				//apply wave
				if(surfaceNoise){
					_WaveColor.a=saturate(-_WaveTransparent/i.cameraLength)*1;
					float3 blendColor = (_WaveColor.rgb * _WaveColor.a) + (color.rgb * (1 - _WaveColor.a));
				    float alpha = _WaveColor.a + color.a * (1 - _WaveColor.a);

				    color = float4(blendColor, alpha);
				}
                //apply wave high light
				surfaceNoise = combinedSurfaceNoise > _WaveHighLightNoiseLimit ? 1 : 0;
                if(surfaceNoise){
					_WaveHighLightColor.a=saturate(-_WaveTransparent/i.cameraLength)*1;
					float3 blendColor = (_WaveHighLightColor.rgb * _WaveHighLightColor.a) + (color.rgb * (1 - _WaveHighLightColor.a));
				    float alpha = _WaveHighLightColor.a + color.a * (1 - _WaveHighLightColor.a);

				    color = float4(blendColor, alpha);
				}

				return color;
				
			}
			ENDCG
		}

	}
}