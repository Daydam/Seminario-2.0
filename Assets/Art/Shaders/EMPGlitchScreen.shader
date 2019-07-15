// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Hidden/FIREPOWER/EMPGlitchScreen"
{
	Properties
	{
		_MainTex ( "Screen", 2D ) = "black" {}
		_UVModifier("UVModifier", 2D) = "white" {}
		_GlitchAmount("GlitchAmount", Range( 0 , 1)) = 0
		_GlitchStrenght("GlitchStrenght", Float) = 0
		_ChromaticAberrationStrenght("ChromaticAberrationStrenght", Float) = 0
		_Activation("Activation", Int) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
	}

	SubShader
	{
		
		
		ZTest Always
		Cull Off
		ZWrite Off

		
		Pass
		{ 
			CGPROGRAM 

			#pragma vertex vert_img_custom 
			#pragma fragment frag
			#pragma target 3.0
			#include "UnityCG.cginc"
			#include "UnityShaderVariables.cginc"


			struct appdata_img_custom
			{
				float4 vertex : POSITION;
				half2 texcoord : TEXCOORD0;
				
			};

			struct v2f_img_custom
			{
				float4 pos : SV_POSITION;
				half2 uv   : TEXCOORD0;
				half2 stereoUV : TEXCOORD2;
		#if UNITY_UV_STARTS_AT_TOP
				half4 uv2 : TEXCOORD1;
				half4 stereoUV2 : TEXCOORD3;
		#endif
				
			};

			uniform sampler2D _MainTex;
			uniform half4 _MainTex_TexelSize;
			uniform half4 _MainTex_ST;
			
			uniform sampler2D _UVModifier;
			uniform float _GlitchAmount;
			uniform float _GlitchStrenght;
			uniform float _ChromaticAberrationStrenght;
			uniform int _Activation;

			v2f_img_custom vert_img_custom ( appdata_img_custom v  )
			{
				v2f_img_custom o;
				
				o.pos = UnityObjectToClipPos ( v.vertex );
				o.uv = float4( v.texcoord.xy, 1, 1 );

				#if UNITY_UV_STARTS_AT_TOP
					o.uv2 = float4( v.texcoord.xy, 1, 1 );
					o.stereoUV2 = UnityStereoScreenSpaceUVAdjust ( o.uv2, _MainTex_ST );

					if ( _MainTex_TexelSize.y < 0.0 )
						o.uv.y = 1.0 - o.uv.y;
				#endif
				o.stereoUV = UnityStereoScreenSpaceUVAdjust ( o.uv, _MainTex_ST );
				return o;
			}

			half4 frag ( v2f_img_custom i ) : SV_Target
			{
				#ifdef UNITY_UV_STARTS_AT_TOP
					half2 uv = i.uv2;
					half2 stereoUV = i.stereoUV2;
				#else
					half2 uv = i.uv;
					half2 stereoUV = i.stereoUV;
				#endif	
				
				half4 finalColor;

				// ase common template code
				float2 uv_MainTex = i.uv.xy * _MainTex_ST.xy + _MainTex_ST.zw;
				float2 uv37 = i.uv.xy * float2( 1,1 ) + float2( 0,0 );
				float2 temp_cast_1 = (_GlitchAmount).xx;
				float2 uv36 = i.uv.xy * temp_cast_1 + float2( 0,0 );
				float2 lerpResult40 = lerp( uv37 , (tex2D( _UVModifier, uv36 )).rg , ( _SinTime.w * _GlitchStrenght ));
				float4 tex2DNode3 = tex2D( _MainTex, lerpResult40 );
				float4 break28 = tex2DNode3;
				float lerpResult30 = lerp( _ChromaticAberrationStrenght , ( _ChromaticAberrationStrenght * -1.0 ) , _SinTime.w);
				float2 temp_cast_2 = (lerpResult30).xx;
				float2 uv32 = i.uv.xy * float2( 1,1 ) + temp_cast_2;
				float4 tex2DNode7 = tex2D( _MainTex, uv32 );
				float4 appendResult27 = (float4(break28.r , tex2DNode7.g , break28.b , 1.0));
				float4 lerpResult56 = lerp( tex2D( _MainTex, uv_MainTex ) , appendResult27 , (float)ceil( _Activation ));
				

				finalColor = lerpResult56;

				return finalColor;
			} 
			ENDCG 
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=16100
245;118;1364;704;1184.101;749.6463;1.964537;True;False
Node;AmplifyShaderEditor.CommentaryNode;52;-2030.1,19.68276;Float;False;1907.368;878.8286;Puto;8;48;49;36;11;37;39;40;102;GlitchTiling;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;48;-1980.1,536.7134;Float;False;Property;_GlitchAmount;GlitchAmount;2;0;Create;True;0;0;False;0;0;0.28;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;36;-1532.488,599.5114;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;0.15,0.15;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;49;-1545.365,240.571;Float;False;Property;_GlitchStrenght;GlitchStrenght;3;0;Create;True;0;0;False;0;0;0.2659515;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;11;-1128.15,603.2079;Float;True;Property;_UVModifier;UVModifier;0;0;Create;True;0;0;False;0;f7297a5a96d54ef44b95dc43219604f6;f7297a5a96d54ef44b95dc43219604f6;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SinTimeNode;20;-1513.857,-305.0302;Float;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;50;-1859.769,-919.9906;Float;False;Property;_ChromaticAberrationStrenght;ChromaticAberrationStrenght;4;0;Create;True;0;0;False;0;0;0.02923284;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;102;-672.8131,575.0632;Float;False;True;True;False;False;1;0;COLOR;0,0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;39;-1267.045,163.2282;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0.1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;37;-942.6031,50.38671;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;51;-1405.789,-688.4917;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;-1;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;34;-1099.806,-761.5437;Float;False;955.6104;519.0493;;2;32;30;Chromatic Aberration;1,1,1,1;0;0
Node;AmplifyShaderEditor.LerpOp;40;-387.7327,315.2754;Float;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TemplateShaderPropertyNode;1;39.47207,-699.9681;Float;False;0;0;_MainTex;Shader;0;5;SAMPLER2D;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;30;-1049.806,-712.6933;Float;False;3;0;FLOAT;0.001;False;1;FLOAT;-0.001;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;3;588.8388,-356.9935;Float;True;Property;_TextureSample0;Texture Sample 0;0;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;32;-404.1953,-541.4944;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.BreakToComponentsNode;28;942.42,-464.6693;Float;False;COLOR;1;0;COLOR;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.IntNode;57;2311.276,-127.1157;Float;False;Property;_Activation;Activation;5;0;Create;True;0;0;False;0;0;1;0;1;INT;0
Node;AmplifyShaderEditor.SamplerNode;7;588.2355,-120.7683;Float;True;Property;_TextureSample1;Texture Sample 1;0;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;54;1257.95,-938.9193;Float;True;Property;_TextureSample2;Texture Sample 2;5;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CeilOpNode;58;2519.093,-119.1078;Float;False;1;0;INT;0;False;1;INT;0
Node;AmplifyShaderEditor.CommentaryNode;53;233.821,1028.035;Float;False;371;280;Comment;1;41;El peluquero Gastón Gimenez;1,1,1,1;0;0
Node;AmplifyShaderEditor.DynamicAppendNode;27;1563.692,-619.1093;Float;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;1;False;1;FLOAT4;0
Node;AmplifyShaderEditor.LerpOp;100;1104.194,-99.66711;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;41;303.4255,1097.639;Float;True;Property;_GASTONGIMENEZ;GASTON GIMENEZ;1;0;Create;True;0;0;False;0;402a6d7fdbb28a747a9ef437da17d619;402a6d7fdbb28a747a9ef437da17d619;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;56;2791.44,-470.1328;Float;False;3;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;0;3116.321,-496.7448;Float;False;True;2;Float;ASEMaterialInspector;0;2;Hidden/FIREPOWER/EMPGlitchScreen;c71b220b631b6344493ea3cf87110c93;0;0;SubShader 0 Pass 0;1;False;False;True;2;False;-1;False;False;True;2;False;-1;True;7;False;-1;False;True;0;False;0;False;False;False;False;False;False;False;False;False;True;2;0;;0;0;Standard;0;1;0;FLOAT4;0,0,0,0;False;0
WireConnection;36;0;48;0
WireConnection;11;1;36;0
WireConnection;102;0;11;0
WireConnection;39;0;20;4
WireConnection;39;1;49;0
WireConnection;51;0;50;0
WireConnection;40;0;37;0
WireConnection;40;1;102;0
WireConnection;40;2;39;0
WireConnection;30;0;50;0
WireConnection;30;1;51;0
WireConnection;30;2;20;4
WireConnection;3;0;1;0
WireConnection;3;1;40;0
WireConnection;32;1;30;0
WireConnection;28;0;3;0
WireConnection;7;0;1;0
WireConnection;7;1;32;0
WireConnection;54;0;1;0
WireConnection;58;0;57;0
WireConnection;27;0;28;0
WireConnection;27;1;7;2
WireConnection;27;2;28;2
WireConnection;100;0;3;0
WireConnection;100;1;7;0
WireConnection;100;2;7;2
WireConnection;41;1;40;0
WireConnection;56;0;54;0
WireConnection;56;1;27;0
WireConnection;56;2;58;0
WireConnection;0;0;56;0
ASEEND*/
//CHKSM=94CB330EBBF003BD0FEE3937E00515AC738B3EC7