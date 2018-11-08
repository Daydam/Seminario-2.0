// Upgrade NOTE: upgraded instancing buffer 'BestPlayer' to new syntax.

// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "BestPlayer"
{
	Properties
	{
		[HideInInspector] __dirty( "", Int ) = 1
		[Header(Rim)]
		_MaskClipValue( "Mask Clip Value", Float ) = 0.5
		_Speed("Speed", Range( 0 , 50)) = 0
		_HologramColor("Hologram Color", Color) = (0,1,0.751724,0)
		_TextureSample1("Texture Sample 1", 2D) = "bump" {}
		_RimPower("Rim Power", Range( 0 , 10)) = 0
		_ScanLinesAmount("ScanLinesAmount", Range( 0 , 50)) = 0
		[HideInInspector]_isBest("isBest", Range( 0 , 1)) = 0
		_CollapsePosition("Collapse Position", Vector) = (5000,5000,5000,0)
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+1" "IgnoreProjector" = "True" "IsEmissive" = "true" "VertexCollapse"="true" "BestPlayerMaterial"="true" }
		Cull Back
		ZTest Greater
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#pragma multi_compile_instancing
		#ifdef UNITY_PASS_SHADOWCASTER
			#undef INTERNAL_DATA
			#undef WorldReflectionVector
			#undef WorldNormalVector
			#define INTERNAL_DATA half3 internalSurfaceTtoW0; half3 internalSurfaceTtoW1; half3 internalSurfaceTtoW2;
			#define WorldReflectionVector(data,normal) reflect (data.worldRefl, half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal)))
			#define WorldNormalVector(data,normal) fixed3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal))
		#endif
		struct Input
		{
			float3 worldPos;
			float2 texcoord_0;
			float3 viewDir;
			INTERNAL_DATA
		};

		uniform float4 _HologramColor;
		uniform float _ScanLinesAmount;
		uniform float _Speed;
		uniform sampler2D _TextureSample1;
		uniform float _RimPower;
		uniform float _MaskClipValue = 0.5;

		UNITY_INSTANCING_BUFFER_START(BestPlayer)
			UNITY_DEFINE_INSTANCED_PROP(float, _isBest)
#define _isBest_arr BestPlayer
			UNITY_DEFINE_INSTANCED_PROP(float3, _CollapsePosition)
#define _CollapsePosition_arr BestPlayer
		UNITY_INSTANCING_BUFFER_END(BestPlayer)

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			o.texcoord_0.xy = v.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
			float3 ase_worldPos = mul(unity_ObjectToWorld, v.vertex);
			float3 _CollapsePosition_Instance = UNITY_ACCESS_INSTANCED_PROP(_CollapsePosition_arr, _CollapsePosition);
			float3 lerpResult12_g3 = lerp( float3( 0,0,0 ) , ( ( ase_worldPos + ( 1.0 - _CollapsePosition_Instance ) ) + float3(-1,-1,-1) ) , ( saturate( pow( ( distance( ase_worldPos , _CollapsePosition_Instance ) / 3.0 ) , 1.3 ) ) - 1.0 ));
			float4 transform15_g3 = mul(unity_WorldToObject,float4( lerpResult12_g3 , 0.0 ));
			v.vertex.xyz += transform15_g3.xyz;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			o.Normal = float3(0,0,1);
			float3 ase_worldPos = i.worldPos;
			float componentMask6_g5 = ( 1.0 - ( _Speed * _Time ) ).y;
			float temp_output_11_0_g5 = sin( ( ( ( _ScanLinesAmount * ase_worldPos.y ) + componentMask6_g5 ) * ( 2.49 * UNITY_PI ) ) );
			float4 lerpResult16_g5 = lerp( float4( 1,1,1,1 ) , float4( 0,0,0,0 ) , saturate( (0.54 + (temp_output_11_0_g5 - 0.0) * (0.42 - 0.54) / (1.0 - 0.0)) ));
			float4 temp_cast_0 = (temp_output_11_0_g5).xxxx;
			float3 normalizeResult8_g7 = normalize( i.viewDir );
			float dotResult10_g7 = dot( UnpackNormal( tex2D( _TextureSample1, ( ( ( _Speed / 50.0 ) * _Time ) + float4( i.texcoord_0, 0.0 , 0.0 ) ).xy ) ) , normalizeResult8_g7 );
			float _isBest_Instance = UNITY_ACCESS_INSTANCED_PROP(_isBest_arr, _isBest);
			float temp_output_91_0 = trunc( _isBest_Instance );
			o.Emission = ( ( _HologramColor * ( ( lerpResult16_g5 - temp_cast_0 ) + pow( ( 1.0 - saturate( dotResult10_g7 ) ) , ( 9.24 - _RimPower ) ) ) ) * temp_output_91_0 ).rgb;
			o.Alpha = temp_output_91_0;
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
			# include "HLSLSupport.cginc"
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
				float3 worldPos : TEXCOORD6;
				float4 tSpace0 : TEXCOORD1;
				float4 tSpace1 : TEXCOORD2;
				float4 tSpace2 : TEXCOORD3;
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
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				fixed3 worldTangent = UnityObjectToWorldDir( v.tangent.xyz );
				fixed tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				fixed3 worldBinormal = cross( worldNormal, worldTangent ) * tangentSign;
				o.tSpace0 = float4( worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x );
				o.tSpace1 = float4( worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y );
				o.tSpace2 = float4( worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z );
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
				float3 worldPos = float3( IN.tSpace0.w, IN.tSpace1.w, IN.tSpace2.w );
				fixed3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.viewDir = IN.tSpace0.xyz * worldViewDir.x + IN.tSpace1.xyz * worldViewDir.y + IN.tSpace2.xyz * worldViewDir.z;
				surfIN.worldPos = worldPos;
				surfIN.internalSurfaceTtoW0 = IN.tSpace0.xyz;
				surfIN.internalSurfaceTtoW1 = IN.tSpace1.xyz;
				surfIN.internalSurfaceTtoW2 = IN.tSpace2.xyz;
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
Version=13101
11;76;1352;692;-1288.09;376.1622;1.457922;True;False
Node;AmplifyShaderEditor.RangedFloatNode;1;612.9409,82.67126;Float;False;Property;_Speed;Speed;1;0;0;0;50;0;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;29;616.4516,-43.89869;Float;False;Property;_ScanLinesAmount;ScanLinesAmount;5;0;0;0;50;0;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;18;548.409,240.7043;Float;False;Property;_RimPower;Rim Power;4;0;0;0;10;0;1;FLOAT
Node;AmplifyShaderEditor.FunctionNode;106;905.051,219.6818;Float;False;Rim;0;;7;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.FunctionNode;104;906.2282,4.609021;Float;False;ScanLines;-1;;5;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;COLOR
Node;AmplifyShaderEditor.ColorNode;3;1047.634,-265.5793;Float;False;Property;_HologramColor;Hologram Color;2;0;0,1,0.751724,0;0;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;59;1249.667,114.4587;Float;False;2;2;0;COLOR;0.0;False;1;FLOAT;0.0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.RangedFloatNode;90;1747.584,136.0418;Float;False;InstancedProperty;_isBest;isBest;6;1;[HideInInspector];0;0;1;0;1;FLOAT
Node;AmplifyShaderEditor.Vector3Node;100;2027.316,334.9594;Float;False;InstancedProperty;_CollapsePosition;Collapse Position;7;0;5000,5000,5000;0;4;FLOAT3;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.TruncOpNode;91;2171.756,146.942;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;60;1494.868,5.558702;Float;False;2;2;0;COLOR;0.0,0,0,0;False;1;COLOR;0.0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.WorldPosInputsNode;99;2024.987,593.7531;Float;False;0;4;FLOAT3;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;92;2557.085,-158.5916;Float;False;2;2;0;COLOR;0.0;False;1;FLOAT;0.0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.FunctionNode;102;2508.208,451.3085;Float;False;VertexCollapse;-1;;3;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;3116.049,-25.43897;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;BestPlayer;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;Back;0;2;False;0;0;Custom;0.5;True;True;1;True;Transparent;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;255;255;255;7;3;0;0;False;0;4;10;25;False;0.5;True;0;Zero;Zero;0;Zero;Zero;Add;Add;0;False;0;0,0,0,0;VertexOffset;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;2;VertexCollapse=true;BestPlayerMaterial=true;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;OBJECT;0.0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;106;0;18;0
WireConnection;106;1;1;0
WireConnection;104;0;29;0
WireConnection;104;1;1;0
WireConnection;59;0;104;0
WireConnection;59;1;106;0
WireConnection;91;0;90;0
WireConnection;60;0;3;0
WireConnection;60;1;59;0
WireConnection;92;0;60;0
WireConnection;92;1;91;0
WireConnection;102;0;100;0
WireConnection;102;1;99;0
WireConnection;0;2;92;0
WireConnection;0;9;91;0
WireConnection;0;11;102;0
ASEEND*/
//CHKSM=3AA3F2BD4491E35FC4416C477B8EC940DBF7AF3E