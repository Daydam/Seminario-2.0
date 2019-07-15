// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Clase05/Flowmap"
{
	Properties
	{
		_Albedo("Albedo", 2D) = "white" {}
		_Flowmap("Flowmap", 2D) = "white" {}
		_FlowMapIntensity("FlowMap Intensity", Range( 0 , 1)) = 0
		_Speed("Speed", Vector) = (0,0,0,0)
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
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows vertex:vertexDataFunc 
		struct Input
		{
			float2 texcoord_0;
			float2 uv_texcoord;
		};

		uniform sampler2D _Albedo;
		uniform float2 _Speed;
		uniform sampler2D _Flowmap;
		uniform float4 _Flowmap_ST;
		uniform float _FlowMapIntensity;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			o.texcoord_0.xy = v.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_Flowmap = i.uv_texcoord * _Flowmap_ST.xy + _Flowmap_ST.zw;
			float4 lerpResult11 = lerp( float4( i.texcoord_0, 0.0 , 0.0 ) , tex2D( _Flowmap, uv_Flowmap ) , _FlowMapIntensity);
			float2 panner12 = ( lerpResult11.rg + 1.0 * _Time.y * _Speed);
			o.Albedo = tex2D( _Albedo, panner12 ).rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=13701
0;92;710;270;2616.385;172.7595;3.127689;True;False
Node;AmplifyShaderEditor.RangedFloatNode;5;-1987.471,193.5615;Float;False;Property;_FlowMapIntensity;FlowMap Intensity;2;0;0;0;1;0;1;FLOAT
Node;AmplifyShaderEditor.SamplerNode;3;-2022.314,-7.789854;Float;True;Property;_Flowmap;Flowmap;1;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.TextureCoordinatesNode;8;-1972.092,-289.2418;Float;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.LerpOp;11;-1541.574,-123.1913;Float;False;3;0;COLOR;1,1,1,0;False;1;COLOR;0.0;False;2;FLOAT;0.0;False;1;COLOR
Node;AmplifyShaderEditor.Vector2Node;13;-1236.392,-4.927456;Float;False;Property;_Speed;Speed;3;0;0,0;0;3;FLOAT2;FLOAT;FLOAT
Node;AmplifyShaderEditor.PannerNode;12;-1021.795,-145.3951;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1.0;False;1;FLOAT2
Node;AmplifyShaderEditor.SamplerNode;1;-568.2369,-74.95183;Float;True;Property;_Albedo;Albedo;0;0;Assets/Lava.png;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;68,0;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;Clase05/Flowmap;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;0;False;0;0;Opaque;0.5;True;True;0;False;Opaque;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;0;0;0;0;False;2;15;10;25;False;0.5;True;0;Zero;Zero;0;Zero;Zero;OFF;OFF;0;False;0;0,0,0,0;VertexOffset;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;0;0;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;FLOAT;0.0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;11;0;8;0
WireConnection;11;1;3;0
WireConnection;11;2;5;0
WireConnection;12;0;11;0
WireConnection;12;2;13;0
WireConnection;1;1;12;0
WireConnection;0;0;1;0
ASEEND*/
//CHKSM=B7D94BC5E3F2F1527B1E35BDF589D566AE77573A