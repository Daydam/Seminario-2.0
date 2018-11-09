// Upgrade NOTE: upgraded instancing buffer 'Rim' to new syntax.

// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Rim"
{
	Properties
	{
		_Bias("Bias", Float) = 0
		_Scale("Scale", Float) = 0
		_Power("Power", Float) = 0
		_Tint("Tint", Color) = (1,0,0,0)
		_Active("Active", Float) = 0
		_CollapsePosition("CollapsePosition", Vector) = (0,0,0,0)
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Pass
		{
			ColorMask 0
			ZTest Always
			ZWrite On
		}

		Tags{ "RenderType" = "Transparent"  "Queue" = "Overlay+0" "IsEmissive" = "true"  "VertexCollapse"="true" }
		Cull Back
		ZTest LEqual
		Blend One Zero
		BlendOp Max
		CGINCLUDE
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#pragma multi_compile_instancing
		struct Input
		{
			float3 worldPos;
			float3 worldNormal;
		};

		uniform float _Bias;
		uniform float _Scale;
		uniform float _Power;

		UNITY_INSTANCING_BUFFER_START(Rim)
			UNITY_DEFINE_INSTANCED_PROP(float3, _CollapsePosition)
#define _CollapsePosition_arr Rim
			UNITY_DEFINE_INSTANCED_PROP(float4, _Tint)
#define _Tint_arr Rim
			UNITY_DEFINE_INSTANCED_PROP(float, _Active)
#define _Active_arr Rim
		UNITY_INSTANCING_BUFFER_END(Rim)

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float3 ase_worldPos = mul( unity_ObjectToWorld, v.vertex );
			float3 temp_output_16_0_g1 = ase_worldPos;
			float3 _CollapsePosition_Instance = UNITY_ACCESS_INSTANCED_PROP(_CollapsePosition_arr, _CollapsePosition);
			float3 temp_output_17_0_g1 = _CollapsePosition_Instance;
			float3 lerpResult12_g1 = lerp( float3( 0,0,0 ) , ( ( temp_output_16_0_g1 + ( 1.0 - temp_output_17_0_g1 ) ) + float3(-1,-1,-1) ) , ( saturate( pow( ( distance( temp_output_16_0_g1 , temp_output_17_0_g1 ) / 3.0 ) , 1.3 ) ) - 1.0 ));
			float4 transform15_g1 = mul(unity_WorldToObject,float4( lerpResult12_g1 , 0.0 ));
			v.vertex.xyz += transform15_g1.xyz;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float4 _Tint_Instance = UNITY_ACCESS_INSTANCED_PROP(_Tint_arr, _Tint);
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = i.worldNormal;
			float fresnelNDotV1 = dot( normalize( ase_worldNormal ), ase_worldViewDir );
			float fresnelNode1 = ( _Bias + _Scale * pow( 1.0 - fresnelNDotV1, _Power ) );
			float _Active_Instance = UNITY_ACCESS_INSTANCED_PROP(_Active_arr, _Active);
			float4 temp_output_8_0 = ( _Tint_Instance * saturate( fresnelNode1 ) * _Active_Instance );
			o.Emission = temp_output_8_0.rgb;
			o.Alpha = temp_output_8_0.r;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard keepalpha fullforwardshadows vertex:vertexDataFunc 

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
				float3 worldPos : TEXCOORD1;
				float3 worldNormal : TEXCOORD2;
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
				o.worldNormal = worldNormal;
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
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
				float3 worldPos = IN.worldPos;
				fixed3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = IN.worldNormal;
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
7;115;1067;606;589.299;-99.24487;1;True;False
Node;AmplifyShaderEditor.RangedFloatNode;4;-1034.984,266.844;Float;False;Property;_Power;Power;3;0;Create;True;0;0;False;0;0;3;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;2;-1032.984,116.8439;Float;False;Property;_Bias;Bias;1;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;3;-1035.984,190.8439;Float;False;Property;_Scale;Scale;2;0;Create;True;0;0;False;0;0;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FresnelNode;1;-802.9839,106.2438;Float;True;World;4;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;19;-145.299,588.2449;Float;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.Vector3Node;18;-295.299,415.2449;Float;False;InstancedProperty;_CollapsePosition;CollapsePosition;6;0;Create;True;0;0;False;0;0,0,0;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SaturateNode;13;-380.5085,48.3675;Float;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;6;-688.5192,-263.1642;Float;False;InstancedProperty;_Tint;Tint;4;0;Create;True;0;0;False;0;1,0,0,0;1,1,1,1;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;14;-461.7329,369.2131;Float;False;InstancedProperty;_Active;Active;5;0;Create;True;0;0;False;0;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;17;-12.67896,365.5306;Float;False;VertexCollapse;-1;;1;71a74875a0190c346ab880dd8b435206;0;2;17;FLOAT3;0,0,0;False;16;FLOAT3;0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;8;-123.3476,-1.722147;Float;True;3;3;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;16;334.7754,-62.06316;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;Rim;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;3;False;-1;False;0;1;True;7;Custom;0.5;True;True;0;True;Transparent;;Overlay;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;1;1;False;-1;0;False;-1;0;10;False;-1;0;False;-1;4;False;-1;3;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;1;VertexCollapse=true;0;False;0;0;0;False;-1;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;1;1;2;0
WireConnection;1;2;3;0
WireConnection;1;3;4;0
WireConnection;13;0;1;0
WireConnection;17;17;18;0
WireConnection;17;16;19;0
WireConnection;8;0;6;0
WireConnection;8;1;13;0
WireConnection;8;2;14;0
WireConnection;16;2;8;0
WireConnection;16;9;8;0
WireConnection;16;11;17;0
ASEEND*/
//CHKSM=C26270EC0B5DDB283F0B8BC1EA9BCAF45C42EE1D