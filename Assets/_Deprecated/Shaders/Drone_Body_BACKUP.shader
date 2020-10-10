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
		_Roughness("Roughness", 2D) = "white" {}
		_LifeEmission("Life Emission", 2D) = "white" {}
		_Life("Life", Range( 0 , 1)) = 1
		_DefEmission("Def Emission", 2D) = "white" {}
		_Available("Available", Range( 0 , 1)) = 1
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#pragma target 3.0
		#pragma multi_compile_instancing
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _Albedo;
		uniform float4 _Albedo_ST;
		uniform sampler2D _LifeEmission;
		uniform float4 _LifeEmission_ST;
		uniform sampler2D _DefEmission;
		uniform float4 _DefEmission_ST;
		uniform sampler2D _Metallic;
		uniform float4 _Metallic_ST;
		uniform sampler2D _Roughness;
		uniform float4 _Roughness_ST;

		UNITY_INSTANCING_BUFFER_START(Drone_Body)
			UNITY_DEFINE_INSTANCED_PROP(float, _Life)
#define _Life_arr Drone_Body
			UNITY_DEFINE_INSTANCED_PROP(float, _Available)
#define _Available_arr Drone_Body
		UNITY_INSTANCING_BUFFER_END(Drone_Body)

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_Albedo = i.uv_texcoord * _Albedo_ST.xy + _Albedo_ST.zw;
			o.Albedo = tex2D( _Albedo, uv_Albedo ).rgb;
			float2 uv_LifeEmission = i.uv_texcoord * _LifeEmission_ST.xy + _LifeEmission_ST.zw;
			float _Life_Instance = UNITY_ACCESS_INSTANCED_PROP(_Life_arr, _Life);
			float4 lerpResult8 = lerp( float4(1,0,0,0) , float4(0,1,0,0) , _Life_Instance);
			float2 uv_DefEmission = i.uv_texcoord * _DefEmission_ST.xy + _DefEmission_ST.zw;
			float _Available_Instance = UNITY_ACCESS_INSTANCED_PROP(_Available_arr, _Available);
			float4 lerpResult14 = lerp( float4(0.4632353,0.4632353,0.4632353,0) , float4(0,1,1,0) , _Available_Instance);
			o.Emission = ( ( tex2D( _LifeEmission, uv_LifeEmission ) * lerpResult8 ) + ( tex2D( _DefEmission, uv_DefEmission ) * lerpResult14 ) ).rgb;
			float2 uv_Metallic = i.uv_texcoord * _Metallic_ST.xy + _Metallic_ST.zw;
			o.Metallic = tex2D( _Metallic, uv_Metallic ).r;
			float2 uv_Roughness = i.uv_texcoord * _Roughness_ST.xy + _Roughness_ST.zw;
			o.Smoothness = ( 1.0 - tex2D( _Roughness, uv_Roughness ) ).r;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=13101
78;53;1091;606;1658.339;730.7406;1.787168;True;False
Node;AmplifyShaderEditor.ColorNode;10;-2150.609,120.7014;Float;False;Constant;_Alive;Alive;5;0;0,1,0,0;0;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.ColorNode;11;-2150.609,-44.4222;Float;False;Constant;_Dead;Dead;5;0;1,0,0,0;0;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;12;-2138.377,324.5579;Float;False;InstancedProperty;_Life;Life;4;0;1;0;1;0;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;18;-2263.262,1050.151;Float;False;InstancedProperty;_Available;Available;6;0;1;0;1;0;1;FLOAT
Node;AmplifyShaderEditor.ColorNode;17;-2255.348,856.3675;Float;False;Constant;_Color3;Color 3;5;0;0,1,1,0;0;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.ColorNode;16;-2255.348,691.2439;Float;False;Constant;_Color1;Color 1;5;0;0.4632353,0.4632353,0.4632353,0;0;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SamplerNode;3;-1997.715,-227.8927;Float;True;Property;_LifeEmission;Life Emission;3;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SamplerNode;15;-2102.454,507.7737;Float;True;Property;_DefEmission;Def Emission;5;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.LerpOp;14;-1733.473,825.7893;Float;True;3;0;COLOR;0.0;False;1;COLOR;0.0,0,0,0;False;2;FLOAT;0.0;False;1;COLOR
Node;AmplifyShaderEditor.LerpOp;8;-1628.735,90.12325;Float;True;3;0;COLOR;0.0;False;1;COLOR;0.0,0,0,0;False;2;FLOAT;0.0;False;1;COLOR
Node;AmplifyShaderEditor.SamplerNode;5;-866.3083,516.1832;Float;True;Property;_Roughness;Roughness;2;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;19;-1422.472,614.6387;Float;False;2;2;0;COLOR;0.0;False;1;COLOR;0.0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;13;-1394.301,-44.42228;Float;False;2;2;0;COLOR;0.0;False;1;COLOR;0.0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.SamplerNode;1;-815.3989,-613.9595;Float;True;Property;_Albedo;Albedo;0;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.OneMinusNode;6;-413.7469,381.6378;Float;False;1;0;COLOR;0.0;False;1;COLOR
Node;AmplifyShaderEditor.SimpleAddOpNode;7;-1125.209,132.933;Float;False;2;2;0;COLOR;0.0;False;1;COLOR;0.0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.SamplerNode;4;-872.4241,326.5969;Float;True;Property;_Metallic;Metallic;1;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;14.29734,-16.08451;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;Drone_Body;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;0;False;0;0;Opaque;0.5;True;True;0;False;Opaque;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;False;0;4;10;25;False;0.5;True;0;Zero;Zero;0;Zero;Zero;Add;Add;0;False;0;0,0,0,0;VertexOffset;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;0;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;OBJECT;0.0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;14;0;16;0
WireConnection;14;1;17;0
WireConnection;14;2;18;0
WireConnection;8;0;11;0
WireConnection;8;1;10;0
WireConnection;8;2;12;0
WireConnection;19;0;15;0
WireConnection;19;1;14;0
WireConnection;13;0;3;0
WireConnection;13;1;8;0
WireConnection;6;0;5;0
WireConnection;7;0;13;0
WireConnection;7;1;19;0
WireConnection;0;0;1;0
WireConnection;0;2;7;0
WireConnection;0;3;4;0
WireConnection;0;4;6;0
ASEEND*/
//CHKSM=400FFF9E97CFBD492E7BA16C47A35C5A426E58B0