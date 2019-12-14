// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Gastón Zabala/Height Sand"
{
	Properties
	{
		_EdgeLength ( "Edge length", Range( 2, 50 ) ) = 10
		_TilingAlbedoNormalHeight("Tiling (Albedo, Normal, Height)", Float) = 0
		_Albedo("Albedo", 2D) = "white" {}
		_Normal("Normal", 2D) = "white" {}
		_Height("Height", 2D) = "white" {}
		_Radius("Radius", Float) = 0
		_Metallic("Metallic", Float) = 0
		_Intensity("Intensity", Float) = 0
		_Smoothness("Smoothness", Float) = 0
		_ScaleHeight("Scale Height", Float) = 0
		_RT_Drones("RT_Drones", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGPROGRAM
		#include "Tessellation.cginc"
		#pragma target 4.6
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows vertex:vertexDataFunc tessellate:tessFunction 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _Height;
		uniform float _TilingAlbedoNormalHeight;
		uniform sampler2D _RT_Drones;
		uniform float _Radius;
		uniform float _Intensity;
		uniform float _ScaleHeight;
		uniform sampler2D _Normal;
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
			float2 uv_TexCoord79 = v.texcoord.xy * temp_cast_0;
			float temp_output_81_0 = pow( ( tex2Dlod( _RT_Drones, float4( ( 1.0 - v.texcoord.xy ), 0, 0.0) ).r / _Radius ) , _Intensity );
			float temp_output_78_0 = ( 1.0 - temp_output_81_0 );
			v.vertex.xyz += ( ( ( tex2Dlod( _Height, float4( uv_TexCoord79, 0, 0.0) ).r - 0.3 ) * float3(0,1,0) * temp_output_78_0 ) * _ScaleHeight );
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 temp_cast_0 = (_TilingAlbedoNormalHeight).xx;
			float2 uv_TexCoord79 = i.uv_texcoord * temp_cast_0;
			o.Normal = UnpackNormal( tex2D( _Normal, uv_TexCoord79 ) );
			o.Albedo = tex2D( _Albedo, uv_TexCoord79 ).rgb;
			o.Metallic = _Metallic;
			o.Smoothness = _Smoothness;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16100
7;1;1352;698;2685.96;-2.454418;2.281704;True;False
Node;AmplifyShaderEditor.TextureCoordinatesNode;67;-2487.405,961.1472;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;68;-2205.696,962.6462;Float;False;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;80;-2218.598,296.1122;Float;False;Property;_TilingAlbedoNormalHeight;Tiling (Albedo, Normal, Height);5;0;Create;True;0;0;False;0;0;20;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;66;-2033.39,933.8653;Float;True;Property;_RT_Drones;RT_Drones;14;0;Create;True;0;0;False;0;None;4fa09269bf77113458c9d4ea221c50e3;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;82;-1850.059,1219.112;Float;False;Property;_Radius;Radius;9;0;Create;True;0;0;False;0;0;0.02;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;79;-1877.502,277.3752;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleDivideOpNode;83;-1634.085,955.3936;Float;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;84;-1575.059,1223.112;Float;False;Property;_Intensity;Intensity;11;0;Create;True;0;0;False;0;0;0.13;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;76;-1478.918,390.0935;Float;True;Property;_Height;Height;8;0;Create;True;0;0;False;0;None;2dafa32145f0d6340993725c1101ce17;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PowerNode;81;-1399.187,952.1384;Float;True;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;75;-1118.403,418.1642;Float;False;2;0;FLOAT;0;False;1;FLOAT;0.3;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector3Node;9;-1077.546,641.2828;Float;False;Constant;_YDeformation;Y = Deformation;5;0;Create;True;0;0;False;0;0,1,0;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.OneMinusNode;78;-1088.4,955.1234;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;10;-676.8079,729.8944;Float;False;Property;_ScaleHeight;Scale Height;13;0;Create;True;0;0;False;0;0;0.61;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;8;-698.3357,510.0584;Float;False;3;3;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;38;-921.4384,198.6446;Float;True;Property;_Normal;Normal;7;0;Create;True;0;0;False;0;None;2dafa32145f0d6340993725c1101ce17;True;0;False;white;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SmoothstepOpNode;87;-829.5447,1070.852;Float;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0.1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;45;-235.7512,251.1758;Float;False;Property;_Smoothness;Smoothness;12;0;Create;True;0;0;False;0;0;0.2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;1;-923.716,-24.45245;Float;True;Property;_Albedo;Albedo;6;0;Create;True;0;0;False;0;None;2dafa32145f0d6340993725c1101ce17;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;46;-236.5204,154.1758;Float;False;Property;_Metallic;Metallic;10;0;Create;True;0;0;False;0;0;0.09;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;85;-899.0206,953.789;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;77;-436.6941,509.4762;Float;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.TFHCRemapNode;86;-1123.97,1178.084;Float;True;5;0;FLOAT;0;False;1;FLOAT;0.1;False;2;FLOAT;1;False;3;FLOAT;-1.08;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;6;Float;ASEMaterialInspector;0;0;Standard;Gastón Zabala/Height Sand;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;True;2;10;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;0;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;68;0;67;0
WireConnection;66;1;68;0
WireConnection;79;0;80;0
WireConnection;83;0;66;1
WireConnection;83;1;82;0
WireConnection;76;1;79;0
WireConnection;81;0;83;0
WireConnection;81;1;84;0
WireConnection;75;0;76;1
WireConnection;78;0;81;0
WireConnection;8;0;75;0
WireConnection;8;1;9;0
WireConnection;8;2;78;0
WireConnection;38;1;79;0
WireConnection;87;0;81;0
WireConnection;1;1;79;0
WireConnection;85;0;78;0
WireConnection;77;0;8;0
WireConnection;77;1;10;0
WireConnection;86;0;81;0
WireConnection;0;0;1;0
WireConnection;0;1;38;0
WireConnection;0;3;46;0
WireConnection;0;4;45;0
WireConnection;0;11;77;0
ASEEND*/
//CHKSM=BB9063793A6E493AB5BE11FA7CA05D2C0529BF7D