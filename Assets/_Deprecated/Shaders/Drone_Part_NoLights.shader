// Upgrade NOTE: upgraded instancing buffer 'Drone_Part_NoLights' to new syntax.

// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Drone_Part_NoLights"
{
	Properties
	{
		_Albedo("Albedo", 2D) = "white" {}
		_Metallic("Metallic", 2D) = "white" {}
		_Roughness("Roughness", 2D) = "bump" {}
		_CollapsePosition("Collapse Position", Vector) = (0,0,0,0)
		_Normal("Normal", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGPROGRAM
		#pragma target 3.0
		#pragma multi_compile_instancing
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows vertex:vertexDataFunc 
		struct Input
		{
			float3 worldPos;
			float2 uv_texcoord;
		};

		uniform sampler2D _Normal;
		uniform sampler2D _Albedo;
		uniform sampler2D _Metallic;
		uniform sampler2D _Roughness;

		UNITY_INSTANCING_BUFFER_START(Drone_Part_NoLights)
			UNITY_DEFINE_INSTANCED_PROP(float4, _Metallic_ST)
#define _Metallic_ST_arr Drone_Part_NoLights
			UNITY_DEFINE_INSTANCED_PROP(float4, _Roughness_ST)
#define _Roughness_ST_arr Drone_Part_NoLights
			UNITY_DEFINE_INSTANCED_PROP(float4, _Albedo_ST)
#define _Albedo_ST_arr Drone_Part_NoLights
			UNITY_DEFINE_INSTANCED_PROP(float4, _Normal_ST)
#define _Normal_ST_arr Drone_Part_NoLights
			UNITY_DEFINE_INSTANCED_PROP(float3, _CollapsePosition)
#define _CollapsePosition_arr Drone_Part_NoLights
		UNITY_INSTANCING_BUFFER_END(Drone_Part_NoLights)

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
			float4 _Normal_ST_Instance = UNITY_ACCESS_INSTANCED_PROP(_Normal_ST_arr, _Normal_ST);
			float2 uv_Normal = i.uv_texcoord * _Normal_ST_Instance.xy + _Normal_ST_Instance.zw;
			o.Normal = tex2D( _Normal, uv_Normal ).rgb;
			float4 _Albedo_ST_Instance = UNITY_ACCESS_INSTANCED_PROP(_Albedo_ST_arr, _Albedo_ST);
			float2 uv_Albedo = i.uv_texcoord * _Albedo_ST_Instance.xy + _Albedo_ST_Instance.zw;
			o.Albedo = tex2D( _Albedo, uv_Albedo ).rgb;
			float4 _Metallic_ST_Instance = UNITY_ACCESS_INSTANCED_PROP(_Metallic_ST_arr, _Metallic_ST);
			float2 uv_Metallic = i.uv_texcoord * _Metallic_ST_Instance.xy + _Metallic_ST_Instance.zw;
			o.Metallic = tex2D( _Metallic, uv_Metallic ).r;
			float4 _Roughness_ST_Instance = UNITY_ACCESS_INSTANCED_PROP(_Roughness_ST_arr, _Roughness_ST);
			float2 uv_Roughness = i.uv_texcoord * _Roughness_ST_Instance.xy + _Roughness_ST_Instance.zw;
			o.Smoothness = ( 1.0 - UnpackNormal( tex2D( _Roughness, uv_Roughness ) ) ).x;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16700
7;115;1352;584;1074.669;138.4679;1.296109;True;False
Node;AmplifyShaderEditor.SamplerNode;5;-866.3083,516.1832;Float;True;Property;_Roughness;Roughness;2;0;Create;True;0;0;False;0;None;7ea049794ffe9ee48bd03e32f4ecfa7d;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector3Node;8;-263.7653,608.8922;Float;False;InstancedProperty;_CollapsePosition;Collapse Position;3;0;Create;True;0;0;False;0;0,0,0;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.WorldPosInputsNode;7;-307.3286,888.2378;Float;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SamplerNode;1;-1054.955,-551.9569;Float;True;Property;_Albedo;Albedo;0;0;Create;True;0;0;False;0;None;c096f0d35d145a4468cc872a84559dc1;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;10;-473.2739,-6.264722;Float;True;Property;_Normal;Normal;4;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;4;-872.4241,326.5969;Float;True;Property;_Metallic;Metallic;1;0;Create;True;0;0;False;0;None;1da6639249dfe214897575d317411dad;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FunctionNode;9;-30.6317,794.8535;Float;False;VertexCollapse;-1;;1;71a74875a0190c346ab880dd8b435206;0;2;17;FLOAT3;0,0,0;False;16;FLOAT3;0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.OneMinusNode;6;-413.7469,381.6378;Float;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;120.5382,16.84942;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;Drone_Part_NoLights;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;0;4;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;1;False;-1;1;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;9;17;8;0
WireConnection;9;16;7;0
WireConnection;6;0;5;0
WireConnection;0;0;1;0
WireConnection;0;1;10;0
WireConnection;0;3;4;0
WireConnection;0;4;6;0
WireConnection;0;11;9;0
ASEEND*/
//CHKSM=7BFB2F758FA4ECB90ADA1DD2847FCEA750AD66B1