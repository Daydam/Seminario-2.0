// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Gastón Zabala/Skills/RepulsiveBattery/Shield"
{
	Properties
	{
		[HDR]_PatternColor("Pattern Color", Color) = (0,0,0,0)
		_Main_Texture("Main_Texture", 2D) = "white" {}
		_Flowmap_Intensity("Flowmap_Intensity", Range( 0 , 1)) = 0
		_Flowmap_SpeedX("Flowmap_SpeedX", Float) = 0
		_Flowmap_SpeedY("Flowmap_SpeedY", Float) = 0
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		_Flowmap2_SpeedX("Flowmap2_SpeedX", Float) = 0
		_Flowmap2_SpeedY("Flowmap2_SpeedY", Float) = 0
		_FresnelColor_Intensity("FresnelColor_Intensity", Float) = 0
		_Fresnal_Bias("Fresnal_Bias", Float) = 0
		_Fresnel_Scale("Fresnel_Scale", Float) = 0
		_Fresnel_Power("Fresnel_Power", Float) = 0
		_Distortion_Amount("Distortion_Amount", Float) = 0
		_NormalDistortion("Normal Distortion", 2D) = "bump" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		GrabPass{ }
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityStandardUtils.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		struct Input
		{
			float2 uv_texcoord;
			float4 vertexColor : COLOR;
			float3 worldPos;
			float3 worldNormal;
			float4 screenPos;
		};

		uniform float4 _PatternColor;
		uniform sampler2D _Main_Texture;
		uniform float _Flowmap_SpeedX;
		uniform float _Flowmap_SpeedY;
		uniform sampler2D _TextureSample0;
		uniform float4 _TextureSample0_ST;
		uniform float _Flowmap_Intensity;
		uniform float _Flowmap2_SpeedX;
		uniform float _Flowmap2_SpeedY;
		uniform float _FresnelColor_Intensity;
		uniform float _Fresnal_Bias;
		uniform float _Fresnel_Scale;
		uniform float _Fresnel_Power;
		uniform sampler2D _GrabTexture;
		uniform float _Distortion_Amount;
		uniform sampler2D _NormalDistortion;


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
			float2 appendResult10 = (float2(_Flowmap_SpeedX , _Flowmap_SpeedY));
			float2 uv_TextureSample0 = i.uv_texcoord * _TextureSample0_ST.xy + _TextureSample0_ST.zw;
			float4 tex2DNode18 = tex2D( _TextureSample0, uv_TextureSample0 );
			float4 lerpResult20 = lerp( float4( i.uv_texcoord, 0.0 , 0.0 ) , tex2DNode18 , _Flowmap_Intensity);
			float2 panner6 = ( 1.0 * _Time.y * appendResult10 + lerpResult20.rg);
			float4 tex2DNode1 = tex2D( _Main_Texture, panner6 );
			float2 appendResult30 = (float2(_Flowmap2_SpeedX , _Flowmap2_SpeedY));
			float2 uv_TexCoord47 = i.uv_texcoord * float2( 0.2,0.2 );
			float4 lerpResult45 = lerp( float4( uv_TexCoord47, 0.0 , 0.0 ) , tex2DNode18 , _Flowmap_Intensity);
			float2 panner27 = ( 1.0 * _Time.y * appendResult30 + lerpResult45.rg);
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = i.worldNormal;
			float fresnelNdotV34 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode34 = ( _Fresnal_Bias + _Fresnel_Scale * pow( 1.0 - fresnelNdotV34, _Fresnel_Power ) );
			float temp_output_35_0 = saturate( fresnelNode34 );
			float4 lerpResult39 = lerp( ( _PatternColor * saturate( ( tex2DNode1.r * ( tex2D( _Main_Texture, panner27 ).r * 0.5 ) ) ) ) , ( ( i.vertexColor * _FresnelColor_Intensity ) * temp_output_35_0 ) , temp_output_35_0);
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_grabScreenPos = ASE_ComputeGrabScreenPos( ase_screenPos );
			float4 screenColor55 = tex2Dproj( _GrabTexture, UNITY_PROJ_COORD( ( float4( UnpackScaleNormal( tex2D( _NormalDistortion, panner27 ), _Distortion_Amount ) , 0.0 ) + ase_grabScreenPos ) ) );
			float temp_output_5_0 = ( tex2DNode1.r * i.vertexColor.a );
			float temp_output_60_0 = ( 1.0 - temp_output_5_0 );
			float4 lerpResult62 = lerp( float4( 0,0,0,0 ) , screenColor55 , temp_output_60_0);
			o.Emission = ( lerpResult39 + lerpResult62 ).rgb;
			float lerpResult64 = lerp( temp_output_5_0 , temp_output_60_0 , temp_output_60_0);
			o.Alpha = lerpResult64;
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
-22;160;1266;653;2010.1;786.3744;2.711709;True;False
Node;AmplifyShaderEditor.RangedFloatNode;19;-1780.552,2.293463;Float;False;Property;_Flowmap_Intensity;Flowmap_Intensity;2;0;Create;True;0;0;False;0;0;0.09;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;18;-1816.025,-224.1642;Float;True;Property;_TextureSample0;Texture Sample 0;5;0;Create;True;0;0;False;0;None;920b2289f820f91489558539f21b1313;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;47;-1713.315,360.2113;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;0.2,0.2;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;28;-1662.347,515.3079;Float;False;Property;_Flowmap2_SpeedX;Flowmap2_SpeedX;6;0;Create;True;0;0;False;0;0;-0.07;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;29;-1681.018,632.5292;Float;False;Property;_Flowmap2_SpeedY;Flowmap2_SpeedY;7;0;Create;True;0;0;False;0;0;-0.05;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;8;-1755.815,126.3882;Float;False;Property;_Flowmap_SpeedX;Flowmap_SpeedX;3;0;Create;True;0;0;False;0;0;0.1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;9;-1760.815,249.3881;Float;False;Property;_Flowmap_SpeedY;Flowmap_SpeedY;4;0;Create;True;0;0;False;0;0;0.1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;7;-1788.914,-361.2181;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;45;-1358.495,356.8727;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.DynamicAppendNode;30;-1382.347,535.308;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.LerpOp;20;-1410.905,-243.1969;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.DynamicAppendNode;10;-1468.815,135.3882;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;27;-1127.475,350.244;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;6;-1129.988,106.9564;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;26;-910.664,321.3086;Float;True;Property;_TextureSample1;Texture Sample 1;1;0;Create;True;0;0;False;0;None;54398fd5d6ce02e46b5a36ea32b4a26c;True;0;False;white;Auto;False;Instance;1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;37;-959.2358,-164.1155;Float;False;Property;_Fresnel_Scale;Fresnel_Scale;10;0;Create;True;0;0;False;0;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;36;-958.0247,-258.5883;Float;False;Property;_Fresnal_Bias;Fresnal_Bias;9;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;65;-929.8261,752.7952;Float;False;Property;_Distortion_Amount;Distortion_Amount;12;0;Create;True;0;0;False;0;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;1;-855.2562,81.35922;Float;True;Property;_Main_Texture;Main_Texture;1;0;Create;True;0;0;False;0;None;54398fd5d6ce02e46b5a36ea32b4a26c;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;38;-968.9252,-69.64283;Float;False;Property;_Fresnel_Power;Fresnel_Power;11;0;Create;True;0;0;False;0;0;5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;42;-585.8884,348.5739;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;56;-656.0584,703.9746;Float;True;Property;_NormalDistortion;Normal Distortion;13;0;Create;True;0;0;False;0;None;2c9df508228ae2644a235156585a9f0c;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FresnelNode;34;-691.1947,-261.1003;Float;False;Standard;WorldNormal;ViewDir;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;52;-371.8522,-403.2347;Float;False;Property;_FresnelColor_Intensity;FresnelColor_Intensity;8;0;Create;True;0;0;False;0;0;11.64;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;43;-440.87,177.4254;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GrabScreenPosition;54;-595.1309,945.9724;Float;False;1;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.VertexColorNode;4;-409.4222,516.4166;Float;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;57;-314.9656,821.7523;Float;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;49;-102.5288,-421.8638;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;35;-390.1974,-262.9011;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;5;-147.5797,481.5234;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;3;-462.5947,-97.175;Float;False;Property;_PatternColor;Pattern Color;0;1;[HDR];Create;True;0;0;False;0;0,0,0,0;0,29.61569,95.87451,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;33;-286.1426,174.8171;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;53;68.77659,-422.5665;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;2;-113.3368,85.52084;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ScreenColorNode;55;-132.2481,816.2014;Float;False;Global;_GrabScreen0;Grab Screen 0;13;0;Create;True;0;0;False;0;Object;-1;False;True;1;0;FLOAT4;0,0,0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;60;174.9176,485.7119;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;62;416.8366,478.6096;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;39;329.0323,62.77998;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;64;480.4818,303.485;Float;False;3;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;63;775.9515,172.3928;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1002.243,134.9417;Float;False;True;2;Float;ASEMaterialInspector;0;0;Unlit;Gastón Zabala/Skills/RepulsiveBattery/Shield;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;45;0;47;0
WireConnection;45;1;18;0
WireConnection;45;2;19;0
WireConnection;30;0;28;0
WireConnection;30;1;29;0
WireConnection;20;0;7;0
WireConnection;20;1;18;0
WireConnection;20;2;19;0
WireConnection;10;0;8;0
WireConnection;10;1;9;0
WireConnection;27;0;45;0
WireConnection;27;2;30;0
WireConnection;6;0;20;0
WireConnection;6;2;10;0
WireConnection;26;1;27;0
WireConnection;1;1;6;0
WireConnection;42;0;26;1
WireConnection;56;1;27;0
WireConnection;56;5;65;0
WireConnection;34;1;36;0
WireConnection;34;2;37;0
WireConnection;34;3;38;0
WireConnection;43;0;1;1
WireConnection;43;1;42;0
WireConnection;57;0;56;0
WireConnection;57;1;54;0
WireConnection;49;0;4;0
WireConnection;49;1;52;0
WireConnection;35;0;34;0
WireConnection;5;0;1;1
WireConnection;5;1;4;4
WireConnection;33;0;43;0
WireConnection;53;0;49;0
WireConnection;53;1;35;0
WireConnection;2;0;3;0
WireConnection;2;1;33;0
WireConnection;55;0;57;0
WireConnection;60;0;5;0
WireConnection;62;1;55;0
WireConnection;62;2;60;0
WireConnection;39;0;2;0
WireConnection;39;1;53;0
WireConnection;39;2;35;0
WireConnection;64;0;5;0
WireConnection;64;1;60;0
WireConnection;64;2;60;0
WireConnection;63;0;39;0
WireConnection;63;1;62;0
WireConnection;0;2;63;0
WireConnection;0;9;64;0
ASEEND*/
//CHKSM=603682B6D53ACACFCD803424D19F1D099834809E