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
		_Normalmap("Normal map", 2D) = "bump" {}
		_AO("AO", 2D) = "white" {}
		_CooldownLerp("CooldownLerp", Range( 0 , 1)) = 0
		_CooldownColor("CooldownColor", Color) = (0,0,0,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  "SkillStateColor"="Complementary" "VertexCollapse"="true" "Wallhack"="DronePart" }
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

		uniform sampler2D _Normalmap;
		uniform sampler2D _Albedo;
		uniform float4 _CooldownColor;
		uniform sampler2D _PartEmission;
		uniform float4 _SkillStateColor;
		uniform float _CooldownLerp;
		uniform sampler2D _Metallic;
		uniform float _Metallicintensity;
		uniform sampler2D _Roughness;
		uniform sampler2D _AO;

		UNITY_INSTANCING_BUFFER_START(Drone_Part)
			UNITY_DEFINE_INSTANCED_PROP(float4, _Metallic_ST)
#define _Metallic_ST_arr Drone_Part
			UNITY_DEFINE_INSTANCED_PROP(float4, _Roughness_ST)
#define _Roughness_ST_arr Drone_Part
			UNITY_DEFINE_INSTANCED_PROP(float4, _AO_ST)
#define _AO_ST_arr Drone_Part
			UNITY_DEFINE_INSTANCED_PROP(float4, _PartEmission_ST)
#define _PartEmission_ST_arr Drone_Part
			UNITY_DEFINE_INSTANCED_PROP(float4, _Normalmap_ST)
#define _Normalmap_ST_arr Drone_Part
			UNITY_DEFINE_INSTANCED_PROP(float4, _Albedo_ST)
#define _Albedo_ST_arr Drone_Part
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
			float4 _Normalmap_ST_Instance = UNITY_ACCESS_INSTANCED_PROP(_Normalmap_ST_arr, _Normalmap_ST);
			float2 uv_Normalmap = i.uv_texcoord * _Normalmap_ST_Instance.xy + _Normalmap_ST_Instance.zw;
			o.Normal = UnpackNormal( tex2D( _Normalmap, uv_Normalmap ) );
			float4 _Albedo_ST_Instance = UNITY_ACCESS_INSTANCED_PROP(_Albedo_ST_arr, _Albedo_ST);
			float2 uv_Albedo = i.uv_texcoord * _Albedo_ST_Instance.xy + _Albedo_ST_Instance.zw;
			o.Albedo = tex2D( _Albedo, uv_Albedo ).rgb;
			float4 _PartEmission_ST_Instance = UNITY_ACCESS_INSTANCED_PROP(_PartEmission_ST_arr, _PartEmission_ST);
			float2 uv_PartEmission = i.uv_texcoord * _PartEmission_ST_Instance.xy + _PartEmission_ST_Instance.zw;
			float4 lerpResult35 = lerp( _CooldownColor , ( tex2D( _PartEmission, uv_PartEmission ) * _SkillStateColor * 10.0 ) , step( ( 1.0 - i.uv_texcoord.x ) , _CooldownLerp ));
			o.Emission = lerpResult35.rgb;
			float4 _Metallic_ST_Instance = UNITY_ACCESS_INSTANCED_PROP(_Metallic_ST_arr, _Metallic_ST);
			float2 uv_Metallic = i.uv_texcoord * _Metallic_ST_Instance.xy + _Metallic_ST_Instance.zw;
			o.Metallic = ( tex2D( _Metallic, uv_Metallic ) * _Metallicintensity ).r;
			float4 _Roughness_ST_Instance = UNITY_ACCESS_INSTANCED_PROP(_Roughness_ST_arr, _Roughness_ST);
			float2 uv_Roughness = i.uv_texcoord * _Roughness_ST_Instance.xy + _Roughness_ST_Instance.zw;
			o.Smoothness = ( 1.0 - tex2D( _Roughness, uv_Roughness ) ).r;
			float4 _AO_ST_Instance = UNITY_ACCESS_INSTANCED_PROP(_AO_ST_arr, _AO_ST);
			float2 uv_AO = i.uv_texcoord * _AO_ST_Instance.xy + _AO_ST_Instance.zw;
			o.Occlusion = tex2D( _AO, uv_AO ).r;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16700
38;222;1266;948;3370.303;941.2909;3.176749;True;False
Node;AmplifyShaderEditor.TextureCoordinatesNode;33;-1818.997,-168.5762;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;17;-2328.352,175.2657;Float;False;Property;_SkillStateColor;SkillStateColor;5;0;Create;True;0;0;False;0;0,1,1,0;0.7775417,0.3632069,1,1;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;42;-1517.236,-143.7553;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;15;-2060.179,-27.70416;Float;True;Property;_PartEmission;Part Emission;4;0;Create;True;0;0;False;0;None;afd46f43f80d708438db134a56917cba;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;20;-1945.584,487.0986;Float;False;Constant;_EmissionIntensity;Emission Intensity;5;0;Create;True;0;0;False;0;10;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;31;-1616.248,18.50972;Float;False;Property;_CooldownLerp;CooldownLerp;9;0;Create;True;0;0;False;0;0;0.565;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector3Node;22;-513.8018,456.0167;Float;False;InstancedProperty;_CollapsePosition;Collapse Position;6;0;Create;True;0;0;False;0;0,0,0;5555,5555,5555;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SamplerNode;25;-880.4645,540.1752;Float;True;Property;_AO;AO;8;0;Create;True;0;0;False;0;None;49382a8ed687cfe4497de2dd0027959d;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;5;-1238.084,535.7755;Float;True;Property;_Roughness;Roughness;3;0;Create;True;0;0;False;0;None;8d24af1acdd25464ea741b1c819ebbf6;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;19;-1453.559,186.4998;Float;False;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StepOpNode;32;-1300.172,-43.33561;Float;True;2;0;FLOAT;0;False;1;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;21;-559.3574,738.983;Float;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SamplerNode;4;-1217.053,242.1836;Float;True;Property;_Metallic;Metallic;1;0;Create;True;0;0;False;0;None;5543af887dbf80d4a8a8cdd10fa1a0c9;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;27;-1215.84,438.1693;Float;False;Property;_Metallicintensity;Metallic intensity;2;0;Create;True;0;0;False;0;1;0.75;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;36;-1221.962,-230.5375;Float;False;Property;_CooldownColor;CooldownColor;10;0;Create;True;0;0;False;0;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;28;-897.9935,249.9774;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;6;-809.3391,360.9265;Float;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;35;-913.2399,91.78936;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;1;-1054.955,-551.9569;Float;True;Property;_Albedo;Albedo;0;0;Create;True;0;0;False;0;None;c126515de2f5f0e4a9a807bb7e22c952;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FunctionNode;23;-282.6605,645.5987;Float;False;VertexCollapse;-1;;1;71a74875a0190c346ab880dd8b435206;0;2;17;FLOAT3;0,0,0;False;16;FLOAT3;0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.WireNode;26;-534.6069,423.6669;Float;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;24;-822.9826,-167.6245;Float;True;Property;_Normalmap;Normal map;7;0;Create;True;0;0;False;0;None;e7a5a64ca39284e4c8356ffe01cab78c;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;Drone_Part;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;0;4;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;1;False;-1;1;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;3;SkillStateColor=Complementary;VertexCollapse=true;Wallhack=DronePart;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;42;0;33;1
WireConnection;19;0;15;0
WireConnection;19;1;17;0
WireConnection;19;2;20;0
WireConnection;32;0;42;0
WireConnection;32;1;31;0
WireConnection;28;0;4;0
WireConnection;28;1;27;0
WireConnection;6;0;5;0
WireConnection;35;0;36;0
WireConnection;35;1;19;0
WireConnection;35;2;32;0
WireConnection;23;17;22;0
WireConnection;23;16;21;0
WireConnection;26;0;25;0
WireConnection;0;0;1;0
WireConnection;0;1;24;0
WireConnection;0;2;35;0
WireConnection;0;3;28;0
WireConnection;0;4;6;0
WireConnection;0;5;26;0
WireConnection;0;11;23;0
ASEEND*/
//CHKSM=3B2A4A6179DFEE295200333EE1ADEB3E3C811335