// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Hidden/FIREPOWER/EMPGlitchScreen"
{
	Properties
	{
		_MainTex ( "Screen", 2D ) = "black" {}
		_UVModifier("UVModifier", 2D) = "white" {}
		_GlitchStrenght("GlitchStrenght", Float) = 0
		_ChromaticAberrationStrenght("ChromaticAberrationStrenght", Float) = 0
		_TimeScale("Time Scale", Float) = 0
		_Tiling("Tiling", Vector) = (0,0,0,0)
		_Min("Min", Float) = 0
		_Max("Max", Float) = 0
		_Activation("Activation", Range( 0 , 1)) = 0
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
			
			uniform float _Min;
			uniform float _Max;
			uniform sampler2D _UVModifier;
			uniform float2 _Tiling;
			uniform float _TimeScale;
			uniform float _GlitchStrenght;
			uniform float _ChromaticAberrationStrenght;
			uniform float _Activation;

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
				float2 uv036 = i.uv.xy * _Tiling + float2( 0,0 );
				float mulTime75 = _Time.y * _TimeScale;
				float temp_output_76_0 = sin( mulTime75 );
				float2 lerpResult40 = lerp( uv037 , (tex2D( _UVModifier, uv036 )).rg , ( ( temp_output_76_0 * 0.1 ) * _GlitchStrenght ));
				float4 tex2DNode3 = tex2D( _MainTex, lerpResult40 );
				float smoothstepResult92 = smoothstep( _Min , _Max , tex2DNode3.r);
				float2 uv065 = i.uv.xy * float2( 1,1 ) + float2( 0,0 );
				float4 tex2DNode7 = tex2D( _MainTex, ( _ChromaticAberrationStrenght * ( uv065 + ( temp_output_76_0 * 0.01 ) ) ) );
				float4 appendResult113 = (float4(tex2DNode54.r , tex2DNode7.g , tex2DNode54.b , 0.0));
				float4 lerpResult70 = lerp( ( tex2DNode54 * smoothstepResult92 ) , appendResult113 , tex2DNode3.r);
				float4 lerpResult56 = lerp( tex2DNode54 , lerpResult70 , ceil( _Activation ));
				

				finalColor = lerpResult56;

				return finalColor;
			} 
			ENDCG 
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=16700
39;84;1352;698;779.7447;1033.729;1.775364;True;False
Node;AmplifyShaderEditor.RangedFloatNode;77;-1853.322,-142.6346;Float;False;Property;_TimeScale;Time Scale;4;0;Create;True;0;0;False;0;0;20;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;52;-2030.1,164.1098;Float;False;1907.368;878.8286;Puto;10;48;49;36;11;37;39;40;71;80;100;GlitchTiling;1,1,1,1;0;0
Node;AmplifyShaderEditor.Vector2Node;80;-1883.192,692.9305;Float;False;Property;_Tiling;Tiling;5;0;Create;True;0;0;False;0;0,0;3,3;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleTimeNode;75;-1648.844,-140.017;Float;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;36;-1578.688,827.5387;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;0.15,0.15;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SinOpNode;76;-1440.844,-141.617;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;71;-1092.635,501.6684;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0.1;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;34;-1376.035,-840.6206;Float;False;955.6104;519.0493;;5;50;72;64;65;66;Chromatic Aberration;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;49;-1200.846,631.9074;Float;False;Property;_GlitchStrenght;GlitchStrenght;2;0;Create;True;0;0;False;0;0;1.11;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;11;-1304.15,798.2353;Float;True;Property;_UVModifier;UVModifier;0;0;Create;True;0;0;False;0;None;f7297a5a96d54ef44b95dc43219604f6;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;65;-1300.562,-589.9279;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;64;-1004.375,-432.948;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0.01;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;37;-950.403,241.6138;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ComponentMaskNode;100;-801.9554,797.3493;Float;False;True;True;False;False;1;0;COLOR;0,0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;39;-879.2472,501.4815;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0.1;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;108;-78.57169,-783.7128;Float;False;589.78;280;;2;54;1;Main Texture;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;50;-1115.184,-707.2058;Float;False;Property;_ChromaticAberrationStrenght;ChromaticAberrationStrenght;3;0;Create;True;0;0;False;0;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;66;-845.5464,-507.1975;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.LerpOp;40;-449.3654,460.7024;Float;True;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;111;950.3314,355.9045;Float;False;571.577;297.0004;Usar sólo cuando el Normalize no sea suficiente para aclarar la imagen;3;94;93;92;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;72;-585.6078,-485.8143;Float;False;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;94;1009.331,537.905;Float;False;Property;_Max;Max;7;0;Create;True;0;0;False;0;0;0.27;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;93;1000.331,405.9045;Float;False;Property;_Min;Min;6;0;Create;True;0;0;False;0;0;-0.17;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;3;387.356,422.5193;Float;True;Property;_TextureSample0;Texture Sample 0;5;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Instance;54;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TemplateShaderPropertyNode;1;-23.24562,-725.7609;Float;False;0;0;_MainTex;Shader;0;5;SAMPLER2D;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SmoothstepOpNode;92;1321.908,442.4241;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;54;190.2083,-735.416;Float;True;Property;_TextureSample2;Texture Sample 2;5;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;7;138.0996,-259.5071;Float;True;Property;_TextureSample1;Texture Sample 1;5;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Instance;54;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;112;2235.053,4.772994;Float;False;Property;_Activation;Activation;8;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;104;1540.182,-418.2988;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.DynamicAppendNode;113;767.1467,-264.5887;Float;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.CeilOpNode;58;2537.827,-56.09161;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;70;2022.339,-284.0046;Float;False;3;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.DynamicAppendNode;67;794.8276,-529.3781;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;48;-1953.125,853.8789;Float;False;Property;_GlitchAmount;GlitchAmount;1;0;Create;True;0;0;False;0;0;10;1;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;69;1026.399,-356.8165;Float;False;FLOAT3;4;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;56;2571.664,-470.1328;Float;False;3;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;0;3116.321,-496.7448;Float;False;True;2;Float;ASEMaterialInspector;0;7;Hidden/FIREPOWER/EMPGlitchScreen;c71b220b631b6344493ea3cf87110c93;True;SubShader 0 Pass 0;0;0;SubShader 0 Pass 0;1;False;False;False;True;2;False;-1;False;False;True;2;False;-1;True;7;False;-1;False;True;0;False;0;False;False;False;False;False;False;False;False;False;False;True;2;0;;0;0;Standard;0;0;1;True;False;1;0;FLOAT4;0,0,0,0;False;0
WireConnection;75;0;77;0
WireConnection;36;0;80;0
WireConnection;76;0;75;0
WireConnection;71;0;76;0
WireConnection;11;1;36;0
WireConnection;64;0;76;0
WireConnection;100;0;11;0
WireConnection;39;0;71;0
WireConnection;39;1;49;0
WireConnection;66;0;65;0
WireConnection;66;1;64;0
WireConnection;40;0;37;0
WireConnection;40;1;100;0
WireConnection;40;2;39;0
WireConnection;72;0;50;0
WireConnection;72;1;66;0
WireConnection;3;1;40;0
WireConnection;92;0;3;1
WireConnection;92;1;93;0
WireConnection;92;2;94;0
WireConnection;54;0;1;0
WireConnection;7;1;72;0
WireConnection;104;0;54;0
WireConnection;104;1;92;0
WireConnection;113;0;54;1
WireConnection;113;1;7;2
WireConnection;113;2;54;3
WireConnection;58;0;112;0
WireConnection;70;0;104;0
WireConnection;70;1;113;0
WireConnection;70;2;3;1
WireConnection;67;0;54;1
WireConnection;67;1;7;2
WireConnection;69;0;67;0
WireConnection;69;2;54;3
WireConnection;56;0;54;0
WireConnection;56;1;70;0
WireConnection;56;2;58;0
WireConnection;0;0;56;0
ASEEND*/
//CHKSM=91AF6ACE0197522C324ADDB1C99AC1A901B36255