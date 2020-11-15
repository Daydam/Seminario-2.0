// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Gastón Zabala/Laser"
{
	Properties
	{
		_MainTex("MainTex", 2D) = "white" {}
		_Tiling("Tiling", Vector) = (0,0,0,0)
		_PannerSpeed("Panner Speed", Float) = 0
		[HDR]_Color0("Color 0", Color) = (0,0,0,0)
	}
	
	SubShader
	{
		
		
		Tags { "RenderType"="Transparent" }
		LOD 100

		CGINCLUDE
		#pragma target 3.0
		ENDCG
		Blend SrcAlpha OneMinusSrcAlpha , One One
		Cull Off
		ColorMask RGBA
		ZWrite On
		ZTest LEqual
		Offset 0 , 0
		
		
		
		Pass
		{
			Name "Unlit"
			
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_instancing
			#include "UnityCG.cginc"
			#include "UnityShaderVariables.cginc"


			struct appdata
			{
				float4 vertex : POSITION;
				float4 color : COLOR;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				float4 ase_texcoord : TEXCOORD0;
			};
			
			struct v2f
			{
				float4 vertex : SV_POSITION;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
				float4 ase_texcoord : TEXCOORD0;
			};

			uniform float4 _Color0;
			uniform sampler2D _MainTex;
			uniform float _PannerSpeed;
			uniform float2 _Tiling;
			
			v2f vert ( appdata v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				UNITY_TRANSFER_INSTANCE_ID(v, o);

				o.ase_texcoord.xy = v.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord.zw = 0;
				float3 vertexValue =  float3(0,0,0) ;
				#if ASE_ABSOLUTE_VERTEX_POS
				v.vertex.xyz = vertexValue;
				#else
				v.vertex.xyz += vertexValue;
				#endif
				o.vertex = UnityObjectToClipPos(v.vertex);
				return o;
			}
			
			fixed4 frag (v2f i ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID(i);
				fixed4 finalColor;
				float mulTime19 = _Time.y * _PannerSpeed;
				float2 uv03 = i.ase_texcoord.xy * _Tiling + float2( 0,0 );
				float2 panner2 = ( mulTime19 * float2( 1,0 ) + uv03);
				float4 tex2DNode1 = tex2D( _MainTex, panner2 );
				float grayscale24 = (tex2DNode1.rgb.r + tex2DNode1.rgb.g + tex2DNode1.rgb.b) / 3;
				float4 temp_output_21_0 = ( _Color0 * grayscale24 );
				float4 appendResult11 = (float4(temp_output_21_0.rgb , tex2DNode1.a));
				
				
				finalColor = appendResult11;
				return finalColor;
			}
			ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=16700
7;276;1352;376;1114.63;278.7122;1.339433;True;False
Node;AmplifyShaderEditor.RangedFloatNode;20;-1386.618,154.4118;Float;False;Property;_PannerSpeed;Panner Speed;3;0;Create;True;0;0;False;0;0;-2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;13;-1490.407,2.1661;Float;False;Property;_Tiling;Tiling;1;0;Create;True;0;0;False;0;0,0;1,1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleTimeNode;19;-1173.618,158.4118;Float;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;3;-1262,-16.5;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;2;-952,-17.5;Float;False;3;0;FLOAT2;1,0;False;2;FLOAT2;1,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;1;-642,-43.5;Float;True;Property;_MainTex;MainTex;0;0;Create;True;0;0;False;0;None;4bd2a14ae52b72a418e909b2ebcc8b87;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TFHCGrayscale;24;-319.8346,-45.77583;Float;False;2;1;0;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;23;-346.4345,-254.987;Float;False;Property;_Color0;Color 0;4;1;[HDR];Create;True;0;0;False;0;0,0,0,0;0,3.132364,8,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;21;-13.52612,-206.5258;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.DynamicAppendNode;11;117.6056,117.1513;Float;False;FLOAT4;4;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;5;-319.0911,179.2899;Float;False;Property;_Intensity;Intensity;2;0;Create;True;0;0;False;0;0;3.28;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;12;-68.8103,23.94337;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;0;373.3015,-7.308584;Float;False;True;2;Float;ASEMaterialInspector;0;1;Gastón Zabala/Laser;0770190933193b94aaa3065e307002fa;True;Unlit;0;0;Unlit;2;True;2;5;False;-1;10;False;-1;4;1;False;-1;1;False;-1;True;0;False;-1;0;False;-1;True;False;True;2;False;-1;True;True;True;True;True;0;False;-1;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;True;1;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;1;RenderType=Transparent=RenderType;True;2;0;False;False;False;False;False;False;False;False;False;True;0;False;0;;0;0;Standard;1;Vertex Position,InvertActionOnDeselection;1;0;1;True;False;2;0;FLOAT4;0,0,0,0;False;1;FLOAT3;0,0,0;False;0
WireConnection;19;0;20;0
WireConnection;3;0;13;0
WireConnection;2;0;3;0
WireConnection;2;1;19;0
WireConnection;1;1;2;0
WireConnection;24;0;1;0
WireConnection;21;0;23;0
WireConnection;21;1;24;0
WireConnection;11;0;21;0
WireConnection;11;3;1;4
WireConnection;12;0;21;0
WireConnection;12;1;5;0
WireConnection;0;0;11;0
ASEEND*/
//CHKSM=5FD45DD874DCB25E4680527EA05DFB534791A894