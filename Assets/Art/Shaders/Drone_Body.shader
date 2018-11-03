// Upgrade NOTE: upgraded instancing buffer 'Drone_Body' to new syntax.

// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Drone_Body"
{
	Properties
	{
		[HideInInspector] __dirty( "", Int ) = 1
		_Size("Size", Float) = 33
		_Albedo("Albedo", 2D) = "white" {}
		_Metallic("Metallic", 2D) = "white" {}
		_Falloff("Falloff", Range( 0 , 20)) = 1.3
		[Header(VertexCollapse)]
		_Roughness("Roughness", 2D) = "white" {}
		_LifeEmission("Life Emission", 2D) = "white" {}
		_SkillStateColor("SkillStateColor", Color) = (0,1,1,0)
		_DefEmission("Def Emission", 2D) = "white" {}
		_CollapsePosition("Collapse Position", Vector) = (5000,5000,5000,0)
		_LifeRamp("LifeRamp", 2D) = "white" {}
		PlayerColorTexture("PlayerColorTexture", 2D) = "white" {}
		_Life("Life", Range( 0 , 1)) = 0
		_PlayerColor("PlayerColor", Color) = (0,0,0,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true" "SkillStateColor"="Defensive" "VertexCollapse"="true" }
		Cull Back
		CGPROGRAM
		#pragma target 3.0
		#pragma multi_compile_instancing
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows vertex:vertexDataFunc 
		struct Input
		{
			float2 uv_texcoord;
			float2 texcoord_0;
		};

		uniform sampler2D _Albedo;
		uniform float4 _Albedo_ST;
		uniform sampler2D PlayerColorTexture;
		uniform float4 PlayerColorTexture_ST;
		uniform sampler2D _LifeRamp;
		uniform sampler2D _LifeEmission;
		uniform float4 _LifeEmission_ST;
		uniform sampler2D _DefEmission;
		uniform float4 _DefEmission_ST;
		uniform float4 _SkillStateColor;
		uniform sampler2D _Metallic;
		uniform float4 _Metallic_ST;
		uniform sampler2D _Roughness;
		uniform float4 _Roughness_ST;
		uniform float _Size;
		uniform float _Falloff;

		UNITY_INSTANCING_BUFFER_START(Drone_Body)
			UNITY_DEFINE_INSTANCED_PROP(float4, _PlayerColor)
#define _PlayerColor_arr Drone_Body
			UNITY_DEFINE_INSTANCED_PROP(float, _Life)
#define _Life_arr Drone_Body
			UNITY_DEFINE_INSTANCED_PROP(float3, _CollapsePosition)
#define _CollapsePosition_arr Drone_Body
		UNITY_INSTANCING_BUFFER_END(Drone_Body)

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			o.texcoord_0.xy = v.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
			float3 ase_worldPos = mul(unity_ObjectToWorld, v.vertex);
			float3 _CollapsePosition_Instance = UNITY_ACCESS_INSTANCED_PROP(_CollapsePosition_arr, _CollapsePosition);
			float3 lerpResult12_g11 = lerp( float3( 0,0,0 ) , ( ( ase_worldPos + ( 1.0 - _CollapsePosition_Instance ) ) + float3(-1,-1,-1) ) , ( saturate( pow( ( distance( ase_worldPos , _CollapsePosition_Instance ) / _Size ) , _Falloff ) ) - 1.0 ));
			float4 transform15_g11 = mul(unity_WorldToObject,float4( lerpResult12_g11 , 0.0 ));
			v.vertex.xyz += transform15_g11.xyz;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_Albedo = i.uv_texcoord * _Albedo_ST.xy + _Albedo_ST.zw;
			o.Albedo = tex2D( _Albedo, uv_Albedo ).rgb;
			float2 uvPlayerColorTexture = i.uv_texcoord * PlayerColorTexture_ST.xy + PlayerColorTexture_ST.zw;
			float4 _PlayerColor_Instance = UNITY_ACCESS_INSTANCED_PROP(_PlayerColor_arr, _PlayerColor);
			float4 temp_cast_2 = (i.texcoord_0.x).xxxx;
			float _Life_Instance = UNITY_ACCESS_INSTANCED_PROP(_Life_arr, _Life);
			// *** BEGIN Flipbook UV Animation vars ***
			// Total tiles of Flipbook Texture
			float fbtotaltiles28 = 5.0 * 1.0;
			// Offsets for cols and rows of Flipbook Texture
			float fbcolsoffset28 = 1.0f / 5.0;
			float fbrowsoffset28 = 1.0f / 1.0;
			// Speed of animation
			float fbspeed28 = _Time[1] * 0.0;
			// UV Tiling (col and row offset)
			float2 fbtiling28 = float2(fbcolsoffset28, fbrowsoffset28);
			// UV Offset - calculate current tile linear index, and convert it to (X * coloffset, Y * rowoffset)
			// Calculate current tile linear index
			float fbcurrenttileindex28 = round( fmod( fbspeed28 + (3.8 + (_Life_Instance - 0.0) * (0.0 - 3.8) / (1.0 - 0.0)), fbtotaltiles28) );
			fbcurrenttileindex28 += ( fbcurrenttileindex28 < 0) ? fbtotaltiles28 : 0;
			// Obtain Offset X coordinate from current tile linear index
			float fblinearindextox28 = round ( fmod ( fbcurrenttileindex28, 5.0 ) );
			// Multiply Offset X by coloffset
			float fboffsetx28 = fblinearindextox28 * fbcolsoffset28;
			// Obtain Offset Y coordinate from current tile linear index
			float fblinearindextoy28 = round( fmod( ( fbcurrenttileindex28 - fblinearindextox28 ) / 5.0, 1.0 ) );
			// Reverse Y to get tiles from Top to Bottom
			fblinearindextoy28 = (int)(1.0-1) - fblinearindextoy28;
			// Multiply Offset Y by rowoffset
			float fboffsety28 = fblinearindextoy28 * fbrowsoffset28;
			// UV Offset
			float2 fboffset28 = float2(fboffsetx28, fboffsety28);
			// Flipbook UV
			half2 fbuv28 = temp_cast_2 * fbtiling28 + fboffset28;
			// *** END Flipbook UV Animation vars ***
			float2 uv_LifeEmission = i.uv_texcoord * _LifeEmission_ST.xy + _LifeEmission_ST.zw;
			float2 uv_DefEmission = i.uv_texcoord * _DefEmission_ST.xy + _DefEmission_ST.zw;
			o.Emission = ( saturate( ( ( tex2D( PlayerColorTexture, uvPlayerColorTexture ) * _PlayerColor_Instance ) * 0.7 ) ) + ( ( ( tex2D( _LifeRamp, fbuv28 ) * tex2D( _LifeEmission, uv_LifeEmission ) ) + ( tex2D( _DefEmission, uv_DefEmission ) * _SkillStateColor ) ) * 5.0 ) ).xyz;
			float2 uv_Metallic = i.uv_texcoord * _Metallic_ST.xy + _Metallic_ST.zw;
			o.Metallic = tex2D( _Metallic, uv_Metallic ).r;
			float2 uv_Roughness = i.uv_texcoord * _Roughness_ST.xy + _Roughness_ST.zw;
			o.Smoothness = ( 1.0 - tex2D( _Roughness, uv_Roughness ) ).r;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=13101
7;29;1199;638;718.0636;-624.8286;1;True;False
Node;AmplifyShaderEditor.RangedFloatNode;35;-2614.488,-458.1126;Float;False;InstancedProperty;_Life;Life;13;0;0;0;1;0;1;FLOAT
Node;AmplifyShaderEditor.TFHCRemap;38;-2220.62,-528.2474;Float;False;5;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;3;FLOAT;3.8;False;4;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.TextureCoordinatesNode;36;-2112.075,-795.5917;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.TFHCFlipBookUVAnimation;28;-1810.048,-621.0539;Float;False;0;0;5;0;FLOAT2;0,0;False;1;FLOAT;5.0;False;2;FLOAT;1.0;False;3;FLOAT;0.0;False;4;FLOAT;1.0;False;3;FLOAT2;FLOAT;FLOAT
Node;AmplifyShaderEditor.SamplerNode;29;-1371.883,-644.9987;Float;True;Property;_LifeRamp;LifeRamp;11;0;Assets/Art/Textures/LifeRampTest.png;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.ColorNode;41;-980.0304,-578.0621;Float;False;InstancedProperty;_PlayerColor;PlayerColor;14;0;0,0,0,0;0;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SamplerNode;40;-1028.382,-775.0621;Float;True;Property;PlayerColorTexture;PlayerColorTexture;13;0;Assets/Art/Textures/Beetledrone_Paint.png;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SamplerNode;15;-2102.454,507.7737;Float;True;Property;_DefEmission;Def Emission;10;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.ColorNode;17;-2026.107,823.8917;Float;False;Property;_SkillStateColor;SkillStateColor;9;0;0,1,1,0;0;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SamplerNode;3;-1691.199,-91.99553;Float;True;Property;_LifeEmission;Life Emission;7;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;39;-599.1735,-639.556;Float;False;2;2;0;FLOAT4;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;FLOAT4
Node;AmplifyShaderEditor.RangedFloatNode;46;-688.719,-417.0808;Float;False;Constant;_MultiplierEmission;MultiplierEmission;13;0;0.7;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;13;-1191.844,-211.771;Float;False;2;2;0;FLOAT4;0,0,0,0;False;1;COLOR;0.0,0,0,0;False;1;FLOAT4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;19;-1422.472,614.6387;Float;False;2;2;0;COLOR;0.0;False;1;COLOR;0.0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.SimpleAddOpNode;7;-1033.923,3.210692;Float;False;2;2;0;FLOAT4;0.0;False;1;COLOR;0.0,0,0,0;False;1;FLOAT4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;47;-332.7679,-349.6658;Float;False;2;2;0;FLOAT4;0.0;False;1;FLOAT;0,0,0,0;False;1;FLOAT4
Node;AmplifyShaderEditor.RangedFloatNode;20;-970.4698,219.6131;Float;False;Constant;_EmissionIntensity;Emission Intensity;7;0;5;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.CommentaryNode;37;-3443.131,-277.38;Float;False;708.882;676.98;oLD AS YOUR FUCKING MOM;4;10;8;11;12;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SamplerNode;5;-989.8559,886.8249;Float;True;Property;_Roughness;Roughness;6;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.Vector3Node;23;-370.4669,872.0783;Float;False;InstancedProperty;_CollapsePosition;Collapse Position;11;0;5000,5000,5000;0;4;FLOAT3;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.WorldPosInputsNode;24;-383.2232,1087.705;Float;False;0;4;FLOAT3;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;21;-562.3788,-178.256;Float;False;2;2;0;FLOAT4;0.0;False;1;FLOAT;0.0,0,0,0;False;1;FLOAT4
Node;AmplifyShaderEditor.SaturateNode;50;-145.6347,-278.7605;Float;False;1;0;FLOAT4;0.0;False;1;FLOAT4
Node;AmplifyShaderEditor.LerpOp;8;-2999.248,-113.8482;Float;True;3;0;COLOR;0.0;False;1;COLOR;0.0,0,0,0;False;2;FLOAT;0.0;False;1;COLOR
Node;AmplifyShaderEditor.SamplerNode;1;-1037.452,-995.3718;Float;True;Property;_Albedo;Albedo;0;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;48;154.9778,-239.6306;Float;False;2;2;0;FLOAT4;0.0,0,0,0;False;1;FLOAT4;0.0,0,0,0;False;1;FLOAT4
Node;AmplifyShaderEditor.OneMinusNode;6;-269.6087,597.8453;Float;False;1;0;COLOR;0.0;False;1;COLOR
Node;AmplifyShaderEditor.ColorNode;10;-3393.131,-62.25648;Float;False;Constant;_Alive;Alive;5;0;0,1,0,0;0;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;12;-3380.899,141.6001;Float;False;InstancedProperty;_LifeOld;LifeOld;8;0;1;0;1;0;1;FLOAT
Node;AmplifyShaderEditor.FunctionNode;22;-93.79565,1011.847;Float;False;VertexCollapse;2;;11;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT4
Node;AmplifyShaderEditor.ColorNode;11;-3393.131,-227.3799;Float;False;Constant;_Dead;Dead;5;0;1,0,0,0;0;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SamplerNode;4;-995.9717,697.2382;Float;True;Property;_Metallic;Metallic;1;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;457.3707,-240.0998;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;Drone_Body;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;0;False;0;0;Opaque;0.5;True;True;0;False;Opaque;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;False;0;4;10;25;False;0.5;True;0;Zero;Zero;0;Zero;Zero;Add;Add;0;False;0;0,0,0,0;VertexOffset;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;2;SkillStateColor=Defensive;VertexCollapse=true;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;OBJECT;0.0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;38;0;35;0
WireConnection;28;0;36;1
WireConnection;28;4;38;0
WireConnection;29;1;28;0
WireConnection;39;0;40;0
WireConnection;39;1;41;0
WireConnection;13;0;29;0
WireConnection;13;1;3;0
WireConnection;19;0;15;0
WireConnection;19;1;17;0
WireConnection;7;0;13;0
WireConnection;7;1;19;0
WireConnection;47;0;39;0
WireConnection;47;1;46;0
WireConnection;21;0;7;0
WireConnection;21;1;20;0
WireConnection;50;0;47;0
WireConnection;8;0;11;0
WireConnection;8;1;10;0
WireConnection;8;2;12;0
WireConnection;48;0;50;0
WireConnection;48;1;21;0
WireConnection;6;0;5;0
WireConnection;22;0;23;0
WireConnection;22;1;24;0
WireConnection;0;0;1;0
WireConnection;0;2;48;0
WireConnection;0;3;4;0
WireConnection;0;4;6;0
WireConnection;0;11;22;0
ASEEND*/
//CHKSM=DF05D7C9FB4B2077979BBE1705A0EF765ACDDBE6