// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Gastón Zabala/HeightSand_New"
{
	Properties
	{
		_SandbaseColor("Sand - baseColor", 2D) = "white"{}
		_Sandmetallic("Sand - metallic", 2D) = "white"{}
		_Sandnormal("Sand - normal", 2D) = "white"{}
		_TilingAlbedoNormalHeight("Tiling (Albedo, Normal, Height)", Float) = 0
		_Smoothness("Smoothness", Range( 0 , 1)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGPROGRAM
		#pragma target 4.6
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _Sandnormal;
		uniform float _TilingAlbedoNormalHeight;
		uniform sampler2D _SandbaseColor;
		uniform sampler2D _Sandmetallic;
		uniform float _Smoothness;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 temp_cast_0 = (_TilingAlbedoNormalHeight).xx;
			float2 uv_TexCoord6 = i.uv_texcoord * temp_cast_0;
			float4 _Sandnormal59 = tex2D(_Sandnormal, uv_TexCoord6);
			o.Normal = UnpackNormal( _Sandnormal59 );
			float4 _SandbaseColor59 = tex2D(_SandbaseColor, uv_TexCoord6);
			o.Albedo = _SandbaseColor59.rgb;
			float4 _Sandmetallic59 = tex2D(_Sandmetallic, uv_TexCoord6);
			o.Metallic = (_Sandmetallic59).r;
			o.Smoothness = _Smoothness;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16700
7;7;1266;948;34.63403;6.836334;1;True;False
Node;AmplifyShaderEditor.RangedFloatNode;3;-1239.612,98.49892;Float;False;Property;_TilingAlbedoNormalHeight;Tiling (Albedo, Normal, Height);0;0;Create;True;0;0;False;0;0;10;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;6;-898.5145,79.76185;Float;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SubstanceSamplerNode;59;-592.267,76.05628;Float;True;Property;_SubstanceSample0;Substance Sample 0;2;0;Create;True;0;0;False;0;2dafa32145f0d6340993725c1101ce17;0;True;1;0;FLOAT2;0,0;False;4;COLOR;0;COLOR;1;COLOR;2;COLOR;3
Node;AmplifyShaderEditor.Vector3Node;64;102.6597,734.5047;Float;False;Constant;_Vector0;Vector 0;6;0;Create;True;0;0;False;0;0,0,1;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.ComponentMaskNode;60;156.3964,441.5522;Float;False;False;False;False;True;1;0;COLOR;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;65;634.997,336.4568;Float;False;Property;_Smoothness;Smoothness;1;0;Create;True;0;0;False;0;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;52;439.0162,588.9007;Float;False;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ComponentMaskNode;62;168.3394,333.586;Float;False;True;False;False;False;1;0;COLOR;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.UnpackScaleNormalNode;58;137.5664,171.7236;Float;False;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.ComponentMaskNode;63;32.86178,586.8034;Float;False;True;False;False;False;1;0;COLOR;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;61;524.1748,440.101;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;972.5865,123.8393;Float;False;True;6;Float;ASEMaterialInspector;0;0;Standard;Gastón Zabala/HeightSand_New;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;18;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;6;0;3;0
WireConnection;59;0;6;0
WireConnection;60;0;59;2
WireConnection;52;0;63;0
WireConnection;52;1;64;0
WireConnection;62;0;59;2
WireConnection;58;0;59;1
WireConnection;63;0;59;3
WireConnection;61;0;60;0
WireConnection;0;0;59;0
WireConnection;0;1;58;0
WireConnection;0;3;62;0
WireConnection;0;4;65;0
ASEEND*/
//CHKSM=CF55C096B7D8C0B543C37C9298BD04C999714BF8