// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Hidden/FIREPOWER/RepulsionScreen"
{
	Properties
	{
		_MainTex ( "Screen", 2D ) = "black" {}
		_PlayerPosition("PlayerPosition", Vector) = (0,0,0,0)
		_RepulsionRadius("RepulsionRadius", Float) = 3
		_Falloff("Falloff", Float) = 0.1
		_Activation("Activation", Range( 0 , 1)) = 0
		_Speed("Speed", Float) = 0
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
				float4 ase_texcoord4 : TEXCOORD4;
			};

			uniform sampler2D _MainTex;
			uniform half4 _MainTex_TexelSize;
			uniform half4 _MainTex_ST;
			
			uniform float _Speed;
			uniform float3 _PlayerPosition;
			uniform float _RepulsionRadius;
			uniform float _Falloff;
			uniform float _Activation;

			v2f_img_custom vert_img_custom ( appdata_img_custom v  )
			{
				v2f_img_custom o;
				float3 ase_worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				o.ase_texcoord4.xyz = ase_worldPos;
				
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord4.w = 0;
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
				float4 tex2DNode25 = tex2D( _MainTex, uv_MainTex );
				float4 color19 = IsGammaSpace() ? float4(0.0235849,0.9629096,1,0) : float4(0.001825457,0.9176907,1,0);
				float mulTime56 = _Time.y * _Speed;
				float3 ase_worldPos = i.ase_texcoord4.xyz;
				float4 lerpResult21 = lerp( tex2DNode25 , color19 , (0.6 + (pow( frac( ( mulTime56 + ( distance( _PlayerPosition , ase_worldPos ) / _RepulsionRadius ) ) ) , _Falloff ) - 0.0) * (0.4 - 0.6) / (0.3 - 0.0)));
				float4 lerpResult22 = lerp( tex2DNode25 , lerpResult21 , ceil( _Activation ));
				

				finalColor = lerpResult22;

				return finalColor;
			} 
			ENDCG 
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=16700
137;4;1364;704;661.1514;1952.364;1.897415;False;False
Node;AmplifyShaderEditor.Vector3Node;2;-1399.531,-1191.442;Float;False;Property;_PlayerPosition;PlayerPosition;0;0;Create;True;0;0;False;0;0,0,0;10.47109,1.4804,-10.04108;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.WorldPosInputsNode;9;-1394.467,-692.4493;Float;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;55;-370.3661,-1471.456;Float;False;Property;_Speed;Speed;4;0;Create;True;0;0;False;0;0;0.75;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;3;-361.7581,-954.669;Float;False;Property;_RepulsionRadius;RepulsionRadius;1;0;Create;True;0;0;False;0;3;12;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DistanceOpNode;11;-686.2057,-1083.723;Float;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;56;-92.2229,-1394.789;Float;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;13;-126.6198,-1078.656;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;53;189.4919,-1274.33;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FractNode;54;513.9873,-1257.5;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;16;519.3334,-926.3452;Float;False;Property;_Falloff;Falloff;2;0;Create;True;0;0;False;0;0.1;0.2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TemplateShaderPropertyNode;1;1438.476,-1946.867;Float;False;0;0;_MainTex;Shader;0;5;SAMPLER2D;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PowerNode;17;828.8903,-1138.896;Float;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;24;1799.871,-1030.491;Float;False;Property;_Activation;Activation;3;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;58;1149.678,-1174.464;Float;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0.3;False;3;FLOAT;0.6;False;4;FLOAT;0.4;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;25;1694.765,-1860.152;Float;True;Property;_TextureSample0;Texture Sample 0;4;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;19;1288.98,-1491.245;Float;False;Constant;_Color0;Color 0;3;0;Create;True;0;0;False;0;0.0235849,0.9629096,1,0;0,0.6342764,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;21;2200.46,-1468.822;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.CeilOpNode;23;2177.528,-1025.291;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;20;1346.734,-1723.197;Float;False;Constant;_Color1;Color 1;3;0;Create;True;0;0;False;0;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;22;3363.237,-1558.848;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.CrossProductOpNode;78;-467.6061,-666.0067;Float;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;64;2323.339,-2051.888;Float;False;ScreenTx;-1;True;1;0;SAMPLER2D;0,0,0,0;False;1;SAMPLER2D;0
Node;AmplifyShaderEditor.WorldNormalVector;79;-965.4207,-415.9762;Float;False;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;81;-967.1268,-686.356;Float;False;World;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;0;3686.995,-1559.601;Float;False;True;2;Float;ASEMaterialInspector;0;7;Hidden/FIREPOWER/RepulsionScreen;c71b220b631b6344493ea3cf87110c93;True;SubShader 0 Pass 0;0;0;SubShader 0 Pass 0;1;False;False;False;True;2;False;-1;False;False;True;2;False;-1;True;7;False;-1;False;True;0;False;0;False;False;False;False;False;False;False;False;False;False;True;2;0;;0;0;Standard;0;0;1;True;False;1;0;FLOAT4;0,0,0,0;False;0
Node;AmplifyShaderEditor.CommentaryNode;5;1154.636,-1987.339;Float;False;100;100;;0;Desfazar UV de la RT de Screen;1,0,0,1;0;0
WireConnection;11;0;2;0
WireConnection;11;1;9;0
WireConnection;56;0;55;0
WireConnection;13;0;11;0
WireConnection;13;1;3;0
WireConnection;53;0;56;0
WireConnection;53;1;13;0
WireConnection;54;0;53;0
WireConnection;17;0;54;0
WireConnection;17;1;16;0
WireConnection;58;0;17;0
WireConnection;25;0;1;0
WireConnection;21;0;25;0
WireConnection;21;1;19;0
WireConnection;21;2;58;0
WireConnection;23;0;24;0
WireConnection;22;0;25;0
WireConnection;22;1;21;0
WireConnection;22;2;23;0
WireConnection;78;0;81;0
WireConnection;78;1;79;0
WireConnection;64;0;1;0
WireConnection;0;0;22;0
ASEEND*/
//CHKSM=CB5415937A74DA0E17D0045E7683809E7250C02F