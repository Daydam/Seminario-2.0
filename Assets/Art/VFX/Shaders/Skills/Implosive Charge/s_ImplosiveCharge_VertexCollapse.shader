// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Gastón Zabala/Skills/ImplosiveCharge_VertexCollapse"
{
	Properties
	{
		_Smoothness("Smoothness", Float) = 0
		_MainColor("Main Color", Color) = (0,0,0,0)
		_BlackHolePos("BlackHolePos", Vector) = (0,0,0,0)
		_Radius("Radius", Float) = 0
		_FallOff("FallOff", Float) = 0
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows vertex:vertexDataFunc 
		struct Input
		{
			float3 worldPos;
		};

		uniform float3 _BlackHolePos;
		uniform float _Radius;
		uniform float _FallOff;
		uniform float4 _MainColor;
		uniform float _Smoothness;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float3 ase_worldPos = mul( unity_ObjectToWorld, v.vertex );
			float3 lerpResult17 = lerp( float3( 0,0,0 ) , ( ( ase_worldPos + ( 1.0 - _BlackHolePos ) ) - float3( 1,1,1 ) ) , ( saturate( pow( ( distance( ase_worldPos , _BlackHolePos ) / _Radius ) , _FallOff ) ) - 1.0 ));
			float4 transform18 = mul(unity_WorldToObject,float4( lerpResult17 , 0.0 ));
			v.vertex.xyz += transform18.xyz;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			o.Albedo = _MainColor.rgb;
			o.Smoothness = _Smoothness;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16700
7;1;1266;954;999.1857;57.29709;1;True;False
Node;AmplifyShaderEditor.WorldPosInputsNode;3;-1937.325,38.11499;Float;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.Vector3Node;4;-1931.473,229.8072;Float;False;Property;_BlackHolePos;BlackHolePos;2;0;Create;True;0;0;False;0;0,0,0;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;6;-1522.365,253.7437;Float;False;Property;_Radius;Radius;3;0;Create;True;0;0;False;0;0;2.6;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DistanceOpNode;5;-1537.182,135.9918;Float;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;9;-1345.581,259.2574;Float;False;Property;_FallOff;FallOff;4;0;Create;True;0;0;False;0;0;1.95;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;10;-1334.847,128.7533;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;15;-1651.539,493.8852;Float;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.PowerNode;11;-1127.847,137.7533;Float;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;12;-941.8469,138.7533;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;14;-1405.225,469.3797;Float;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;13;-760.8469,141.7533;Float;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;16;-1181.824,469.8503;Float;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;1,1,1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;17;-538.0107,331.6081;Float;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;1;-212.958,87.37259;Float;False;Property;_Smoothness;Smoothness;0;0;Create;True;0;0;False;0;0;0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.WorldToObjectTransfNode;18;-320.0759,326.729;Float;False;1;0;FLOAT4;0,0,0,1;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;2;-248.958,-159.6274;Float;False;Property;_MainColor;Main Color;1;0;Create;True;0;0;False;0;0,0,0,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;Gastón Zabala/Skills/ImplosiveCharge_VertexCollapse;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;5;0;3;0
WireConnection;5;1;4;0
WireConnection;10;0;5;0
WireConnection;10;1;6;0
WireConnection;15;0;4;0
WireConnection;11;0;10;0
WireConnection;11;1;9;0
WireConnection;12;0;11;0
WireConnection;14;0;3;0
WireConnection;14;1;15;0
WireConnection;13;0;12;0
WireConnection;16;0;14;0
WireConnection;17;1;16;0
WireConnection;17;2;13;0
WireConnection;18;0;17;0
WireConnection;0;0;2;0
WireConnection;0;4;1;0
WireConnection;0;11;18;0
ASEEND*/
//CHKSM=BB358021B18E9CC16C37C34D9CC6BF4A2AF2CDB2