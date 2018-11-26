// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "My Shaders/Electricity"
{
	Properties
	{
		_MainTexture("Main Texture", 2D) = "white" {}
		_Metallic("Metallic", Range( 0 , 1)) = 0
		_Smoothness("Smoothness", Range( 0 , 1)) = 0
		_Dissolve("Dissolve", Range( 0 , 1)) = 0
		_NoiseTexture("Noise Texture", 2D) = "white" {}
		_UTilingincludesinstance("U Tiling (includes instance)", Float) = 0
		_VTilingincludesinstance("V Tiling (includes instance)", Float) = 1
		_USpeed("U Speed", Float) = 0
		_VSpeed("V Speed", Float) = 0
		_USpeedInstance("U Speed Instance", Float) = 0
		_VSpeedInstance("V Speed Instance", Float) = 0
		_NormalDistortion("Normal Distortion", 2D) = "bump" {}
		_DistortionIntensity("Distortion Intensity", Range( 0 , 1)) = 0.292
		_UNormalDistortion("U Normal Distortion", Float) = 0
		_VNormalDistortion("V Normal Distortion", Float) = 0
		_TintLightning("Tint Lightning", Color) = (0,0,0,0)
		_LightningIntensity("Lightning Intensity", Range( 0 , 10)) = 0
		_TintGeometry("Tint Geometry", Color) = (0,0,0,0)
		_IntersectColor("Intersect Color", Color) = (0,0,0,0)
		_IntersectIntensity("Intersect Intensity", Range( 0 , 10)) = 0
		_IntensityLimitZone("Intensity LimitZone", Float) = 0
		_SpeedofLimitZone("Speed of LimitZone", Float) = 0
		_TotalLines("Total Lines", Float) = 0
		_Method("Method", Range( 0 , 1)) = 0
		_Strech("Strech", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Off
		GrabPass{ }
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityCG.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		struct Input
		{
			float4 screenPos;
			float2 uv_texcoord;
		};

		uniform sampler2D _GrabTexture;
		uniform sampler2D _NormalDistortion;
		uniform float _UNormalDistortion;
		uniform float _VNormalDistortion;
		uniform float _DistortionIntensity;
		uniform float4 _IntersectColor;
		uniform float _IntensityLimitZone;
		uniform float _TotalLines;
		uniform sampler2D _CameraDepthTexture;
		uniform float _IntersectIntensity;
		uniform float _SpeedofLimitZone;
		uniform float _Method;
		uniform sampler2D _MainTexture;
		uniform float _Dissolve;
		uniform sampler2D _NoiseTexture;
		uniform float _USpeed;
		uniform float _VSpeed;
		uniform float _UTilingincludesinstance;
		uniform float _VTilingincludesinstance;
		uniform float _USpeedInstance;
		uniform float _VSpeedInstance;
		uniform float _LightningIntensity;
		uniform float4 _TintGeometry;
		uniform float4 _TintLightning;
		uniform float _Metallic;
		uniform float _Smoothness;
		uniform float _Strech;


		inline float4 ASE_ComputeGrabScreenPos( float4 pos )
		{
			#if UNITY_UV_STARTS_AT_TOP
			float scale = -1.0;
			#else
			float scale = 1.0;
			#endif
			float4 o = pos;
			o.y = pos.w * 0.5f;
			o.y = ( pos.y - o.y ) * _ProjectionParams.x * scale + o.y;
			return o;
		}


		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float3 appendResult230 = (float3(0.0 , 0.0 , 0.0));
			v.vertex.xyz += appendResult230;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_grabScreenPos = ASE_ComputeGrabScreenPos( ase_screenPos );
			float4 ase_grabScreenPosNorm = ase_grabScreenPos / ase_grabScreenPos.w;
			float2 appendResult163 = (float2(_UNormalDistortion , _VNormalDistortion));
			float2 uv_TexCoord157 = i.uv_texcoord * appendResult163;
			float2 panner158 = ( 1.0 * _Time.y * float2( 0.07,-0.07 ) + uv_TexCoord157);
			float4 screenColor119 = tex2D( _GrabTexture, ( (ase_grabScreenPosNorm).xy + (( UnpackNormal( tex2D( _NormalDistortion, panner158 ) ) * _DistortionIntensity )).xy ) );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float screenDepth108 = LinearEyeDepth(UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture,UNITY_PROJ_COORD(ase_screenPos))));
			float distanceDepth108 = abs( ( screenDepth108 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( _IntersectIntensity ) );
			float Depth209 = saturate( distanceDepth108 );
			float mulTime182 = _Time.y * _SpeedofLimitZone;
			float4 lerpResult224 = lerp( ( _IntersectColor * _IntensityLimitZone ) , ( _IntersectColor * saturate( sin( ( ( _TotalLines * Depth209 ) + mulTime182 ) ) ) ) , _Method);
			float4 SimpleOrHologram222 = lerpResult224;
			float temp_output_25_0 = (-0.9 + (( 1.0 - _Dissolve ) - 0.0) * (0.9 - -0.9) / (1.0 - 0.0));
			float2 appendResult7 = (float2(_USpeed , _VSpeed));
			float2 appendResult229 = (float2(_UTilingincludesinstance , _VTilingincludesinstance));
			float2 uv_TexCoord19 = i.uv_texcoord * appendResult229;
			float2 appendResult15 = (float2(_USpeedInstance , _VSpeedInstance));
			float2 appendResult31 = (float2(( 1.0 - saturate( (-10.0 + (( ( temp_output_25_0 + tex2D( _NoiseTexture, ( ( appendResult7 * _Time.y ) + uv_TexCoord19 ) ).r ) * ( temp_output_25_0 + tex2D( _NoiseTexture, ( uv_TexCoord19 + ( _Time.y * appendResult15 ) ) ).r ) ) - 0.0) * (10.0 - -10.0) / (1.0 - 0.0)) ) ) , 0.0));
			float4 tex2DNode1 = tex2D( _MainTexture, appendResult31 );
			float temp_output_146_0 = ( saturate( (0.01 + (tex2DNode1.r - 0.12) * (1.0 - 0.01) / (1.0 - 0.12)) ) * _LightningIntensity );
			float4 lerpResult154 = lerp( SimpleOrHologram222 , ( ( ( 1.0 - temp_output_146_0 ) * _TintGeometry ) + ( temp_output_146_0 * _TintLightning ) ) , Depth209);
			o.Emission = ( screenColor119 * lerpResult154 ).rgb;
			o.Metallic = _Metallic;
			o.Smoothness = _Smoothness;
			float Opacity139 = ( _Strech * tex2DNode1.r );
			o.Alpha = Opacity139;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard alpha:fade keepalpha fullforwardshadows exclude_path:deferred vertex:vertexDataFunc 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			sampler3D _DitherMaskLOD;
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
				float4 screenPos : TEXCOORD3;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				vertexDataFunc( v, customInputData );
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				fixed3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				o.screenPos = ComputeScreenPos( o.pos );
				return o;
			}
			fixed4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = IN.worldPos;
				fixed3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.screenPos = IN.screenPos;
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				half alphaRef = tex3D( _DitherMaskLOD, float3( vpos.xy * 0.25, o.Alpha * 0.9375 ) ).a;
				clip( alphaRef - 0.01 );
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=15301
132;633;1266;948;221.5599;-282.6476;1.491035;True;False
Node;AmplifyShaderEditor.RangedFloatNode;8;-2829.012,-190.8512;Float;False;Property;_VSpeed;V Speed;8;0;Create;True;0;0;False;0;0;0.2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;3;-2826.877,-286.5432;Float;False;Property;_USpeed;U Speed;7;0;Create;True;0;0;False;0;0;0.2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;13;-2875.58,347.7518;Float;False;Property;_VSpeedInstance;V Speed Instance;10;0;Create;True;0;0;False;0;0;-0.05;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;228;-2947.503,129.3264;Float;False;Property;_VTilingincludesinstance;V Tiling (includes instance);6;0;Create;True;0;0;False;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;12;-2877.897,255.0291;Float;False;Property;_USpeedInstance;U Speed Instance;9;0;Create;True;0;0;False;0;0;0.2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;227;-2944.701,-2.482314;Float;False;Property;_UTilingincludesinstance;U Tiling (includes instance);5;0;Create;True;0;0;False;0;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;229;-2622.503,66.32636;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TimeNode;11;-2646.426,-92.34396;Float;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;7;-2621.397,-239.7491;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;15;-2618.298,277.0157;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;9;-2396.979,-230.1525;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;19;-2394.426,19.5298;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;16;-2393.918,210.7881;Float;False;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;6;-2354.321,-539.696;Float;False;Property;_Dissolve;Dissolve;3;0;Create;True;0;0;False;0;0;0.382;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;20;-2097.272,188.0843;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;18;-2104.559,-230.3726;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.OneMinusNode;24;-2044.998,-534.4343;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;25;-1847.004,-534.8466;Float;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;-0.9;False;4;FLOAT;0.9;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;5;-1913.372,-260.2391;Float;True;Property;_NoiseTexture;Noise Texture;4;0;Create;True;0;0;False;0;None;33a99c29c7ed32544ade03f5bcff5192;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;22;-1899.849,159.4213;Float;True;Property;_TextureSample0;Texture Sample 0;4;0;Create;True;0;0;False;0;None;33a99c29c7ed32544ade03f5bcff5192;True;0;False;white;Auto;False;Instance;5;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;23;-1480.058,159.4068;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;26;-1483.305,-261.1528;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;156;0.2880421,135.9148;Float;False;1480.434;650.5578;;16;145;143;146;142;149;150;148;153;151;154;108;110;2;209;223;109;Tint Edges & Calculate Depth;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;27;-1281.152,-69.2503;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;28;-1079.172,-69.74516;Float;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;-10;False;4;FLOAT;10;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;109;374.6318,685.2412;Float;False;Property;_IntersectIntensity;Intersect Intensity;19;0;Create;True;0;0;False;0;0;2.94;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.DepthFade;108;684.2177,688.9254;Float;False;True;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;29;-851.5911,-69.46516;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;30;-674.198,-69.01082;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;221;-289.9817,1385.018;Float;False;1199.199;418.3276;;9;183;181;214;182;180;198;185;186;212;Hologram Effect;1,1,1,1;0;0
Node;AmplifyShaderEditor.SaturateNode;110;916.78,687.4907;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;209;1070.649,681.5962;Float;False;Depth;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;161;-1353.532,-681.2454;Float;False;Property;_UNormalDistortion;U Normal Distortion;13;0;Create;True;0;0;False;0;0;3;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;162;-1354.532,-550.2454;Float;False;Property;_VNormalDistortion;V Normal Distortion;14;0;Create;True;0;0;False;0;0;3;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;183;-239.9818,1688.346;Float;False;Property;_SpeedofLimitZone;Speed of LimitZone;21;0;Create;True;0;0;False;0;0;5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;181;-203.3138,1435.018;Float;False;Property;_TotalLines;Total Lines;22;0;Create;True;0;0;False;0;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;214;-217.154,1587.724;Float;False;209;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;31;-494.7472,-69.06226;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ColorNode;112;-327.8222,983.1448;Float;False;Property;_IntersectColor;Intersect Color;18;0;Create;True;0;0;False;0;0,0,0,0;1,0,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;163;-1125.532,-625.2454;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleTimeNode;182;16.01787,1688.346;Float;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;1;-301.3719,-95.59686;Float;True;Property;_MainTexture;Main Texture;0;0;Create;True;0;0;False;0;None;28beeab828589e34e8972433e4514f8b;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;180;21.18505,1540.56;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;145;50.28799,190.188;Float;False;5;0;FLOAT;0;False;1;FLOAT;0.12;False;2;FLOAT;1;False;3;FLOAT;0.01;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;157;-970.3136,-648.6968;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WireNode;219;158.6338,847.8192;Float;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;198;217.2407,1565.322;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;158;-687.3687,-647.3997;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0.07,-0.07;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;142;119.9756,376.1549;Float;False;Property;_LightningIntensity;Lightning Intensity;16;0;Create;True;0;0;False;0;0;10;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;138;-489.2008,-826.582;Float;False;1289.547;474.5042;;8;120;113;115;117;114;116;118;119;Screen Distortion;1,1,1,1;0;0
Node;AmplifyShaderEditor.SaturateNode;143;274.2502,188.1033;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;220;399.2415,811.8883;Float;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SinOpNode;185;368.2133,1632.727;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;210;342.5986,904.0476;Float;False;731.452;360.8593;;5;203;208;205;206;204;Simple Intensity;1,1,1,1;0;0
Node;AmplifyShaderEditor.SaturateNode;186;535.1175,1635.63;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;113;-414.6583,-467.0778;Float;False;Property;_DistortionIntensity;Distortion Intensity;12;0;Create;True;0;0;False;0;0.292;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;120;-468.2008,-675.7203;Float;True;Property;_NormalDistortion;Normal Distortion;11;0;Create;True;0;0;False;0;None;302951faffe230848aa0d3df7bb70faa;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WireNode;218;609.7242,849.6234;Float;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;206;419.1475,1149.907;Float;False;Property;_IntensityLimitZone;Intensity LimitZone;20;0;Create;True;0;0;False;0;0;3;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;146;468.0748,186.825;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GrabScreenPosition;114;-92.07652,-777.7598;Float;False;0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;226;793.9507,1294.243;Float;False;Property;_Method;Method;23;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;149;666.8237,187.9206;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;151;665.1089,479.354;Float;False;Property;_TintGeometry;Tint Geometry;17;0;Create;True;0;0;False;0;0,0,0,0;0.05482266,0.2867647,0.2483743,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;203;905.0507,1024.585;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;115;-58.37273,-553.3511;Float;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;212;740.2176,1612.225;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;2;409.1815,476.4499;Float;False;Property;_TintLightning;Tint Lightning;15;0;Create;True;0;0;False;0;0,0,0,0;0,0.8758624,1,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ComponentMaskNode;117;134.7207,-555.551;Float;False;True;True;False;True;1;0;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;148;687.5321,352.4449;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;150;901.1012,185.9148;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ComponentMaskNode;116;191.5207,-775.4501;Float;False;True;True;False;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.LerpOp;224;1127.067,1249.246;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;222;1307.738,1241.29;Float;False;SimpleOrHologram;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;223;958.0106,477.9673;Float;False;222;0;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;153;1103.95,329.921;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;32;-131.203,-263.0352;Float;False;Property;_Strech;Strech;24;0;Create;True;0;0;False;0;0;5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;118;419.9262,-703.35;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;39;64.26413,-205.8127;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ScreenColorNode;119;600.3466,-708.9749;Float;False;Global;_GrabScreen1;Grab Screen 1;-1;0;Create;True;0;0;False;0;Object;-1;False;False;1;0;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;154;1296.722,483.8349;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;80;1668.522,71.11005;Float;False;Property;_Smoothness;Smoothness;2;0;Create;True;0;0;False;0;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;139;255.4288,-210.6905;Float;False;Opacity;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;230;1849.242,347.6765;Float;False;FLOAT3;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;155;1650.432,-216.3201;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;140;1726.12,163.6795;Float;False;139;0;1;FLOAT;0
Node;AmplifyShaderEditor.AbsOpNode;205;559.3079,1027.074;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;208;727.2222,1044.914;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;81;1668.549,-29.50435;Float;False;Property;_Metallic;Metallic;1;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SinTimeNode;204;392.5986,954.0478;Float;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;2055.523,-64.06399;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;My Shaders/Electricity;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;0;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;ForwardOnly;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;4;1;False;-1;1;False;-1;-1;False;-1;-1;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;0;0;False;0;0;0;False;-1;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;229;0;227;0
WireConnection;229;1;228;0
WireConnection;7;0;3;0
WireConnection;7;1;8;0
WireConnection;15;0;12;0
WireConnection;15;1;13;0
WireConnection;9;0;7;0
WireConnection;9;1;11;2
WireConnection;19;0;229;0
WireConnection;16;0;11;2
WireConnection;16;1;15;0
WireConnection;20;0;19;0
WireConnection;20;1;16;0
WireConnection;18;0;9;0
WireConnection;18;1;19;0
WireConnection;24;0;6;0
WireConnection;25;0;24;0
WireConnection;5;1;18;0
WireConnection;22;1;20;0
WireConnection;23;0;25;0
WireConnection;23;1;22;1
WireConnection;26;0;25;0
WireConnection;26;1;5;1
WireConnection;27;0;26;0
WireConnection;27;1;23;0
WireConnection;28;0;27;0
WireConnection;108;0;109;0
WireConnection;29;0;28;0
WireConnection;30;0;29;0
WireConnection;110;0;108;0
WireConnection;209;0;110;0
WireConnection;31;0;30;0
WireConnection;163;0;161;0
WireConnection;163;1;162;0
WireConnection;182;0;183;0
WireConnection;1;1;31;0
WireConnection;180;0;181;0
WireConnection;180;1;214;0
WireConnection;145;0;1;1
WireConnection;157;0;163;0
WireConnection;219;0;112;0
WireConnection;198;0;180;0
WireConnection;198;1;182;0
WireConnection;158;0;157;0
WireConnection;143;0;145;0
WireConnection;220;0;219;0
WireConnection;185;0;198;0
WireConnection;186;0;185;0
WireConnection;120;1;158;0
WireConnection;218;0;220;0
WireConnection;146;0;143;0
WireConnection;146;1;142;0
WireConnection;149;0;146;0
WireConnection;203;0;218;0
WireConnection;203;1;206;0
WireConnection;115;0;120;0
WireConnection;115;1;113;0
WireConnection;212;0;112;0
WireConnection;212;1;186;0
WireConnection;117;0;115;0
WireConnection;148;0;146;0
WireConnection;148;1;2;0
WireConnection;150;0;149;0
WireConnection;150;1;151;0
WireConnection;116;0;114;0
WireConnection;224;0;203;0
WireConnection;224;1;212;0
WireConnection;224;2;226;0
WireConnection;222;0;224;0
WireConnection;153;0;150;0
WireConnection;153;1;148;0
WireConnection;118;0;116;0
WireConnection;118;1;117;0
WireConnection;39;0;32;0
WireConnection;39;1;1;1
WireConnection;119;0;118;0
WireConnection;154;0;223;0
WireConnection;154;1;153;0
WireConnection;154;2;209;0
WireConnection;139;0;39;0
WireConnection;155;0;119;0
WireConnection;155;1;154;0
WireConnection;205;0;204;4
WireConnection;208;0;205;0
WireConnection;0;2;155;0
WireConnection;0;3;81;0
WireConnection;0;4;80;0
WireConnection;0;9;140;0
WireConnection;0;11;230;0
ASEEND*/
//CHKSM=CB9612FE78C003CD5B985DA4B68C37042CB9F2F2