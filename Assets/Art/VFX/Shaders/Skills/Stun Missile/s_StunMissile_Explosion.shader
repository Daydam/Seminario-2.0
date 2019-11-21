// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Gastón Zabala/StunMissile_Explosion"
{
	Properties
	{
		_Tiling("Tiling", Float) = 0
		_BaseColor("Base Color", Color) = (0,0,0,0)
		[HDR]_EmissionColor("Emission Color", Color) = (0,0,0,0)
		_MainTextureREmissionGOpacity("Main Texture (R) Emission (G) Opacity", 2D) = "white" {}
		_Panner_Speed_X("Panner_Speed_X", Float) = 0
		_Panner_Speed_Y("Panner_Speed_Y", Float) = 0
		[HDR]_Fresnel_Color("Fresnel_Color", Color) = (0,0,0,0)
		_Fresnel_Bias("Fresnel_Bias", Float) = 0
		_Fresnel_Scale("Fresnel_Scale", Float) = 0
		_Fresnel_Power("Fresnel_Power", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		struct Input
		{
			float2 uv_texcoord;
			float3 worldPos;
			float3 worldNormal;
		};

		uniform float4 _EmissionColor;
		uniform sampler2D _MainTextureREmissionGOpacity;
		uniform float _Panner_Speed_X;
		uniform float _Panner_Speed_Y;
		uniform float _Tiling;
		uniform float4 _BaseColor;
		uniform float4 _Fresnel_Color;
		uniform float _Fresnel_Bias;
		uniform float _Fresnel_Scale;
		uniform float _Fresnel_Power;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float2 appendResult25 = (float2(_Panner_Speed_X , _Panner_Speed_Y));
			float2 temp_cast_0 = (_Tiling).xx;
			float2 uv_TexCoord5 = i.uv_texcoord * temp_cast_0;
			float2 panner22 = ( 1.0 * _Time.y * appendResult25 + uv_TexCoord5);
			float4 tex2DNode1 = tex2D( _MainTextureREmissionGOpacity, panner22 );
			float4 lerpResult13 = lerp( ( _EmissionColor * tex2DNode1.r ) , ( _BaseColor * tex2DNode1.g ) , tex2DNode1.g);
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = i.worldNormal;
			float fresnelNdotV14 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode14 = ( _Fresnel_Bias + _Fresnel_Scale * pow( 1.0 - fresnelNdotV14, _Fresnel_Power ) );
			float4 lerpResult21 = lerp( lerpResult13 , ( _Fresnel_Color * fresnelNode14 ) , saturate( fresnelNode14 ));
			o.Emission = lerpResult21.rgb;
			o.Alpha = tex2DNode1.g;
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
				float3 worldNormal : TEXCOORD3;
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
				o.worldNormal = worldNormal;
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
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
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = IN.worldNormal;
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
7;7;1266;948;2620.05;152.8223;1;True;False
Node;AmplifyShaderEditor.RangedFloatNode;7;-2077.584,200.2358;Float;False;Property;_Tiling;Tiling;0;0;Create;True;0;0;False;0;0;4;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;23;-2026.674,333.8331;Float;False;Property;_Panner_Speed_X;Panner_Speed_X;4;0;Create;True;0;0;False;0;0;0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;24;-2029.674,427.8331;Float;False;Property;_Panner_Speed_Y;Panner_Speed_Y;5;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;25;-1784.666,362.6949;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;5;-1884.701,182.9034;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;22;-1578.14,184.3329;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0.5,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;16;-1582.496,688.4609;Float;False;Property;_Fresnel_Scale;Fresnel_Scale;8;0;Create;True;0;0;False;0;0;4.14;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;17;-1584.068,787.517;Float;False;Property;_Fresnel_Power;Fresnel_Power;9;0;Create;True;0;0;False;0;0;5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;1;-1356.979,156.8602;Float;True;Property;_MainTextureREmissionGOpacity;Main Texture (R) Emission (G) Opacity;3;0;Create;True;0;0;False;0;None;f303f13b0db3fa74488dace53e9790fa;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;15;-1576.239,586.9764;Float;False;Property;_Fresnel_Bias;Fresnel_Bias;7;0;Create;True;0;0;False;0;0;0.17;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;4;-1210.981,-322.5046;Float;False;Property;_EmissionColor;Emission Color;2;1;[HDR];Create;True;0;0;False;0;0,0,0,0;47.93726,23.84314,6.776471,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;8;-1295.572,-98.69315;Float;False;Property;_BaseColor;Base Color;1;0;Create;True;0;0;False;0;0,0,0,0;0,0.5372549,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;18;-1330.242,390.0349;Float;False;Property;_Fresnel_Color;Fresnel_Color;6;1;[HDR];Create;True;0;0;False;0;0,0,0,0;3.921569,0.6477507,0.1254902,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;10;-1020.771,20.28447;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.FresnelNode;14;-1319.295,618.3101;Float;False;Standard;WorldNormal;ViewDir;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;3;-871.1528,-103.9663;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;19;-1058.669,513.3914;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;20;-1001.845,618.8233;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;13;-714.3661,-6.920349;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;21;-446.3616,429.2725;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1.569242,9.35917;Float;False;True;2;Float;ASEMaterialInspector;0;0;Unlit;Gastón Zabala/StunMissile_Explosion;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;25;0;23;0
WireConnection;25;1;24;0
WireConnection;5;0;7;0
WireConnection;22;0;5;0
WireConnection;22;2;25;0
WireConnection;1;1;22;0
WireConnection;10;0;8;0
WireConnection;10;1;1;2
WireConnection;14;1;15;0
WireConnection;14;2;16;0
WireConnection;14;3;17;0
WireConnection;3;0;4;0
WireConnection;3;1;1;1
WireConnection;19;0;18;0
WireConnection;19;1;14;0
WireConnection;20;0;14;0
WireConnection;13;0;3;0
WireConnection;13;1;10;0
WireConnection;13;2;1;2
WireConnection;21;0;13;0
WireConnection;21;1;19;0
WireConnection;21;2;20;0
WireConnection;0;2;21;0
WireConnection;0;9;1;2
ASEEND*/
//CHKSM=1AB8D5D96B8CD158150B9E5405BC8F022B0A7285