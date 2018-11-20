// Upgrade NOTE: upgraded instancing buffer 'Drone_Part' to new syntax.

// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Drone_Part"
{
	Properties
	{
		_Albedo("Albedo", 2D) = "white" {}
		_Metallic("Metallic", 2D) = "white" {}
		_Metallicintensity("Metallic intensity", Range( 0 , 1)) = 1
		_Roughness("Roughness", 2D) = "white" {}
		_PartEmission("Part Emission", 2D) = "white" {}
		_SkillStateColor("SkillStateColor", Color) = (0,1,1,0)
		_CollapsePosition("Collapse Position", Vector) = (0,0,0,0)
		_Normalmap("Normal map", 2D) = "white" {}
		_AO("AO", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  "SkillStateColor"="Complementary" "VertexCollapse"="true" }
		Cull Back
		CGPROGRAM
		#pragma target 3.0
		#pragma multi_compile_instancing
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows vertex:vertexDataFunc 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _Normalmap;
		uniform float4 _Normalmap_ST;
		uniform sampler2D _Albedo;
		uniform float4 _Albedo_ST;
		uniform sampler2D _PartEmission;
		uniform float4 _PartEmission_ST;
		uniform float4 _SkillStateColor;
		uniform sampler2D _Metallic;
		uniform float4 _Metallic_ST;
		uniform float _Metallicintensity;
		uniform sampler2D _Roughness;
		uniform float4 _Roughness_ST;
		uniform sampler2D _AO;
		uniform float4 _AO_ST;

		UNITY_INSTANCING_BUFFER_START(Drone_Part)
			UNITY_DEFINE_INSTANCED_PROP(float3, _CollapsePosition)
#define _CollapsePosition_arr Drone_Part
		UNITY_INSTANCING_BUFFER_END(Drone_Part)

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
			float2 uv_Normalmap = i.uv_texcoord * _Normalmap_ST.xy + _Normalmap_ST.zw;
			o.Normal = UnpackNormal( tex2D( _Normalmap, uv_Normalmap ) );
			float2 uv_Albedo = i.uv_texcoord * _Albedo_ST.xy + _Albedo_ST.zw;
			o.Albedo = tex2D( _Albedo, uv_Albedo ).rgb;
			float2 uv_PartEmission = i.uv_texcoord * _PartEmission_ST.xy + _PartEmission_ST.zw;
			o.Emission = ( tex2D( _PartEmission, uv_PartEmission ) * _SkillStateColor * 10.0 ).rgb;
			float2 uv_Metallic = i.uv_texcoord * _Metallic_ST.xy + _Metallic_ST.zw;
			o.Metallic = ( tex2D( _Metallic, uv_Metallic ) * _Metallicintensity ).r;
			float2 uv_Roughness = i.uv_texcoord * _Roughness_ST.xy + _Roughness_ST.zw;
			o.Smoothness = ( 1.0 - tex2D( _Roughness, uv_Roughness ) ).r;
			float2 uv_AO = i.uv_texcoord * _AO_ST.xy + _AO_ST.zw;
			o.Occlusion = tex2D( _AO, uv_AO ).r;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=15301
94;72;1091;606;2091.38;339.187;1.831991;True;False
Node;AmplifyShaderEditor.WorldPosInputsNode;21;-559.3574,738.983;Float;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SamplerNode;25;-880.4645,540.1752;Float;True;Property;_AO;AO;8;0;Create;True;0;0;False;0;None;a205a864f03471b43893d1f2e360facf;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;20;-1957.696,393.2313;Float;False;Constant;_EmissionIntensity;Emission Intensity;5;0;Create;True;0;0;False;0;10;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;15;-2060.179,-27.70416;Float;True;Property;_PartEmission;Part Emission;4;0;Create;True;0;0;False;0;None;e9593224ee667eb43ade74dadeaa0ca2;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;17;-1989.217,178.2936;Float;False;Property;_SkillStateColor;SkillStateColor;5;0;Create;True;0;0;False;0;0,1,1,0;0,1,1,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector3Node;22;-513.8018,456.0167;Float;False;InstancedProperty;_CollapsePosition;Collapse Position;6;0;Create;True;0;0;False;0;0,0,0;5555,5555,5555;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;27;-1215.84,438.1693;Float;False;Property;_Metallicintensity;Metallic intensity;2;0;Create;True;0;0;False;0;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;5;-1238.084,535.7755;Float;True;Property;_Roughness;Roughness;3;0;Create;True;0;0;False;0;None;2c3a89fa795db9f429af9596040f924e;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;4;-1217.053,242.1836;Float;True;Property;_Metallic;Metallic;1;0;Create;True;0;0;False;0;None;9a51f865bc63a7c43b280dcc91472f93;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;6;-809.3391,360.9265;Float;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;19;-1388.652,166.5283;Float;False;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;23;-282.6605,645.5987;Float;False;VertexCollapse;-1;;1;71a74875a0190c346ab880dd8b435206;0;2;17;FLOAT3;0,0,0;False;16;FLOAT3;0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.WireNode;26;-534.6069,423.6669;Float;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;24;-822.9826,-167.6245;Float;True;Property;_Normalmap;Normal map;7;0;Create;True;0;0;False;0;None;44510ab34caa23c4a9d03f23cc3e2e2a;True;0;False;white;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;28;-897.9935,249.9774;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;1;-1054.955,-551.9569;Float;True;Property;_Albedo;Albedo;0;0;Create;True;0;0;False;0;None;8409beb3bb55f9746819b110ce5e598d;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;Drone_Part;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;0;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;0;4;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;2;SkillStateColor=Complementary;VertexCollapse=true;0;False;0;0;0;False;-1;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;6;0;5;0
WireConnection;19;0;15;0
WireConnection;19;1;17;0
WireConnection;19;2;20;0
WireConnection;23;17;22;0
WireConnection;23;16;21;0
WireConnection;26;0;25;0
WireConnection;28;0;4;0
WireConnection;28;1;27;0
WireConnection;0;0;1;0
WireConnection;0;1;24;0
WireConnection;0;2;19;0
WireConnection;0;3;28;0
WireConnection;0;4;6;0
WireConnection;0;5;26;0
WireConnection;0;11;23;0
ASEEND*/
//CHKSM=5A142F0133D88FCA0A7230E8BAD6FE475E520B81