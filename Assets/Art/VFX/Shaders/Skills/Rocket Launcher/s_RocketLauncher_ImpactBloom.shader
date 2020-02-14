// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Gastón Zabala/Skills/RocketLauncher/ImpactBloom"
{
	Properties
	{
		[HDR]_AlbedoColor("Albedo Color", Color) = (0,0,0,0)
		[HDR]_EmissionColor("Emission Color", Color) = (0,0,0,0)
		_MainTexture("Main Texture", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Standard alpha:fade keepalpha noshadow 
		struct Input
		{
			float2 uv_texcoord;
			float4 vertexColor : COLOR;
		};

		uniform float4 _AlbedoColor;
		uniform float4 _EmissionColor;
		uniform sampler2D _MainTexture;
		uniform float4 _MainTexture_ST;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			o.Albedo = _AlbedoColor.rgb;
			float2 uv_MainTexture = i.uv_texcoord * _MainTexture_ST.xy + _MainTexture_ST.zw;
			float4 tex2DNode1 = tex2D( _MainTexture, uv_MainTexture );
			o.Emission = ( _EmissionColor * tex2DNode1.g ).rgb;
			o.Alpha = ( tex2DNode1.g * i.vertexColor.a );
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16700
7;29;1266;860;1207.323;689.7623;1.3;True;False
Node;AmplifyShaderEditor.ColorNode;5;-574.3386,-312.9402;Float;False;Property;_EmissionColor;Emission Color;1;1;[HDR];Create;True;0;0;False;0;0,0,0,0;11.98431,5.960784,1.694118,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.VertexColorNode;3;-476.1987,250.8002;Float;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;1;-610.952,-100.7391;Float;True;Property;_MainTexture;Main Texture;2;0;Create;True;0;0;False;0;None;87473b44dad94a84e868a8983459cb6d;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;4;-219.1987,182.8002;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;6;-218.7664,-109.7021;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;8;-302.5231,-427.1623;Float;False;Property;_AlbedoColor;Albedo Color;0;1;[HDR];Create;True;0;0;False;0;0,0,0,0;0.7490196,0.372549,0.1058824,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;Gastón Zabala/Skills/RocketLauncher/ImpactBloom;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;False;0;False;Transparent;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;4;0;1;2
WireConnection;4;1;3;4
WireConnection;6;0;5;0
WireConnection;6;1;1;2
WireConnection;0;0;8;0
WireConnection;0;2;6;0
WireConnection;0;9;4;0
ASEEND*/
//CHKSM=AD09B0E18DD4FDD66364F916FDDA02229C99BFA1