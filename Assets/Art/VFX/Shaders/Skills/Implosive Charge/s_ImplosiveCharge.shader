// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Gastón Zabala/Skills/ImplosiveCharge"
{
	Properties
	{
		_Opacity("Opacity", Float) = 0
		_TextureSample0("Texture Sample 0", 2D) = "bump" {}
		_MainTextureRLightRingBOpacity("Main Texture (R) LightRing (B) Opacity", 2D) = "white" {}
		_TextureSample2("Texture Sample 2", 2D) = "white" {}
		[HDR]_Color_RingLight("Color_RingLight", Color) = (0,0,0,0)
		[HDR]_Color_Bloom("Color_Bloom", Color) = (0,0,0,0)
		_Distortion_Mask("Distortion_Mask", Float) = 0
		_Distortion_Intensity("Distortion_Intensity", Float) = 0
		_Flowmap("Flowmap", 2D) = "white" {}
		_Flowmap_Amount("Flowmap_Amount", Range( 0 , 1)) = 0
		_Scale("Scale", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		Stencil
		{
			Ref 1
			Comp Always
			Pass Keep
		}
		GrabPass{ }
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityStandardUtils.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#ifdef UNITY_PASS_SHADOWCASTER
			#undef INTERNAL_DATA
			#undef WorldReflectionVector
			#undef WorldNormalVector
			#define INTERNAL_DATA half3 internalSurfaceTtoW0; half3 internalSurfaceTtoW1; half3 internalSurfaceTtoW2;
			#define WorldReflectionVector(data,normal) reflect (data.worldRefl, half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal)))
			#define WorldNormalVector(data,normal) half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal))
		#endif
		struct Input
		{
			float2 uv_texcoord;
			float3 viewDir;
			INTERNAL_DATA
			float4 screenPos;
			float4 vertexColor : COLOR;
		};

		uniform float4 _Color_RingLight;
		uniform sampler2D _MainTextureRLightRingBOpacity;
		uniform sampler2D _Flowmap;
		uniform sampler2D _TextureSample2;
		uniform float4 _TextureSample2_ST;
		uniform float _Scale;
		uniform float _Flowmap_Amount;
		uniform float4 _Color_Bloom;
		uniform float _Opacity;
		uniform float _Distortion_Mask;
		uniform sampler2D _GrabTexture;
		uniform float _Distortion_Intensity;
		uniform sampler2D _TextureSample0;


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
			o.Normal = float3(0,0,1);
			float2 panner109 = ( 1.0 * _Time.y * float2( 0.3,0.3 ) + i.uv_texcoord);
			float2 uv_TextureSample2 = i.uv_texcoord * _TextureSample2_ST.xy + _TextureSample2_ST.zw;
			float4 tex2DNode133 = tex2D( _TextureSample2, uv_TextureSample2 );
			float2 Offset134 = ( ( tex2DNode133.b - 1 ) * float3( 0,0,0 ).xy * _Scale ) + panner109;
			float2 Offset138 = ( ( tex2DNode133.b - 1 ) * i.viewDir.xy * _Scale ) + Offset134;
			float2 Offset139 = ( ( tex2DNode133.b - 1 ) * i.viewDir.xy * _Scale ) + Offset138;
			float2 Offset140 = ( ( tex2DNode133.b - 1 ) * i.viewDir.xy * _Scale ) + Offset139;
			float4 lerpResult105 = lerp( float4( i.uv_texcoord, 0.0 , 0.0 ) , tex2D( _Flowmap, Offset140 ) , _Flowmap_Amount);
			float2 temp_output_118_0 = (lerpResult105).rg;
			float smoothstepResult121 = smoothstep( -0.04 , 0.17 , saturate( ( distance( temp_output_118_0 , float2( 0.5,0.5 ) ) + _Opacity ) ));
			float smoothstepResult145 = smoothstep( -0.04 , 0.17 , saturate( ( distance( temp_output_118_0 , float2( 0.5,0.5 ) ) + _Distortion_Mask ) ));
			float DistortionMask66 = ( smoothstepResult121 * smoothstepResult145 );
			float4 lerpResult148 = lerp( ( _Color_RingLight * tex2D( _MainTextureRLightRingBOpacity, lerpResult105.rg ).r ) , ( _Color_Bloom * DistortionMask66 ) , DistortionMask66);
			float2 uv_TexCoord57 = i.uv_texcoord * float2( 0.73,0.73 ) + float2( 0.13,0.13 );
			float4 lerpResult124 = lerp( float4( uv_TexCoord57, 0.0 , 0.0 ) , tex2D( _Flowmap, Offset140 ) , _Flowmap_Amount);
			float cos58 = cos( 1.0 * _Time.y );
			float sin58 = sin( 1.0 * _Time.y );
			float2 rotator58 = mul( lerpResult124.rg - float2( 0.5,0.5 ) , float2x2( cos58 , -sin58 , sin58 , cos58 )) + float2( 0.5,0.5 );
			float3 tex2DNode56 = UnpackScaleNormal( tex2D( _TextureSample0, rotator58 ), _Distortion_Intensity );
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_grabScreenPos = ASE_ComputeGrabScreenPos( ase_screenPos );
			float4 ase_grabScreenPosNorm = ase_grabScreenPos / ase_grabScreenPos.w;
			float4 screenColor34 = tex2Dproj( _GrabTexture, UNITY_PROJ_COORD( ( float4( tex2DNode56 , 0.0 ) + ase_grabScreenPosNorm ) ) );
			float4 OutDistorsion65 = screenColor34;
			float4 lerpResult48 = lerp( lerpResult148 , OutDistorsion65 , DistortionMask66);
			float4 Emission87 = lerpResult48;
			o.Emission = Emission87.rgb;
			float temp_output_131_0 = saturate( ( ( 1.0 - smoothstepResult121 ) * 2.0 ) );
			float Opacity67 = temp_output_131_0;
			o.Alpha = ( Opacity67 * i.vertexColor.a );
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
				float4 screenPos : TEXCOORD2;
				float4 tSpace0 : TEXCOORD3;
				float4 tSpace1 : TEXCOORD4;
				float4 tSpace2 : TEXCOORD5;
				half4 color : COLOR0;
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
				half3 worldTangent = UnityObjectToWorldDir( v.tangent.xyz );
				half tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				half3 worldBinormal = cross( worldNormal, worldTangent ) * tangentSign;
				o.tSpace0 = float4( worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x );
				o.tSpace1 = float4( worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y );
				o.tSpace2 = float4( worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				o.screenPos = ComputeScreenPos( o.pos );
				o.color = v.color;
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
				float3 worldPos = float3( IN.tSpace0.w, IN.tSpace1.w, IN.tSpace2.w );
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.viewDir = IN.tSpace0.xyz * worldViewDir.x + IN.tSpace1.xyz * worldViewDir.y + IN.tSpace2.xyz * worldViewDir.z;
				surfIN.internalSurfaceTtoW0 = IN.tSpace0.xyz;
				surfIN.internalSurfaceTtoW1 = IN.tSpace1.xyz;
				surfIN.internalSurfaceTtoW2 = IN.tSpace2.xyz;
				surfIN.screenPos = IN.screenPos;
				surfIN.vertexColor = IN.color;
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
7;104;1266;851;-71.38605;167.7927;1;True;False
Node;AmplifyShaderEditor.Vector2Node;110;-2989.988,432.3929;Float;False;Constant;_Vector0;Vector 0;8;0;Create;True;0;0;False;0;0.3,0.3;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;108;-2982.01,642.5859;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;135;-3525.56,-134.1749;Float;False;Property;_Scale;Scale;10;0;Create;True;0;0;False;0;0;0.78;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;109;-2765.401,415.513;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;133;-3580.477,-601.8592;Float;True;Property;_TextureSample2;Texture Sample 2;3;0;Create;True;0;0;False;0;None;0000000000000000f000000000000000;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ParallaxMappingNode;134;-3229.437,-344.5768;Float;False;Normal;4;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;136;-3570.446,-22.21179;Float;False;Tangent;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.ParallaxMappingNode;138;-3052.697,-183.547;Float;False;Normal;4;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ParallaxMappingNode;139;-2915.142,1.027885;Float;False;Normal;4;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ParallaxMappingNode;140;-2684.504,110.0141;Float;False;Normal;4;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;107;-2507.889,624.9558;Float;False;Property;_Flowmap_Amount;Flowmap_Amount;9;0;Create;True;0;0;False;0;0;0.03;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;104;-2517.753,386.4644;Float;True;Property;_Flowmap;Flowmap;8;0;Create;True;0;0;False;0;None;bf324afe2f0f40e469ada9b430482d90;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;70;-1537.203,636.5624;Float;False;1676.571;535.9196;;16;66;41;40;42;43;67;131;129;122;121;24;21;20;22;118;145;Opacity & Distorsion Mask;1,1,1,1;0;0
Node;AmplifyShaderEditor.LerpOp;105;-2099.274,451.3388;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ComponentMaskNode;118;-1482.92,844.6721;Float;False;True;True;False;False;1;0;COLOR;0,0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DistanceOpNode;42;-1155.838,931.0198;Float;False;2;0;FLOAT2;0,0;False;1;FLOAT2;0.5,0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;43;-1212.146,1056.927;Float;False;Property;_Distortion_Mask;Distortion_Mask;6;0;Create;True;0;0;False;0;0;-0.15;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;22;-1231.203,808.7922;Float;False;Property;_Opacity;Opacity;0;0;Create;True;0;0;False;0;0;-0.32;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DistanceOpNode;20;-1225.203,695.6153;Float;False;2;0;FLOAT2;0,0;False;1;FLOAT2;0.5,0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;21;-1048.203,697.6153;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;125;-2516.615,993.7542;Float;True;Property;_TextureSample1;Texture Sample 1;8;0;Create;True;0;0;False;0;None;bf324afe2f0f40e469ada9b430482d90;True;0;False;white;Auto;False;Instance;104;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;57;-2305.837,1257.04;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;0.73,0.73;False;1;FLOAT2;0.13,0.13;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;41;-957.6844,929.9941;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;71;-1532.411,1317.316;Float;False;1830.718;485.5314;;7;56;62;35;61;36;34;65;Out Distorsion;1,1,1,1;0;0
Node;AmplifyShaderEditor.SaturateNode;40;-817.4778,928.9724;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;124;-2103.451,1076.628;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;24;-895.8943,698.1063;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RotatorNode;58;-1845.752,1260.51;Float;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0.5,0.5;False;2;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SmoothstepOpNode;145;-641.108,927.9059;Float;False;3;0;FLOAT;0;False;1;FLOAT;-0.04;False;2;FLOAT;0.17;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;121;-727.3921,696.7769;Float;False;3;0;FLOAT;0;False;1;FLOAT;-0.04;False;2;FLOAT;0.17;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;62;-1285.731,1454.693;Float;False;Property;_Distortion_Intensity;Distortion_Intensity;7;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;56;-1018.411,1367.316;Float;True;Property;_TextureSample0;Texture Sample 0;1;0;Create;True;0;0;False;0;None;2c9df508228ae2644a235156585a9f0c;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;147;-417.6287,861.4386;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GrabScreenPosition;35;-580.6554,1595.849;Float;False;0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;98;-1533.358,-317.4654;Float;False;1659.653;709.0657;;11;4;10;1;5;11;72;69;48;87;146;148;Emission;1,1,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;66;-245.4138,859.5069;Float;False;DistortionMask;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;36;-325.3682,1419.976;Float;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.GetLocalVarNode;146;-1177.641,313.1385;Float;False;66;DistortionMask;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ScreenColorNode;34;-160.3758,1417.588;Float;False;Global;_GrabScreen0;Grab Screen 0;5;0;Create;True;0;0;False;0;Object;-1;False;True;1;0;FLOAT4;0,0,0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;122;-545.5971,695.3022;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;10;-1366.851,137.6322;Float;False;Property;_Color_Bloom;Color_Bloom;5;1;[HDR];Create;True;0;0;False;0;0,0,0,0;0.2666667,0,0.627451,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;1;-1455.524,-74.84705;Float;True;Property;_MainTextureRLightRingBOpacity;Main Texture (R) LightRing (B) Opacity;2;0;Create;True;0;0;False;0;None;f5117ac104b03694a9e63d45562c877f;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;4;-1343.205,-267.4653;Float;False;Property;_Color_RingLight;Color_RingLight;4;1;[HDR];Create;True;0;0;False;0;0,0,0,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;11;-1071.166,138.5497;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;129;-383.8804,694.0732;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;5;-1056.047,-260.5111;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;65;55.30678,1414.741;Float;False;OutDistorsion;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;148;-696.0784,-152.5937;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;131;-241.4644,695.191;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;72;-637.4778,188.7694;Float;False;66;DistortionMask;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;69;-660.3533,92.86456;Float;False;65;OutDistorsion;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;67;-83.05841,688.5624;Float;False;Opacity;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;48;-345.6422,-32.14174;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;68;575.4853,172.3755;Float;False;67;Opacity;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;150;585.386,281.2073;Float;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;87;-131.7046,-36.77307;Float;False;Emission;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;149;825.386,175.2073;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;61;-531.8893,1418.256;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;143;247.8875,850.0473;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;89;849.7072,33.60797;Float;False;87;Emission;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1062.285,-6.558378;Float;False;True;2;Float;ASEMaterialInspector;0;0;Unlit;Gastón Zabala/Skills/ImplosiveCharge;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;True;1;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;109;0;108;0
WireConnection;109;2;110;0
WireConnection;134;0;109;0
WireConnection;134;1;133;3
WireConnection;134;2;135;0
WireConnection;138;0;134;0
WireConnection;138;1;133;3
WireConnection;138;2;135;0
WireConnection;138;3;136;0
WireConnection;139;0;138;0
WireConnection;139;1;133;3
WireConnection;139;2;135;0
WireConnection;139;3;136;0
WireConnection;140;0;139;0
WireConnection;140;1;133;3
WireConnection;140;2;135;0
WireConnection;140;3;136;0
WireConnection;104;1;140;0
WireConnection;105;0;108;0
WireConnection;105;1;104;0
WireConnection;105;2;107;0
WireConnection;118;0;105;0
WireConnection;42;0;118;0
WireConnection;20;0;118;0
WireConnection;21;0;20;0
WireConnection;21;1;22;0
WireConnection;125;1;140;0
WireConnection;41;0;42;0
WireConnection;41;1;43;0
WireConnection;40;0;41;0
WireConnection;124;0;57;0
WireConnection;124;1;125;0
WireConnection;124;2;107;0
WireConnection;24;0;21;0
WireConnection;58;0;124;0
WireConnection;145;0;40;0
WireConnection;121;0;24;0
WireConnection;56;1;58;0
WireConnection;56;5;62;0
WireConnection;147;0;121;0
WireConnection;147;1;145;0
WireConnection;66;0;147;0
WireConnection;36;0;56;0
WireConnection;36;1;35;0
WireConnection;34;0;36;0
WireConnection;122;0;121;0
WireConnection;1;1;105;0
WireConnection;11;0;10;0
WireConnection;11;1;146;0
WireConnection;129;0;122;0
WireConnection;5;0;4;0
WireConnection;5;1;1;1
WireConnection;65;0;34;0
WireConnection;148;0;5;0
WireConnection;148;1;11;0
WireConnection;148;2;146;0
WireConnection;131;0;129;0
WireConnection;67;0;131;0
WireConnection;48;0;148;0
WireConnection;48;1;69;0
WireConnection;48;2;72;0
WireConnection;87;0;48;0
WireConnection;149;0;68;0
WireConnection;149;1;150;4
WireConnection;61;0;56;2
WireConnection;143;0;131;0
WireConnection;143;1;34;0
WireConnection;0;2;89;0
WireConnection;0;9;149;0
ASEEND*/
//CHKSM=8B0BCF04677B69D0573D4296B7CA8E9755E3F47D