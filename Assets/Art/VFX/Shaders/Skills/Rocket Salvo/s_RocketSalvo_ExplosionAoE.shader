// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Gastón Zabala/Skills/RocketSalvo/ExplosionAoE"
{
	Properties
	{
		_DepthFade_Controller("DepthFade_Controller", Float) = 0
		_Fresnel_Scale("Fresnel_Scale", Float) = 0
		_Fresnel_Power("Fresnel_Power", Float) = 0
		_Fresnel_Bias("Fresnel_Bias", Float) = 0
		_MainTexture("Main Texture", 2D) = "white" {}
		_Noise_Adjustment("Noise_Adjustment", Float) = 0
		[HDR]_Emission_Color("Emission_Color", Color) = (0,0,0,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityCG.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		struct Input
		{
			float4 screenPos;
			float3 worldPos;
			float3 worldNormal;
			float2 uv_texcoord;
			float4 vertexColor : COLOR;
		};

		uniform float4 _Emission_Color;
		UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
		uniform float4 _CameraDepthTexture_TexelSize;
		uniform float _DepthFade_Controller;
		uniform float _Fresnel_Bias;
		uniform float _Fresnel_Scale;
		uniform float _Fresnel_Power;
		uniform sampler2D _MainTexture;
		uniform float4 _MainTexture_ST;
		uniform float _Noise_Adjustment;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float screenDepth1 = LinearEyeDepth(UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture,UNITY_PROJ_COORD( ase_screenPos ))));
			float distanceDepth1 = abs( ( screenDepth1 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( _DepthFade_Controller ) );
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = i.worldNormal;
			float fresnelNdotV8 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode8 = ( _Fresnel_Bias + _Fresnel_Scale * pow( 1.0 - fresnelNdotV8, _Fresnel_Power ) );
			float2 uv_MainTexture = i.uv_texcoord * _MainTexture_ST.xy + _MainTexture_ST.zw;
			float temp_output_15_0 = ( tex2D( _MainTexture, uv_MainTexture ).a - _Noise_Adjustment );
			float DepthFresnel16 = ( ( 1.0 - saturate( distanceDepth1 ) ) + ( saturate( fresnelNode8 ) * temp_output_15_0 ) );
			float temp_output_20_0 = saturate( ( DepthFresnel16 + temp_output_15_0 ) );
			o.Emission = ( ( _Emission_Color * temp_output_20_0 ) * float4( (i.vertexColor).rgb , 0.0 ) ).rgb;
			o.Alpha = saturate( ( temp_output_20_0 * i.vertexColor.a ) );
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
				float3 worldNormal : TEXCOORD4;
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
				o.worldNormal = worldNormal;
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				o.worldPos = worldPos;
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
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = IN.worldNormal;
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
7;74;1266;818;1234.83;270.3492;1.348608;True;False
Node;AmplifyShaderEditor.RangedFloatNode;7;-1788.919,129.7246;Float;False;Property;_Fresnel_Scale;Fresnel_Scale;1;0;Create;True;0;0;False;0;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;2;-1803,-109.2279;Float;False;Property;_DepthFade_Controller;DepthFade_Controller;0;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;5;-1781.919,31.72461;Float;False;Property;_Fresnel_Bias;Fresnel_Bias;3;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;6;-1788.919,223.7246;Float;False;Property;_Fresnel_Power;Fresnel_Power;2;0;Create;True;0;0;False;0;0;5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DepthFade;1;-1507.302,-127.577;Float;False;True;False;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.FresnelNode;8;-1494.919,71.72462;Float;False;Standard;WorldNormal;ViewDir;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;11;-1686.422,607.7416;Float;False;Property;_Noise_Adjustment;Noise_Adjustment;5;0;Create;True;0;0;False;0;0;0.16;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;10;-1764.417,378.9564;Float;True;Property;_MainTexture;Main Texture;4;0;Create;True;0;0;False;0;None;87473b44dad94a84e868a8983459cb6d;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;9;-1195.919,70.72462;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;15;-1386.97,542.6658;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;3;-1221.919,-126.2754;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;13;-1006.565,70.87607;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;4;-1037.919,-126.2754;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;14;-833.676,-25.31788;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;16;-670.9547,-30.93039;Float;False;DepthFresnel;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;19;-992.9589,291.7673;Float;False;16;DepthFresnel;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;18;-764.9219,310.1617;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;22;-693.9589,133.7674;Float;False;Property;_Emission_Color;Emission_Color;6;1;[HDR];Create;True;0;0;False;0;0,0,0,0;1,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;20;-573.9589,308.7673;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;23;-820.451,476.3;Float;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;21;-398.959,279.7673;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;26;-353.0883,539.178;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;24;-615.5475,473.116;Float;False;True;True;True;False;1;0;COLOR;0,0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;25;-231.5475,401.1159;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT3;0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;27;-175.0883,544.178;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;133.6483,293.5882;Float;False;True;2;Float;ASEMaterialInspector;0;0;Unlit;Gastón Zabala/Skills/RocketSalvo/ExplosionAoE;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;1;0;2;0
WireConnection;8;1;5;0
WireConnection;8;2;7;0
WireConnection;8;3;6;0
WireConnection;9;0;8;0
WireConnection;15;0;10;4
WireConnection;15;1;11;0
WireConnection;3;0;1;0
WireConnection;13;0;9;0
WireConnection;13;1;15;0
WireConnection;4;0;3;0
WireConnection;14;0;4;0
WireConnection;14;1;13;0
WireConnection;16;0;14;0
WireConnection;18;0;19;0
WireConnection;18;1;15;0
WireConnection;20;0;18;0
WireConnection;21;0;22;0
WireConnection;21;1;20;0
WireConnection;26;0;20;0
WireConnection;26;1;23;4
WireConnection;24;0;23;0
WireConnection;25;0;21;0
WireConnection;25;1;24;0
WireConnection;27;0;26;0
WireConnection;0;2;25;0
WireConnection;0;9;27;0
ASEEND*/
//CHKSM=85B454D2FA57237A82CD68B85E0EDD957DC2C175