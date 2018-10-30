// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Hologram"
{
	Properties
	{
		[HideInInspector] __dirty( "", Int ) = 1
		_Speed("Speed", Range( 0 , 50)) = 0
		_HologramColor("Hologram Color", Color) = (0,1,0.751724,0)
		_TextureSample0("Texture Sample 0", 2D) = "bump" {}
		_RimPower("Rim Power", Range( 0 , 10)) = 0
		_ScanLines("Scan Lines", Range( 0 , 50)) = 0
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
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
		uniform float _ScanLines;
		uniform float _Speed;
		uniform sampler2D _TextureSample0;
		uniform float _RimPower;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			o.texcoord_0.xy = v.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			o.Normal = float3(0,0,1);
			float4 HologramColor4 = _HologramColor;
			float3 ase_worldPos = i.worldPos;
			float Speed2 = _Speed;
			float componentMask33 = ( 1.0 - ( Speed2 * _Time ) ).y;
			float temp_output_45_0 = sin( ( ( ( _ScanLines * ase_worldPos.y ) + componentMask33 ) * ( 2.49 * UNITY_PI ) ) );
			float4 lerpResult53 = lerp( float4(1,1,1,0) , float4(0,0,0,0) , saturate( (0.54 + (temp_output_45_0 - 0.0) * (0.42 - 0.54) / (1.0 - 0.0)) ));
			float Lines46 = temp_output_45_0;
			float4 temp_cast_0 = (Lines46).xxxx;
			float4 FinalLines55 = ( lerpResult53 - temp_cast_0 );
			float3 normalizeResult15 = normalize( i.viewDir );
			float dotResult17 = dot( UnpackNormal( tex2D( _TextureSample0, ( ( ( Speed2 / 50.0 ) * _Time ) + float4( i.texcoord_0, 0.0 , 0.0 ) ).xy ) ) , normalizeResult15 );
			float Rim24 = pow( ( 1.0 - saturate( dotResult17 ) ) , ( 9.24 - _RimPower ) );
			float4 temp_output_60_0 = ( HologramColor4 * ( FinalLines55 + Rim24 ) );
			o.Emission = temp_output_60_0.rgb;
			o.Alpha = saturate( ( temp_output_60_0.a + temp_output_60_0.g + temp_output_60_0.b ) );
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard alpha:fade keepalpha fullforwardshadows vertex:vertexDataFunc 

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
Version=12001
387;98;854;615;476.4969;-1772.395;1.3;True;False
Node;AmplifyShaderEditor.CommentaryNode;6;-846.0991,-330.0001;Float;False;640.8001;306.0001;Speed;2;2;1;;0;0
Node;AmplifyShaderEditor.RangedFloatNode;1;-775.199,-266.7002;Float;False;Property;_Speed;Speed;0;0;0;0;50;0;1;FLOAT
Node;AmplifyShaderEditor.CommentaryNode;56;-1863.802,1250.198;Float;False;3593.5;1746.098;ScanLines;23;54;55;29;30;33;36;32;49;47;46;41;52;53;27;42;39;31;26;34;35;28;45;51;;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;2;-456.8997,-278.4001;Float;False;Speed;-1;True;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.TimeNode;28;-1813.802,2567.297;Float;False;0;5;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.GetLocalVarNode;26;-1785.002,2338.497;Float;False;2;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;27;-1519.302,2420.997;Float;False;2;2;0;FLOAT;0.0,0,0,0;False;1;FLOAT4;0.0;False;1;FLOAT4
Node;AmplifyShaderEditor.CommentaryNode;25;-1738.001,102.8999;Float;False;2763.2;1054.499;Rim;16;8;9;12;11;7;10;13;17;19;21;14;15;18;23;22;24;;0;0
Node;AmplifyShaderEditor.OneMinusNode;32;-1240.002,2455.197;Float;False;1;0;FLOAT4;0.0;False;1;FLOAT4
Node;AmplifyShaderEditor.GetLocalVarNode;7;-1668.999,152.8999;Float;False;2;0;1;FLOAT
Node;AmplifyShaderEditor.WorldPosInputsNode;30;-1496.603,2172.495;Float;False;0;4;FLOAT3;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;29;-1510.003,1947.096;Float;False;Property;_ScanLines;Scan Lines;4;0;0;0;50;0;1;FLOAT
Node;AmplifyShaderEditor.ComponentMaskNode;33;-1000.001,2459.398;Float;False;False;True;False;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleDivideOpNode;10;-1439.401,216.2995;Float;False;2;0;FLOAT;0.0;False;1;FLOAT;50.0;False;1;FLOAT
Node;AmplifyShaderEditor.TimeNode;8;-1684.901,449.6998;Float;False;0;5;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;31;-1017.701,2151.197;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.TextureCoordinatesNode;11;-1319.299,400.0991;Float;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;34;-641.5011,2335.497;Float;False;2;2;0;FLOAT;0.0,0,0,0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.PiNode;36;-585.9016,2621.797;Float;False;1;0;FLOAT;2.49;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;9;-1227.9,255.9996;Float;False;2;2;0;FLOAT;0,0,0,0;False;1;FLOAT4;0.0;False;1;FLOAT4
Node;AmplifyShaderEditor.SimpleAddOpNode;12;-996.4005,297.2994;Float;False;2;2;0;FLOAT4;0,0;False;1;FLOAT2;0.0,0,0,0;False;1;FLOAT4
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;14;-989.6014,535.7992;Float;False;Tangent;0;4;FLOAT3;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;35;-356.5016,2403.897;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.NormalizeNode;15;-719.5015,477.299;Float;True;1;0;FLOAT3;0,0,0,0;False;1;FLOAT3
Node;AmplifyShaderEditor.SamplerNode;13;-757.0017,255.4992;Float;True;Property;_TextureSample0;Texture Sample 0;2;0;Assets/Demo-normal.jpg;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT3;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SinOpNode;45;-36.90245,2441.197;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.DotProductOpNode;17;-409.801,355.5992;Float;True;2;0;FLOAT3;0.0,0,0;False;1;FLOAT3;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.TFHCRemap;47;213.4987,2609.097;Float;False;5;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;3;FLOAT;0.54;False;4;FLOAT;0.42;False;1;FLOAT
Node;AmplifyShaderEditor.SaturateNode;19;-118.6007,532.3994;Float;True;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.ColorNode;51;532.5988,2259.397;Float;False;Constant;_Tint01;Tint01;6;0;1,1,1,0;0;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.ColorNode;52;533.6988,2438.998;Float;False;Constant;_Tint02;Tint02;5;0;0,0,0,0;0;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;18;-570.9009,893.0995;Float;False;Property;_RimPower;Rim Power;3;0;0;0;10;0;1;FLOAT
Node;AmplifyShaderEditor.SaturateNode;49;608.5994,2744.295;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.LerpOp;53;919.3983,2446.597;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0.0;False;2;FLOAT;0.0;False;1;COLOR
Node;AmplifyShaderEditor.RegisterLocalVarNode;46;486.7993,1963.997;Float;True;Lines;-1;True;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.OneMinusNode;21;156.1984,609.6993;Float;True;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleSubtractOpNode;23;-145.2013,903.0993;Float;False;2;0;FLOAT;9.24;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.PowerNode;22;466.5985,809.0995;Float;False;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleSubtractOpNode;54;1183.199,2427.497;Float;True;2;0;COLOR;0.0;False;1;FLOAT;0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.CommentaryNode;5;-1521.8,-361.7001;Float;False;586.4001;335.2;Hologram Color;2;3;4;;0;0
Node;AmplifyShaderEditor.GetLocalVarNode;58;1235.193,286.3965;Float;False;24;0;1;FLOAT
Node;AmplifyShaderEditor.GetLocalVarNode;57;1225.194,79.39676;Float;False;55;0;1;COLOR
Node;AmplifyShaderEditor.RegisterLocalVarNode;24;782.1989,835.0997;Float;False;Rim;-1;True;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.ColorNode;3;-1471.8,-311.7001;Float;False;Property;_HologramColor;Hologram Color;1;0;0,1,0.751724,0;0;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RegisterLocalVarNode;55;1486.698,2425.396;Float;False;FinalLines;-1;True;1;0;COLOR;0.0;False;1;COLOR
Node;AmplifyShaderEditor.SimpleAddOpNode;59;1531.392,163.5967;Float;False;2;2;0;COLOR;0.0;False;1;FLOAT;0.0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.GetLocalVarNode;61;1499.793,-79.70324;Float;False;4;0;1;COLOR
Node;AmplifyShaderEditor.RegisterLocalVarNode;4;-1212.299,-258.9001;Float;False;HologramColor;-1;True;1;0;COLOR;0.0;False;1;COLOR
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;60;1776.593,54.69675;Float;False;2;2;0;COLOR;0.0,0,0,0;False;1;COLOR;0.0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.BreakToComponentsNode;64;1828.69,239.9954;Float;False;COLOR;1;0;COLOR;0.0;False;16;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;65;2075.091,213.5954;Float;True;3;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SaturateNode;67;2332.49,277.3954;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;62;1650.893,415.5968;Float;False;Constant;_Float0;Float 0;7;0;0.7;0;1;0;1;FLOAT
Node;AmplifyShaderEditor.NoiseGeneratorNode;41;-310.0018,1926.596;Float;False;Simplex2D;1;0;FLOAT2;0,0;False;1;FLOAT
Node;AmplifyShaderEditor.TimeNode;39;-835.8022,1759.899;Float;False;0;5;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;42;214.898,2049.796;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleDivideOpNode;70;-507.9991,1798.693;Float;False;2;0;FLOAT;0.0,0,0,0;False;1;FLOAT4;0.0;False;1;FLOAT4
Node;AmplifyShaderEditor.GetLocalVarNode;68;-680.5004,1636.593;Float;False;2;0;1;FLOAT
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;2498.2,-79.89988;Float;False;True;2;Float;ASEMaterialInspector;0;Standard;Hologram;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;Back;0;0;False;0;0;Transparent;0.5;True;True;0;False;Transparent;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;False;0;4;10;25;False;0.5;True;0;Zero;Zero;0;Zero;Zero;Add;Add;0;False;0;0,0,0,0;VertexOffset;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;0;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;OBJECT;0.0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;2;0;1;0
WireConnection;27;0;26;0
WireConnection;27;1;28;0
WireConnection;32;0;27;0
WireConnection;33;0;32;0
WireConnection;10;0;7;0
WireConnection;31;0;29;0
WireConnection;31;1;30;2
WireConnection;34;0;31;0
WireConnection;34;1;33;0
WireConnection;9;0;10;0
WireConnection;9;1;8;0
WireConnection;12;0;9;0
WireConnection;12;1;11;0
WireConnection;35;0;34;0
WireConnection;35;1;36;0
WireConnection;15;0;14;0
WireConnection;13;1;12;0
WireConnection;45;0;35;0
WireConnection;17;0;13;0
WireConnection;17;1;15;0
WireConnection;47;0;45;0
WireConnection;19;0;17;0
WireConnection;49;0;47;0
WireConnection;53;0;51;0
WireConnection;53;1;52;0
WireConnection;53;2;49;0
WireConnection;46;0;45;0
WireConnection;21;0;19;0
WireConnection;23;1;18;0
WireConnection;22;0;21;0
WireConnection;22;1;23;0
WireConnection;54;0;53;0
WireConnection;54;1;46;0
WireConnection;24;0;22;0
WireConnection;55;0;54;0
WireConnection;59;0;57;0
WireConnection;59;1;58;0
WireConnection;4;0;3;0
WireConnection;60;0;61;0
WireConnection;60;1;59;0
WireConnection;64;0;60;0
WireConnection;65;0;64;3
WireConnection;65;1;64;1
WireConnection;65;2;64;2
WireConnection;67;0;65;0
WireConnection;41;0;70;0
WireConnection;42;0;41;0
WireConnection;42;1;45;0
WireConnection;70;0;68;0
WireConnection;70;1;39;0
WireConnection;0;2;60;0
WireConnection;0;9;67;0
ASEEND*/
//CHKSM=18ACD1E70EEB4B54B3C4B62EAC3CDE5554C8D7CF