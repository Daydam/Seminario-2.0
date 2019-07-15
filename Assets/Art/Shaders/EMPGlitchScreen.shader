// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Hidden/FIREPOWER/EMPGlitchScreen"
{
	Properties
	{
		_MainTex ( "Screen", 2D ) = "black" {}
		_UVModifier("UVModifier", 2D) = "white" {}
		_GlitchAmount("GlitchAmount", Range( 1 , 10)) = 0
		_GlitchStrenght("GlitchStrenght", Float) = 0
		_ChromaticAberrationStrenght("ChromaticAberrationStrenght", Float) = 0
		_TimeScale("Time Scale", Float) = 0
		[Toggle]_ActivateGlitch("Activate Glitch", Float) = 1
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
			
			uniform float _ActivateGlitch;
			uniform sampler2D _UVModifier;
			uniform float _GlitchAmount;
			uniform float _TimeScale;
			uniform float _GlitchStrenght;
			uniform float _ChromaticAberrationStrenght;

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
				float4 tex2DNode54 = tex2D( _MainTex, uv_MainTex );
				float2 uv037 = i.uv.xy * float2( 1,1 ) + float2( 0,0 );
				float2 temp_cast_0 = (_GlitchAmount).xx;
				float2 uv036 = i.uv.xy * temp_cast_0 + float2( 0,0 );
				float mulTime75 = _Time.y * _TimeScale;
				float temp_output_76_0 = sin( mulTime75 );
				float2 lerpResult40 = lerp( uv037 , (tex2D( _UVModifier, uv036 )).rg , ( ( temp_output_76_0 * 0.1 ) * _GlitchStrenght ));
				float4 tex2DNode3 = tex2D( _MainTex, lerpResult40 );
				float4 normalizeResult107 = normalize( ( tex2DNode54 * tex2DNode3.r ) );
				float2 uv065 = i.uv.xy * float2( 1,1 ) + float2( 0,0 );
				float2 appendResult67 = (float2(tex2DNode54.r , tex2D( _MainTex, ( _ChromaticAberrationStrenght * ( uv065 + ( temp_output_76_0 * 0.01 ) ) ) ).g));
				float3 appendResult69 = (float3(appendResult67 , tex2DNode54.b));
				float4 lerpResult70 = lerp( normalizeResult107 , float4( appendResult69 , 0.0 ) , tex2DNode3.r);
				

				finalColor = lerp(tex2DNode54,lerpResult70,_ActivateGlitch);

				return finalColor;
			} 
			ENDCG 
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=16700
0;387;1278;960;1016.097;961.8901;2.949484;True;False
Node;AmplifyShaderEditor.RangedFloatNode;77;-1853.322,-142.6346;Float;False;Property;_TimeScale;Time Scale;6;0;Create;True;0;0;False;0;0;30;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;52;-2030.1,164.1098;Float;False;1907.368;878.8286;Puto;10;48;49;36;11;37;39;40;71;80;100;GlitchTiling;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleTimeNode;75;-1648.844,-140.017;Float;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;48;-1953.125,853.8789;Float;False;Property;_GlitchAmount;GlitchAmount;2;0;Create;True;0;0;False;0;0;1;1;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;76;-1440.844,-141.617;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;34;-1376.035,-840.6206;Float;False;955.6104;519.0493;;5;50;72;64;65;66;Chromatic Aberration;1,1,1,1;0;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;36;-1578.688,827.5387;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;0.15,0.15;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;49;-1200.846,631.9074;Float;False;Property;_GlitchStrenght;GlitchStrenght;3;0;Create;True;0;0;False;0;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;71;-1092.635,501.6684;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0.1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;11;-1304.15,798.2353;Float;True;Property;_UVModifier;UVModifier;0;0;Create;True;0;0;False;0;None;b1800c80f00d7f5419921069fc6419fe;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;65;-1300.562,-589.9279;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;64;-1004.375,-432.948;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0.01;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;66;-845.5464,-507.1975;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;39;-879.2472,501.4815;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0.1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;100;-801.9554,797.3493;Float;False;True;True;False;False;1;0;COLOR;0,0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;37;-950.403,241.6138;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;50;-1115.184,-707.2058;Float;False;Property;_ChromaticAberrationStrenght;ChromaticAberrationStrenght;4;0;Create;True;0;0;False;0;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;108;-78.57169,-783.7128;Float;False;589.78;280;;2;54;1;Main Texture;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;72;-585.6078,-485.8143;Float;False;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TemplateShaderPropertyNode;1;-28.57171,-725.7609;Float;False;0;0;_MainTex;Shader;0;5;SAMPLER2D;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;40;-449.3654,460.7024;Float;True;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;3;387.356,422.5193;Float;True;Property;_TextureSample0;Texture Sample 0;5;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Instance;54;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;7;181.0144,-395.7145;Float;True;Property;_TextureSample1;Texture Sample 1;5;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Instance;54;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;54;190.2083,-733.7128;Float;True;Property;_TextureSample2;Texture Sample 2;5;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;104;1540.182,-418.2988;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.DynamicAppendNode;67;794.8276,-529.3781;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.NormalizeNode;107;1719.413,-418.7289;Float;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.DynamicAppendNode;69;1026.399,-356.8165;Float;False;FLOAT3;4;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.CommentaryNode;53;233.821,1028.035;Float;False;371;280;Comment;1;41;El peluquero Gastón Gimenez;1,1,1,1;0;0
Node;AmplifyShaderEditor.LerpOp;70;1981.464,-278.8952;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;111;1152.112,346.409;Float;False;571.577;297.0004;Usar sólo cuando el Normalize no sea suficiente para aclarar la imagen;3;94;93;92;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;101;2468.93,-808.4318;Float;False;305;188;Usar Switch es malo;1;86;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SmoothstepOpNode;92;1523.689,432.9286;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;94;1211.112,528.4094;Float;False;Property;_Max;Max;10;0;Create;True;0;0;False;0;0;0.22;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ToggleSwitchNode;86;2518.93,-758.4318;Float;False;Property;_ActivateGlitch;Activate Glitch;8;0;Create;True;0;0;False;0;1;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;41;303.4255,1097.639;Float;True;Property;_GASTONGIMENEZ;GASTON GIMENEZ;1;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.IntNode;57;2301.276,-127.1157;Float;False;Property;_Activation;Activation;5;0;Create;True;0;0;False;0;0;1;0;1;INT;0
Node;AmplifyShaderEditor.CeilOpNode;58;2519.093,-119.1078;Float;False;1;0;INT;0;False;1;INT;0
Node;AmplifyShaderEditor.LerpOp;56;2571.664,-470.1328;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.Vector2Node;80;-1883.192,692.9305;Float;False;Property;_Tiling;Tiling;7;0;Create;True;0;0;False;0;0,0;3,3;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RangedFloatNode;93;1202.112,396.409;Float;False;Property;_Min;Min;9;0;Create;True;0;0;False;0;0;0.02;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;0;3116.321,-496.7448;Float;False;True;2;Float;ASEMaterialInspector;0;7;Hidden/FIREPOWER/EMPGlitchScreen;c71b220b631b6344493ea3cf87110c93;True;SubShader 0 Pass 0;0;0;SubShader 0 Pass 0;1;False;False;False;True;2;False;-1;False;False;True;2;False;-1;True;7;False;-1;False;True;0;False;0;False;False;False;False;False;False;False;False;False;False;True;2;0;;0;0;Standard;0;0;1;True;False;1;0;FLOAT4;0,0,0,0;False;0
WireConnection;75;0;77;0
WireConnection;76;0;75;0
WireConnection;36;0;48;0
WireConnection;71;0;76;0
WireConnection;11;1;36;0
WireConnection;64;0;76;0
WireConnection;66;0;65;0
WireConnection;66;1;64;0
WireConnection;39;0;71;0
WireConnection;39;1;49;0
WireConnection;100;0;11;0
WireConnection;72;0;50;0
WireConnection;72;1;66;0
WireConnection;40;0;37;0
WireConnection;40;1;100;0
WireConnection;40;2;39;0
WireConnection;3;1;40;0
WireConnection;7;1;72;0
WireConnection;54;0;1;0
WireConnection;104;0;54;0
WireConnection;104;1;3;1
WireConnection;67;0;54;1
WireConnection;67;1;7;2
WireConnection;107;0;104;0
WireConnection;69;0;67;0
WireConnection;69;2;54;3
WireConnection;70;0;107;0
WireConnection;70;1;69;0
WireConnection;70;2;3;1
WireConnection;92;0;3;1
WireConnection;92;1;93;0
WireConnection;92;2;94;0
WireConnection;86;0;54;0
WireConnection;86;1;70;0
WireConnection;41;1;40;0
WireConnection;58;0;57;0
WireConnection;56;1;70;0
WireConnection;56;2;58;0
WireConnection;0;0;86;0
ASEEND*/
//CHKSM=84257ED5B3561C018CB12A195EA06C089521EFC5