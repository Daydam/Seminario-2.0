// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Clase04/SnowRT"
{
	Properties
	{
		_EdgeLength ( "Edge length", Range( 2, 50 ) ) = 2
		_RT("RT", 2D) = "white" {}
		_Intensity("Intensity", Float) = 0
		_FloorAlbedo("FloorAlbedo", 2D) = "white" {}
		_SnowAlbedo("SnowAlbedo", 2D) = "white" {}
		_SnowNormal("SnowNormal", 2D) = "bump" {}
		_SnowAO("SnowAO", 2D) = "white" {}
		_FloorAO("FloorAO", 2D) = "white" {}
		_FloorNormal("FloorNormal", 2D) = "bump" {}
		_GreyRange("Grey Range", Range( 0.6 , 0.9)) = 0
		_FloorRough("FloorRough", 2D) = "white" {}
		_SnowRough("SnowRough", 2D) = "white" {}
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

		struct appdata
		{
			float4 vertex : POSITION;
			float4 tangent : TANGENT;
			float3 normal : NORMAL;
			float4 texcoord : TEXCOORD0;
			float4 texcoord1 : TEXCOORD1;
			float4 texcoord2 : TEXCOORD2;
			float4 texcoord3 : TEXCOORD3;
			fixed4 color : COLOR;
			UNITY_VERTEX_INPUT_INSTANCE_ID
		};

		uniform sampler2D _SnowNormal;
		uniform float4 _SnowNormal_ST;
		uniform sampler2D _FloorNormal;
		uniform float4 _FloorNormal_ST;
		uniform sampler2D _RT;
		uniform float4 _RT_ST;
		uniform float _GreyRange;
		uniform sampler2D _SnowAlbedo;
		uniform float4 _SnowAlbedo_ST;
		uniform sampler2D _FloorAlbedo;
		uniform float4 _FloorAlbedo_ST;
		uniform sampler2D _SnowRough;
		uniform float4 _SnowRough_ST;
		uniform sampler2D _FloorRough;
		uniform float4 _FloorRough_ST;
		uniform sampler2D _SnowAO;
		uniform float4 _SnowAO_ST;
		uniform sampler2D _FloorAO;
		uniform float4 _FloorAO_ST;
		uniform float _Intensity;
		uniform float _EdgeLength;

		float4 tessFunction( appdata v0, appdata v1, appdata v2 )
		{
			return UnityEdgeLengthBasedTess (v0.vertex, v1.vertex, v2.vertex, _EdgeLength);
		}

		void vertexDataFunc( inout appdata v )
		{
			float2 uv_RT = v.texcoord * _RT_ST.xy + _RT_ST.zw;
			float4 tex2DNode1 = tex2Dlod( _RT, float4( uv_RT, 0, 0.0) );
			v.vertex.xyz += ( ( float3(0,1,0) * ( 1.0 - tex2DNode1.r ) ) * _Intensity );
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_SnowNormal = i.uv_texcoord * _SnowNormal_ST.xy + _SnowNormal_ST.zw;
			float2 uv_FloorNormal = i.uv_texcoord * _FloorNormal_ST.xy + _FloorNormal_ST.zw;
			float2 uv_RT = i.uv_texcoord * _RT_ST.xy + _RT_ST.zw;
			float4 tex2DNode1 = tex2D( _RT, uv_RT );
			float SnowMask53 = saturate( ( ( tex2DNode1.r - _GreyRange ) * 150.0 ) );
			float3 lerpResult19 = lerp( UnpackNormal( tex2D( _SnowNormal, uv_SnowNormal ) ) , UnpackNormal( tex2D( _FloorNormal, uv_FloorNormal ) ) , SnowMask53);
			o.Normal = lerpResult19;
			float2 uv_SnowAlbedo = i.uv_texcoord * _SnowAlbedo_ST.xy + _SnowAlbedo_ST.zw;
			float2 uv_FloorAlbedo = i.uv_texcoord * _FloorAlbedo_ST.xy + _FloorAlbedo_ST.zw;
			float SnowMask59 = 0.0;
			float4 lerpResult2 = lerp( tex2D( _SnowAlbedo, uv_SnowAlbedo ) , tex2D( _FloorAlbedo, uv_FloorAlbedo ) , SnowMask59);
			o.Albedo = lerpResult2.rgb;
			float2 uv_SnowRough = i.uv_texcoord * _SnowRough_ST.xy + _SnowRough_ST.zw;
			float2 uv_FloorRough = i.uv_texcoord * _FloorRough_ST.xy + _FloorRough_ST.zw;
			float lerpResult23 = lerp( tex2D( _SnowRough, uv_SnowRough ).r , tex2D( _FloorRough, uv_FloorRough ).r , SnowMask53);
			o.Smoothness = ( 1.0 - lerpResult23 );
			float2 uv_SnowAO = i.uv_texcoord * _SnowAO_ST.xy + _SnowAO_ST.zw;
			float2 uv_FloorAO = i.uv_texcoord * _FloorAO_ST.xy + _FloorAO_ST.zw;
			float lerpResult22 = lerp( tex2D( _SnowAO, uv_SnowAO ).r , tex2D( _FloorAO, uv_FloorAO ).r , SnowMask53);
			o.Occlusion = lerpResult22;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=13701
0;92;710;270;2140.466;898.7571;7.657254;True;False
Node;AmplifyShaderEditor.CommentaryNode;57;-2859.348,399.7664;Float;False;635.491;307.6016;Resta para sacar los grises;2;49;51;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SamplerNode;1;-3287.325,483.9514;Float;True;Property;_RT;RT;0;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.CommentaryNode;58;-2211.059,389.5402;Float;False;524.8273;303;Multiplicacion absurda para elevar los grises restantes y pasarlos a blancos;2;50;38;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;51;-2809.348,592.3682;Float;False;Property;_GreyRange;Grey Range;11;0;0;0.6;0.9;0;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;38;-2161.059,524.6043;Float;False;Constant;_Float0;Float 0;11;0;150;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleSubtractOpNode;49;-2458.857,449.7664;Float;True;2;0;FLOAT;0.0;False;1;FLOAT;3.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;50;-1921.232,439.5402;Float;True;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SaturateNode;52;-1558.807,362.6969;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.CommentaryNode;60;-944.0086,1788.45;Float;False;248;302.9999;Invert para poder levantar lo que antes era engro;1;5;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;61;-618.9052,1631.078;Float;False;477.0584;275.7266;Vector3.Up;2;6;7;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SamplerNode;21;-518.0378,821.9851;Float;True;Property;_SnowRough;SnowRough;13;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SamplerNode;17;-541.3886,225.1112;Float;True;Property;_FloorNormal;FloorNormal;10;0;None;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT3;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.OneMinusNode;5;-894.0086,1838.45;Float;True;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.Vector3Node;6;-568.9052,1681.078;Float;False;Constant;_Vector0;Vector 0;2;0;0,1,0;0;4;FLOAT3;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RegisterLocalVarNode;53;-1282.635,386.8655;Float;False;SnowMask;-1;True;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.CommentaryNode;62;-225.6889,1915.396;Float;False;404.6217;314.9746;Multiply para controlar la elevacion de la nievo;2;8;9;;1,1,1,1;0;0
Node;AmplifyShaderEditor.GetLocalVarNode;56;-180.7561,1091.506;Float;False;53;0;1;FLOAT
Node;AmplifyShaderEditor.SamplerNode;18;-516.7876,1021.594;Float;True;Property;_FloorRough;FloorRough;11;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SamplerNode;13;-526.1846,410.3529;Float;True;Property;_SnowAO;SnowAO;6;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.LerpOp;23;93.29275,862.8212;Float;False;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.GetLocalVarNode;55;-153.1373,694.4368;Float;False;53;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;7;-310.8468,1773.805;Float;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0.0,0,0;False;1;FLOAT3
Node;AmplifyShaderEditor.RangedFloatNode;9;-175.689,2115.371;Float;False;Property;_Intensity;Intensity;2;0;0;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.SamplerNode;10;-550.2471,-213.3111;Float;True;Property;_FloorAlbedo;FloorAlbedo;3;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RegisterLocalVarNode;59;6.591089,-142.3983;Float;False;SnowMask;-1;True;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.GetLocalVarNode;54;-96.69787,135.6039;Float;False;53;0;1;FLOAT
Node;AmplifyShaderEditor.SamplerNode;11;-550.376,-406.8858;Float;True;Property;_SnowAlbedo;SnowAlbedo;4;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SamplerNode;16;-520.0344,624.0508;Float;True;Property;_FloorAO;FloorAO;9;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SamplerNode;12;-529.5276,14.46182;Float;True;Property;_SnowNormal;SnowNormal;5;0;None;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT3;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.WireNode;31;-243.0276,123.0492;Float;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;8;9.932752,1965.396;Float;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0.0,0,0;False;1;FLOAT3
Node;AmplifyShaderEditor.LerpOp;2;331.2225,-231.7651;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0.0,0,0,0;False;2;FLOAT;0.0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.OneMinusNode;27;523.5129,398.8959;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.LerpOp;22;139.2461,471.4755;Float;False;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.LerpOp;19;203.0245,41.41898;Float;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0,0,0;False;1;FLOAT3
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1790.144,436.3894;Float;False;True;6;Float;ASEMaterialInspector;0;0;Standard;Clase04/SnowRT;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;0;False;0;0;Opaque;0.5;True;True;0;False;Opaque;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;0;0;0;0;True;2;2;10;25;False;0.5;True;0;Zero;Zero;0;Zero;Zero;OFF;OFF;0;False;0;0,0,0,0;VertexOffset;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;0;0;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;FLOAT;0.0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;49;0;1;1
WireConnection;49;1;51;0
WireConnection;50;0;49;0
WireConnection;50;1;38;0
WireConnection;52;0;50;0
WireConnection;5;0;1;1
WireConnection;53;0;52;0
WireConnection;23;0;21;1
WireConnection;23;1;18;1
WireConnection;23;2;56;0
WireConnection;7;0;6;0
WireConnection;7;1;5;0
WireConnection;31;0;17;0
WireConnection;8;0;7;0
WireConnection;8;1;9;0
WireConnection;2;0;11;0
WireConnection;2;1;10;0
WireConnection;2;2;59;0
WireConnection;27;0;23;0
WireConnection;22;0;13;1
WireConnection;22;1;16;1
WireConnection;22;2;55;0
WireConnection;19;0;12;0
WireConnection;19;1;31;0
WireConnection;19;2;54;0
WireConnection;0;0;2;0
WireConnection;0;1;19;0
WireConnection;0;4;27;0
WireConnection;0;5;22;0
WireConnection;0;11;8;0
ASEEND*/
//CHKSM=F427DC3363BD13C0FD8B04CB4804806D10C74284