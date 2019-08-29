// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "PiccioneCustoms/HeightSandShader"
{
	Properties
	{
		_TilingAlbedoNormalHeight("Tiling (Albedo, Normal, Height)", Float) = 0
		_Albedo("Albedo", 2D) = "white" {}
		_Normal("Normal", 2D) = "white" {}
		_Metallic("Metallic", Float) = 0
		_Smoothness("Smoothness", Float) = 0
		_TessAmount("TessAmount", Range( 0 , 1)) = 0
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

		uniform sampler2D _Normal;
		uniform float _TilingAlbedoNormalHeight;
		uniform sampler2D _Albedo;
		uniform float _Metallic;
		uniform float _Smoothness;
		uniform float _TessAmount;

		float4 tessFunction( appdata_full v0, appdata_full v1, appdata_full v2 )
		{
			float4 temp_cast_0 = (_TessAmount).xxxx;
			return temp_cast_0;
		}

		void vertexDataFunc( inout appdata_full v )
		{
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 temp_cast_0 = (_TilingAlbedoNormalHeight).xx;
			float2 uv_TexCoord6 = i.uv_texcoord * temp_cast_0;
			o.Normal = UnpackNormal( tex2D( _Normal, uv_TexCoord6 ) );
			o.Albedo = tex2D( _Albedo, uv_TexCoord6 ).rgb;
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
Version=16700
136;500;1266;954;-399.3926;-210.8889;1;True;False
Node;AmplifyShaderEditor.RangedFloatNode;3;-2802.345,189.9793;Float;False;Property;_TilingAlbedoNormalHeight;Tiling (Albedo, Normal, Height);0;0;Create;True;0;0;False;0;0;10;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;6;-2461.248,171.2422;Float;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;16;-1007.385,92.51162;Float;True;Property;_Normal;Normal;2;0;Create;True;0;0;False;0;None;2dafa32145f0d6340993725c1101ce17;True;0;False;white;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;49;-951.0389,428.6111;Float;True;Property;_Height;Height;9;0;Create;True;0;0;False;0;None;2dafa32145f0d6340993725c1101ce17;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;46;-211.1683,577.6953;Float;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;20;-361.0569,103.3279;Float;False;Property;_Metallic;Metallic;3;0;Create;True;0;0;False;0;0;0.09;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;48;391.3517,648.6178;Float;True;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;47;46.99898,907.4879;Float;False;Property;_ScaleHeight;ScaleHeight;6;0;Create;True;0;0;False;0;0;0.95;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;19;-1009.663,-130.5854;Float;True;Property;_Albedo;Albedo;1;0;Create;True;0;0;False;0;None;2dafa32145f0d6340993725c1101ce17;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;52;-179.6202,326.1588;Float;False;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;26;706.5126,876.0599;Float;False;Property;_TessAmount;TessAmount;8;0;Create;True;0;0;False;0;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;18;-524.1628,199.2863;Float;False;Property;_Smoothness;Smoothness;4;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;56;-1465.945,761.7003;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;40;-1106.639,1036.694;Float;False;Property;_GrayRange;GrayRange;5;0;Create;True;0;0;False;0;0;0.217;0.2;0.5;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;55;-1640.345,720.8002;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;57;-2127.232,780.0977;Float;False;Property;_OffsetRT_Drones;Offset RT_Drones;10;0;Create;True;0;0;False;0;0,0;-0.04,-0.002;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;54;-1931.345,717.8002;Float;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0.48,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;43;-337.1076,743.9531;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector3Node;45;-500.8306,508.4473;Float;False;Constant;_UpVector;UpVector;9;0;Create;True;0;0;False;0;0,1,0;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;41;-531.6378,740.8292;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;550;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;4;-1258.726,714.2305;Float;True;Property;_RT_Drones;RT_Drones;7;0;Create;True;0;0;False;0;None;4fa09269bf77113458c9d4ea221c50e3;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleSubtractOpNode;39;-796.8766,744.9675;Float;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;972.5865,123.8393;Float;False;True;6;Float;ASEMaterialInspector;0;0;Standard;PiccioneCustoms/HeightSandShader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;True;2;18;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;6;0;3;0
WireConnection;16;1;6;0
WireConnection;49;1;6;0
WireConnection;46;0;45;0
WireConnection;46;1;43;0
WireConnection;48;0;52;0
WireConnection;48;1;47;0
WireConnection;19;1;6;0
WireConnection;52;0;49;1
WireConnection;52;1;45;0
WireConnection;56;0;55;0
WireConnection;56;1;54;2
WireConnection;55;0;54;1
WireConnection;54;1;57;0
WireConnection;43;0;41;0
WireConnection;41;0;39;0
WireConnection;4;1;56;0
WireConnection;39;0;4;1
WireConnection;39;1;40;0
WireConnection;0;0;19;0
WireConnection;0;1;16;0
WireConnection;0;3;20;0
WireConnection;0;4;18;0
WireConnection;0;14;26;0
ASEEND*/
//CHKSM=C800C57FE8C569C1303CE50170CF0FE16DD3DCEB