// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Gastón Zabala/Skills/ImplosiveCharge_Distortion"
{
	Properties
	{
		[HDR]_Color_Distortion("Color_Distortion", Color) = (0,0,0,0)
		_MainTextureGDistortion("Main Texture (G) Distortion", 2D) = "white" {}
		_Distortion_Intensity("Distortion_Intensity", Float) = 0
		_Flowmap("Flowmap", 2D) = "white" {}
		_Flowmap_Intensity("Flowmap_Intensity", Range( 0 , 1)) = 0
		_Mask("Mask", Float) = 0
		_Border_Intensity("Border_Intensity", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		GrabPass{ }
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Unlit alpha:fade keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
			float4 screenPos;
		};

		uniform sampler2D _GrabTexture;
		uniform sampler2D _MainTextureGDistortion;
		uniform sampler2D _Flowmap;
		uniform float _Flowmap_Intensity;
		uniform float _Distortion_Intensity;
		uniform float4 _Color_Distortion;
		uniform float _Mask;
		uniform float _Border_Intensity;


		inline float4 ASE_ComputeGrabScreenPos( float4 pos )
		{
			#if UNITY_UV_STARTS_AT_TOP
			float scale = -1.0;
			#else
			float scale = 1.0;
			#endif
			float4 o = pos;
			o.y = pos.w * 0.5f;
			o.y = ( pos.y - o.y ) * _ProjectionParams.x * scale + o.y;
			return o;
		}


		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float2 panner37 = ( 1.0 * _Time.y * float2( 0.3,0.3 ) + i.uv_texcoord);
			float4 lerpResult35 = lerp( float4( i.uv_texcoord, 0.0 , 0.0 ) , tex2D( _Flowmap, panner37 ) , _Flowmap_Intensity);
			float4 tex2DNode1 = tex2D( _MainTextureGDistortion, lerpResult35.rg );
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_grabScreenPos = ASE_ComputeGrabScreenPos( ase_screenPos );
			float4 ase_grabScreenPosNorm = ase_grabScreenPos / ase_grabScreenPos.w;
			float4 screenColor4 = tex2Dproj( _GrabTexture, UNITY_PROJ_COORD( ( ( tex2DNode1.g * _Distortion_Intensity ) + ase_grabScreenPosNorm ) ) );
			float smoothstepResult45 = smoothstep( 0.04 , 0.24 , ( distance( (lerpResult35).rg , float2( 0.5,0.5 ) ) + _Mask ));
			float4 lerpResult12 = lerp( screenColor4 , _Color_Distortion , saturate( ( ( smoothstepResult45 * _Border_Intensity ) * tex2DNode1.g ) ));
			o.Emission = lerpResult12.rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16700
7;1;1266;954;2937.115;240.8379;1;True;False
Node;AmplifyShaderEditor.TextureCoordinatesNode;33;-2644.053,174.0423;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;37;-2375.817,261.2693;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0.3,0.3;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;34;-2160.628,276.3468;Float;True;Property;_Flowmap;Flowmap;3;0;Create;True;0;0;False;0;None;bf324afe2f0f40e469ada9b430482d90;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;36;-2145.628,514.3466;Float;False;Property;_Flowmap_Intensity;Flowmap_Intensity;4;0;Create;True;0;0;False;0;0;0.03;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;35;-1800.283,177.4517;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ComponentMaskNode;39;-1634.934,-128.6713;Float;False;True;True;False;False;1;0;COLOR;0,0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DistanceOpNode;38;-1382.79,-127.4238;Float;False;2;0;FLOAT2;0,0;False;1;FLOAT2;0.5,0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;41;-1400.589,-6.620392;Float;False;Property;_Mask;Mask;5;0;Create;True;0;0;False;0;0;-0.25;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;40;-1188.589,-125.6204;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;45;-1043.71,-125.742;Float;False;3;0;FLOAT;0;False;1;FLOAT;0.04;False;2;FLOAT;0.24;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;1;-1283.07,143.7383;Float;True;Property;_MainTextureGDistortion;Main Texture (G) Distortion;1;0;Create;True;0;0;False;0;None;f5117ac104b03694a9e63d45562c877f;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;10;-1238.601,360.5532;Float;False;Property;_Distortion_Intensity;Distortion_Intensity;2;0;Create;True;0;0;False;0;0;-0.02;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;47;-1075.337,2.333252;Float;False;Property;_Border_Intensity;Border_Intensity;6;0;Create;True;0;0;False;0;0;10;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;11;-925.5354,255.6383;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GrabScreenPosition;5;-929.2048,416.4404;Float;False;0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;46;-830.2356,-126.9859;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;6;-604.2508,399.219;Float;False;2;2;0;FLOAT;0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;42;-673.9987,160.5645;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;3;-397.2673,-48.90049;Float;False;Property;_Color_Distortion;Color_Distortion;0;1;[HDR];Create;True;0;0;False;0;0,0,0,0;6.368628,1.433775,0.3667796,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;44;-463.3411,161.2267;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ScreenColorNode;4;-413.9942,391.3401;Float;False;Global;_GrabScreen0;Grab Screen 0;2;0;Create;True;0;0;False;0;Object;-1;False;True;1;0;FLOAT4;0,0,0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ParallaxMappingNode;49;-2762.662,-1024.582;Float;False;Normal;4;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.LerpOp;12;-104.7917,23.86875;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;13;-160.0107,-196.2643;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;48;-3258.793,-1128.581;Float;True;Property;_TextureSample0;Texture Sample 0;7;0;Create;True;0;0;False;0;None;0000000000000000f000000000000000;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ParallaxMappingNode;54;-2541.391,-334.2851;Float;False;Normal;4;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ParallaxMappingNode;53;-2613.08,-573.9009;Float;False;Normal;4;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ParallaxMappingNode;52;-2721.33,-797.6954;Float;False;Normal;4;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;50;-3188.402,-851.9171;Float;False;Tangent;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;51;-3174.863,-937.8307;Float;False;Property;_Scale;Scale;8;0;Create;True;0;0;False;0;0;0.44;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;116,-19;Float;False;True;2;Float;ASEMaterialInspector;0;0;Unlit;Gastón Zabala/Skills/ImplosiveCharge_Distortion;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.02;True;True;0;False;Transparent;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;37;0;33;0
WireConnection;34;1;37;0
WireConnection;35;0;33;0
WireConnection;35;1;34;0
WireConnection;35;2;36;0
WireConnection;39;0;35;0
WireConnection;38;0;39;0
WireConnection;40;0;38;0
WireConnection;40;1;41;0
WireConnection;45;0;40;0
WireConnection;1;1;35;0
WireConnection;11;0;1;2
WireConnection;11;1;10;0
WireConnection;46;0;45;0
WireConnection;46;1;47;0
WireConnection;6;0;11;0
WireConnection;6;1;5;0
WireConnection;42;0;46;0
WireConnection;42;1;1;2
WireConnection;44;0;42;0
WireConnection;4;0;6;0
WireConnection;49;0;37;0
WireConnection;49;1;48;1
WireConnection;49;2;51;0
WireConnection;49;3;50;0
WireConnection;12;0;4;0
WireConnection;12;1;3;0
WireConnection;12;2;44;0
WireConnection;13;0;3;0
WireConnection;13;1;1;2
WireConnection;54;0;53;0
WireConnection;54;1;48;1
WireConnection;54;2;51;0
WireConnection;54;3;50;0
WireConnection;53;0;52;0
WireConnection;53;1;48;1
WireConnection;53;2;51;0
WireConnection;53;3;50;0
WireConnection;52;0;49;0
WireConnection;52;1;48;1
WireConnection;52;2;51;0
WireConnection;52;3;50;0
WireConnection;0;2;12;0
ASEEND*/
//CHKSM=4C77299410086725F04AC24CD05E3484494CC4BD