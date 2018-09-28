// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "ShaderFalopa"
{
	Properties
	{
		[HideInInspector] __dirty( "", Int ) = 1
		_Size("Size", Float) = 64
		_Falloff("Falloff", Range( 0 , 20)) = 10
		_GlowColor("Glow Color", Color) = (0,0.1724138,1,0)
		_CollapseLocation("Collapse Location", Vector) = (0,0,0,0)
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows vertex:vertexDataFunc 
		struct Input
		{
			float3 worldPos;
		};

		uniform float4 _GlowColor;
		uniform float3 _CollapseLocation;
		uniform float _Size;
		uniform float _Falloff;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float3 ase_worldPos = mul(unity_ObjectToWorld, v.vertex);
			float temp_output_27_0 = saturate( pow( ( distance( ase_worldPos , _CollapseLocation ) / _Size ) , _Falloff ) );
			float3 lerpResult28 = lerp( float3( 0,0,0 ) , ( ( ase_worldPos + ( 1.0 - _CollapseLocation ) ) + float3(-1,-1,-1) ) , ( temp_output_27_0 - 1.0 ));
			float4 transform37 = mul(unity_WorldToObject,float4( lerpResult28 , 0.0 ));
			v.vertex.xyz += transform37.xyz;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float3 temp_cast_0 = (0.88).xxx;
			o.Albedo = temp_cast_0;
			float3 ase_worldPos = i.worldPos;
			float temp_output_27_0 = saturate( pow( ( distance( ase_worldPos , _CollapseLocation ) / _Size ) , _Falloff ) );
			float4 lerpResult33 = lerp( _GlowColor , float4( 0,0,0,0 ) , temp_output_27_0);
			o.Emission = lerpResult33.rgb;
			o.Metallic = 0.5;
			o.Smoothness = 0.8;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=13101
0;436;1087;270;3305.829;-528.342;5.999736;True;False
Node;AmplifyShaderEditor.WorldPosInputsNode;19;-1271.036,1017.774;Float;False;0;4;FLOAT3;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.Vector3Node;47;-1284.62,1182.547;Float;False;Property;_CollapseLocation;Collapse Location;3;0;0,0,0;0;4;FLOAT3;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.DistanceOpNode;22;-840.5709,1026.741;Float;False;2;0;FLOAT3;0.0;False;1;FLOAT3;0,0,0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;23;-799.7662,1216.987;Float;False;Property;_Size;Size;0;0;64;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleDivideOpNode;24;-483.4955,1141.752;Float;False;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;26;-534.1331,1358.854;Float;False;Property;_Falloff;Falloff;1;0;10;0;20;0;1;FLOAT
Node;AmplifyShaderEditor.PowerNode;25;-142.3883,1198.786;Float;True;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.OneMinusNode;42;-876.0607,1588.332;Float;False;1;0;FLOAT3;0.0;False;1;FLOAT3
Node;AmplifyShaderEditor.SimpleAddOpNode;43;-428.0787,1643.834;Float;False;2;2;0;FLOAT3;0.0,0,0;False;1;FLOAT3;0.0;False;1;FLOAT3
Node;AmplifyShaderEditor.Vector3Node;65;-237.6352,1720.4;Float;False;Constant;_Vector1;Vector 1;4;0;-1,-1,-1;0;4;FLOAT3;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SaturateNode;27;204.5502,1239.268;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;63;41.73966,1642.119;Float;False;2;2;0;FLOAT3;0.0,0,0;False;1;FLOAT3;0.0;False;1;FLOAT3
Node;AmplifyShaderEditor.SimpleSubtractOpNode;35;484.6665,1261.186;Float;False;2;0;FLOAT;0.0;False;1;FLOAT;1.0;False;1;FLOAT
Node;AmplifyShaderEditor.ColorNode;34;680.0706,1678.068;Float;False;Property;_GlowColor;Glow Color;2;0;0,0.1724138,1,0;0;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.LerpOp;28;971.4504,1404.635;Float;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0.0,0,0;False;2;FLOAT;0.0;False;1;FLOAT3
Node;AmplifyShaderEditor.RangedFloatNode;3;1422.217,1062.808;Float;False;Constant;_Float2;Float 2;0;0;0.8;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;1;1455.602,888.4766;Float;False;Constant;_Float0;Float 0;0;0;0.88;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.LerpOp;33;965.9507,1719.291;Float;False;3;0;COLOR;0.0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0.0;False;1;COLOR
Node;AmplifyShaderEditor.WorldToObjectTransfNode;37;1308.477,1257.614;Float;False;1;0;FLOAT4;0,0,0,1;False;5;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;2;1443.277,970.4764;Float;False;Constant;_Float1;Float 1;0;0;0.5;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1698.725,925.8684;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;ShaderFalopa;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;0;False;0;0;Opaque;0.5;True;True;0;False;Opaque;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;False;0;4;10;25;False;0.5;True;0;Zero;Zero;0;Zero;Zero;Add;Add;0;False;0;0,0,0,0;VertexOffset;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;0;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;OBJECT;0.0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;22;0;19;0
WireConnection;22;1;47;0
WireConnection;24;0;22;0
WireConnection;24;1;23;0
WireConnection;25;0;24;0
WireConnection;25;1;26;0
WireConnection;42;0;47;0
WireConnection;43;0;19;0
WireConnection;43;1;42;0
WireConnection;27;0;25;0
WireConnection;63;0;43;0
WireConnection;63;1;65;0
WireConnection;35;0;27;0
WireConnection;28;1;63;0
WireConnection;28;2;35;0
WireConnection;33;0;34;0
WireConnection;33;2;27;0
WireConnection;37;0;28;0
WireConnection;0;0;1;0
WireConnection;0;2;33;0
WireConnection;0;3;2;0
WireConnection;0;4;3;0
WireConnection;0;11;37;0
ASEEND*/
//CHKSM=CABA893D10BDA511CCA54A6F46C124C7E70325C3