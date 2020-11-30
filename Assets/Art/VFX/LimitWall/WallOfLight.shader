// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Hidden/Templates/Unlit/WallOfLight"
{
	Properties
	{
		[HDR]_MainColor("Main Color", Color) = (0,1,1,0)
		_Distance("Distance", Float) = 1
		[HDR]_IntersectionColor("IntersectionColor", Color) = (1,1,0,0)
		_Tiling("Tiling", Vector) = (2.56,0.1,0,0)
		_HeightMaskController("HeightMaskController", Float) = 0.68
	}
	
	SubShader
	{
		
		
		Tags { "RenderType"="Transparent" }
		LOD 100

		CGINCLUDE
		#pragma target 3.0
		ENDCG
		Blend SrcAlpha OneMinusSrcAlpha
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
				
			};
			
			struct v2f
			{
				float4 vertex : SV_POSITION;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_texcoord1 : TEXCOORD1;
				float4 ase_texcoord2 : TEXCOORD2;
			};

			uniform float4 _IntersectionColor;
			uniform float4 _MainColor;
			UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
			uniform float4 _CameraDepthTexture_TexelSize;
			uniform float _Distance;
			uniform float2 _Tiling;
			uniform float _HeightMaskController;
			float3 mod2D289( float3 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }
			float2 mod2D289( float2 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }
			float3 permute( float3 x ) { return mod2D289( ( ( x * 34.0 ) + 1.0 ) * x ); }
			float snoise( float2 v )
			{
				const float4 C = float4( 0.211324865405187, 0.366025403784439, -0.577350269189626, 0.024390243902439 );
				float2 i = floor( v + dot( v, C.yy ) );
				float2 x0 = v - i + dot( i, C.xx );
				float2 i1;
				i1 = ( x0.x > x0.y ) ? float2( 1.0, 0.0 ) : float2( 0.0, 1.0 );
				float4 x12 = x0.xyxy + C.xxzz;
				x12.xy -= i1;
				i = mod2D289( i );
				float3 p = permute( permute( i.y + float3( 0.0, i1.y, 1.0 ) ) + i.x + float3( 0.0, i1.x, 1.0 ) );
				float3 m = max( 0.5 - float3( dot( x0, x0 ), dot( x12.xy, x12.xy ), dot( x12.zw, x12.zw ) ), 0.0 );
				m = m * m;
				m = m * m;
				float3 x = 2.0 * frac( p * C.www ) - 1.0;
				float3 h = abs( x ) - 0.5;
				float3 ox = floor( x + 0.5 );
				float3 a0 = x - ox;
				m *= 1.79284291400159 - 0.85373472095314 * ( a0 * a0 + h * h );
				float3 g;
				g.x = a0.x * x0.x + h.x * x0.y;
				g.yz = a0.yz * x12.xz + h.yz * x12.yw;
				return 130.0 * dot( m, g );
			}
			
			
			v2f vert ( appdata v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				UNITY_TRANSFER_INSTANCE_ID(v, o);

				float4 ase_clipPos = UnityObjectToClipPos(v.vertex);
				float4 screenPos = ComputeScreenPos(ase_clipPos);
				o.ase_texcoord = screenPos;
				float3 ase_worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				o.ase_texcoord1.xyz = ase_worldPos;
				
				o.ase_texcoord2 = v.vertex;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord1.w = 0;
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
				float4 screenPos = i.ase_texcoord;
				float4 ase_screenPosNorm = screenPos / screenPos.w;
				ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
				float screenDepth6 = LinearEyeDepth(UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture,UNITY_PROJ_COORD( screenPos ))));
				float distanceDepth6 = abs( ( screenDepth6 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( _Distance ) );
				float4 lerpResult9 = lerp( _IntersectionColor , _MainColor , saturate( distanceDepth6 ));
				float3 ase_worldPos = i.ase_texcoord1.xyz;
				float2 appendResult31 = (float2(ase_worldPos.x , ase_worldPos.y));
				float2 panner28 = ( 1.0 * _Time.y * float2( 0,-1 ) + ( appendResult31 * _Tiling ));
				float simplePerlin2D25 = snoise( panner28 );
				float temp_output_29_0 = saturate( simplePerlin2D25 );
				float4 appendResult3 = (float4(( (lerpResult9).rgb * temp_output_29_0 ) , temp_output_29_0));
				float4 transform49 = mul(unity_ObjectToWorld,float4( i.ase_texcoord2.xyz , 0.0 ));
				float4 lerpResult37 = lerp( float4( 0,0,0,0 ) , appendResult3 , ( 1.0 - saturate( ( transform49.y + _HeightMaskController ) ) ));
				
				
				finalColor = lerpResult37;
				return finalColor;
			}
			ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=16700
87;197;1352;584;2206.197;359.9489;1.751134;True;False
Node;AmplifyShaderEditor.WorldPosInputsNode;30;-1194.938,530.0214;Float;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.Vector2Node;33;-1110.328,701.8531;Float;False;Property;_Tiling;Tiling;3;0;Create;True;0;0;False;0;2.56,0.1;2.56,0.1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RangedFloatNode;7;-1222.719,250.5215;Float;False;Property;_Distance;Distance;1;0;Create;True;0;0;False;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;31;-973.7419,550.5249;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DepthFade;6;-1018.322,233.7676;Float;False;True;False;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;32;-799.8344,637.5974;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;28;-624.1317,636.7858;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,-1;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SaturateNode;8;-731.8271,232.0922;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;4;-1050.125,-255.6463;Float;False;Property;_MainColor;Main Color;0;1;[HDR];Create;True;0;0;False;0;0,1,1,0;0,6.70157,32,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PosVertexDataNode;48;-1435.456,1146.697;Float;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;14;-1063.809,9.142239;Float;False;Property;_IntersectionColor;IntersectionColor;2;1;[HDR];Create;True;0;0;False;0;1,1,0,0;14.75442,11.27825,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.NoiseGeneratorNode;25;-388.8283,633.1868;Float;False;Simplex2D;1;0;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ObjectToWorldTransfNode;49;-996.1078,979.4364;Float;False;1;0;FLOAT4;0,0,0,1;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;9;-577.8491,23.10501;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,1,0.9903989,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;42;-781.0367,921.5927;Float;False;Property;_HeightMaskController;HeightMaskController;4;0;Create;True;0;0;False;0;0.68;-0.2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;29;-151.0086,637.1094;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;10;-383.0698,-38.79419;Float;False;True;True;True;False;1;0;COLOR;0,0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;40;-426.7773,917.8839;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0.28;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;41;-237.1759,912.7557;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;24;-127.5831,-33.47529;Float;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.DynamicAppendNode;3;215.369,595.3826;Float;False;FLOAT4;4;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0.25;False;1;FLOAT4;0
Node;AmplifyShaderEditor.OneMinusNode;43;-63.60308,914.4088;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;37;894.5623,714.4453;Float;False;3;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;0;1446.104,860.8557;Float;False;True;2;Float;ASEMaterialInspector;0;1;Hidden/Templates/Unlit/WallOfLight;0770190933193b94aaa3065e307002fa;True;Unlit;0;0;Unlit;2;True;2;5;False;-1;10;False;-1;0;1;False;-1;1;False;-1;True;0;False;-1;0;False;-1;True;False;True;2;False;-1;True;True;True;True;True;0;False;-1;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;True;1;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;1;RenderType=Transparent=RenderType;True;2;0;False;False;False;False;False;False;False;False;False;True;0;False;0;;0;0;Standard;1;Vertex Position,InvertActionOnDeselection;1;0;1;True;False;2;0;FLOAT4;0,0,0,0;False;1;FLOAT3;0,0,0;False;0
WireConnection;31;0;30;1
WireConnection;31;1;30;2
WireConnection;6;0;7;0
WireConnection;32;0;31;0
WireConnection;32;1;33;0
WireConnection;28;0;32;0
WireConnection;8;0;6;0
WireConnection;25;0;28;0
WireConnection;49;0;48;0
WireConnection;9;0;14;0
WireConnection;9;1;4;0
WireConnection;9;2;8;0
WireConnection;29;0;25;0
WireConnection;10;0;9;0
WireConnection;40;0;49;2
WireConnection;40;1;42;0
WireConnection;41;0;40;0
WireConnection;24;0;10;0
WireConnection;24;1;29;0
WireConnection;3;0;24;0
WireConnection;3;3;29;0
WireConnection;43;0;41;0
WireConnection;37;1;3;0
WireConnection;37;2;43;0
WireConnection;0;0;37;0
ASEEND*/
//CHKSM=C0EB3AB0B13291DAC4B4494208738200C1611076