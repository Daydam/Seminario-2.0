// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Gastón Zabala/LedEffect"
{
	Properties
	{
		_LedTexture("Led Texture", 2D) = "white" {}
		_RenderTexture("Render Texture", 2D) = "white" {}
		_Tiling("Tiling", Float) = 1
		_Speed("Speed", Vector) = (0,0,0,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _RenderTexture;
		uniform float4 _RenderTexture_ST;
		uniform sampler2D _LedTexture;
		uniform float2 _Speed;
		uniform float _Tiling;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_RenderTexture = i.uv_texcoord * _RenderTexture_ST.xy + _RenderTexture_ST.zw;
			float4 tex2DNode23 = tex2D( _RenderTexture, uv_RenderTexture );
			float2 panner19 = ( 1.0 * _Time.y * _Speed + i.uv_texcoord);
			float4 blendOpSrc18 = tex2DNode23;
			float4 blendOpDest18 = tex2D( _LedTexture, ( panner19 * _Tiling ) );
			o.Emission = ( saturate( min( blendOpSrc18 , blendOpDest18 ) )).rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16700
7;7;1266;948;1314.183;509.3137;1.3;True;False
Node;AmplifyShaderEditor.Vector2Node;14;-1118.485,122.794;Float;False;Property;_Speed;Speed;3;0;Create;True;0;0;False;0;0,0;30,30;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;29;-1141.636,-41.31989;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;19;-893.8608,59.65137;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;1,1;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;3;-877.3823,213.6082;Float;False;Property;_Tiling;Tiling;2;0;Create;True;0;0;False;0;1;30;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;26;-608.6025,59.1692;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;20;-399.6288,29.68505;Float;True;Property;_LedTexture;Led Texture;0;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;23;-421.7874,-206.9436;Float;True;Property;_RenderTexture;Render Texture;1;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;21;-471.6971,-686.3265;Float;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;27;51.26873,-685.1917;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;-0.48;False;1;FLOAT;0
Node;AmplifyShaderEditor.BlendOpsNode;18;51.80526,2.254472;Float;False;Darken;True;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;25;372.8503,-513.0217;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.CeilOpNode;22;223.5687,-684.2916;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DistanceOpNode;24;-202.1981,-683.1863;Float;False;2;0;FLOAT2;0,0;False;1;FLOAT2;0.5,0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;421.2672,-39.71059;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;Gastón Zabala/LedEffect;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;19;0;29;0
WireConnection;19;2;14;0
WireConnection;26;0;19;0
WireConnection;26;1;3;0
WireConnection;20;1;26;0
WireConnection;27;0;24;0
WireConnection;18;0;23;0
WireConnection;18;1;20;0
WireConnection;25;0;23;0
WireConnection;25;2;22;0
WireConnection;22;0;27;0
WireConnection;24;0;21;0
WireConnection;0;2;18;0
ASEEND*/
//CHKSM=79C5589EDBBCE25843CC0817CCF16F8D86DE01D9