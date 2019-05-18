// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Custom shaders/Height Sand"
{
	Properties
	{
		_EdgeLength ( "Edge length", Range( 2, 50 ) ) = 25
		_TilingAlbedoNormalHeight("Tiling (Albedo), (Normal), (Height)", Float) = 0
		_Albedo("Albedo", 2D) = "white" {}
		_Normal("Normal", 2D) = "white" {}
		_Height("Height", 2D) = "white" {}
		_Metallic("Metallic", Float) = 0
		_Smoothness("Smoothness", Float) = 0
		_ScaleParallax("Scale Parallax", Range( 0 , 0.1)) = 0.05
		_ScaleHeight("Scale Height", Float) = 0
		_RT_Drones("RT_Drones", 2D) = "white" {}
		_CavityIntensity("Cavity Intensity", Range( -1 , 0)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGINCLUDE
		#include "Tessellation.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 4.6
		#ifdef UNITY_PASS_SHADOWCASTER
			#undef INTERNAL_DATA
			#undef WorldReflectionVector
			#undef WorldNormalVector
			#define INTERNAL_DATA half3 internalSurfaceTtoW0; half3 internalSurfaceTtoW1; half3 internalSurfaceTtoW2;
			#define WorldReflectionVector(data,normal) reflect (data.worldRefl, half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal)))
			#define WorldNormalVector(data,normal) half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal))
		#endif
		struct Input
		{
			float2 uv_texcoord;
			float3 viewDir;
			INTERNAL_DATA
		};

		uniform sampler2D _Height;
		uniform float _TilingAlbedoNormalHeight;
		uniform float _ScaleHeight;
		uniform float _CavityIntensity;
		uniform sampler2D _RT_Drones;
		uniform sampler2D _Normal;
		uniform float _ScaleParallax;
		uniform sampler2D _Albedo;
		uniform float _Metallic;
		uniform float _Smoothness;
		uniform float _EdgeLength;

		float4 tessFunction( appdata_full v0, appdata_full v1, appdata_full v2 )
		{
			return UnityEdgeLengthBasedTess (v0.vertex, v1.vertex, v2.vertex, _EdgeLength);
		}

		void vertexDataFunc( inout appdata_full v )
		{
			float2 temp_cast_0 = (_TilingAlbedoNormalHeight).xx;
			float2 uv_TexCoord26 = v.texcoord.xy * temp_cast_0;
			float4 tex2DNode27 = tex2Dlod( _Height, float4( uv_TexCoord26, 0, 0.0) );
			float3 _YDeformation = float3(0,1,0);
			float3 lerpResult59 = lerp( ( tex2DNode27.r * _YDeformation * _ScaleHeight ) , ( _YDeformation * _CavityIntensity ) , tex2Dlod( _RT_Drones, float4( ( 1.0 - v.texcoord.xy ), 0, 0.0) ).r);
			v.vertex.xyz += lerpResult59;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 temp_cast_0 = (_TilingAlbedoNormalHeight).xx;
			float2 uv_TexCoord26 = i.uv_texcoord * temp_cast_0;
			float4 tex2DNode27 = tex2D( _Height, uv_TexCoord26 );
			float2 Offset25 = ( ( tex2DNode27.r - 1 ) * i.viewDir.xy * _ScaleParallax ) + uv_TexCoord26;
			float2 Offset30 = ( ( tex2D( _Height, Offset25 ).r - 1 ) * i.viewDir.xy * _ScaleParallax ) + Offset25;
			float2 Offset47 = ( ( tex2D( _Height, Offset30 ).r - 1 ) * i.viewDir.xy * _ScaleParallax ) + Offset30;
			float2 Offset49 = ( ( tex2D( _Height, Offset47 ).r - 1 ) * i.viewDir.xy * _ScaleParallax ) + Offset47;
			o.Normal = UnpackNormal( tex2D( _Normal, Offset49 ) );
			o.Albedo = tex2D( _Albedo, Offset49 ).rgb;
			o.Metallic = _Metallic;
			o.Smoothness = _Smoothness;
			o.Alpha = 1;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard keepalpha fullforwardshadows vertex:vertexDataFunc tessellate:tessFunction 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 4.6
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
				vertexDataFunc( v );
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				half3 worldTangent = UnityObjectToWorldDir( v.tangent.xyz );
				half tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				half3 worldBinormal = cross( worldNormal, worldTangent ) * tangentSign;
				o.tSpace0 = float4( worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x );
				o.tSpace1 = float4( worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y );
				o.tSpace2 = float4( worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
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
				float3 worldPos = float3( IN.tSpace0.w, IN.tSpace1.w, IN.tSpace2.w );
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.viewDir = IN.tSpace0.xyz * worldViewDir.x + IN.tSpace1.xyz * worldViewDir.y + IN.tSpace2.xyz * worldViewDir.z;
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
Version=16100
36;612;1266;954;1699.027;222.9283;1.9;True;False
Node;AmplifyShaderEditor.CommentaryNode;51;-3637.976,-127.9954;Float;False;2231.25;761.855;;12;4;26;29;27;28;25;31;30;48;49;50;47;Parallax;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;4;-3599.976,24.23345;Float;False;Property;_TilingAlbedoNormalHeight;Tiling (Albedo), (Normal), (Height);5;0;Create;True;0;0;False;0;0;10;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;26;-3239.205,-77.99538;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;28;-3197.991,399.9468;Float;False;Tangent;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SamplerNode;27;-3273.391,69.74677;Float;True;Property;_Height;Height;8;0;Create;True;0;0;False;0;None;2dafa32145f0d6340993725c1101ce17;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;29;-3291.458,282.919;Float;False;Property;_ScaleParallax;Scale Parallax;11;0;Create;True;0;0;False;0;0.05;0.1;0;0.1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ParallaxMappingNode;25;-3006.187,-37.01614;Float;False;Normal;4;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;31;-2787.905,134.1347;Float;True;Property;;;8;0;Create;True;0;0;False;0;None;2dafa32145f0d6340993725c1101ce17;True;0;False;white;Auto;False;Instance;27;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ParallaxMappingNode;30;-2432.684,136.5937;Float;False;Normal;4;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;48;-2433.243,347.9778;Float;True;Property;;;8;0;Create;True;0;0;False;0;None;2dafa32145f0d6340993725c1101ce17;True;0;False;white;Auto;False;Instance;27;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;67;-1343.038,931.8937;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ParallaxMappingNode;47;-2100.313,193.5681;Float;False;Normal;4;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.OneMinusNode;68;-1061.33,933.3927;Float;False;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;50;-2009.351,403.8596;Float;True;Property;;;8;0;Create;True;0;0;False;0;None;2dafa32145f0d6340993725c1101ce17;True;0;False;white;Auto;False;Instance;27;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;10;-1028.702,713.6478;Float;False;Property;_ScaleHeight;Scale Height;12;0;Create;True;0;0;False;0;0;0.18;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;73;-1080.739,804.2183;Float;False;Property;_CavityIntensity;Cavity Intensity;14;0;Create;True;0;0;False;0;0;-0.17;-1;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector3Node;9;-1051.389,562.3911;Float;False;Constant;_YDeformation;Y = Deformation;5;0;Create;True;0;0;False;0;0,1,0;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.ParallaxMappingNode;49;-1665.725,296.28;Float;False;Normal;4;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;71;-736.5389,711.1561;Float;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;8;-698.3357,510.0584;Float;False;3;3;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;66;-889.0228,904.6118;Float;True;Property;_RT_Drones;RT_Drones;13;0;Create;True;0;0;False;0;None;4fa09269bf77113458c9d4ea221c50e3;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;59;-468.2135,566.3941;Float;True;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;45;-215.7512,244.1758;Float;False;Property;_Smoothness;Smoothness;10;0;Create;True;0;0;False;0;0;0.2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;1;-907.9794,42.12481;Float;True;Property;_Albedo;Albedo;6;0;Create;True;0;0;False;0;None;2dafa32145f0d6340993725c1101ce17;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;38;-905.7017,265.2219;Float;True;Property;_Normal;Normal;7;0;Create;True;0;0;False;0;None;2dafa32145f0d6340993725c1101ce17;True;0;False;white;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;46;-203.7512,154.1758;Float;False;Property;_Metallic;Metallic;9;0;Create;True;0;0;False;0;0;0.09;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;6;Float;ASEMaterialInspector;0;0;Standard;Custom shaders/Height Sand;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;True;2;25;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;0;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;26;0;4;0
WireConnection;27;1;26;0
WireConnection;25;0;26;0
WireConnection;25;1;27;1
WireConnection;25;2;29;0
WireConnection;25;3;28;0
WireConnection;31;1;25;0
WireConnection;30;0;25;0
WireConnection;30;1;31;1
WireConnection;30;2;29;0
WireConnection;30;3;28;0
WireConnection;48;1;30;0
WireConnection;47;0;30;0
WireConnection;47;1;48;1
WireConnection;47;2;29;0
WireConnection;47;3;28;0
WireConnection;68;0;67;0
WireConnection;50;1;47;0
WireConnection;49;0;47;0
WireConnection;49;1;50;1
WireConnection;49;2;29;0
WireConnection;49;3;28;0
WireConnection;71;0;9;0
WireConnection;71;1;73;0
WireConnection;8;0;27;1
WireConnection;8;1;9;0
WireConnection;8;2;10;0
WireConnection;66;1;68;0
WireConnection;59;0;8;0
WireConnection;59;1;71;0
WireConnection;59;2;66;1
WireConnection;1;1;49;0
WireConnection;38;1;49;0
WireConnection;0;0;1;0
WireConnection;0;1;38;0
WireConnection;0;3;46;0
WireConnection;0;4;45;0
WireConnection;0;11;59;0
ASEEND*/
//CHKSM=A583776761A1EC5472984CBDB1CE96EC24003340