// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Hidden/FIREPOWER/BlinkPostPro"
{
	Properties
	{
		_MainTex ( "Screen", 2D ) = "black" {}
		_FlickerSpeed("FlickerSpeed", Float) = 0.45
		_SpeedLines("SpeedLines", 2D) = "white" {}
		_LinesColor("LinesColor", Color) = (0.9512354,0.7028302,1,0)
		_LinesIntensity("LinesIntensity", Float) = 0.75
		_Activation("Activation", Range( 0 , 1)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
	}

	SubShader
	{
		
		
		ZTest Always
		Cull Back
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
			
			uniform float4 _LinesColor;
			uniform sampler2D _SpeedLines;
			uniform float _FlickerSpeed;
			uniform float _LinesIntensity;
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
				float4 tex2DNode61 = tex2D( _MainTex, uv_MainTex );
				float2 temp_cast_0 = (_FlickerSpeed).xx;
				float2 uv04 = i.uv.xy * float2( 1,1 ) + float2( 0,0 );
				float2 normalizeResult6 = normalize( ( uv04 - float2( 0.5,0.5 ) ) );
				float2 panner7 = ( 1.0 * _Time.y * temp_cast_0 + normalizeResult6);
				float4 lerpResult18 = lerp( tex2DNode61 , _LinesColor , ( ( tex2D( _SpeedLines, panner7 ) * ( distance( uv04 , float2( 0.5,0.5 ) ) + -0.3 ) ) * _LinesIntensity ));
				float4 lerpResult42 = lerp( tex2DNode61 , lerpResult18 , _Activation);
				

				finalColor = lerpResult42;

				return finalColor;
			} 
			ENDCG 
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=16700
101;443;1352;565;1475.794;161.2123;1;True;False
Node;AmplifyShaderEditor.CommentaryNode;23;-767.3174,-291.4345;Float;False;285;303;;1;5;Center of screen;1,1,1,1;0;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;4;-1339.855,-523.4775;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleSubtractOpNode;5;-717.3174,-241.4345;Float;False;2;0;FLOAT2;0,0;False;1;FLOAT2;0.5,0.5;False;1;FLOAT2;0
Node;AmplifyShaderEditor.NormalizeNode;6;-478.9332,-289.4267;Float;False;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;8;-471.9501,-36.65231;Float;False;Property;_FlickerSpeed;FlickerSpeed;0;0;Create;True;0;0;False;0;0.45;0.25;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;37;-1377.966,136.2822;Float;False;Constant;_Vector0;Vector 0;4;0;Create;True;0;0;False;0;0.5,0.5;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.DistanceOpNode;38;-997.5399,-6.099311;Float;False;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;7;-106.4115,-265.1138;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;28;-1046.137,262.3303;Float;False;Constant;_RadialGradient;RadialGradient;4;0;Create;True;0;0;False;0;-0.3;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;9;320.575,-263.5384;Float;True;Property;_SpeedLines;SpeedLines;1;0;Create;True;0;0;False;0;ca79c60ffbae8f243b27621b4366c44c;ca79c60ffbae8f243b27621b4366c44c;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;39;-706.1028,207.4729;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;14;739.9523,153.1078;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.TemplateShaderPropertyNode;62;688.7326,-796.594;Float;False;0;0;_MainTex;Shader;0;5;SAMPLER2D;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;19;864.6819,417.1949;Float;False;Property;_LinesIntensity;LinesIntensity;3;0;Create;True;0;0;False;0;0.75;0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;61;1059.735,-774.2083;Float;True;Property;_TextureSample2;Texture Sample 2;5;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;16;934.8172,-337.0154;Float;False;Property;_LinesColor;LinesColor;2;0;Create;True;0;0;False;0;0.9512354,0.7028302,1,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;21;1035.617,191.3715;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;18;1490.433,-274.2438;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,1,1,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;41;1240.762,542.3727;Float;False;Property;_Activation;Activation;4;0;Create;True;0;0;False;0;0;0.1854228;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;42;1772.652,-302.5389;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;46;2315.919,-328.1945;Float;False;True;2;Float;ASEMaterialInspector;0;7;Hidden/FIREPOWER/BlinkPostPro;c71b220b631b6344493ea3cf87110c93;True;SubShader 0 Pass 0;0;0;SubShader 0 Pass 0;1;False;False;False;True;0;False;-1;False;False;True;2;False;-1;True;7;False;-1;False;True;0;False;0;False;False;False;False;False;False;False;False;False;False;True;2;0;;0;0;Standard;0;0;1;True;False;1;0;FLOAT4;0,0,0,0;False;0
WireConnection;5;0;4;0
WireConnection;6;0;5;0
WireConnection;38;0;4;0
WireConnection;38;1;37;0
WireConnection;7;0;6;0
WireConnection;7;2;8;0
WireConnection;9;1;7;0
WireConnection;39;0;38;0
WireConnection;39;1;28;0
WireConnection;14;0;9;0
WireConnection;14;1;39;0
WireConnection;61;0;62;0
WireConnection;21;0;14;0
WireConnection;21;1;19;0
WireConnection;18;0;61;0
WireConnection;18;1;16;0
WireConnection;18;2;21;0
WireConnection;42;0;61;0
WireConnection;42;1;18;0
WireConnection;42;2;41;0
WireConnection;46;0;42;0
ASEEND*/
//CHKSM=37C16E05522A151817A38A9D9B95ED42626EB87A