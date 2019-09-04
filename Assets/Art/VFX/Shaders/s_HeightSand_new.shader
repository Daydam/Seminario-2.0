// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Gastón Zabala/HeightSand_New"
{
	Properties
	{
		_Sandemissive("Sand - emissive", 2D) = "white"{}
		_SandbaseColor("Sand - baseColor", 2D) = "white"{}
		_SandambientOcclusion("Sand - ambientOcclusion", 2D) = "white"{}
		_Sandmetallic("Sand - metallic", 2D) = "white"{}
		_Sandnormal("Sand - normal", 2D) = "white"{}
		_TilingAlbedoNormalHeight("Tiling (Albedo, Normal, Height)", Float) = 0
		_ScaleNormal("Scale Normal", Float) = 1
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityStandardUtils.cginc"
		#pragma target 4.6
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _Sandnormal;
		uniform float _TilingAlbedoNormalHeight;
		uniform float _ScaleNormal;
		uniform sampler2D _SandbaseColor;
		uniform sampler2D _Sandemissive;
		uniform sampler2D _Sandmetallic;
		uniform sampler2D _SandambientOcclusion;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 temp_cast_0 = (_TilingAlbedoNormalHeight).xx;
			float2 uv_TexCoord6 = i.uv_texcoord * temp_cast_0;
			float4 _Sandnormal67 = tex2D(_Sandnormal, uv_TexCoord6);
			o.Normal = UnpackScaleNormal( _Sandnormal67, _ScaleNormal );
			float4 _SandbaseColor67 = tex2D(_SandbaseColor, uv_TexCoord6);
			o.Albedo = _SandbaseColor67.rgb;
			float4 _Sandemissive67 = tex2D(_Sandemissive, uv_TexCoord6);
			o.Emission = _Sandemissive67.rgb;
			float4 _Sandmetallic67 = tex2D(_Sandmetallic, uv_TexCoord6);
			o.Metallic = (_Sandmetallic67).r;
			o.Smoothness = (_Sandmetallic67).a;
			float4 _SandambientOcclusion67 = tex2D(_SandambientOcclusion, uv_TexCoord6);
			o.Occlusion = (_SandambientOcclusion67).r;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16700
-8;411;1266;948;1325.833;116.6256;2.402737;True;False
Node;AmplifyShaderEditor.RangedFloatNode;3;-1210.157,-60.90624;Float;False;Property;_TilingAlbedoNormalHeight;Tiling (Albedo, Normal, Height);0;0;Create;True;0;0;False;0;0;10;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;6;-869.0592,-79.64333;Float;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SubstanceSamplerNode;67;-556.5867,-84.58364;Float;True;Property;_SubstanceSample0;Substance Sample 0;3;0;Create;True;0;0;False;0;0f81d0ae7b37c30449126e868f51c558;0;True;1;0;FLOAT2;0,0;False;6;COLOR;0;COLOR;1;COLOR;2;COLOR;3;COLOR;4;COLOR;5
Node;AmplifyShaderEditor.RangedFloatNode;69;-241.6092,309.1285;Float;False;Property;_ScaleNormal;Scale Normal;4;0;Create;True;0;0;False;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;68;126.8163,425.8901;Float;False;True;False;False;False;1;0;COLOR;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.UnpackScaleNormalNode;58;132.9664,182.4236;Float;False;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.ComponentMaskNode;62;129.3394,335.586;Float;False;True;False;False;False;1;0;COLOR;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;60;127.2964,503.8522;Float;False;False;False;False;True;1;0;COLOR;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;52;439.0162,588.9007;Float;False;3;3;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ComponentMaskNode;63;113.4618,605.0034;Float;False;True;False;False;False;1;0;COLOR;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;66;108.5573,936.7634;Float;False;Property;_ScaleHeight;Scale Height;2;0;Create;True;0;0;False;0;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector3Node;64;102.6597,734.5047;Float;False;Constant;_Vector0;Vector 0;6;0;Create;True;0;0;False;0;0,0,1;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;65;592.097,336.4568;Float;False;Property;_Smoothness;Smoothness;1;0;Create;True;0;0;False;0;0;0.2;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;972.5865,123.8393;Float;False;True;6;Float;ASEMaterialInspector;0;0;Standard;Gastón Zabala/HeightSand_New;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;18;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;6;0;3;0
WireConnection;67;0;6;0
WireConnection;68;0;67;4
WireConnection;58;0;67;1
WireConnection;58;1;69;0
WireConnection;62;0;67;2
WireConnection;60;0;67;2
WireConnection;52;0;63;0
WireConnection;52;1;64;0
WireConnection;52;2;66;0
WireConnection;63;0;67;3
WireConnection;0;0;67;0
WireConnection;0;1;58;0
WireConnection;0;2;67;5
WireConnection;0;3;62;0
WireConnection;0;4;60;0
WireConnection;0;5;68;0
ASEEND*/
//CHKSM=5C198AD0419086A1373C9EAA1A7846E58E2EA19D