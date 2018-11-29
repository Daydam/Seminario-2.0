// Upgrade NOTE: upgraded instancing buffer 'DissolvewRamp' to new syntax.

// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Dissolve w Ramp"
{
	Properties
	{
		_Albedo("Albedo ", 2D) = "white" {}
		_Normal("Normal", 2D) = "bump" {}
		_Metallic("Metallic", 2D) = "white" {}
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		_DissolveNoise("Dissolve Noise", 2D) = "white" {}
		_Dissolved("Dissolved", Range( 0 , 1)) = 0
		_RampTexture("Ramp Texture", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Off
		ZWrite On
		ZTest LEqual
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
		uniform sampler2D _Albedo;
		uniform float4 _Albedo_ST;
		uniform sampler2D _RampTexture;
		uniform float4 _RampTexture_ST;
		uniform sampler2D _DissolveNoise;
		uniform float4 _DissolveNoise_ST;
		uniform sampler2D _Metallic;
		uniform float4 _Metallic_ST;
		uniform float _Cutoff = 0.5;

		UNITY_INSTANCING_BUFFER_START(DissolvewRamp)
			UNITY_DEFINE_INSTANCED_PROP(float, _Dissolved)
#define _Dissolved_arr DissolvewRamp
		UNITY_INSTANCING_BUFFER_END(DissolvewRamp)

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_Normal = i.uv_texcoord * _Normal_ST.xy + _Normal_ST.zw;
			o.Normal = UnpackNormal( tex2D( _Normal, uv_Normal ) );
			float2 uv_Albedo = i.uv_texcoord * _Albedo_ST.xy + _Albedo_ST.zw;
			o.Albedo = tex2D( _Albedo, uv_Albedo ).rgb;
			float2 uv_RampTexture = i.uv_texcoord * _RampTexture_ST.xy + _RampTexture_ST.zw;
			float2 uv_DissolveNoise = i.uv_texcoord * _DissolveNoise_ST.xy + _DissolveNoise_ST.zw;
			float _Dissolved_Instance = UNITY_ACCESS_INSTANCED_PROP(_Dissolved_arr, _Dissolved);
			float temp_output_33_0 = saturate( ( ( tex2D( _DissolveNoise, uv_DissolveNoise ).r * 0.5 ) + (-1.0 + (( 1.0 - _Dissolved_Instance ) - 0.0) * (1.0 - -1.0) / (1.0 - 0.0)) ) );
			o.Emission = ( tex2D( _RampTexture, uv_RampTexture ) * ( 1.0 - temp_output_33_0 ) * 7.5 ).rgb;
			float2 uv_Metallic = i.uv_texcoord * _Metallic_ST.xy + _Metallic_ST.zw;
			o.Metallic = tex2D( _Metallic, uv_Metallic ).r;
			o.Alpha = 1;
			clip( temp_output_33_0 - _Cutoff );
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=15301
94;72;1091;606;1895.044;937.4258;2.790737;True;False
Node;AmplifyShaderEditor.CommentaryNode;13;-1818.998,797.4994;Float;False;917.9009;505.0993;Dissolve;8;10;2;8;9;1;33;38;39;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;2;-1786.497,1216.101;Float;False;InstancedProperty;_Dissolved;Dissolved;5;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;8;-1522.797,1059.601;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;39;-1777.206,1031.464;Float;False;Constant;_NumeroMagico2;NumeroMagico2;6;0;Create;True;0;0;False;0;0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;1;-1786.363,837.5106;Float;True;Property;_DissolveNoise;Dissolve Noise;4;0;Create;True;0;0;False;0;None;707a86d0822a1e440a1e1298adfe4589;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;38;-1473.564,903.1268;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;9;-1340.297,1115.601;Float;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;-1;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;10;-1326.799,859.0986;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;33;-1102.423,840.8483;Float;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;26;-1553.997,203.5003;Float;False;863.9988;534.2992;Ramp;3;18;17;19;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;25;-832.0973,-10.00011;Float;False;474.7993;169.2496;Aux - hechos para este caso en particular;1;21;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;21;-565.298,39.99989;Float;False;Constant;_DarkenValue;Darken Value;6;0;Create;True;0;0;False;0;7.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;17;-1009.578,468.4731;Float;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;23;-1080.898,-527.5004;Float;False;747.8004;491.5003;Texture;2;16;15;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SamplerNode;18;-1503.997,253.5002;Float;True;Property;_RampTexture;Ramp Texture;6;0;Create;True;0;0;False;0;None;4bd446f459e5b8942b05d483ca3c3a67;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;28;-152.0234,977.3477;Float;False;3;3;0;FLOAT;0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.Vector3Node;30;-647.0606,1105.324;Float;False;Constant;_Vector0;Vector 0;6;0;Create;True;0;0;False;0;0,0,0;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleSubtractOpNode;31;-408.3594,1147.024;Float;True;2;0;FLOAT3;0,0,0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SamplerNode;16;-1030.898,-266.0001;Float;True;Property;_Normal;Normal;1;0;Create;True;0;0;False;0;None;None;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;15;-1019.199,-477.5004;Float;True;Property;_Albedo;Albedo ;0;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;35;-591.4557,1263.225;Float;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.ObjectToWorldTransfNode;34;-857.9962,1366.22;Float;False;1;0;FLOAT4;0,0,0,1;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;37;-309.2987,1416.855;Float;False;Constant;_NumeroMagico1;NumeroMagico1;6;0;Create;True;0;0;False;0;-0.35;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;19;-858.9985,321.5998;Float;False;3;3;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.PosVertexDataNode;36;-843.4191,1206.717;Float;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;27;-587.1014,870.0494;Float;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;42;-135.4838,-102.9955;Float;True;Property;_Metallic;Metallic;2;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;956.1782,-158.5856;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;Dissolve w Ramp;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;Off;1;False;-1;3;False;-1;False;0;0;False;0;Custom;0.5;True;True;0;True;TransparentCutout;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;255;False;-1;255;False;-1;255;False;-1;6;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;0;4;10;25;False;0.5;True;0;4;False;-1;1;False;-1;0;2;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0.02;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;3;-1;-1;-1;0;0;0;False;0;0;0;False;-1;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;8;0;2;0
WireConnection;38;0;1;1
WireConnection;38;1;39;0
WireConnection;9;0;8;0
WireConnection;10;0;38;0
WireConnection;10;1;9;0
WireConnection;33;0;10;0
WireConnection;17;0;33;0
WireConnection;28;0;27;0
WireConnection;28;1;31;0
WireConnection;28;2;37;0
WireConnection;31;0;30;0
WireConnection;31;1;35;0
WireConnection;35;0;36;0
WireConnection;35;1;34;0
WireConnection;19;0;18;0
WireConnection;19;1;17;0
WireConnection;19;2;21;0
WireConnection;27;0;33;0
WireConnection;0;0;15;0
WireConnection;0;1;16;0
WireConnection;0;2;19;0
WireConnection;0;3;42;0
WireConnection;0;10;33;0
ASEEND*/
//CHKSM=9F466EB7BE19A0747F683DE73188A0BCC7D94401