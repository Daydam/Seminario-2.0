// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Gastón Zabala/Skills/ImplosiveCharge"
{
	Properties
	{
		_MainTextureRLightRingBOpacity("Main Texture (R) LightRing (B) Opacity", 2D) = "white" {}
		[HDR]_Color_RingLight("Color_RingLight", Color) = (0,0,0,0)
		[HDR]_Color_Bloom("Color_Bloom", Color) = (0,0,0,0)
		_OutDistortionRing("OutDistortionRing", Float) = 0
		_Center("Center", Float) = 0
		_IntensityDistortion("Intensity Distortion", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		GrabPass{ }
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		struct Input
		{
			float2 uv_texcoord;
			float4 screenPos;
		};

		uniform float4 _Color_RingLight;
		uniform sampler2D _MainTextureRLightRingBOpacity;
		uniform float4 _MainTextureRLightRingBOpacity_ST;
		uniform float4 _Color_Bloom;
		uniform sampler2D _GrabTexture;
		uniform float _IntensityDistortion;
		uniform float _Center;
		uniform float _OutDistortionRing;


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
			float2 uv_MainTextureRLightRingBOpacity = i.uv_texcoord * _MainTextureRLightRingBOpacity_ST.xy + _MainTextureRLightRingBOpacity_ST.zw;
			float4 tex2DNode1 = tex2D( _MainTextureRLightRingBOpacity, uv_MainTextureRLightRingBOpacity );
			float4 blendOpSrc12 = ( _Color_RingLight * tex2DNode1.r );
			float4 blendOpDest12 = ( tex2DNode1.b * _Color_Bloom );
			float2 uv_TexCoord57 = i.uv_texcoord * float2( 0.73,0.73 ) + float2( 0.13,0.13 );
			float cos58 = cos( 1.0 * _Time.y );
			float sin58 = sin( 1.0 * _Time.y );
			float2 rotator58 = mul( uv_TexCoord57 - float2( 0.5,0.5 ) , float2x2( cos58 , -sin58 , sin58 , cos58 )) + float2( 0.5,0.5 );
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_grabScreenPos = ASE_ComputeGrabScreenPos( ase_screenPos );
			float4 ase_grabScreenPosNorm = ase_grabScreenPos / ase_grabScreenPos.w;
			float4 screenColor34 = tex2Dproj( _GrabTexture, UNITY_PROJ_COORD( ( ( tex2D( _MainTextureRLightRingBOpacity, rotator58 ).g * _IntensityDistortion ) + ase_grabScreenPosNorm ) ) );
			float4 OutDistorsion65 = screenColor34;
			float temp_output_26_0 = ( 1.0 - ceil( saturate( ( distance( i.uv_texcoord , float2( 0.5,0.5 ) ) + _Center ) ) ) );
			float Mask66 = ( temp_output_26_0 * ceil( saturate( ( distance( i.uv_texcoord , float2( 0.5,0.5 ) ) + _OutDistortionRing ) ) ) );
			float4 lerpResult48 = lerp( saturate( ( saturate( ( 1.0 - ( 1.0 - blendOpSrc12 ) * ( 1.0 - blendOpDest12 ) ) )) ) , OutDistorsion65 , Mask66);
			float4 Emission87 = lerpResult48;
			o.Emission = Emission87.rgb;
			float Opacity67 = temp_output_26_0;
			o.Alpha = Opacity67;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Unlit alpha:fade keepalpha fullforwardshadows 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			sampler3D _DitherMaskLOD;
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
				float4 screenPos : TEXCOORD3;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				o.screenPos = ComputeScreenPos( o.pos );
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.screenPos = IN.screenPos;
				SurfaceOutput o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutput, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				half alphaRef = tex3D( _DitherMaskLOD, float3( vpos.xy * 0.25, o.Alpha * 0.9375 ) ).a;
				clip( alphaRef - 0.01 );
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16700
24;479;1266;948;1718.468;-312.6271;1.3;True;False
Node;AmplifyShaderEditor.CommentaryNode;70;-1537.203,636.5624;Float;False;1592.553;538.3906;;16;44;43;42;66;18;20;22;21;24;23;26;67;41;40;38;46;Opacity & Distorsion Mask;1,1,1,1;0;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;18;-1487.203,695.6153;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;71;-1532.411,1317.316;Float;False;1830.718;485.5314;;9;57;58;56;62;35;61;36;34;65;Out Distorsion;1,1,1,1;0;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;44;-1296.838,931.0198;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;57;-1450.411,1395.316;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;0.73,0.73;False;1;FLOAT2;0.13,0.13;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DistanceOpNode;20;-1225.203,695.6153;Float;False;2;0;FLOAT2;0,0;False;1;FLOAT2;0.5,0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;22;-1231.203,824.6153;Float;False;Property;_Center;Center;4;0;Create;True;0;0;False;0;0;-0.46;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RotatorNode;58;-1209.732,1394.986;Float;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0.5,0.5;False;2;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;43;-1112.299,1059.953;Float;False;Property;_OutDistortionRing;OutDistortionRing;3;0;Create;True;0;0;False;0;0;-0.42;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DistanceOpNode;42;-1034.838,931.0198;Float;False;2;0;FLOAT2;0,0;False;1;FLOAT2;0.5,0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;21;-1048.203,697.6153;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;56;-1018.411,1367.316;Float;True;Property;_TextureSample0;Texture Sample 0;0;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Instance;1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;24;-895.8943,698.1063;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;62;-816.0282,1580.08;Float;False;Property;_IntensityDistortion;Intensity Distortion;5;0;Create;True;0;0;False;0;0;0.18;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;41;-857.8378,933.0198;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GrabScreenPosition;35;-580.6554,1595.849;Float;False;0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;88;-1531.371,-262.7403;Float;False;1615.165;709.0657;;11;1;10;11;5;12;52;48;4;72;69;87;Emission;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;61;-531.8893,1418.256;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;40;-705.5287,933.5108;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CeilOpNode;23;-738.8943,695.1063;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;36;-325.3682,1419.976;Float;False;2;2;0;FLOAT;0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SamplerNode;1;-1481.371,7.677518;Float;True;Property;_MainTextureRLightRingBOpacity;Main Texture (R) LightRing (B) Opacity;0;0;Create;True;0;0;False;0;None;f5117ac104b03694a9e63d45562c877f;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;26;-587.0687,693.4238;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CeilOpNode;38;-546.5287,937.5108;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;10;-1366.396,239.3254;Float;False;Property;_Color_Bloom;Color_Bloom;2;1;[HDR];Create;True;0;0;False;0;0,0,0,0;0.238025,0.01401745,0.4245283,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;4;-1341.218,-212.7403;Float;False;Property;_Color_RingLight;Color_RingLight;1;1;[HDR];Create;True;0;0;False;0;0,0,0,0;2.996078,2.996078,2.996078,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;46;-368.7878,914.7158;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;11;-1056.26,209.9543;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;5;-1054.06,-205.7861;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ScreenColorNode;34;-160.3758,1417.588;Float;False;Global;_GrabScreen0;Grab Screen 0;5;0;Create;True;0;0;False;0;Object;-1;False;True;1;0;FLOAT4;0,0,0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;66;-187.6493,908.5626;Float;False;Mask;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.BlendOpsNode;12;-825.9863,18.52297;Float;False;Screen;True;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;65;55.30678,1414.741;Float;False;OutDistorsion;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;52;-559.7902,23.95229;Float;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;69;-632.3276,143.5836;Float;False;65;OutDistorsion;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;72;-601.4402,237.4854;Float;False;66;Mask;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;48;-343.6556,22.58346;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;87;-159.2058,18.86825;Float;False;Emission;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;67;-380.0584,689.5624;Float;False;Opacity;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;68;863.4853,171.3755;Float;False;67;Opacity;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;89;849.7072,33.60797;Float;False;87;Emission;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1062.285,-6.558378;Float;False;True;2;Float;ASEMaterialInspector;0;0;Unlit;Gastón Zabala/Skills/ImplosiveCharge;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;20;0;18;0
WireConnection;58;0;57;0
WireConnection;42;0;44;0
WireConnection;21;0;20;0
WireConnection;21;1;22;0
WireConnection;56;1;58;0
WireConnection;24;0;21;0
WireConnection;41;0;42;0
WireConnection;41;1;43;0
WireConnection;61;0;56;2
WireConnection;61;1;62;0
WireConnection;40;0;41;0
WireConnection;23;0;24;0
WireConnection;36;0;61;0
WireConnection;36;1;35;0
WireConnection;26;0;23;0
WireConnection;38;0;40;0
WireConnection;46;0;26;0
WireConnection;46;1;38;0
WireConnection;11;0;1;3
WireConnection;11;1;10;0
WireConnection;5;0;4;0
WireConnection;5;1;1;1
WireConnection;34;0;36;0
WireConnection;66;0;46;0
WireConnection;12;0;5;0
WireConnection;12;1;11;0
WireConnection;65;0;34;0
WireConnection;52;0;12;0
WireConnection;48;0;52;0
WireConnection;48;1;69;0
WireConnection;48;2;72;0
WireConnection;87;0;48;0
WireConnection;67;0;26;0
WireConnection;0;2;89;0
WireConnection;0;9;68;0
ASEEND*/
//CHKSM=BC5E3D1FFB7BDABB4D165E502D69D76B5465610A