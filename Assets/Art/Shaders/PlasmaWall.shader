// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "PlasmaWall"
{
	Properties
	{
		[HideInInspector] __dirty( "", Int ) = 1
		_Texture("Texture", 2D) = "white" {}
		_OffsetSpeed("OffsetSpeed", Float) = 0
		_EmissionIntensity("EmissionIntensity", Float) = 0
		_EmissionColor("EmissionColor", Color) = (0,0,0,0)
		_XTiling("XTiling", Float) = 0
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
		};

		uniform sampler2D _Texture;
		uniform float _XTiling;
		uniform float _OffsetSpeed;
		uniform float4 _EmissionColor;
		uniform float _EmissionIntensity;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float2 temp_cast_0 = (_XTiling).xx;
			float mulTime11 = _Time.y * _OffsetSpeed;
			float2 temp_cast_1 = (mulTime11).xx;
			o.texcoord_0.xy = v.texcoord.xy * temp_cast_0 + temp_cast_1;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 temp_cast_0 = (i.texcoord_0.x).xx;
			float4 temp_output_8_0 = ( tex2D( _Texture, temp_cast_0 ) * _EmissionColor * _EmissionIntensity );
			o.Emission = temp_output_8_0.rgb;
			o.Alpha = temp_output_8_0.r;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=13101
88;160;1352;692;1245.336;443.8718;1;True;False
Node;AmplifyShaderEditor.RangedFloatNode;3;-844.0793,-179.3506;Float;False;Property;_OffsetSpeed;OffsetSpeed;1;0;0;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;12;-768.3364,-339.8718;Float;False;Property;_XTiling;XTiling;4;0;0;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleTimeNode;11;-817.3102,-43.6899;Float;False;1;0;FLOAT;1.0;False;1;FLOAT
Node;AmplifyShaderEditor.TextureCoordinatesNode;2;-544.0793,-183.3506;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;5;-466.6768,427.1356;Float;False;Property;_EmissionIntensity;EmissionIntensity;2;0;0;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.SamplerNode;1;-225.1445,-412.3552;Float;True;Property;_Texture;Texture;0;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.ColorNode;7;-469.4505,212.0888;Float;False;Property;_EmissionColor;EmissionColor;3;0;0,0,0,0;0;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;8;187.2065,-34.31758;Float;False;3;3;0;COLOR;0.0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;531.5857,-143.3516;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;PlasmaWall;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;Back;0;0;False;0;0;Transparent;0.5;True;False;0;True;Transparent;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;False;0;4;10;25;False;0.5;False;0;Zero;Zero;0;Zero;Zero;Add;Add;0;False;0;0,0,0,0;VertexOffset;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;0;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;OBJECT;0.0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;11;0;3;0
WireConnection;2;0;12;0
WireConnection;2;1;11;0
WireConnection;1;1;2;1
WireConnection;8;0;1;0
WireConnection;8;1;7;0
WireConnection;8;2;5;0
WireConnection;0;2;8;0
WireConnection;0;9;8;0
WireConnection;0;10;8;0
ASEEND*/
//CHKSM=B34B9265AE251BCAE893F214CB18F561C761033B