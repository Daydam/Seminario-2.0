// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Gastón Zabala/Skills/RocketLauncher/ExplosionAoE"
{
	Properties
	{
		[HDR]_Emission_Color("Emission_Color", Color) = (0,0,0,0)
		_Noise_Adjustment("Noise_Adjustment", Float) = 0
		_MainTexture("Main Texture", 2D) = "white" {}
		_Panner_SpeedX("Panner_SpeedX", Float) = 0
		_Panner_SpeedY("Panner_SpeedY", Float) = 0
		_DepthFade_Controller("DepthFade_Controller", Float) = 0
		[HDR]_Fresnel_Color("Fresnel_Color", Color) = (0,0,0,0)
		_Fresnel_Bias("Fresnel_Bias", Float) = 0
		_Fresnel_Scale("Fresnel_Scale", Float) = 0
		_Fresnel_Power("Fresnel_Power", Float) = 0
		_DistortionTexture("Distortion Texture", 2D) = "bump" {}
		_DistortionAmount("Distortion Amount", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		GrabPass{ }
		CGINCLUDE
		#include "UnityCG.cginc"
		#include "UnityShaderVariables.cginc"
		#include "UnityStandardUtils.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		struct Input
		{
			float4 screenPos;
			float3 worldPos;
			float3 worldNormal;
			float2 uv_texcoord;
			float4 vertexColor : COLOR;
		};

		uniform float4 _Fresnel_Color;
		uniform float4 _Emission_Color;
		UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
		uniform float4 _CameraDepthTexture_TexelSize;
		uniform float _DepthFade_Controller;
		uniform float _Fresnel_Bias;
		uniform float _Fresnel_Scale;
		uniform float _Fresnel_Power;
		uniform sampler2D _MainTexture;
		uniform float _Panner_SpeedX;
		uniform float _Panner_SpeedY;
		uniform float _Noise_Adjustment;
		uniform sampler2D _GrabTexture;
		uniform float _DistortionAmount;
		uniform sampler2D _DistortionTexture;
		uniform float4 _DistortionTexture_ST;


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


		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float screenDepth1 = LinearEyeDepth(UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture,UNITY_PROJ_COORD( ase_screenPos ))));
			float distanceDepth1 = abs( ( screenDepth1 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( _DepthFade_Controller ) );
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = i.worldNormal;
			float fresnelNdotV8 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode8 = ( _Fresnel_Bias + _Fresnel_Scale * pow( 1.0 - fresnelNdotV8, _Fresnel_Power ) );
			float2 appendResult38 = (float2(_Panner_SpeedX , _Panner_SpeedY));
			float2 panner33 = ( 1.0 * _Time.y * appendResult38 + i.uv_texcoord);
			float temp_output_15_0 = ( tex2D( _MainTexture, panner33 ).a - _Noise_Adjustment );
			float DepthFresnel16 = saturate( ( ( 1.0 - saturate( distanceDepth1 ) ) + ( saturate( fresnelNode8 ) * temp_output_15_0 ) ) );
			float4 lerpResult31 = lerp( _Fresnel_Color , _Emission_Color , DepthFresnel16);
			float2 uv_DistortionTexture = i.uv_texcoord * _DistortionTexture_ST.xy + _DistortionTexture_ST.zw;
			float4 ase_grabScreenPos = ASE_ComputeGrabScreenPos( ase_screenPos );
			float4 ase_grabScreenPosNorm = ase_grabScreenPos / ase_grabScreenPos.w;
			float4 screenColor39 = tex2Dproj( _GrabTexture, UNITY_PROJ_COORD( ( float4( UnpackScaleNormal( tex2D( _DistortionTexture, uv_DistortionTexture ), _DistortionAmount ) , 0.0 ) + ase_grabScreenPosNorm ) ) );
			float4 temp_output_44_0 = ( ( lerpResult31 * float4( (i.vertexColor).rgb , 0.0 ) ) + screenColor39 );
			float temp_output_27_0 = saturate( ( saturate( ( DepthFresnel16 + temp_output_15_0 ) ) * i.vertexColor.a ) );
			float4 lerpResult45 = lerp( temp_output_44_0 , temp_output_44_0 , ( 1.0 - temp_output_27_0 ));
			o.Emission = lerpResult45.rgb;
			o.Alpha = temp_output_27_0;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Unlit alpha:fade keepalpha fullforwardshadows 

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
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
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
				float3 worldNormal : TEXCOORD4;
				half4 color : COLOR0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.worldNormal = worldNormal;
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				o.screenPos = ComputeScreenPos( o.pos );
				o.color = v.color;
				return o;
			}
			half4 frag( v2f IN
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
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = IN.worldNormal;
				surfIN.screenPos = IN.screenPos;
				surfIN.vertexColor = IN.color;
				SurfaceOutput o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutput, o )
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
Version=16700
67;464;1266;948;2651.804;-111.713;2.986923;True;False
Node;AmplifyShaderEditor.RangedFloatNode;37;-2412.203,666.9229;Float;False;Property;_Panner_SpeedY;Panner_SpeedY;4;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;36;-2416.899,569.4868;Float;False;Property;_Panner_SpeedX;Panner_SpeedX;3;0;Create;True;0;0;False;0;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;34;-2291.408,404.6688;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;38;-2168.027,589.4435;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;5;-2018.569,-238.4661;Float;False;Property;_Fresnel_Bias;Fresnel_Bias;7;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;7;-2025.569,-140.4661;Float;False;Property;_Fresnel_Scale;Fresnel_Scale;8;0;Create;True;0;0;False;0;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;6;-2025.569,-46.46622;Float;False;Property;_Fresnel_Power;Fresnel_Power;9;0;Create;True;0;0;False;0;0;5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;33;-1984.12,408.0516;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,1;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;2;-2039.649,-379.4184;Float;False;Property;_DepthFade_Controller;DepthFade_Controller;5;0;Create;True;0;0;False;0;0;1.06;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FresnelNode;8;-1731.569,-198.4661;Float;False;Standard;WorldNormal;ViewDir;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;11;-1670.422,607.7416;Float;False;Property;_Noise_Adjustment;Noise_Adjustment;1;0;Create;True;0;0;False;0;0;0.07;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;10;-1764.417,378.9564;Float;True;Property;_MainTexture;Main Texture;2;0;Create;True;0;0;False;0;None;87473b44dad94a84e868a8983459cb6d;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DepthFade;1;-1743.952,-397.7675;Float;False;True;False;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;15;-1377.97,550.6658;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;9;-1432.569,-199.4661;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;3;-1458.569,-396.4659;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;4;-1274.569,-396.4659;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;13;-1243.215,-199.3147;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;14;-1070.326,-295.5085;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;32;-912.1959,-297.3032;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;16;-738.7755,-302.4297;Float;False;DepthFresnel;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;19;-1004.379,303.1877;Float;False;16;DepthFresnel;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;42;-1167.566,857.1357;Float;False;Property;_DistortionAmount;Distortion Amount;11;0;Create;True;0;0;False;0;0;0.4;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;18;-746.5993,328.4843;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;20;-569.9589,328.7673;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;23;-820.451,476.3;Float;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;41;-921.489,812.6113;Float;True;Property;_DistortionTexture;Distortion Texture;10;0;Create;True;0;0;False;0;None;2c9df508228ae2644a235156585a9f0c;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GrabScreenPosition;40;-862.3667,1095.44;Float;False;0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;28;-1021.598,-149.8305;Float;False;Property;_Fresnel_Color;Fresnel_Color;6;1;[HDR];Create;True;0;0;False;0;0,0,0,0;0.4319513,0,0.55,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;22;-1035.82,69.50453;Float;False;Property;_Emission_Color;Emission_Color;0;1;[HDR];Create;True;0;0;False;0;0,0,0,0;0,16.56471,95.87451,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;31;-670.3013,77.09167;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;43;-547.2825,958.4259;Float;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.ComponentMaskNode;24;-615.5475,473.116;Float;False;True;True;True;False;1;0;COLOR;0,0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;26;-353.0883,539.178;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ScreenColorNode;39;-351.7522,953.0203;Float;False;Global;_GrabScreen0;Grab Screen 0;10;0;Create;True;0;0;False;0;Object;-1;False;True;1;0;FLOAT4;0,0,0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;27;-175.0883,544.178;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;25;-231.5475,401.1159;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT3;0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;44;-74.49768,735.1403;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;46;-26.64392,481.3821;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;45;166.3561,679.3821;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;468.0172,357.7417;Float;False;True;2;Float;ASEMaterialInspector;0;0;Unlit;Gastón Zabala/Skills/RocketLauncher/ExplosionAoE;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;38;0;36;0
WireConnection;38;1;37;0
WireConnection;33;0;34;0
WireConnection;33;2;38;0
WireConnection;8;1;5;0
WireConnection;8;2;7;0
WireConnection;8;3;6;0
WireConnection;10;1;33;0
WireConnection;1;0;2;0
WireConnection;15;0;10;4
WireConnection;15;1;11;0
WireConnection;9;0;8;0
WireConnection;3;0;1;0
WireConnection;4;0;3;0
WireConnection;13;0;9;0
WireConnection;13;1;15;0
WireConnection;14;0;4;0
WireConnection;14;1;13;0
WireConnection;32;0;14;0
WireConnection;16;0;32;0
WireConnection;18;0;19;0
WireConnection;18;1;15;0
WireConnection;20;0;18;0
WireConnection;41;5;42;0
WireConnection;31;0;28;0
WireConnection;31;1;22;0
WireConnection;31;2;19;0
WireConnection;43;0;41;0
WireConnection;43;1;40;0
WireConnection;24;0;23;0
WireConnection;26;0;20;0
WireConnection;26;1;23;4
WireConnection;39;0;43;0
WireConnection;27;0;26;0
WireConnection;25;0;31;0
WireConnection;25;1;24;0
WireConnection;44;0;25;0
WireConnection;44;1;39;0
WireConnection;46;0;27;0
WireConnection;45;0;44;0
WireConnection;45;1;44;0
WireConnection;45;2;46;0
WireConnection;0;2;45;0
WireConnection;0;9;27;0
ASEEND*/
//CHKSM=193707EB70975258C9A9ABDA216D7108FA93D614