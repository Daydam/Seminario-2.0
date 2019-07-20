// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "FIREPOWER/CrystalPyramidRiver"
{
	Properties
	{
		_Albedo("Albedo", 2D) = "white" {}
		_Flowmap("Flowmap", 2D) = "white" {}
		_PanSpeed("PanSpeed", Vector) = (0,0,0,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _Albedo;
		uniform float2 _PanSpeed;
		uniform sampler2D _Flowmap;
		uniform float4 _Flowmap_ST;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_Flowmap = i.uv_texcoord * _Flowmap_ST.xy + _Flowmap_ST.zw;
			float2 uv_TexCoord3 = i.uv_texcoord + tex2D( _Flowmap, uv_Flowmap ).rg;
			float2 panner4 = ( 1.0 * _Time.y * _PanSpeed + uv_TexCoord3);
			o.Albedo = ( tex2D( _Albedo, panner4 ) * float4( 0.6226415,0.6226415,0.6226415,0 ) ).rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16700
7;7;1352;692;1141.187;684.5367;1.511497;True;False
Node;AmplifyShaderEditor.SamplerNode;2;-1215.26,-254.2083;Float;True;Property;_Flowmap;Flowmap;1;0;Create;True;0;0;False;0;None;920b2289f820f91489558539f21b1313;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;5;-806.3481,258.6859;Float;False;Property;_PanSpeed;PanSpeed;2;0;Create;True;0;0;False;0;0,0;0.07,0.07;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;3;-848.7469,83.89519;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;4;-491.2647,-22.25946;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;1;-359.802,-394.7185;Float;True;Property;_Albedo;Albedo;0;0;Create;True;0;0;False;0;None;dd522790051eea340b917609a60bb38b;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;7;89.05141,-343.059;Float;False;285;303;Comment;1;6;DELET DIS;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;6;139.0514,-293.059;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0.6226415,0.6226415,0.6226415,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;FIREPOWER/CrystalPyramidRiver;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;3;1;2;0
WireConnection;4;0;3;0
WireConnection;4;2;5;0
WireConnection;1;1;4;0
WireConnection;6;0;1;0
WireConnection;0;0;6;0
ASEEND*/
//CHKSM=9426B9C3CD0DCC2F4C58E9E67378C8A53D3A1C32