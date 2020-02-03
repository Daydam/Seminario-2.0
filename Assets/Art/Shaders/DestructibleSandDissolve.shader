// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "DestructibleSandDissolve"
{
	Properties
	{
		_AlbedoBase("AlbedoBase", 2D) = "white" {}
		_NormalBase("NormalBase", 2D) = "white" {}
		_SandTex("SandTex", 2D) = "white" {}
		_SandAmount("SandAmount", Range( 0 , 1)) = 0
		_SandNormal("SandNormal", 2D) = "white" {}
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _NormalBase;
		uniform float4 _NormalBase_ST;
		uniform sampler2D _SandNormal;
		uniform float4 _SandNormal_ST;
		uniform sampler2D _TextureSample0;
		uniform float4 _TextureSample0_ST;
		uniform float _SandAmount;
		uniform sampler2D _AlbedoBase;
		uniform float4 _AlbedoBase_ST;
		uniform sampler2D _SandTex;
		uniform float4 _SandTex_ST;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_NormalBase = i.uv_texcoord * _NormalBase_ST.xy + _NormalBase_ST.zw;
			float2 uv_SandNormal = i.uv_texcoord * _SandNormal_ST.xy + _SandNormal_ST.zw;
			float2 uv_TextureSample0 = i.uv_texcoord * _TextureSample0_ST.xy + _TextureSample0_ST.zw;
			float4 lerpResult23 = lerp( float4( 0,0,0,0 ) , tex2D( _TextureSample0, uv_TextureSample0 ) , _SandAmount);
			float4 lerpResult8 = lerp( tex2D( _NormalBase, uv_NormalBase ) , tex2D( _SandNormal, uv_SandNormal ) , lerpResult23);
			o.Normal = lerpResult8.rgb;
			float2 uv_AlbedoBase = i.uv_texcoord * _AlbedoBase_ST.xy + _AlbedoBase_ST.zw;
			float2 uv_SandTex = i.uv_texcoord * _SandTex_ST.xy + _SandTex_ST.zw;
			float4 lerpResult5 = lerp( tex2D( _AlbedoBase, uv_AlbedoBase ) , tex2D( _SandTex, uv_SandTex ) , lerpResult23);
			o.Albedo = lerpResult5.rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16700
-144;348;1276;496;3046.169;431.8344;3.502292;True;True
Node;AmplifyShaderEditor.RangedFloatNode;6;-1667.614,617.6401;Float;False;Property;_SandAmount;SandAmount;3;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;24;-1380.739,858.3308;Float;True;Property;_TextureSample0;Texture Sample 0;5;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;23;-1081.897,674.1587;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;3;-949.879,-372.297;Float;True;Property;_SandTex;SandTex;2;0;Create;True;0;0;False;0;c5e688ba328fca54882b06cd6d6c69e7;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;2;-820.6218,-129.935;Float;True;Property;_NormalBase;NormalBase;1;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;1;-947.9183,-605.1382;Float;True;Property;_AlbedoBase;AlbedoBase;0;0;Create;True;0;0;False;0;ba111d5e6cefed3488435fbf554493e3;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;7;-958.6255,325.696;Float;True;Property;_SandNormal;SandNormal;4;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;22;-816.0628,665.4716;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;5;-392.2321,-366.4279;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;8;-385.16,11.96387;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;149.1016,-33.88672;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;DestructibleSandDissolve;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;23;1;24;0
WireConnection;23;2;6;0
WireConnection;5;0;1;0
WireConnection;5;1;3;0
WireConnection;5;2;23;0
WireConnection;8;0;2;0
WireConnection;8;1;7;0
WireConnection;8;2;23;0
WireConnection;0;0;5;0
WireConnection;0;1;8;0
ASEEND*/
//CHKSM=0FF89C833AD6F328FF1DDFEB76FFF56C313E19C9