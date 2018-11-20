// Upgrade NOTE: upgraded instancing buffer 'Drone_Body' to new syntax.

// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Drone_Body"
{
	Properties
	{
		_Albedo("Albedo", 2D) = "white" {}
		_LifeEmission("Life Emission", 2D) = "white" {}
		_DefEmission("Def Emission", 2D) = "white" {}
		_TextureSample1("Texture Sample 1", 2D) = "bump" {}
		_PlayerColorTexture("PlayerColorTexture", 2D) = "white" {}
		_NormalMap("NormalMap", 2D) = "white" {}
		_Metallic("Metallic", 2D) = "white" {}
		_MetallicIntensity("Metallic Intensity", Range( 0 , 1)) = 1
		_Roughness("Roughness", 2D) = "white" {}
		_AmbientOcclusion("AmbientOcclusion", 2D) = "white" {}
		_SkillStateColor("SkillStateColor", Color) = (0,1,1,0)
		_LifeRamp("LifeRamp", 2D) = "white" {}
		_CollapsePosition("Collapse Position", Vector) = (5000,5000,5000,0)
		_ScanLinesAmount("ScanLinesAmount", Float) = 0
		_isBest("isBest", Range( 0 , 1)) = 0
		_RimPower("RimPower", Float) = 0
		_Speed("Speed", Float) = 0
		_Life("Life", Range( 0 , 1)) = 0
		_PlayerColor("PlayerColor", Color) = (0,0,0,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  "SkillStateColor"="Defensive" "BestPlayerMaterial"="true" "VertexCollapse"="true" }
		Cull Back
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
			float2 uv_texcoord;
			float3 worldPos;
			float3 viewDir;
			INTERNAL_DATA
		};

		uniform sampler2D _NormalMap;
		uniform float4 _NormalMap_ST;
		uniform sampler2D _Albedo;
		uniform float4 _Albedo_ST;
		uniform sampler2D _PlayerColorTexture;
		uniform float4 _PlayerColorTexture_ST;
		uniform sampler2D _LifeRamp;
		uniform sampler2D _LifeEmission;
		uniform float4 _LifeEmission_ST;
		uniform sampler2D _DefEmission;
		uniform float4 _DefEmission_ST;
		uniform float4 _SkillStateColor;
		uniform float _ScanLinesAmount;
		uniform float _Speed;
		uniform sampler2D _TextureSample1;
		uniform float _RimPower;
		uniform sampler2D _Metallic;
		uniform float4 _Metallic_ST;
		uniform float _MetallicIntensity;
		uniform sampler2D _Roughness;
		uniform float4 _Roughness_ST;
		uniform sampler2D _AmbientOcclusion;
		uniform float4 _AmbientOcclusion_ST;

		UNITY_INSTANCING_BUFFER_START(Drone_Body)
			UNITY_DEFINE_INSTANCED_PROP(float3, _CollapsePosition)
#define _CollapsePosition_arr Drone_Body
			UNITY_DEFINE_INSTANCED_PROP(float4, _PlayerColor)
#define _PlayerColor_arr Drone_Body
			UNITY_DEFINE_INSTANCED_PROP(float, _Life)
#define _Life_arr Drone_Body
			UNITY_DEFINE_INSTANCED_PROP(float, _isBest)
#define _isBest_arr Drone_Body
		UNITY_INSTANCING_BUFFER_END(Drone_Body)

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float3 ase_worldPos = mul( unity_ObjectToWorld, v.vertex );
			float3 temp_output_16_0_g27 = ase_worldPos;
			float3 _CollapsePosition_Instance = UNITY_ACCESS_INSTANCED_PROP(_CollapsePosition_arr, _CollapsePosition);
			float3 temp_output_17_0_g27 = _CollapsePosition_Instance;
			float3 lerpResult12_g27 = lerp( float3( 0,0,0 ) , ( ( temp_output_16_0_g27 + ( 1.0 - temp_output_17_0_g27 ) ) + float3(-1,-1,-1) ) , ( saturate( pow( ( distance( temp_output_16_0_g27 , temp_output_17_0_g27 ) / 3.0 ) , 1.3 ) ) - 1.0 ));
			float4 transform15_g27 = mul(unity_WorldToObject,float4( lerpResult12_g27 , 0.0 ));
			v.vertex.xyz += transform15_g27.xyz;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_NormalMap = i.uv_texcoord * _NormalMap_ST.xy + _NormalMap_ST.zw;
			o.Normal = UnpackNormal( tex2D( _NormalMap, uv_NormalMap ) );
			float2 uv_Albedo = i.uv_texcoord * _Albedo_ST.xy + _Albedo_ST.zw;
			o.Albedo = tex2D( _Albedo, uv_Albedo ).rgb;
			float2 uv_PlayerColorTexture = i.uv_texcoord * _PlayerColorTexture_ST.xy + _PlayerColorTexture_ST.zw;
			float4 _PlayerColor_Instance = UNITY_ACCESS_INSTANCED_PROP(_PlayerColor_arr, _PlayerColor);
			float2 temp_cast_1 = (i.uv_texcoord.x).xx;
			float _Life_Instance = UNITY_ACCESS_INSTANCED_PROP(_Life_arr, _Life);
			// *** BEGIN Flipbook UV Animation vars ***
			// Total tiles of Flipbook Texture
			float fbtotaltiles28 = 5.0 * 1.0;
			// Offsets for cols and rows of Flipbook Texture
			float fbcolsoffset28 = 1.0f / 5.0;
			float fbrowsoffset28 = 1.0f / 1.0;
			// Speed of animation
			float fbspeed28 = _Time[ 1 ] * 0.0;
			// UV Tiling (col and row offset)
			float2 fbtiling28 = float2(fbcolsoffset28, fbrowsoffset28);
			// UV Offset - calculate current tile linear index, and convert it to (X * coloffset, Y * rowoffset)
			// Calculate current tile linear index
			float fbcurrenttileindex28 = round( fmod( fbspeed28 + (3.8 + (_Life_Instance - 0.0) * (0.0 - 3.8) / (1.0 - 0.0)), fbtotaltiles28) );
			fbcurrenttileindex28 += ( fbcurrenttileindex28 < 0) ? fbtotaltiles28 : 0;
			// Obtain Offset X coordinate from current tile linear index
			float fblinearindextox28 = round ( fmod ( fbcurrenttileindex28, 5.0 ) );
			// Multiply Offset X by coloffset
			float fboffsetx28 = fblinearindextox28 * fbcolsoffset28;
			// Obtain Offset Y coordinate from current tile linear index
			float fblinearindextoy28 = round( fmod( ( fbcurrenttileindex28 - fblinearindextox28 ) / 5.0, 1.0 ) );
			// Reverse Y to get tiles from Top to Bottom
			fblinearindextoy28 = (int)(1.0-1) - fblinearindextoy28;
			// Multiply Offset Y by rowoffset
			float fboffsety28 = fblinearindextoy28 * fbrowsoffset28;
			// UV Offset
			float2 fboffset28 = float2(fboffsetx28, fboffsety28);
			// Flipbook UV
			half2 fbuv28 = temp_cast_1 * fbtiling28 + fboffset28;
			// *** END Flipbook UV Animation vars ***
			float2 uv_LifeEmission = i.uv_texcoord * _LifeEmission_ST.xy + _LifeEmission_ST.zw;
			float2 uv_DefEmission = i.uv_texcoord * _DefEmission_ST.xy + _DefEmission_ST.zw;
			float lerpResult16_g25 = lerp( 1.0 , 0.0 , saturate( 0.0 ));
			float3 ase_worldPos = i.worldPos;
			float temp_output_14_0_g24 = _Speed;
			float3 normalizeResult8_g26 = normalize( i.viewDir );
			float dotResult10_g26 = dot( UnpackNormal( tex2D( _TextureSample1, ( ( ( temp_output_14_0_g24 / 50.0 ) * _Time ) + float4( i.uv_texcoord, 0.0 , 0.0 ) ).xy ) ) , normalizeResult8_g26 );
			float _isBest_Instance = UNITY_ACCESS_INSTANCED_PROP(_isBest_arr, _isBest);
			float temp_output_11_0_g24 = ( ( ( lerpResult16_g25 - sin( ( ( ( _ScanLinesAmount * ase_worldPos.y ) + (( 1.0 - ( temp_output_14_0_g24 * _Time ) )).y ) * ( 2.49 * UNITY_PI ) ) ) ) + pow( ( 1.0 - saturate( dotResult10_g26 ) ) , ( 9.24 - _RimPower ) ) ) * trunc( _isBest_Instance ) );
			float4 lerpResult59 = lerp( ( saturate( ( ( tex2D( _PlayerColorTexture, uv_PlayerColorTexture ) * _PlayerColor_Instance ) * 0.7 ) ) + ( ( ( tex2D( _LifeRamp, fbuv28 ) * tex2D( _LifeEmission, uv_LifeEmission ) ) + ( tex2D( _DefEmission, uv_DefEmission ) * _SkillStateColor ) ) * 5.0 ) ) , ( float4(1,0.9724138,0,0) * temp_output_11_0_g24 ) , saturate( temp_output_11_0_g24 ));
			o.Emission = lerpResult59.rgb;
			float2 uv_Metallic = i.uv_texcoord * _Metallic_ST.xy + _Metallic_ST.zw;
			o.Metallic = ( tex2D( _Metallic, uv_Metallic ) * _MetallicIntensity ).r;
			float2 uv_Roughness = i.uv_texcoord * _Roughness_ST.xy + _Roughness_ST.zw;
			o.Smoothness = ( 1.0 - tex2D( _Roughness, uv_Roughness ) ).r;
			float2 uv_AmbientOcclusion = i.uv_texcoord * _AmbientOcclusion_ST.xy + _AmbientOcclusion_ST.zw;
			o.Occlusion = tex2D( _AmbientOcclusion, uv_AmbientOcclusion ).r;
			o.Alpha = 1;
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
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float4 tSpace0 : TEXCOORD2;
				float4 tSpace1 : TEXCOORD3;
				float4 tSpace2 : TEXCOORD4;
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
				fixed3 worldTangent = UnityObjectToWorldDir( v.tangent.xyz );
				fixed tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				fixed3 worldBinormal = cross( worldNormal, worldTangent ) * tangentSign;
				o.tSpace0 = float4( worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x );
				o.tSpace1 = float4( worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y );
				o.tSpace2 = float4( worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
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
				surfIN.uv_texcoord = IN.customPack1.xy;
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
94;72;1091;606;1877.417;539.9504;2.37764;True;False
Node;AmplifyShaderEditor.RangedFloatNode;35;-2614.488,-458.1126;Float;False;InstancedProperty;_Life;Life;16;0;Create;True;0;0;False;0;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;36;-2112.075,-796.0167;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TFHCRemapNode;38;-2220.62,-528.2474;Float;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;3.8;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCFlipBookUVAnimation;28;-1810.048,-622.0981;Float;False;0;0;6;0;FLOAT2;0,0;False;1;FLOAT;5;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;5;FLOAT;0;False;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SamplerNode;29;-1470.743,-646.7331;Float;True;Property;_LifeRamp;LifeRamp;10;0;Create;True;0;0;False;0;4d64ea565d9a404479a62e2bdadb63a6;4d64ea565d9a404479a62e2bdadb63a6;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;15;-2102.454,507.7737;Float;True;Property;_DefEmission;Def Emission;2;0;Create;True;0;0;False;0;None;ecb0650971a8e7a478dd39f312bb42be;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;3;-1691.199,-91.99553;Float;True;Property;_LifeEmission;Life Emission;1;0;Create;True;0;0;False;0;None;10e3522f15a131b449ee6e36c6709104;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;17;-2026.107,823.8917;Float;False;Property;_SkillStateColor;SkillStateColor;9;0;Create;True;0;0;False;0;0,1,1,0;0,1,1,1;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;40;-1028.382,-775.0621;Float;True;Property;_PlayerColorTexture;PlayerColorTexture;3;0;Create;True;0;0;False;0;e4bd927386352c649bfd7f64c6869152;798bed6ff2228394fa0d1c78fa306457;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;41;-980.0304,-578.0621;Float;False;InstancedProperty;_PlayerColor;PlayerColor;17;0;Create;True;0;0;False;0;0,0,0,0;0,0,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;39;-599.1735,-639.556;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;13;-1191.844,-211.771;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;46;-688.719,-417.0808;Float;False;Constant;_MultiplierEmission;MultiplierEmission;13;0;Create;True;0;0;False;0;0.7;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;19;-1422.472,614.6387;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;47;-332.7679,-349.6658;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;7;-1033.923,3.210692;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;20;-970.4698,219.6131;Float;False;Constant;_EmissionIntensity;Emission Intensity;7;0;Create;True;0;0;False;0;5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;50;-145.6347,-278.7605;Float;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;55;620.7263,762.7181;Float;False;Property;_RimPower;RimPower;14;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;57;617.3719,926.1673;Float;False;Property;_ScanLinesAmount;ScanLinesAmount;12;0;Create;True;0;0;False;0;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;21;-562.3788,-178.256;Float;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;58;615.1719,1011.867;Float;False;Property;_Speed;Speed;15;0;Create;True;0;0;False;0;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;56;619.7263,845.7181;Float;False;InstancedProperty;_isBest;isBest;13;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector3Node;23;-360.0426,818.194;Float;False;InstancedProperty;_CollapsePosition;Collapse Position;11;0;Create;True;0;0;False;0;5000,5000,5000;5555,5555,5555;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.FunctionNode;68;1062.306,633.2125;Float;False;BestPlayer;-1;;24;492b74c59acac8c4c84a79bed1dbc325;0;4;15;FLOAT;0;False;16;FLOAT;0;False;12;FLOAT;0;False;14;FLOAT;0;False;2;FLOAT;19;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;48;154.9778,-239.6306;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;53;-525.1414,68.0598;Float;True;Property;_Metallic;Metallic;5;0;Create;True;0;0;False;0;None;b902bd77d59a0d04c8f0bb8b2d20cbe8;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WorldPosInputsNode;24;-372.799,1033.822;Float;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;69;-453.4702,305.4801;Float;False;Property;_MetallicIntensity;Metallic Intensity;6;0;Create;True;0;0;False;0;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;5;-423.5882,407.0842;Float;True;Property;_Roughness;Roughness;7;0;Create;True;0;0;False;0;None;c1898d050ece4bd439d9fead7f3e337f;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;1;-1037.452,-995.3718;Float;True;Property;_Albedo;Albedo;0;0;Create;True;0;0;False;0;None;d6a0f2b548f96bf4480d5c1d1617a7e6;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;51;-385.1213,593.2842;Float;True;Property;_AmbientOcclusion;AmbientOcclusion;8;0;Create;True;0;0;False;0;None;dc3b535c1aa1d4b4f8b89d5c2d47eb2d;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;52;-258.1645,-99.18912;Float;True;Property;_NormalMap;NormalMap;4;0;Create;True;0;0;False;0;None;06b9ea65e784ca24fa40bd22d76d144c;True;0;True;white;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;59;1776.105,439.7803;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;6;41.07049,276.9182;Float;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;70;-128.4911,133.9312;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;22;-83.3712,957.9627;Float;False;VertexCollapse;-1;;27;71a74875a0190c346ab880dd8b435206;0;2;17;FLOAT3;0,0,0;False;16;FLOAT3;0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;2002.142,-565.7762;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;Drone_Body;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;0;False;2;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;6;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;0;4;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;3;SkillStateColor=Defensive;BestPlayerMaterial=true;VertexCollapse=true;0;False;0;0;0;False;-1;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;38;0;35;0
WireConnection;28;0;36;1
WireConnection;28;4;38;0
WireConnection;29;1;28;0
WireConnection;39;0;40;0
WireConnection;39;1;41;0
WireConnection;13;0;29;0
WireConnection;13;1;3;0
WireConnection;19;0;15;0
WireConnection;19;1;17;0
WireConnection;47;0;39;0
WireConnection;47;1;46;0
WireConnection;7;0;13;0
WireConnection;7;1;19;0
WireConnection;50;0;47;0
WireConnection;21;0;7;0
WireConnection;21;1;20;0
WireConnection;68;15;55;0
WireConnection;68;16;56;0
WireConnection;68;12;57;0
WireConnection;68;14;58;0
WireConnection;48;0;50;0
WireConnection;48;1;21;0
WireConnection;59;0;48;0
WireConnection;59;1;68;0
WireConnection;59;2;68;19
WireConnection;6;0;5;0
WireConnection;70;0;53;0
WireConnection;70;1;69;0
WireConnection;22;17;23;0
WireConnection;22;16;24;0
WireConnection;0;0;1;0
WireConnection;0;1;52;0
WireConnection;0;2;59;0
WireConnection;0;3;70;0
WireConnection;0;4;6;0
WireConnection;0;5;51;0
WireConnection;0;11;22;0
ASEEND*/
//CHKSM=530B4AD3C7AAF022D012FCC362D48AB438AD082A