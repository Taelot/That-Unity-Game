Shader "Cartoon"
{

	Properties
	{
		[HDR]
		_Color("Color", Color) = (0.5,0.5,0.5,0.5)
		_MainTex("Main Texture", 2D) = "white" {}
		// Ambient light is applied uniformly to all surfaces on the object.
		[HDR]
		_AmbientColor("Ambient Color", Color) = (0.5,0.5,0.5,0.5)
		[HDR]
		_SpecularColor("Specular Color", Color) = (0.5,0.5,0.5,0.5)
		//the size of the specular reflection.
		_Glossiness("Glossiness", Float) = 32
		[HDR]
		_RimColor("Rim Color", Color) = (0.5,0.5,0.5,0.5)
		_RimAmount("Rim Amount", Range(0, 1)) = 0
		// how smoothly the rim blends 
		_RimThreshold("Rim Threshold", Range(0, 1)) = 0.1		

		//outline
		_OutlineColor ("Outline Color", Color) = (0.5,0.5,0.5,0.5)
		_Outline ("Outline width", Range (0, 5)) = .1
		_Limit ("Outline width limiter", Range (0, 5)) = 0

		//how transparent the object will be when camera is near the object
        _Transparent ("Transparent", float) =  0
		_ShadowRamp ("Shadow Ramp",float) =1

	}
	SubShader
	{
	//creates a bigger vertex depending on the camera's width
	//so it doesn't render only outline will be 
	Tags { "Queue"="Transparent" "RenderType"="Transparent" }

	Pass {

		//so it doesn't render only outline will be 
		//for transparent purpose
        ColorMask 0
		CGINCLUDE
		#include "UnityCG.cginc"
 
		struct vertIn1 {
			float4 vertex : POSITION;
			float3 normal : NORMAL;
		};
 
		struct vertOut1 {
			float4 pos : POSITION;
			float4 color : COLOR;
		};
 
		uniform float _Outline;
		uniform float _Limit;
		uniform float4 _OutlineColor;
 
		vertOut1 vert(vertIn1 v) {

			vertOut1 o;
			//orthographic camera’s width
    		float multiplier = unity_OrthoParams.x * 0.1;
			//if limit not set, the ouline wwidth will change when camera/game display size change
			if(multiplier < _Limit){
    			multiplier = _Limit;
    		}
			v.vertex *=  _Outline * multiplier;

			o.pos = UnityObjectToClipPos(v.vertex);
			o.color = _OutlineColor;
			return o;
		}
		ENDCG
	}
	// only the back faces are rendered to create a "outline"
	Pass {
		Cull Front
		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		//render the outline color 
		fixed4 frag(vertOut1 i): COLOR {
			 return i.color; 
		}
		ENDCG
	}


	Pass{
	Tags { 
		"Queue"="Transparent"
 		"IgnoreProjector"="True" 
 		"RenderType"="Transparent" 
 	}
    Blend SrcAlpha OneMinusSrcAlpha
	 ZWrite Off
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			//for this shader to interact with other shadow
			#pragma multi_compile_fwdbase
			
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
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
				float3 worldNormal : NORMAL;
				float2 uv : TEXCOORD0;
				float3 viewDir : TEXCOORD1;	
				float3 worldPos : TEXCOORD2;
				float3 cameraLength :TEXCOORD3;
				//4d value for storing shadow coord
				SHADOW_COORDS(2)
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			// float3 worldPos;
			
			vertOut  vert (vertIn  v)
			{
				vertOut  o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.worldNormal = UnityObjectToWorldNormal(v.normal);		
				o.viewDir = WorldSpaceViewDir(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.worldPos=mul (unity_ObjectToWorld, v.vertex);
				o.cameraLength =length(_WorldSpaceCameraPos - o.worldPos.xyz);
				//calculate inbut vertex in to shadow and store in SHADOW_COORDS
				TRANSFER_SHADOW(o)
				return o;
			}
			
			float4 _Color;
			float4 _AmbientColor;
			float4 _SpecularColor;
			float _Glossiness;		
			float4 _RimColor;
			float _RimAmount;
			float _RimThreshold;	
			float _Transparent;
			float _CameraPlayerDistUp;	
			float _ShadowRamp;	

			float4 frag (vertOut  i) : SV_Target
			{
				float3 normal = normalize(i.worldNormal);
				float3 viewDir = normalize(i.viewDir);

				//base on Blinn-Phong

				//get shadow 
				float NdotL = dot(_WorldSpaceLightPos0, normal);
				float shadow = NdotL > 0 ? 1 : 0;
				//smoothstep increaces contrast make things cartoon like 
				float lightIntensity = smoothstep(_ShadowRamp - NdotL * shadow ,_ShadowRamp + NdotL * shadow , NdotL * shadow);
				//light color
				float4 light = lightIntensity * _LightColor0;

				//specular reflection.
				float3 halfVector = normalize(_WorldSpaceLightPos0 + viewDir);
				float NdotH = dot(normal, halfVector);
				float specularIntensity = pow(NdotH * lightIntensity, _Glossiness * _Glossiness);
				float specularIntensitySmooth = smoothstep(0.005, 0.01, specularIntensity);
				float4 specular = specularIntensitySmooth * _SpecularColor;				

				//rim lighting.
				float rimDot = 1 - dot(viewDir, normal);
				//remove shadow part rim light
				float rimIntensity = rimDot * pow(NdotL, _RimThreshold);
				rimIntensity = smoothstep(_RimAmount - 0.01, _RimAmount + 0.01, rimIntensity);
				float4 rim = rimIntensity * _RimColor;
				float4 sample = tex2D(_MainTex, i.uv);
				float4 color = (light + _AmbientColor + specular + rim) * _Color * sample;

				//become transparent when close to camera
				if(_Transparent > 0){
					_Transparent=-_Transparent;
				}

				if(_Transparent!=0){
				color.a =saturate(-i.cameraLength/_Transparent)*1;
				}else{
					color.a=1;
				}
				
				return color;
			}
			ENDCG
		}

		// Shadow casting support.
        UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
	}
}