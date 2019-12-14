// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Gastón Zabala/Skills/ImplosiveCharge_OutDistortion"
{
	Properties
	{
		_NormalDistortion("Normal Distortion", 2D) = "bump" {}
		_ScaleDistortion("Scale Distortion", Float) = 0
		_Center("Center", Float) = 0
		_Flowmap("Flowmap", 2D) = "white" {}
		_Flowmap_Intensity("Flowmap_Intensity", Range( 0 , 1)) = 0
		_Flowmap_Speed_Y("Flowmap_Speed_Y", Float) = 0
		_Flowmap_Speed_X("Flowmap_Speed_X", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		GrabPass{ }
		CGINCLUDE
		#include "UnityStandardUtils.cginc"
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		struct Input
		{
			float2 uv_texcoord;
			float4 screenPos;
		};

		uniform sampler2D _GrabTexture;
		uniform float _ScaleDistortion;
		uniform sampler2D _NormalDistortion;
		uniform sampler2D _Flowmap;
		uniform float _Flowmap_Speed_X;
		uniform float _Flowmap_Speed_Y;
		uniform float _Flowmap_Intensity;
		uniform float _Center;


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
			float2 appendResult23 = (float2(_Flowmap_Speed_X , _Flowmap_Speed_Y));
			float2 panner16 = ( 1.0 * _Time.y * appendResult23 + i.uv_texcoord);
			float4 lerpResult19 = lerp( float4( i.uv_texcoord, 0.0 , 0.0 ) , tex2D( _Flowmap, panner16 ) , _Flowmap_Intensity);
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_grabScreenPos = ASE_ComputeGrabScreenPos( ase_screenPos );
			float4 ase_grabScreenPosNorm = ase_grabScreenPos / ase_grabScreenPos.w;
			float4 screenColor4 = tex2Dproj( _GrabTexture, UNITY_PROJ_COORD( ( float4( UnpackScaleNormal( tex2D( _NormalDistortion, (lerpResult19).rg ), _ScaleDistortion ) , 0.0 ) + ase_grabScreenPosNorm ) ) );
			float temp_output_10_0 = ( distance( i.uv_texcoord , float2( 0.5,0.5 ) ) + _Center );
			float smoothstepResult12 = smoothstep( 0.28 , 0.89 , temp_output_10_0);
			float temp_output_15_0 = ( 1.0 - smoothstepResult12 );
			float smoothstepResult13 = smoothstep( 0.65 , 0.67 , temp_output_10_0);
			float4 lerpResult26 = lerp( float4( 0,0,0,0 ) , screenColor4 , ( temp_output_15_0 * smoothstepResult13 ));
			o.Emission = lerpResult26.rgb;
			o.Alpha = temp_output_15_0;
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
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				o.screenPos = ComputeScreenPos( o.pos );
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
				surfIN.screenPos = IN.screenPos;
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
7;7;1266;948;2120.867;226.2039;1.05328;True;False
Node;AmplifyShaderEditor.RangedFloatNode;21;-2571.288,156.6447;Float;False;Property;_Flowmap_Speed_Y;Flowmap_Speed_Y;5;0;Create;True;0;0;False;0;0;0.3;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;20;-2570.741,66.22386;Float;False;Property;_Flowmap_Speed_X;Flowmap_Speed_X;6;0;Create;True;0;0;False;0;0;0.3;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;8;-2558.234,-69.11829;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;23;-2346.463,101.9337;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;16;-2184.335,0.597435;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;18;-2042.288,399.6447;Float;False;Property;_Flowmap_Intensity;Flowmap_Intensity;4;0;Create;True;0;0;False;0;0;0.331;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;24;-2117.463,210.9337;Float;False;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;17;-1984.288,-26.35535;Float;True;Property;_Flowmap;Flowmap;3;0;Create;True;0;0;False;0;None;bf324afe2f0f40e469ada9b430482d90;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;11;-1223.698,491.8283;Float;False;Property;_Center;Center;2;0;Create;True;0;0;False;0;0;0.41;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DistanceOpNode;9;-1222.234,385.8817;Float;False;2;0;FLOAT2;0,0;False;1;FLOAT2;0.5,0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;19;-1692.288,186.6447;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;2;-1462.019,-51.62225;Float;False;Property;_ScaleDistortion;Scale Distortion;1;0;Create;True;0;0;False;0;0;4.79;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;10;-1022.698,385.8283;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;22;-1441.288,185.6447;Float;False;True;True;False;False;1;0;COLOR;0,0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;1;-1263.475,-96.66321;Float;True;Property;_NormalDistortion;Normal Distortion;0;0;Create;True;0;0;False;0;None;5c990d6b110a8ca43b9667e762f1f571;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SmoothstepOpNode;12;-839.6984,381.8283;Float;False;3;0;FLOAT;0;False;1;FLOAT;0.28;False;2;FLOAT;0.89;False;1;FLOAT;0
Node;AmplifyShaderEditor.GrabScreenPosition;5;-1205.019,123.3777;Float;False;0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SmoothstepOpNode;13;-848.557,622.5045;Float;False;3;0;FLOAT;0;False;1;FLOAT;0.65;False;2;FLOAT;0.67;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;15;-582.6451,383.313;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;3;-894.0186,10.37775;Float;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.ScreenColorNode;4;-717.0186,5.377747;Float;False;Global;_GrabScreen0;Grab Screen 0;2;0;Create;True;0;0;False;0;Object;-1;False;True;1;0;FLOAT4;0,0,0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;25;-376.115,586.0192;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;26;-241.9825,68.61443;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;7;-43.6,-32.6;Float;False;True;2;Float;ASEMaterialInspector;0;0;Unlit;Gastón Zabala/Skills/ImplosiveCharge_OutDistortion;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;23;0;20;0
WireConnection;23;1;21;0
WireConnection;16;0;8;0
WireConnection;16;2;23;0
WireConnection;24;0;8;0
WireConnection;17;1;16;0
WireConnection;9;0;8;0
WireConnection;19;0;24;0
WireConnection;19;1;17;0
WireConnection;19;2;18;0
WireConnection;10;0;9;0
WireConnection;10;1;11;0
WireConnection;22;0;19;0
WireConnection;1;1;22;0
WireConnection;1;5;2;0
WireConnection;12;0;10;0
WireConnection;13;0;10;0
WireConnection;15;0;12;0
WireConnection;3;0;1;0
WireConnection;3;1;5;0
WireConnection;4;0;3;0
WireConnection;25;0;15;0
WireConnection;25;1;13;0
WireConnection;26;1;4;0
WireConnection;26;2;25;0
WireConnection;7;2;26;0
WireConnection;7;9;15;0
ASEEND*/
//CHKSM=F0596B4DEAF10A5B1445E343AEC1B935A9F373B6