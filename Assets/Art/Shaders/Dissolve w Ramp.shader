// Upgrade NOTE: upgraded instancing buffer 'DissolvewRamp' to new syntax.

// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Dissolve w Ramp"
{
	Properties
	{
		[HideInInspector] __dirty( "", Int ) = 1
		_MaskClipValue( "Mask Clip Value", Float ) = 0.5
		_DissolveNoise("Dissolve Noise", 2D) = "white" {}
		_Dissolved("Dissolved", Range( 0 , 1)) = 0
		_Normal("Normal", 2D) = "bump" {}
		_RampTexture("Ramp Texture", 2D) = "white" {}
		_AlbedoCOlor("AlbedoCOlor", Color) = (0,0,0,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Off
		CGPROGRAM
		#pragma target 3.0
		#pragma multi_compile_instancing
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _Normal;
		uniform float4 _Normal_ST;
		uniform float4 _AlbedoCOlor;
		uniform sampler2D _RampTexture;
		uniform float4 _RampTexture_ST;
		uniform sampler2D _DissolveNoise;
		uniform float4 _DissolveNoise_ST;
		uniform float _MaskClipValue = 0.5;

		UNITY_INSTANCING_BUFFER_START(DissolvewRamp)
			UNITY_DEFINE_INSTANCED_PROP(float, _Dissolved)
#define _Dissolved_arr DissolvewRamp
		UNITY_INSTANCING_BUFFER_END(DissolvewRamp)

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_Normal = i.uv_texcoord * _Normal_ST.xy + _Normal_ST.zw;
			o.Normal = ( UnpackNormal( tex2D( _Normal, uv_Normal ) ) * 2.0 );
			o.Albedo = _AlbedoCOlor.rgb;
			float2 uv_RampTexture = i.uv_texcoord * _RampTexture_ST.xy + _RampTexture_ST.zw;
			float2 uv_DissolveNoise = i.uv_texcoord * _DissolveNoise_ST.xy + _DissolveNoise_ST.zw;
			float _Dissolved_Instance = UNITY_ACCESS_INSTANCED_PROP(_Dissolved_arr, _Dissolved);
			float temp_output_33_0 = saturate( ( ( tex2D( _DissolveNoise, uv_DissolveNoise ).r * 0.5 ) + (-1.0 + (( 1.0 - _Dissolved_Instance ) - 0.0) * (1.0 - -1.0) / (1.0 - 0.0)) ) );
			o.Emission = ( tex2D( _RampTexture, uv_RampTexture ) * ( 1.0 - temp_output_33_0 ) * 7.5 ).xyz;
			o.Alpha = 1;
			clip( temp_output_33_0 - _MaskClipValue );
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=13101
7;29;1199;638;1239.427;584.6874;1.3;True;False
Node;AmplifyShaderEditor.CommentaryNode;13;-1818.998,797.4994;Float;False;917.9009;505.0993;Dissolve;8;10;2;8;9;1;33;38;39;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;2;-1786.497,1216.101;Float;False;InstancedProperty;_Dissolved;Dissolved;2;0;0;0;1;0;1;FLOAT
Node;AmplifyShaderEditor.SamplerNode;1;-1786.363,837.5106;Float;True;Property;_DissolveNoise;Dissolve Noise;1;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;39;-1777.206,1031.464;Float;False;Constant;_NumeroMagico2;NumeroMagico2;6;0;0.5;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.OneMinusNode;8;-1522.797,1059.601;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.TFHCRemap;9;-1340.297,1115.601;Float;False;5;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;3;FLOAT;-1.0;False;4;FLOAT;1.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;38;-1473.564,903.1268;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;10;-1326.799,859.0986;Float;True;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.CommentaryNode;23;-1080.898,-527.5004;Float;False;747.8004;491.5003;Texture;3;16;15;22;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;25;-832.0973,-10.00011;Float;False;474.7993;169.2496;Aux - hechos para este caso en particular;2;21;24;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SaturateNode;33;-1102.423,840.8483;Float;True;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.CommentaryNode;26;-1553.997,203.5003;Float;False;863.9988;534.2992;Ramp;3;18;17;19;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SamplerNode;18;-1503.997,253.5002;Float;True;Property;_RampTexture;Ramp Texture;5;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;24;-782.0973,44.24948;Float;False;Constant;_Normalamplifier;Normal amplifier;6;0;2;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;21;-565.298,39.99989;Float;False;Constant;_DarkenValue;Darken Value;6;0;7.5;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.SamplerNode;16;-1030.898,-266.0001;Float;True;Property;_Normal;Normal;4;0;None;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT3;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.OneMinusNode;17;-1009.578,468.4731;Float;True;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;37;-309.2987,1416.855;Float;False;Constant;_NumeroMagico1;NumeroMagico1;6;0;-0.35;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;22;-619.6978,-224.2009;Float;False;2;2;0;FLOAT3;0.0;False;1;FLOAT;0,0,0;False;1;FLOAT3
Node;AmplifyShaderEditor.ObjectToWorldTransfNode;34;-857.9962,1366.22;Float;False;1;0;FLOAT4;0,0,0,1;False;5;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.ColorNode;40;-183.8271,-296.0873;Float;False;Property;_AlbedoCOlor;AlbedoCOlor;6;0;0,0,0,0;0;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;19;-858.9985,321.5998;Float;False;3;3;0;FLOAT4;0.0;False;1;FLOAT;0.0,0,0,0;False;2;FLOAT;0,0,0,0;False;1;FLOAT4
Node;AmplifyShaderEditor.SamplerNode;15;-1019.199,-477.5004;Float;True;Property;_Albedo;Albedo ;3;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.OneMinusNode;27;-587.1014,870.0494;Float;True;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;28;-152.0234,977.3477;Float;False;3;3;0;FLOAT;0.0;False;1;FLOAT4;0.0;False;2;FLOAT;0,0,0,0;False;1;FLOAT4
Node;AmplifyShaderEditor.SimpleAddOpNode;35;-591.4557,1263.225;Float;False;2;2;0;FLOAT3;0.0,0,0,0;False;1;FLOAT4;0.0,0,0;False;1;FLOAT4
Node;AmplifyShaderEditor.Vector3Node;30;-647.0606,1105.324;Float;False;Constant;_Vector0;Vector 0;6;0;0,0,0;0;4;FLOAT3;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.PosVertexDataNode;36;-843.4191,1206.717;Float;False;0;0;5;FLOAT3;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleSubtractOpNode;31;-408.3594,1147.024;Float;True;2;0;FLOAT3;0,0,0;False;1;FLOAT4;0.0,0,0;False;1;FLOAT4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;Dissolve w Ramp;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;Off;0;0;False;0;0;Custom;0.5;True;True;0;True;TransparentCutout;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;False;0;4;10;25;False;0.5;True;0;OneMinusDstColor;One;0;DstColor;Zero;Add;Add;0;False;0.02;0,0,0,0;VertexOffset;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;0;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;OBJECT;0.0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;8;0;2;0
WireConnection;9;0;8;0
WireConnection;38;0;1;1
WireConnection;38;1;39;0
WireConnection;10;0;38;0
WireConnection;10;1;9;0
WireConnection;33;0;10;0
WireConnection;17;0;33;0
WireConnection;22;0;16;0
WireConnection;22;1;24;0
WireConnection;19;0;18;0
WireConnection;19;1;17;0
WireConnection;19;2;21;0
WireConnection;27;0;33;0
WireConnection;28;0;27;0
WireConnection;28;1;31;0
WireConnection;28;2;37;0
WireConnection;35;0;36;0
WireConnection;35;1;34;0
WireConnection;31;0;30;0
WireConnection;31;1;35;0
WireConnection;0;0;40;0
WireConnection;0;1;22;0
WireConnection;0;2;19;0
WireConnection;0;10;33;0
ASEEND*/
//CHKSM=38D6794737653C5CA72762B34B401B27022B5764