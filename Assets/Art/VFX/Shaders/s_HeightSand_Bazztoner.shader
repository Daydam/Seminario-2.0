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
		_GrayRange("GrayRange", Range( 0.2 , 0.5)) = 0
		_ScaleHeight("ScaleHeight", Float) = 0
		_RT_Drones("RT_Drones", 2D) = "white" {}
		_TessAmount("TessAmount", Float) = 0
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

		uniform sampler2D _RT_Drones;
		uniform float4 _RT_Drones_ST;
		uniform float _GrayRange;
		uniform float _ScaleHeight;
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
			float2 uv_RT_Drones = v.texcoord * _RT_Drones_ST.xy + _RT_Drones_ST.zw;
			v.vertex.xyz += ( ( float3(0,1,0) * saturate( ( ( tex2Dlod( _RT_Drones, float4( uv_RT_Drones, 0, 0.0) ).r - _GrayRange ) * 550.0 ) ) ) * _ScaleHeight );
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
210;36;492;698;1519.059;-96.69622;1.984885;False;False
Node;AmplifyShaderEditor.RangedFloatNode;40;-1106.639,1036.694;Float;False;Property;_GrayRange;GrayRange;6;0;Create;True;0;0;False;0;0;0.2;0.2;0.5;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;4;-1117.726,714.2305;Float;True;Property;_RT_Drones;RT_Drones;8;0;Create;True;0;0;False;0;None;4fa09269bf77113458c9d4ea221c50e3;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleSubtractOpNode;39;-796.8766,744.9675;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;41;-527.6378,751.8292;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;550;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;3;-2304.545,189.9793;Float;False;Property;_TilingAlbedoNormalHeight;Tiling (Albedo, Normal, Height);0;0;Create;True;0;0;False;0;0;20;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector3Node;45;-191.8388,437.9341;Float;False;Constant;_UpVector;UpVector;9;0;Create;True;0;0;False;0;0,1,0;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SaturateNode;43;-197.1076,758.9531;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;6;-1963.448,171.2422;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;47;46.99898,907.4879;Float;False;Property;_ScaleHeight;ScaleHeight;7;0;Create;True;0;0;False;0;0;-2.04;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;46;8.661415,654.5035;Float;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;20;-361.0569,103.3279;Float;False;Property;_Metallic;Metallic;4;0;Create;True;0;0;False;0;0;0.09;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;16;-1007.385,92.51162;Float;True;Property;_Normal;Normal;2;0;Create;True;0;0;False;0;None;2dafa32145f0d6340993725c1101ce17;True;0;False;white;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;9;-1564.865,283.9606;Float;True;Property;_Height;Height;3;0;Create;True;0;0;False;0;None;2dafa32145f0d6340993725c1101ce17;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;19;-1009.663,-130.5854;Float;True;Property;_Albedo;Albedo;1;0;Create;True;0;0;False;0;None;2dafa32145f0d6340993725c1101ce17;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;18;-524.1628,199.2863;Float;False;Property;_Smoothness;Smoothness;5;0;Create;True;0;0;False;0;0;0.2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DegreesOpNode;55;-1819.957,1080.457;Float;False;1;0;FLOAT;270;False;1;FLOAT;0
Node;AmplifyShaderEditor.RotatorNode;53;-1494.877,916.7632;Float;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0.6,0.5;False;2;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;49;-1998.062,581.2747;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;26;706.5126,876.0599;Float;False;Property;_TessAmount;TessAmount;9;0;Create;True;0;0;False;0;0;20;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;48;268.9526,758.2178;Float;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;972.5865,123.8393;Float;False;True;6;Float;ASEMaterialInspector;0;0;Standard;PiccioneCustoms/HeightSandShader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;True;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;39;0;4;1
WireConnection;39;1;40;0
WireConnection;41;0;39;0
WireConnection;43;0;41;0
WireConnection;6;0;3;0
WireConnection;46;0;45;0
WireConnection;46;1;43;0
WireConnection;16;1;6;0
WireConnection;9;1;6;0
WireConnection;19;1;6;0
WireConnection;53;0;49;0
WireConnection;53;2;55;0
WireConnection;48;0;46;0
WireConnection;48;1;47;0
WireConnection;0;0;19;0
WireConnection;0;1;16;0
WireConnection;0;3;20;0
WireConnection;0;4;18;0
WireConnection;0;11;48;0
WireConnection;0;14;26;0
ASEEND*/
//CHKSM=A6296EC184EC862F5502237BFA146F6D24B9C4D6