// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Gastón Zabala/Environment/RockWalls"
{
	Properties
	{
		[NoScaleOffset]_Albedo("Albedo", 2D) = "white" {}
		[NoScaleOffset]_Normal("Normal", 2D) = "bump" {}
		[NoScaleOffset]_RAOGSmoothnessBMetallic("(R) AO (G) Smoothness (B) Metallic", 2D) = "white" {}
		_SmoothnessControl("Smoothness Control", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _Normal;
		uniform sampler2D _Albedo;
		uniform sampler2D _RAOGSmoothnessBMetallic;
		uniform float _SmoothnessControl;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_Normal2 = i.uv_texcoord;
			o.Normal = UnpackNormal( tex2D( _Normal, uv_Normal2 ) );
			float2 uv_Albedo1 = i.uv_texcoord;
			o.Albedo = tex2D( _Albedo, uv_Albedo1 ).rgb;
			float2 uv_RAOGSmoothnessBMetallic3 = i.uv_texcoord;
			float4 tex2DNode3 = tex2D( _RAOGSmoothnessBMetallic, uv_RAOGSmoothnessBMetallic3 );
			o.Metallic = tex2DNode3.b;
			o.Smoothness = ( ( 1.0 - tex2DNode3.g ) * _SmoothnessControl );
			o.Occlusion = tex2DNode3.r;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16700
-30;331;1266;948;1144.278;41.27231;1;True;False
Node;AmplifyShaderEditor.SamplerNode;3;-941.3724,226.0242;Float;True;Property;_RAOGSmoothnessBMetallic;(R) AO (G) Smoothness (B) Metallic;2;1;[NoScaleOffset];Create;True;0;0;False;0;None;2b0a2df57cb981d408fcadc9065d69e4;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;9;-461.8263,384.1648;Float;False;Property;_SmoothnessControl;Smoothness Control;3;0;Create;True;0;0;False;0;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;4;-418.5833,224.175;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;8;-199.8263,296.1648;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;1;-909.6474,-254.6776;Float;True;Property;_Albedo;Albedo;0;1;[NoScaleOffset];Create;True;0;0;False;0;None;0440637b480add74db0e9c22d178f61f;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;2;-910.011,-31.923;Float;True;Property;_Normal;Normal;1;1;[NoScaleOffset];Create;True;0;0;False;0;None;e64da86ba5b2d884d96222059969e6f4;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;Gastón Zabala/Environment/RockWalls;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;4;0;3;2
WireConnection;8;0;4;0
WireConnection;8;1;9;0
WireConnection;0;0;1;0
WireConnection;0;1;2;0
WireConnection;0;3;3;3
WireConnection;0;4;8;0
WireConnection;0;5;3;1
ASEEND*/
//CHKSM=DB5C93A25CBB0EA6B73A12D56205F484820DA129