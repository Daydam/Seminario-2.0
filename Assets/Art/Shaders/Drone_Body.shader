// Upgrade NOTE: upgraded instancing buffer 'Drone_Body' to new syntax.

// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Drone_Body"
{
	Properties
	{
		[HideInInspector] __dirty( "", Int ) = 1
		_Albedo("Albedo", 2D) = "white" {}
		_Metallic("Metallic", 2D) = "white" {}
		[Header(VertexCollapse)]
		_Size("Size", Float) = 64
		_Falloff("Falloff", Range( 0 , 20)) = 0
		_Roughness("Roughness", 2D) = "white" {}
		_LifeEmission("Life Emission", 2D) = "white" {}
		_Life("Life", Range( 0 , 1)) = 1
		_SkillStateColor("SkillStateColor", Color) = (0,1,1,0)
		_DefEmission("Def Emission", 2D) = "white" {}
		_CollapsePosition("Collapse Position", Vector) = (0,0,0,0)
		_IsBest("IsBest", Range( 0 , 1)) = 0
		[Header(BestPlayerRim)]
		_Color1("Color 1", Color) = (1,0.903,0.1397059,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true" "SkillStateColor"="Defensive" "VertexCollapse"="true" }
		Cull Back
		CGINCLUDE
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
			float3 worldNormal;
		};

		uniform sampler2D _Albedo;
		uniform float4 _Albedo_ST;
		uniform sampler2D _LifeEmission;
		uniform float4 _LifeEmission_ST;
		uniform sampler2D _DefEmission;
		uniform float4 _DefEmission_ST;
		uniform float4 _SkillStateColor;
		uniform float4 _Color1;
		uniform sampler2D _Metallic;
		uniform float4 _Metallic_ST;
		uniform sampler2D _Roughness;
		uniform float4 _Roughness_ST;
		uniform float _Size;
		uniform float _Falloff;

		UNITY_INSTANCING_BUFFER_START(Drone_Body)
			UNITY_DEFINE_INSTANCED_PROP(float, _Life)
#define _Life_arr Drone_Body
			UNITY_DEFINE_INSTANCED_PROP(float, _IsBest)
#define _IsBest_arr Drone_Body
			UNITY_DEFINE_INSTANCED_PROP(float3, _CollapsePosition)
#define _CollapsePosition_arr Drone_Body
		UNITY_INSTANCING_BUFFER_END(Drone_Body)

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float3 ase_worldPos = mul(unity_ObjectToWorld, v.vertex);
			float3 _CollapsePosition_Instance = UNITY_ACCESS_INSTANCED_PROP(_CollapsePosition_arr, _CollapsePosition);
			float3 lerpResult12_g3 = lerp( float3( 0,0,0 ) , ( ( ase_worldPos + ( 1.0 - _CollapsePosition_Instance ) ) + float3(-1,-1,-1) ) , ( saturate( pow( ( distance( ase_worldPos , _CollapsePosition_Instance ) / _Size ) , _Falloff ) ) - 1.0 ));
			float4 transform15_g3 = mul(unity_WorldToObject,float4( lerpResult12_g3 , 0.0 ));
			v.vertex.xyz += transform15_g3.xyz;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_Albedo = i.uv_texcoord * _Albedo_ST.xy + _Albedo_ST.zw;
			o.Albedo = tex2D( _Albedo, uv_Albedo ).rgb;
			float2 uv_LifeEmission = i.uv_texcoord * _LifeEmission_ST.xy + _LifeEmission_ST.zw;
			float _Life_Instance = UNITY_ACCESS_INSTANCED_PROP(_Life_arr, _Life);
			float4 lerpResult8 = lerp( float4(1,0,0,0) , float4(0,1,0,0) , _Life_Instance);
			float2 uv_DefEmission = i.uv_texcoord * _DefEmission_ST.xy + _DefEmission_ST.zw;
			float3 worldViewDir = normalize( UnityWorldSpaceViewDir( i.worldPos ) );
			float3 ase_worldNormal = i.worldNormal;
			float3 ase_vertexNormal = mul( unity_WorldToObject, float4( ase_worldNormal, 0 ) );
			float fresnelFinalVal5_g2 = (0.0 + 1.0*pow( 1.0 - dot( ase_vertexNormal, worldViewDir ) , 5.0));
			float _IsBest_Instance = UNITY_ACCESS_INSTANCED_PROP(_IsBest_arr, _IsBest);
			o.Emission = ( ( ( ( tex2D( _LifeEmission, uv_LifeEmission ) * lerpResult8 ) + ( tex2D( _DefEmission, uv_DefEmission ) * _SkillStateColor ) ) * 5.0 ) + ( _Color1 * saturate( fresnelFinalVal5_g2 ) * _IsBest_Instance * 3.0 ) ).rgb;
			float2 uv_Metallic = i.uv_texcoord * _Metallic_ST.xy + _Metallic_ST.zw;
			o.Metallic = tex2D( _Metallic, uv_Metallic ).r;
			float2 uv_Roughness = i.uv_texcoord * _Roughness_ST.xy + _Roughness_ST.zw;
			o.Smoothness = ( 1.0 - tex2D( _Roughness, uv_Roughness ) ).r;
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
				float4 texcoords01 : TEXCOORD4;
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
				o.texcoords01 = float4( v.texcoord.xy, v.texcoord1.xy );
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
				surfIN.uv_texcoord.xy = IN.texcoords01.xy;
				float3 worldPos = IN.worldPos;
				fixed3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = float3( IN.tSpace0.z, IN.tSpace1.z, IN.tSpace2.z );
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
Version=13101
18;91;1091;606;974.797;167.1352;1.3;True;False
Node;AmplifyShaderEditor.ColorNode;10;-2072.285,-18.75265;Float;False;Constant;_Alive;Alive;5;0;0,1,0,0;0;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;12;-2060.053,185.1039;Float;False;InstancedProperty;_Life;Life;8;0;1;0;1;0;1;FLOAT
Node;AmplifyShaderEditor.ColorNode;11;-2072.285,-183.8761;Float;False;Constant;_Dead;Dead;5;0;1,0,0,0;0;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.LerpOp;8;-1678.403,-70.34441;Float;True;3;0;COLOR;0.0;False;1;COLOR;0.0,0,0,0;False;2;FLOAT;0.0;False;1;COLOR
Node;AmplifyShaderEditor.SamplerNode;15;-2102.454,507.7737;Float;True;Property;_DefEmission;Def Emission;10;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.ColorNode;17;-2026.107,823.8917;Float;False;Property;_SkillStateColor;SkillStateColor;9;0;0,1,1,0;0;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SamplerNode;3;-1579.356,-325.3198;Float;True;Property;_LifeEmission;Life Emission;7;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;19;-1422.472,614.6387;Float;False;2;2;0;COLOR;0.0;False;1;COLOR;0.0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;13;-1226.193,-88.35986;Float;False;2;2;0;COLOR;0.0;False;1;COLOR;0.0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.RangedFloatNode;27;-704.415,234.6035;Float;False;InstancedProperty;_IsBest;IsBest;12;0;0;0;1;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;7;-1125.209,132.933;Float;False;2;2;0;COLOR;0.0;False;1;COLOR;0.0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.RangedFloatNode;20;-951.187,219.6131;Float;False;Constant;_EmissionIntensity;Emission Intensity;7;0;5;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.Vector3Node;23;-339.1753,621.0762;Float;False;InstancedProperty;_CollapsePosition;Collapse Position;11;0;0,0,0;0;4;FLOAT3;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.FunctionNode;25;-477.9168,173.4003;Float;False;BestPlayerRim;13;;2;1;0;FLOAT;0.0;False;1;COLOR
Node;AmplifyShaderEditor.WorldPosInputsNode;24;-382.7386,900.4218;Float;False;0;4;FLOAT3;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SamplerNode;5;-866.3083,516.1832;Float;True;Property;_Roughness;Roughness;6;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;21;-534.7342,72.51803;Float;False;2;2;0;COLOR;0.0;False;1;FLOAT;0.0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.SamplerNode;1;-815.3989,-613.9595;Float;True;Property;_Albedo;Albedo;0;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;26;-197.5175,59.10037;Float;False;2;2;0;COLOR;0.0;False;1;COLOR;0.0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.FunctionNode;22;-106.0418,807.0375;Float;False;VertexCollapse;2;;3;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT4
Node;AmplifyShaderEditor.SamplerNode;4;-872.4241,326.5969;Float;True;Property;_Metallic;Metallic;1;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.OneMinusNode;6;-413.7469,381.6378;Float;False;1;0;COLOR;0.0;False;1;COLOR
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;14.29734,-16.08451;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;Drone_Body;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;0;False;0;0;Opaque;0.5;True;True;0;False;Opaque;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;False;0;4;10;25;False;0.5;True;0;Zero;Zero;0;Zero;Zero;Add;Add;0;False;0;0,0,0,0;VertexOffset;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;2;SkillStateColor=Defensive;VertexCollapse=true;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;OBJECT;0.0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;8;0;11;0
WireConnection;8;1;10;0
WireConnection;8;2;12;0
WireConnection;19;0;15;0
WireConnection;19;1;17;0
WireConnection;13;0;3;0
WireConnection;13;1;8;0
WireConnection;7;0;13;0
WireConnection;7;1;19;0
WireConnection;25;0;27;0
WireConnection;21;0;7;0
WireConnection;21;1;20;0
WireConnection;26;0;21;0
WireConnection;26;1;25;0
WireConnection;22;0;23;0
WireConnection;22;1;24;0
WireConnection;6;0;5;0
WireConnection;0;0;1;0
WireConnection;0;2;26;0
WireConnection;0;3;4;0
WireConnection;0;4;6;0
WireConnection;0;11;22;0
ASEEND*/
//CHKSM=7A588136FDB13AE6C26DB9900850B34CC095063C