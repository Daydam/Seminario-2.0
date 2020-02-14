// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Gastón Zabala/Skills/RepulsiveBattery/AoE"
{
	Properties
	{
		[HDR]_Main_Color("Main_Color", Color) = (1,0,0,0)
		_Main_Texture("Main_Texture", 2D) = "white" {}
		[HDR]_Fresnel_Color("Fresnel_Color", Color) = (0,0,0,0)
		_Fresnal_Bias("Fresnal_Bias", Float) = 0
		_Fresnel_Scale("Fresnel_Scale", Float) = 0
		_Fresnal_Power("Fresnal_Power", Float) = 0
		[HDR]_Depth_Color("Depth_Color", Color) = (0,0,0,0)
		_Depth_Distance("Depth_Distance", Float) = 0
		_Normal_Distortion("Normal_Distortion", 2D) = "bump" {}
		_Distortion_TilingX("Distortion_TilingX", Float) = 0
		_Distortion_TilingY("Distortion_TilingY", Float) = 0
		_Distortion_Amount("Distortion_Amount", Float) = 0
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
		#include "UnityStandardUtils.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		struct Input
		{
			float3 worldPos;
			float3 worldNormal;
			float4 screenPos;
			float2 uv_texcoord;
			float4 vertexColor : COLOR;
		};

		uniform float4 _Main_Color;
		uniform float _Fresnal_Bias;
		uniform float _Fresnel_Scale;
		uniform float _Fresnal_Power;
		uniform float4 _Fresnel_Color;
		uniform float4 _Depth_Color;
		UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
		uniform float4 _CameraDepthTexture_TexelSize;
		uniform float _Depth_Distance;
		uniform sampler2D _GrabTexture;
		uniform float _Distortion_Amount;
		uniform sampler2D _Normal_Distortion;
		uniform float _Distortion_TilingX;
		uniform float _Distortion_TilingY;
		uniform sampler2D _Main_Texture;
		uniform float4 _Main_Texture_ST;


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
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = i.worldNormal;
			float fresnelNdotV7 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode7 = ( _Fresnal_Bias + _Fresnel_Scale * pow( 1.0 - fresnelNdotV7, _Fresnal_Power ) );
			float4 lerpResult12 = lerp( _Main_Color , ( fresnelNode7 * _Fresnel_Color ) , saturate( fresnelNode7 ));
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float screenDepth19 = LinearEyeDepth(UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture,UNITY_PROJ_COORD( ase_screenPos ))));
			float distanceDepth19 = saturate( abs( ( screenDepth19 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( _Depth_Distance ) ) );
			float4 lerpResult24 = lerp( lerpResult12 , ( _Depth_Color * distanceDepth19 ) , ( 1.0 - distanceDepth19 ));
			float2 appendResult35 = (float2(_Distortion_TilingX , _Distortion_TilingY));
			float4 ase_grabScreenPos = ASE_ComputeGrabScreenPos( ase_screenPos );
			float4 screenColor30 = tex2Dproj( _GrabTexture, UNITY_PROJ_COORD( ( float4( UnpackScaleNormal( tex2D( _Normal_Distortion, appendResult35 ), _Distortion_Amount ) , 0.0 ) + ase_grabScreenPos ) ) );
			o.Emission = ( lerpResult24 + screenColor30 ).rgb;
			float2 uv_Main_Texture = i.uv_texcoord * _Main_Texture_ST.xy + _Main_Texture_ST.zw;
			o.Alpha = saturate( ( tex2D( _Main_Texture, uv_Main_Texture ).a * i.vertexColor.a ) );
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
7;23;1266;866;1237.692;423.4416;1.862935;True;False
Node;AmplifyShaderEditor.RangedFloatNode;10;-1064.296,-315.049;Float;False;Property;_Fresnal_Power;Fresnal_Power;5;0;Create;True;0;0;False;0;0;5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;9;-1061.296,-413.049;Float;False;Property;_Fresnel_Scale;Fresnel_Scale;4;0;Create;True;0;0;False;0;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;8;-1052.296,-515.049;Float;False;Property;_Fresnal_Bias;Fresnal_Bias;3;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;34;-1181.599,901.6021;Float;False;Property;_Distortion_TilingY;Distortion_TilingY;10;0;Create;True;0;0;False;0;0;0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;33;-1182.599,779.6021;Float;False;Property;_Distortion_TilingX;Distortion_TilingX;9;0;Create;True;0;0;False;0;0;0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;21;-954.0857,668.65;Float;False;Property;_Depth_Distance;Depth_Distance;7;0;Create;True;0;0;False;0;0;0.38;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FresnelNode;7;-766.4437,-513.4481;Float;False;Standard;WorldNormal;ViewDir;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;36;-1178.599,1012.602;Float;False;Property;_Distortion_Amount;Distortion_Amount;11;0;Create;True;0;0;False;0;0;1.17;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;35;-948.5994,838.6021;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ColorNode;14;-812.2637,-338.9583;Float;False;Property;_Fresnel_Color;Fresnel_Color;2;1;[HDR];Create;True;0;0;False;0;0,0,0,0;0,34.13334,47.93726,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;27;-680.9995,422.8164;Float;False;Property;_Depth_Color;Depth_Color;6;1;[HDR];Create;True;0;0;False;0;0,0,0,0;766.9961,84.32941,36.14118,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;6;-798.313,-167.6348;Float;False;Property;_Main_Color;Main_Color;0;1;[HDR];Create;True;0;0;False;0;1,0,0,0;0.627451,0,0.7490196,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;28;-722.2164,811.2061;Float;True;Property;_Normal_Distortion;Normal_Distortion;8;0;Create;True;0;0;False;0;None;2c9df508228ae2644a235156585a9f0c;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GrabScreenPosition;31;-686.9663,1023.599;Float;False;1;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;13;-453.9534,-67.29816;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;11;-434.0912,-362.1029;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.DepthFade;19;-699.9612,648.5313;Float;False;True;True;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;29;-297.8015,813.9036;Float;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.OneMinusNode;25;-382.5486,646.387;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;4;-856.1215,223.436;Float;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;1;-1010.844,14.93842;Float;True;Property;_Main_Texture;Main_Texture;1;0;Create;True;0;0;False;0;None;0000000000000000f000000000000000;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;12;-251.0037,-113.4409;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;22;-320.417,423.631;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ScreenColorNode;30;-92.71629,808.8494;Float;False;Global;_GrabScreen0;Grab Screen 0;9;0;Create;True;0;0;False;0;Object;-1;False;True;1;0;FLOAT4;0,0,0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;24;106.1015,142.4503;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;3;-538.3117,180.8115;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;5;-350.2148,183.3028;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;32;228.1918,460.6065;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;464.0842,92.38261;Float;False;True;2;Float;ASEMaterialInspector;0;0;Unlit;Gastón Zabala/Skills/RepulsiveBattery/AoE;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;7;1;8;0
WireConnection;7;2;9;0
WireConnection;7;3;10;0
WireConnection;35;0;33;0
WireConnection;35;1;34;0
WireConnection;28;1;35;0
WireConnection;28;5;36;0
WireConnection;13;0;7;0
WireConnection;11;0;7;0
WireConnection;11;1;14;0
WireConnection;19;0;21;0
WireConnection;29;0;28;0
WireConnection;29;1;31;0
WireConnection;25;0;19;0
WireConnection;12;0;6;0
WireConnection;12;1;11;0
WireConnection;12;2;13;0
WireConnection;22;0;27;0
WireConnection;22;1;19;0
WireConnection;30;0;29;0
WireConnection;24;0;12;0
WireConnection;24;1;22;0
WireConnection;24;2;25;0
WireConnection;3;0;1;4
WireConnection;3;1;4;4
WireConnection;5;0;3;0
WireConnection;32;0;24;0
WireConnection;32;1;30;0
WireConnection;0;2;32;0
WireConnection;0;9;5;0
ASEEND*/
//CHKSM=5BC4BE7CF284E8E025167B7573BD13ADB2D4F9D6