// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Custom shaders/Height sand"
{
	Properties
	{
		_SandbaseColor("Sand - baseColor", 2D) = "white"{}
		_Sandheight("Sand - height", 2D) = "white"{}
		_EdgeLength ( "Edge length", Range( 2, 50 ) ) = 2
		_Tilingsandsubstance("Tiling sand substance", Float) = 1
		_HeightAmount("Height Amount", Float) = 0
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

		uniform sampler2D _Sandheight;
		uniform float _Tilingsandsubstance;
		uniform float _HeightAmount;
		uniform sampler2D _SandbaseColor;
		uniform float _EdgeLength;

		float4 tessFunction( appdata_full v0, appdata_full v1, appdata_full v2 )
		{
			return UnityEdgeLengthBasedTess (v0.vertex, v1.vertex, v2.vertex, _EdgeLength);
		}

		void vertexDataFunc( inout appdata_full v )
		{
			float2 temp_cast_0 = (_Tilingsandsubstance).xx;
			float2 uv_TexCoord3 = v.texcoord.xy * temp_cast_0;
			float4 _Sandheight1 = tex2Dlod(_Sandheight, float4( uv_TexCoord3, 0.0 , 0.0 ));
			v.vertex.xyz += ( _Sandheight1.r * _HeightAmount * float3(0,1,0) );
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 temp_cast_0 = (_Tilingsandsubstance).xx;
			float2 uv_TexCoord3 = i.uv_texcoord * temp_cast_0;
			float4 _SandbaseColor1 = tex2D(_SandbaseColor, uv_TexCoord3);
			o.Albedo = _SandbaseColor1.rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16100
7;7;1266;948;1565.195;155.9952;1.3;True;False
Node;AmplifyShaderEditor.RangedFloatNode;4;-1532.476,-82.09708;Float;False;Property;_Tilingsandsubstance;Tiling sand substance;6;0;Create;True;0;0;False;0;1;10;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;3;-1258.476,-101.0971;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SubstanceSamplerNode;1;-1000.557,-105.6956;Float;True;Property;_SubstanceSample0;Substance Sample 0;5;0;Create;True;0;0;False;0;2dafa32145f0d6340993725c1101ce17;0;True;1;0;FLOAT2;0,0;False;4;COLOR;0;COLOR;1;COLOR;2;COLOR;3
Node;AmplifyShaderEditor.RangedFloatNode;8;-761.312,162.4025;Float;False;Property;_HeightAmount;Height Amount;7;0;Create;True;0;0;False;0;0;0.16;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector3Node;9;-752.312,277.4025;Float;False;Constant;_Ydeformation;Y deformation;3;0;Create;True;0;0;False;0;0,1,0;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.BreakToComponentsNode;10;-561.5947,14.30479;Float;False;COLOR;1;0;COLOR;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;7;-485.312,135.4025;Float;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;6;Float;ASEMaterialInspector;0;0;Standard;Custom shaders/Height sand;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;True;2;2;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;0;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;3;0;4;0
WireConnection;1;0;3;0
WireConnection;10;0;1;1
WireConnection;7;0;10;0
WireConnection;7;1;8;0
WireConnection;7;2;9;0
WireConnection;0;0;1;0
WireConnection;0;11;7;0
ASEEND*/
//CHKSM=F1F18715B4136118E93970D8EE14A5EFC5440653