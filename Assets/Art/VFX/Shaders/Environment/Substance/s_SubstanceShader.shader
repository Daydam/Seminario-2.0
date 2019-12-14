// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Gastón Zabala/Substance Shader"
{
	Properties
	{
		_Hieroglyphicsnormal("Hieroglyphics - normal", 2D) = "white"{}
		_HieroglyphicsbaseColor("Hieroglyphics - baseColor", 2D) = "white"{}
		_Hieroglyphicsmetallic("Hieroglyphics - metallic", 2D) = "white"{}
		_Hieroglyphicsheight("Hieroglyphics - height", 2D) = "white"{}
		_Tiling("Tiling", Vector) = (1,1,0,0)
		_ScaleNormal("Scale Normal", Float) = 1
		_ScaleHeight("Scale Height", Float) = 0
		_Smoothness("Smoothness", Range( 0 , 1)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGPROGRAM
		#include "UnityStandardUtils.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows vertex:vertexDataFunc 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _Hieroglyphicsheight;
		uniform float2 _Tiling;
		uniform float _ScaleHeight;
		uniform sampler2D _Hieroglyphicsnormal;
		uniform float _ScaleNormal;
		uniform sampler2D _HieroglyphicsbaseColor;
		uniform sampler2D _Hieroglyphicsmetallic;
		uniform float _Smoothness;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float2 uv_TexCoord3 = v.texcoord.xy * _Tiling;
			float4 _Hieroglyphicsheight1 = tex2Dlod(_Hieroglyphicsheight, float4( uv_TexCoord3, 0.0 , 0.0 ));
			v.vertex.xyz += ( (_Hieroglyphicsheight1).r * float3(0,0,1) * _ScaleHeight );
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_TexCoord3 = i.uv_texcoord * _Tiling;
			float4 _Hieroglyphicsnormal1 = tex2D(_Hieroglyphicsnormal, uv_TexCoord3);
			o.Normal = UnpackScaleNormal( _Hieroglyphicsnormal1, _ScaleNormal );
			float4 _HieroglyphicsbaseColor1 = tex2D(_HieroglyphicsbaseColor, uv_TexCoord3);
			o.Albedo = _HieroglyphicsbaseColor1.rgb;
			float4 _Hieroglyphicsmetallic1 = tex2D(_Hieroglyphicsmetallic, uv_TexCoord3);
			o.Metallic = (_Hieroglyphicsmetallic1).r;
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
7;1;1266;954;527.5833;15.67154;1;True;False
Node;AmplifyShaderEditor.Vector2Node;5;-1264.76,26.0383;Float;False;Property;_Tiling;Tiling;0;0;Create;True;0;0;False;0;1,1;1,1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;3;-1054.888,7.505676;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SubstanceSamplerNode;1;-807.0308,2.528321;Float;True;Property;_SubstanceSample0;Substance Sample 0;4;0;Create;True;0;0;False;0;728bd499d1485fd4aabf7777b0df9d66;0;True;1;0;FLOAT2;0,0;False;4;COLOR;0;COLOR;1;COLOR;2;COLOR;3
Node;AmplifyShaderEditor.Vector3Node;13;-239.0305,499.7456;Float;False;Constant;_VectorUP;Vector UP;2;0;Create;True;0;0;False;0;0,0,1;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;14;-237.0305,672.7456;Float;False;Property;_ScaleHeight;Scale Height;2;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;7;-540.2026,217.059;Float;False;Property;_ScaleNormal;Scale Normal;1;0;Create;True;0;0;False;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;11;-263.7607,417.6248;Float;False;True;False;False;False;1;0;COLOR;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;12;-11.43408,421.4282;Float;False;3;3;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ComponentMaskNode;9;-260.7607,319.6248;Float;False;True;False;False;False;1;0;COLOR;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;10;-43.76074,324.6248;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.UnpackScaleNormalNode;6;-279.8623,67.14963;Float;False;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.ComponentMaskNode;8;-263.8081,210.5681;Float;False;True;False;False;False;1;0;COLOR;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;15;-1.50116,244.671;Float;False;Property;_Smoothness;Smoothness;3;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;303.5999,8.300001;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;Gastón Zabala/Substance Shader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;3;0;5;0
WireConnection;1;0;3;0
WireConnection;11;0;1;3
WireConnection;12;0;11;0
WireConnection;12;1;13;0
WireConnection;12;2;14;0
WireConnection;9;0;1;2
WireConnection;10;0;9;0
WireConnection;6;0;1;1
WireConnection;6;1;7;0
WireConnection;8;0;1;2
WireConnection;0;0;1;0
WireConnection;0;1;6;0
WireConnection;0;3;8;0
WireConnection;0;4;15;0
WireConnection;0;11;12;0
ASEEND*/
//CHKSM=1E0AC5046CD5E1084E0DC6AE94D29733416D4E5B