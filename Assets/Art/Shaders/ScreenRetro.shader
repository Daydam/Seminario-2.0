// Upgrade NOTE: upgraded instancing buffer 'RetroScreen' to new syntax.

// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "RetroScreen"
{
	Properties
	{
		[HideInInspector] __dirty( "", Int ) = 1
		_ScreenImage("ScreenImage", 2D) = "white" {}
		_ScreenRetro("ScreenRetro", 2D) = "white" {}
		_LerpValue("LerpValue", Range( 0 , 1)) = 0.1
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma multi_compile_instancing
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows vertex:vertexDataFunc 
		struct Input
		{
			float2 uv_texcoord;
			float2 texcoord_0;
		};

		uniform sampler2D _ScreenImage;
		uniform float4 _ScreenImage_ST;
		uniform sampler2D _ScreenRetro;

		UNITY_INSTANCING_BUFFER_START(RetroScreen)
			UNITY_DEFINE_INSTANCED_PROP(float, _LerpValue)
#define _LerpValue_arr RetroScreen
		UNITY_INSTANCING_BUFFER_END(RetroScreen)

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float2 temp_cast_0 = (_Time.y).xx;
			float2 temp_cast_1 = (_Time.y).xx;
			o.texcoord_0.xy = v.texcoord.xy * temp_cast_0 + temp_cast_1;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_ScreenImage = i.uv_texcoord * _ScreenImage_ST.xy + _ScreenImage_ST.zw;
			float2 temp_cast_0 = (i.texcoord_0.y).xx;
			float _LerpValue_Instance = UNITY_ACCESS_INSTANCED_PROP(_LerpValue_arr, _LerpValue);
			float4 lerpResult4 = lerp( tex2D( _ScreenImage, uv_ScreenImage ) , tex2D( _ScreenRetro, temp_cast_0 ) , _LerpValue_Instance);
			o.Albedo = lerpResult4.rgb;
			o.Emission = lerpResult4.rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=13101
7;29;1352;692;1773.091;557.9362;1.474189;True;False
Node;AmplifyShaderEditor.SimpleTimeNode;8;-1280.237,121.3994;Float;False;1;0;FLOAT;1.0;False;1;FLOAT
Node;AmplifyShaderEditor.TextureCoordinatesNode;6;-1098.237,128.3994;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;5;-541.0648,146.8129;Float;False;InstancedProperty;_LerpValue;LerpValue;2;0;0.1;0;1;0;1;FLOAT
Node;AmplifyShaderEditor.SamplerNode;2;-790.5852,-84.10941;Float;True;Property;_ScreenRetro;ScreenRetro;1;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SamplerNode;1;-702.5852,-326.1094;Float;True;Property;_ScreenImage;ScreenImage;0;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.LerpOp;4;-353.5852,-51.10941;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0.0;False;2;FLOAT;0.0;False;1;COLOR
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;RetroScreen;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;0;False;0;0;Opaque;0.5;True;True;0;False;Opaque;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;False;0;4;10;25;False;0.5;True;0;Zero;Zero;0;Zero;Zero;Add;Add;0;False;0;0,0,0,0;VertexOffset;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;0;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;OBJECT;0.0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;6;0;8;0
WireConnection;6;1;8;0
WireConnection;2;1;6;2
WireConnection;4;0;1;0
WireConnection;4;1;2;0
WireConnection;4;2;5;0
WireConnection;0;0;4;0
WireConnection;0;2;4;0
ASEEND*/
//CHKSM=8941B8C6FC58485D44351EFA80FFF43E3818CF88