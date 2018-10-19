// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "ScoreScreen"
{
	Properties
	{
		[HideInInspector] __dirty( "", Int ) = 1
		_NumberTint("NumberTint", Color) = (1,0,0,0)
		_BackgroundTint("BackgroundTint", Color) = (0.1172414,1,0,0)
		_Main("Main", 2D) = "white" {}
		_Score("Score", 2D) = "white" {}
		_PanScale("PanScale", Float) = 0
		_Alpha("Alpha", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard alpha:fade keepalpha noshadow vertex:vertexDataFunc 
		struct Input
		{
			float2 texcoord_0;
			float2 uv_texcoord;
		};

		uniform float4 _BackgroundTint;
		uniform sampler2D _Main;
		uniform float _PanScale;
		uniform sampler2D _Score;
		uniform float4 _Score_ST;
		uniform float4 _NumberTint;
		uniform float _Alpha;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			o.texcoord_0.xy = v.texcoord.xy * float2( 1,1 ) + float2( 0.45,0 );
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float mulTime13 = _Time.y * _PanScale;
			float2 uv_Score = i.uv_texcoord * _Score_ST.xy + _Score_ST.zw;
			o.Emission = saturate( ( ( _BackgroundTint * tex2D( _Main, (abs( i.texcoord_0+mulTime13 * float2(1,0 ))) ) ) + ( tex2D( _Score, uv_Score ).r * _NumberTint ) ) ).rgb;
			o.Alpha = _Alpha;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=13101
17;29;707;692;638.7684;1106.588;2.331298;True;False
Node;AmplifyShaderEditor.RangedFloatNode;16;-1811.292,-420.5443;Float;False;Property;_PanScale;PanScale;4;0;0;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleTimeNode;13;-1654.192,-413.4614;Float;False;1;0;FLOAT;1.0;False;1;FLOAT
Node;AmplifyShaderEditor.TextureCoordinatesNode;14;-1611.154,-590.6197;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0.45,0;False;5;FLOAT2;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.PannerNode;15;-1286.257,-522.2872;Float;False;1;0;2;0;FLOAT2;0,0;False;1;FLOAT;0.0;False;1;FLOAT2
Node;AmplifyShaderEditor.ColorNode;2;-947.5893,-755.877;Float;False;Property;_BackgroundTint;BackgroundTint;0;0;0.1172414,1,0,0;0;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SamplerNode;7;-973.0222,-517.2037;Float;True;Property;_Main;Main;2;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.ColorNode;34;-966.1727,51.22461;Float;False;Property;_NumberTint;NumberTint;0;0;1,0,0,0;0;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SamplerNode;8;-980.2339,-291.3258;Float;True;Property;_Score;Score;3;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;36;-552.92,-142.9403;Float;False;2;2;0;FLOAT;0.0,0,0,0;False;1;COLOR;0.0;False;1;COLOR
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;35;-478.2946,-570.5198;Float;False;2;2;0;COLOR;0.0;False;1;COLOR;0.0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.SimpleAddOpNode;9;-251.6696,-351.4611;Float;False;2;2;0;COLOR;0.0;False;1;COLOR;0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.RangedFloatNode;30;290.3738,-151.9341;Float;False;Property;_Alpha;Alpha;5;0;0;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.SaturateNode;33;36.58405,-230.7453;Float;False;1;0;COLOR;0.0;False;1;COLOR
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;778.9159,-226.1949;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;ScoreScreen;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;Back;0;0;False;0;0;Transparent;0.5;True;False;0;False;Transparent;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;False;0;4;10;25;False;0.5;False;0;Zero;Zero;0;Zero;Zero;Add;Add;0;False;0;0,0,0,0;VertexOffset;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;0;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;OBJECT;0.0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;13;0;16;0
WireConnection;15;0;14;0
WireConnection;15;1;13;0
WireConnection;7;1;15;0
WireConnection;36;0;8;1
WireConnection;36;1;34;0
WireConnection;35;0;2;0
WireConnection;35;1;7;0
WireConnection;9;0;35;0
WireConnection;9;1;36;0
WireConnection;33;0;9;0
WireConnection;0;2;33;0
WireConnection;0;9;30;0
ASEEND*/
//CHKSM=C08E50432A93F546C593200D94CE3C0398709D93