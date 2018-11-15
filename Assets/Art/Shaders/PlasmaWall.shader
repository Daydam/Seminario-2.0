// Upgrade NOTE: upgraded instancing buffer 'PlasmaWall' to new syntax.

// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "PlasmaWall"
{
	Properties
	{
		_Texture("Texture", 2D) = "white" {}
		_OffsetSpeed("OffsetSpeed", Float) = 0
		_EmissionIntensity("EmissionIntensity", Float) = 0
		_AliveColor("AliveColor", Color) = (0,0,0,0)
		_XTiling("XTiling", Float) = 0
		_DeadColor("DeadColor", Color) = (0,0,0,0)
		_Life("Life", Range( 0 , 1)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma multi_compile_instancing
		#pragma surface surf Standard alpha:fade keepalpha noshadow 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _Texture;
		uniform float _XTiling;
		uniform float _OffsetSpeed;
		uniform float4 _AliveColor;
		uniform float _EmissionIntensity;

		UNITY_INSTANCING_BUFFER_START(PlasmaWall)
			UNITY_DEFINE_INSTANCED_PROP(float4, _DeadColor)
#define _DeadColor_arr PlasmaWall
			UNITY_DEFINE_INSTANCED_PROP(float, _Life)
#define _Life_arr PlasmaWall
		UNITY_INSTANCING_BUFFER_END(PlasmaWall)

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 temp_cast_0 = (_XTiling).xx;
			float mulTime11 = _Time.y * _OffsetSpeed;
			float2 temp_cast_1 = (mulTime11).xx;
			float2 uv_TexCoord2 = i.uv_texcoord * temp_cast_0 + temp_cast_1;
			float2 temp_cast_2 = (uv_TexCoord2.x).xx;
			float grayscale13 = Luminance(tex2D( _Texture, temp_cast_2 ).rgb);
			float4 _DeadColor_Instance = UNITY_ACCESS_INSTANCED_PROP(_DeadColor_arr, _DeadColor);
			float _Life_Instance = UNITY_ACCESS_INSTANCED_PROP(_Life_arr, _Life);
			float4 lerpResult14 = lerp( _DeadColor_Instance , _AliveColor , _Life_Instance);
			float4 temp_output_8_0 = ( grayscale13 * lerpResult14 * _EmissionIntensity );
			o.Emission = temp_output_8_0.rgb;
			o.Alpha = temp_output_8_0.r;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=15301
257;29;1102;692;1519.136;174.0063;1.508169;True;False
Node;AmplifyShaderEditor.RangedFloatNode;3;-989.9292,-159.0938;Float;False;Property;_OffsetSpeed;OffsetSpeed;1;0;Create;True;0;0;False;0;0;0.7;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;12;-768.3364,-339.8718;Float;False;Property;_XTiling;XTiling;4;0;Create;True;0;0;False;0;0;3.57;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;11;-817.3102,-43.6899;Float;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;2;-544.0793,-183.3506;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;7;-988.9979,216.5581;Float;False;Property;_AliveColor;AliveColor;3;0;Create;True;0;0;False;0;0,0,0,0;0.9419878,1,0.6764706,1;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;17;-973.4372,621.9266;Float;False;InstancedProperty;_Life;Life;6;0;Create;True;0;0;False;0;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;16;-983.3699,424.0178;Float;False;InstancedProperty;_DeadColor;DeadColor;5;0;Create;True;0;0;False;0;0,0,0,0;0.9411765,0.606,0.6745098,1;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;1;-225.1445,-412.3552;Float;True;Property;_Texture;Texture;0;0;Create;True;0;0;False;0;None;b38798444239d6648a982cf46c854680;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;14;-581.6339,189.7771;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;5;-466.6768,427.1356;Float;False;Property;_EmissionIntensity;EmissionIntensity;2;0;Create;True;0;0;False;0;0;1.12;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCGrayscale;13;213.3998,-333.4371;Float;False;0;1;0;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;8;187.2065,-34.31758;Float;False;3;3;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;531.5857,-143.3516;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;PlasmaWall;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;0;False;0;Transparent;0.5;True;False;0;False;Transparent;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;0;4;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;0;0;False;0;0;0;False;-1;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;11;0;3;0
WireConnection;2;0;12;0
WireConnection;2;1;11;0
WireConnection;1;1;2;1
WireConnection;14;0;16;0
WireConnection;14;1;7;0
WireConnection;14;2;17;0
WireConnection;13;0;1;0
WireConnection;8;0;13;0
WireConnection;8;1;14;0
WireConnection;8;2;5;0
WireConnection;0;2;8;0
WireConnection;0;9;8;0
ASEEND*/
//CHKSM=BFE85ED815785DE7B479436A56F265E01C54DB10