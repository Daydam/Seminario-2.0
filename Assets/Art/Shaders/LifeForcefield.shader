// Upgrade NOTE: upgraded instancing buffer 'LifeForcefield' to new syntax.

// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "LifeForcefield"
{
	Properties
	{
		_Speed("Speed", Range( 0 , 50)) = 0
		_TextureSample0("Texture Sample 0", 2D) = "bump" {}
		_RimPower("Rim Power", Range( 0 , 10)) = 0
		_ScanLines("Scan Lines", Range( 0 , 50)) = 0
		_LifeRamp("LifeRamp", 2D) = "white" {}
		_Life("Life", Range( 0 , 1)) = 1
		_HologramOpacity("HologramOpacity", Range( 0 , 1)) = 0
		_PatternWaves("PatternWaves", 2D) = "white" {}
		_Float6("Float 6", Range( 0 , 1)) = 0.2
		_ShieldAnimSpeed("ShieldAnimSpeed", Range( -10 , 10)) = 3
		_Color2("Color 2", Color) = (0.03137255,0.2588235,0.3176471,1)
		_HitPosition("HitPosition", Vector) = (0,0,0,0)
		_HitTime("HitTime", Float) = 0
		_HitColor("HitColor", Color) = (1,1,1,1)
		_Size("Size", Float) = 0.2
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityCG.cginc"
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
			float4 screenPos;
		};

		uniform sampler2D _LifeRamp;
		uniform float _ScanLines;
		uniform float _Speed;
		uniform sampler2D _TextureSample0;
		uniform float _RimPower;
		uniform float4 _Color2;
		uniform sampler2D _PatternWaves;
		uniform float _ShieldAnimSpeed;
		uniform float _Size;
		uniform float4 _HitColor;
		uniform sampler2D _CameraDepthTexture;
		uniform float _Float6;

		UNITY_INSTANCING_BUFFER_START(LifeForcefield)
			UNITY_DEFINE_INSTANCED_PROP(float, _Life)
#define _Life_arr LifeForcefield
			UNITY_DEFINE_INSTANCED_PROP(float, _HitTime)
#define _HitTime_arr LifeForcefield
			UNITY_DEFINE_INSTANCED_PROP(float3, _HitPosition)
#define _HitPosition_arr LifeForcefield
			UNITY_DEFINE_INSTANCED_PROP(float, _HologramOpacity)
#define _HologramOpacity_arr LifeForcefield
		UNITY_INSTANCING_BUFFER_END(LifeForcefield)

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			o.Normal = float3(0,0,1);
			float2 temp_cast_0 = (i.uv_texcoord.x).xx;
			float _Life_Instance = UNITY_ACCESS_INSTANCED_PROP(_Life_arr, _Life);
			// *** BEGIN Flipbook UV Animation vars ***
			// Total tiles of Flipbook Texture
			float fbtotaltiles74 = 5.0 * 1.0;
			// Offsets for cols and rows of Flipbook Texture
			float fbcolsoffset74 = 1.0f / 5.0;
			float fbrowsoffset74 = 1.0f / 1.0;
			// Speed of animation
			float fbspeed74 = _Time[ 1 ] * 0.0;
			// UV Tiling (col and row offset)
			float2 fbtiling74 = float2(fbcolsoffset74, fbrowsoffset74);
			// UV Offset - calculate current tile linear index, and convert it to (X * coloffset, Y * rowoffset)
			// Calculate current tile linear index
			float fbcurrenttileindex74 = round( fmod( fbspeed74 + (3.8 + (_Life_Instance - 0.0) * (0.0 - 3.8) / (1.0 - 0.0)), fbtotaltiles74) );
			fbcurrenttileindex74 += ( fbcurrenttileindex74 < 0) ? fbtotaltiles74 : 0;
			// Obtain Offset X coordinate from current tile linear index
			float fblinearindextox74 = round ( fmod ( fbcurrenttileindex74, 5.0 ) );
			// Multiply Offset X by coloffset
			float fboffsetx74 = fblinearindextox74 * fbcolsoffset74;
			// Obtain Offset Y coordinate from current tile linear index
			float fblinearindextoy74 = round( fmod( ( fbcurrenttileindex74 - fblinearindextox74 ) / 5.0, 1.0 ) );
			// Reverse Y to get tiles from Top to Bottom
			fblinearindextoy74 = (int)(1.0-1) - fblinearindextoy74;
			// Multiply Offset Y by rowoffset
			float fboffsety74 = fblinearindextoy74 * fbrowsoffset74;
			// UV Offset
			float2 fboffset74 = float2(fboffsetx74, fboffsety74);
			// Flipbook UV
			half2 fbuv74 = temp_cast_0 * fbtiling74 + fboffset74;
			// *** END Flipbook UV Animation vars ***
			float4 HologramColor4 = tex2D( _LifeRamp, fbuv74 );
			float3 ase_worldPos = i.worldPos;
			float Speed2 = _Speed;
			float temp_output_45_0 = sin( ( ( ( _ScanLines * ase_worldPos.y ) + (( 1.0 - ( Speed2 * _Time ) )).y ) * ( 2.49 * UNITY_PI ) ) );
			float4 lerpResult53 = lerp( float4(1,1,1,0) , float4(0,0,0,0) , saturate( (0.54 + (temp_output_45_0 - 0.0) * (0.42 - 0.54) / (1.0 - 0.0)) ));
			float Lines46 = temp_output_45_0;
			float4 temp_cast_1 = (Lines46).xxxx;
			float4 FinalLines55 = ( lerpResult53 - temp_cast_1 );
			float3 normalizeResult15 = normalize( i.viewDir );
			float dotResult17 = dot( UnpackNormal( tex2D( _TextureSample0, ( ( ( Speed2 / 50.0 ) * _Time ) + float4( i.uv_texcoord, 0.0 , 0.0 ) ).xy ) ) , normalizeResult15 );
			float Rim24 = pow( ( 1.0 - saturate( dotResult17 ) ) , ( 9.24 - _RimPower ) );
			float SpeedShield140 = ( _Time.y * _ShieldAnimSpeed );
			float2 appendResult102 = (float2(1 , ( 1.0 - ( SpeedShield140 / 5.0 ) )));
			float2 uv_TexCoord108 = i.uv_texcoord * float2( 1,1 ) + appendResult102;
			float4 Waves114 = tex2D( _PatternWaves, uv_TexCoord108 );
			float _HitTime_Instance = UNITY_ACCESS_INSTANCED_PROP(_HitTime_arr, _HitTime);
			float3 ase_vertex3Pos = mul( unity_WorldToObject, float4( i.worldPos , 1 ) );
			float3 _HitPosition_Instance = UNITY_ACCESS_INSTANCED_PROP(_HitPosition_arr, _HitPosition);
			float temp_output_90_0 = distance( ase_vertex3Pos , _HitPosition_Instance );
			float HitSiz91 = _Size;
			float4 HitColo101 = _HitColor;
			float4 lerpResult110 = lerp( HologramColor4 , ( HitColo101 * ( HitSiz91 / temp_output_90_0 ) ) , (0.0 + (_HitTime_Instance - 0.0) * (1.0 - 0.0) / (100.0 - 0.0)));
			float4 Hit116 = (( _HitTime_Instance > 0.0 ) ? (( temp_output_90_0 < HitSiz91 ) ? lerpResult110 :  HologramColor4 ) :  HologramColor4 );
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float screenDepth127 = LinearEyeDepth(UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture,UNITY_PROJ_COORD(ase_screenPos))));
			float distanceDepth127 = abs( ( screenDepth127 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( _Float6 ) );
			float clampResult130 = clamp( distanceDepth127 , 0.0 , 1.0 );
			float4 lerpResult133 = lerp( _Color2 , ( ( ( Rim24 + Lines46 ) * Waves114 ) * ( Hit116 * HologramColor4 ) ) , clampResult130);
			float4 Emissive135 = ( lerpResult133 * Speed2 );
			float4 temp_output_60_0 = ( HologramColor4 * ( FinalLines55 + Rim24 ) * Emissive135 );
			o.Emission = temp_output_60_0.rgb;
			float4 break64 = temp_output_60_0;
			float _HologramOpacity_Instance = UNITY_ACCESS_INSTANCED_PROP(_HologramOpacity_arr, _HologramOpacity);
			o.Alpha = ( saturate( ( break64.a + break64.g + break64.b ) ) * _HologramOpacity_Instance );
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard alpha:fade keepalpha fullforwardshadows 

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
				float4 screenPos : TEXCOORD2;
				float4 tSpace0 : TEXCOORD3;
				float4 tSpace1 : TEXCOORD4;
				float4 tSpace2 : TEXCOORD5;
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
				float3 worldPos = float3( IN.tSpace0.w, IN.tSpace1.w, IN.tSpace2.w );
				fixed3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.viewDir = IN.tSpace0.xyz * worldViewDir.x + IN.tSpace1.xyz * worldViewDir.y + IN.tSpace2.xyz * worldViewDir.z;
				surfIN.worldPos = worldPos;
				surfIN.internalSurfaceTtoW0 = IN.tSpace0.xyz;
				surfIN.internalSurfaceTtoW1 = IN.tSpace1.xyz;
				surfIN.internalSurfaceTtoW2 = IN.tSpace2.xyz;
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
7;29;1352;692;5074.414;-459.334;2.867827;True;False
Node;AmplifyShaderEditor.CommentaryNode;6;-846.0991,-330.0001;Float;False;640.8001;306.0001;Speed;2;2;1;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;1;-775.199,-266.7002;Float;False;Property;_Speed;Speed;0;0;Create;True;0;0;False;0;0;1;0;50;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;25;-1738.001,102.8999;Float;False;2763.2;1054.499;Rim;16;8;9;12;11;7;10;13;17;19;21;14;15;18;23;22;24;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;2;-456.8997,-278.4001;Float;False;Speed;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;136;-4467.824,-389.1954;Float;False;830.728;358.1541;Comment;4;140;139;138;137;Animation Speed;1,1,1,1;0;0
Node;AmplifyShaderEditor.GetLocalVarNode;7;-1668.999,152.8999;Float;False;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.TimeNode;139;-4344.232,-339.1964;Float;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;137;-4417.824,-146.0414;Float;False;Property;_ShieldAnimSpeed;ShieldAnimSpeed;9;0;Create;True;0;0;False;0;3;0;-10;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;76;-2977.856,-593.7567;Float;False;2015.557;519.7686;Hologram Color;6;4;71;72;73;74;75;;1,1,1,1;0;0
Node;AmplifyShaderEditor.TimeNode;8;-1684.901,449.6998;Float;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;83;-4163.063,960.28;Float;False;1858.993;1001.87;Comment;22;116;115;113;112;110;109;107;105;104;103;101;100;98;97;95;93;92;91;90;89;86;84;Impact Effect;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;56;-1863.802,1250.198;Float;False;3593.5;1746.098;ScanLines;23;54;55;29;30;33;36;32;49;47;46;41;52;53;27;42;39;31;26;34;35;28;45;51;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;10;-1439.401,216.2995;Float;False;2;0;FLOAT;0;False;1;FLOAT;50;False;1;FLOAT;0
Node;AmplifyShaderEditor.TimeNode;28;-1813.802,2567.297;Float;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;9;-1227.9,255.9996;Float;False;2;2;0;FLOAT;0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.Vector3Node;84;-4115.063,1552.279;Float;False;InstancedProperty;_HitPosition;HitPosition;11;0;Create;True;0;0;False;0;0,0,0;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.GetLocalVarNode;26;-1785.002,2338.497;Float;False;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;85;-4179.063,256.2804;Float;False;1608.543;477.595;Comment;10;114;111;108;106;102;99;96;94;88;87;Shield Wave Effect;1,1,1,1;0;0
Node;AmplifyShaderEditor.PosVertexDataNode;86;-4115.063,1360.28;Float;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;11;-1319.299,400.0991;Float;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;138;-4101.696,-215.7825;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;71;-2927.856,-205.8531;Float;False;InstancedProperty;_Life;Life;5;0;Create;True;0;0;False;0;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;12;-996.4005,297.2994;Float;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT2;0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;140;-3885.096,-205.8556;Float;False;SpeedShield;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;89;-4035.062,1008.28;Float;False;Property;_Size;Size;14;0;Create;True;0;0;False;0;0.2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;27;-1519.302,2420.997;Float;False;2;2;0;FLOAT;0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.GetLocalVarNode;88;-4131.063,528.2805;Float;False;140;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;73;-2533.988,-275.9882;Float;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;3.8;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;72;-2425.443,-543.7567;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;14;-989.6014,535.7992;Float;False;Tangent;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;87;-4115.063,623.2393;Float;False;Constant;_SpeedDividend;SpeedDividend;7;0;Create;True;0;0;False;0;5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DistanceOpNode;90;-3875.062,1456.28;Float;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;94;-3923.062,544.2804;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;93;-3699.062,1696.279;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;13;-757.0017,255.4992;Float;True;Property;_TextureSample0;Texture Sample 0;1;0;Create;True;0;0;False;0;None;a268ab862991c4743a9281c69bb2c36a;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.NormalizeNode;15;-719.5015,477.299;Float;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ColorNode;92;-4083.062,1104.28;Float;False;Property;_HitColor;HitColor;13;0;Create;True;0;0;False;0;1,1,1,1;0,0,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;32;-1240.002,2455.197;Float;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.WorldPosInputsNode;30;-1496.603,2172.495;Float;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.TFHCFlipBookUVAnimation;74;-2123.415,-369.8383;Float;False;0;0;6;0;FLOAT2;0,0;False;1;FLOAT;5;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;5;FLOAT;0;False;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.GetLocalVarNode;95;-3651.062,1616.279;Float;False;91;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;91;-3827.062,1008.28;Float;False;HitSiz;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;29;-1510.003,1947.096;Float;False;Property;_ScanLines;Scan Lines;3;0;Create;True;0;0;False;0;0;6;0;50;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;100;-3443.062,1728.279;Float;False;101;0;1;COLOR;0
Node;AmplifyShaderEditor.ComponentMaskNode;33;-1000.001,2459.398;Float;False;False;True;False;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;98;-3635.062,1488.28;Float;False;InstancedProperty;_HitTime;HitTime;12;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;31;-1017.701,2151.197;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;99;-3779.062,384.2805;Float;False;Constant;_WavesOffset;WavesOffset;7;0;Create;True;0;0;False;0;1,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SamplerNode;75;-1784.11,-394.4733;Float;True;Property;_LifeRamp;LifeRamp;4;0;Create;True;0;0;False;0;4d64ea565d9a404479a62e2bdadb63a6;4d64ea565d9a404479a62e2bdadb63a6;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;101;-3795.062,1120.28;Float;False;HitColo;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;97;-3459.062,1824.279;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;96;-3795.062,544.2804;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DotProductOpNode;17;-409.801,355.5992;Float;True;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;104;-3219.061,1744.279;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.PiNode;36;-585.9016,2621.797;Float;False;1;0;FLOAT;2.49;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;19;-118.6007,532.3994;Float;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;18;-570.9009,893.0995;Float;False;Property;_RimPower;Rim Power;2;0;Create;True;0;0;False;0;0;8;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;34;-641.5011,2335.497;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;106;-3587.062,304.2805;Float;False;Constant;_WavesTiling;WavesTiling;7;0;Create;True;0;0;False;0;1,1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TFHCRemapNode;103;-3411.062,1552.279;Float;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;100;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;4;-1212.299,-258.9001;Float;False;HologramColor;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.DynamicAppendNode;102;-3587.062,432.2805;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;105;-3251.061,1280.28;Float;False;4;0;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;21;156.1984,609.6993;Float;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;110;-3075.062,1552.279;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;108;-3395.062,416.2805;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WireNode;109;-3347.062,1344.28;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;35;-356.5016,2403.897;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;23;-145.2013,903.0993;Float;False;2;0;FLOAT;9.24;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;107;-3283.061,1408.28;Float;False;91;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;22;466.5985,809.0995;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;113;-3379.062,1248.28;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;45;-36.90245,2441.197;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCCompareLower;112;-2883.062,1424.28;Float;False;4;0;FLOAT;0;False;1;FLOAT;0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;117;-4150.808,2223.723;Float;False;1652.997;650.1895;Mix of Pattern, Wave, Rim , Impact and adding intersection highlight;18;135;134;133;132;131;130;129;128;127;126;125;124;123;122;121;120;119;118;Shield Mix for Emission;1,1,1,1;0;0
Node;AmplifyShaderEditor.SamplerNode;111;-3155.061,400.2805;Float;True;Property;_PatternWaves;PatternWaves;7;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TFHCCompareGreater;115;-2739.062,1184.28;Float;False;4;0;FLOAT;0;False;1;FLOAT;0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;46;486.7993,1963.997;Float;True;Lines;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;114;-2819.062,400.2805;Float;False;Waves;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;120;-4081.932,2581.612;Float;False;114;0;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;119;-4090.873,2412.469;Float;False;24;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;24;782.1989,835.0997;Float;False;Rim;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;118;-4100.808,2503.325;Float;False;46;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;123;-3850.446,2589.289;Float;False;116;0;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;125;-3929.076,2672.214;Float;False;4;0;1;COLOR;0
Node;AmplifyShaderEditor.TFHCRemapNode;47;213.4987,2609.097;Float;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0.54;False;4;FLOAT;0.42;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;116;-2515.567,1205.652;Float;False;Hit;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;124;-3937.596,2752.591;Float;False;Property;_Float6;Float 6;8;0;Create;True;0;0;False;0;0.2;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;121;-3883.513,2569.792;Float;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;122;-3863.483,2452.464;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DepthFade;127;-3571.458,2755.484;Float;False;True;1;0;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;128;-3687.349,2476.098;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;52;533.6988,2438.998;Float;False;Constant;_Tint02;Tint02;5;0;Create;True;0;0;False;0;0,0,0,0;0,0,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;126;-3606.32,2656.594;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;49;608.5994,2744.295;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;51;532.5988,2259.397;Float;False;Constant;_Tint01;Tint01;6;0;Create;True;0;0;False;0;1,1,1,0;0,0,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;129;-3467.912,2273.723;Float;False;Property;_Color2;Color 2;10;0;Create;True;0;0;False;0;0.03137255,0.2588235,0.3176471,1;0,0,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;131;-3445.003,2470.917;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ClampOpNode;130;-3355.306,2717.912;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;53;919.3983,2446.597;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;132;-3151.113,2622.226;Float;False;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;54;1183.199,2427.497;Float;True;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;133;-3166.712,2481.822;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;58;1235.193,286.3965;Float;False;24;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;55;1486.698,2425.396;Float;False;FinalLines;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;57;1225.194,79.39676;Float;False;55;0;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;134;-2924.115,2511.825;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;135;-2740.814,2486.823;Float;False;Emissive;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;59;1531.392,163.5967;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;141;1584.275,299.305;Float;False;135;0;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;61;1499.793,-79.70324;Float;False;4;0;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;60;1712.892,52.09675;Float;False;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.BreakToComponentsNode;64;1828.69,239.9954;Float;False;COLOR;1;0;COLOR;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.SimpleAddOpNode;65;2075.091,213.5954;Float;True;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;67;2332.49,277.3954;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;62;2081.714,460.3214;Float;False;InstancedProperty;_HologramOpacity;HologramOpacity;6;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;42;214.898,2049.796;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TimeNode;39;-835.8022,1759.899;Float;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.NoiseGeneratorNode;41;-310.0018,1926.596;Float;False;Simplex2D;1;0;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;70;-507.9991,1798.693;Float;False;2;0;FLOAT;0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;77;2352.974,414.3185;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;68;-680.5004,1636.593;Float;False;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;2498.2,-79.89988;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;LifeForcefield;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;0;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;0;4;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;0;0;False;0;0;0;False;-1;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;2;0;1;0
WireConnection;10;0;7;0
WireConnection;9;0;10;0
WireConnection;9;1;8;0
WireConnection;138;0;139;2
WireConnection;138;1;137;0
WireConnection;12;0;9;0
WireConnection;12;1;11;0
WireConnection;140;0;138;0
WireConnection;27;0;26;0
WireConnection;27;1;28;0
WireConnection;73;0;71;0
WireConnection;90;0;86;0
WireConnection;90;1;84;0
WireConnection;94;0;88;0
WireConnection;94;1;87;0
WireConnection;93;0;90;0
WireConnection;13;1;12;0
WireConnection;15;0;14;0
WireConnection;32;0;27;0
WireConnection;74;0;72;1
WireConnection;74;4;73;0
WireConnection;91;0;89;0
WireConnection;33;0;32;0
WireConnection;31;0;29;0
WireConnection;31;1;30;2
WireConnection;75;1;74;0
WireConnection;101;0;92;0
WireConnection;97;0;95;0
WireConnection;97;1;93;0
WireConnection;96;0;94;0
WireConnection;17;0;13;0
WireConnection;17;1;15;0
WireConnection;104;0;100;0
WireConnection;104;1;97;0
WireConnection;19;0;17;0
WireConnection;34;0;31;0
WireConnection;34;1;33;0
WireConnection;103;0;98;0
WireConnection;4;0;75;0
WireConnection;102;0;99;1
WireConnection;102;1;96;0
WireConnection;21;0;19;0
WireConnection;110;0;105;0
WireConnection;110;1;104;0
WireConnection;110;2;103;0
WireConnection;108;0;106;0
WireConnection;108;1;102;0
WireConnection;109;0;90;0
WireConnection;35;0;34;0
WireConnection;35;1;36;0
WireConnection;23;1;18;0
WireConnection;22;0;21;0
WireConnection;22;1;23;0
WireConnection;113;0;98;0
WireConnection;45;0;35;0
WireConnection;112;0;109;0
WireConnection;112;1;107;0
WireConnection;112;2;110;0
WireConnection;112;3;105;0
WireConnection;111;1;108;0
WireConnection;115;0;113;0
WireConnection;115;2;112;0
WireConnection;115;3;105;0
WireConnection;46;0;45;0
WireConnection;114;0;111;0
WireConnection;24;0;22;0
WireConnection;47;0;45;0
WireConnection;116;0;115;0
WireConnection;121;0;120;0
WireConnection;122;0;119;0
WireConnection;122;1;118;0
WireConnection;127;0;124;0
WireConnection;128;0;122;0
WireConnection;128;1;121;0
WireConnection;126;0;123;0
WireConnection;126;1;125;0
WireConnection;49;0;47;0
WireConnection;131;0;128;0
WireConnection;131;1;126;0
WireConnection;130;0;127;0
WireConnection;53;0;51;0
WireConnection;53;1;52;0
WireConnection;53;2;49;0
WireConnection;54;0;53;0
WireConnection;54;1;46;0
WireConnection;133;0;129;0
WireConnection;133;1;131;0
WireConnection;133;2;130;0
WireConnection;55;0;54;0
WireConnection;134;0;133;0
WireConnection;134;1;132;0
WireConnection;135;0;134;0
WireConnection;59;0;57;0
WireConnection;59;1;58;0
WireConnection;60;0;61;0
WireConnection;60;1;59;0
WireConnection;60;2;141;0
WireConnection;64;0;60;0
WireConnection;65;0;64;3
WireConnection;65;1;64;1
WireConnection;65;2;64;2
WireConnection;67;0;65;0
WireConnection;42;0;41;0
WireConnection;42;1;45;0
WireConnection;41;0;70;0
WireConnection;70;0;68;0
WireConnection;70;1;39;0
WireConnection;77;0;67;0
WireConnection;77;1;62;0
WireConnection;0;2;60;0
WireConnection;0;9;77;0
ASEEND*/
//CHKSM=BFCF2173A9B0DBE92B83E1FB34A3DB641C2AEE61