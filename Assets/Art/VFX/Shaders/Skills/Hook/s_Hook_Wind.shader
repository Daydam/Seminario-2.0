// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Gastón Zabala/Skills/Hook/Wind"
{
	Properties
	{
		_MainTexture("Main Texture", 2D) = "white" {}
		_Rotator_Speed("Rotator_Speed", Float) = 0
		[HDR]_Color0("Color 0", Color) = (0,0,0,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Off
		ZWrite Off
		Blend One OneMinusSrcAlpha
		
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Unlit keepalpha noshadow 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform float4 _Color0;
		uniform sampler2D _MainTexture;
		uniform float _Rotator_Speed;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float mulTime4 = _Time.y * _Rotator_Speed;
			float cos2 = cos( mulTime4 );
			float sin2 = sin( mulTime4 );
			float2 rotator2 = mul( i.uv_texcoord - float2( 0.5,0.5 ) , float2x2( cos2 , -sin2 , sin2 , cos2 )) + float2( 0.5,0.5 );
			float4 tex2DNode1 = tex2D( _MainTexture, rotator2 );
			o.Emission = ( _Color0 * tex2DNode1.b ).rgb;
			o.Alpha = tex2DNode1.b;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16700
21;158;1266;791;1877.87;702.7744;1.907432;True;False
Node;AmplifyShaderEditor.RangedFloatNode;5;-1248.385,234.4505;Float;False;Property;_Rotator_Speed;Rotator_Speed;2;0;Create;True;0;0;False;0;0;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;4;-1014.385,239.4506;Float;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;3;-1099.385,103.4507;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RotatorNode;2;-791.3851,192.4505;Float;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0.5,0.5;False;2;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ColorNode;6;-505.555,-96.72815;Float;False;Property;_Color0;Color 0;3;1;[HDR];Create;True;0;0;False;0;0,0,0,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;1;-579.3851,165.4505;Float;True;Property;_MainTexture;Main Texture;1;0;Create;True;0;0;False;0;None;559f6157633824a418b5821a4b658f7d;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;7;-223.555,43.27188;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;58,-2;Float;False;True;2;Float;ASEMaterialInspector;0;0;Unlit;Gastón Zabala/Skills/Hook/Wind;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Off;2;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;False;0;True;Transparent;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;3;1;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;4;0;5;0
WireConnection;2;0;3;0
WireConnection;2;2;4;0
WireConnection;1;1;2;0
WireConnection;7;0;6;0
WireConnection;7;1;1;3
WireConnection;0;2;7;0
WireConnection;0;9;1;3
ASEEND*/
//CHKSM=E25F335EA428B4B69D842655D447665FF12F246E