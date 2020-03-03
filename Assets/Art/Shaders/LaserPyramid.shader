// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "FIREPOWER/LasertPyramid"
{
	Properties
	{
		_Flowmap("Flowmap", 2D) = "white" {}
		_PanSpeed("PanSpeed", Vector) = (0,0,0,0)
		_Normal("Normal", 2D) = "white" {}
		_Transparency("Transparency", Float) = 0
		_Albedo("Albedo", 2D) = "white" {}
		_Emission("Emission", 2D) = "white" {}
		_GridTiling("GridTiling", Vector) = (0,0,0,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Off
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _Normal;
		uniform float2 _PanSpeed;
		uniform sampler2D _Flowmap;
		uniform float4 _Flowmap_ST;
		uniform sampler2D _Albedo;
		uniform sampler2D _Emission;
		uniform float2 _GridTiling;
		uniform float _Transparency;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_Flowmap = i.uv_texcoord * _Flowmap_ST.xy + _Flowmap_ST.zw;
			float2 uv_TexCoord39 = i.uv_texcoord + tex2D( _Flowmap, uv_Flowmap ).rg;
			float2 panner40 = ( 1.0 * _Time.y * _PanSpeed + uv_TexCoord39);
			o.Normal = tex2D( _Normal, panner40 ).rgb;
			o.Albedo = tex2D( _Albedo, panner40 ).rgb;
			float2 uv_TexCoord13 = i.uv_texcoord * _GridTiling;
			float2 panner19 = ( _Time.y * float2( 0,0.3 ) + uv_TexCoord13);
			float2 lerpResult56 = lerp( panner40 , panner19 , float2( 0.97,0.99 ));
			o.Emission = (float4( 0,0,0,0 ) + (tex2D( _Emission, lerpResult56 ) - float4( 1,1,1,0 )) * (float4( 0.4433962,0,0.008694123,0 ) - float4( 0,0,0,0 )) / (float4( 0.9433962,0.9433962,0.9433962,0 ) - float4( 1,1,1,0 ))).rgb;
			float mulTime66 = _Time.y * 4.2;
			float clampResult68 = clamp( sin( mulTime66 ) , 0.03 , 1.0 );
			o.Alpha = ( clampResult68 * _Transparency );
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard alpha:fade keepalpha fullforwardshadows 

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
				float4 tSpace0 : TEXCOORD3;
				float4 tSpace1 : TEXCOORD4;
				float4 tSpace2 : TEXCOORD5;
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
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
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
423;260;1352;616;1101.654;324.7603;1.380391;True;False
Node;AmplifyShaderEditor.CommentaryNode;59;-4256,-768;Float;False;1105.256;632.2937;Aguante Videla, gordo boludo;4;23;19;13;41;Panning laser grid;0.003575563,1,0,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;58;-4048,-1696;Float;False;1008.968;761.3196;Biagi pelotudo;4;37;38;39;40;Flowmap for distorting the center;1,1,1,1;0;0
Node;AmplifyShaderEditor.SamplerNode;37;-4000,-1648;Float;True;Property;_Flowmap;Flowmap;0;0;Create;True;0;0;False;0;None;920b2289f820f91489558539f21b1313;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;41;-4062.166,-696.9252;Float;False;Property;_GridTiling;GridTiling;6;0;Create;True;0;0;False;0;0,0;10,10;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;38;-3616,-1248;Float;False;Property;_PanSpeed;PanSpeed;1;0;Create;True;0;0;False;0;0,0;0.5,0.2;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;39;-3600,-1600;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;13;-3703.872,-700.0192;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;69;-767.6943,-140.5644;Float;False;999.0789;614.1652;;5;66;63;68;4;62;Laser pulses;1,1,1,1;0;0
Node;AmplifyShaderEditor.TimeNode;23;-3746.165,-414.6035;Float;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;40;-3312,-1376;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;19;-3432.412,-492.5032;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0.3;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleTimeNode;66;-717.6943,-71.64163;Float;False;1;0;FLOAT;4.2;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;56;-2756.679,-957.2828;Float;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT2;0.97,0.99;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SinOpNode;63;-512.0574,-90.56439;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;60;-2032,-928;Float;False;983.9967;407.5174;Hail Hitler;2;10;18;Transform grid texture to rays only, so we can colorize them;1,1,1,1;0;0
Node;AmplifyShaderEditor.SamplerNode;10;-1984,-880;Float;True;Property;_Emission;Emission;5;0;Create;True;0;0;False;0;5cd19191a09d1374581a62e2eb9e72e4;5cd19191a09d1374581a62e2eb9e72e4;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;68;-274.5957,-70.99416;Float;False;3;0;FLOAT;0;False;1;FLOAT;0.03;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;4;-614.2036,214.2204;Float;False;Property;_Transparency;Transparency;3;0;Create;True;0;0;False;0;0;0.75;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;36;-1909.234,-1673.205;Float;True;Property;_Albedo;Albedo;4;0;Create;True;0;0;False;0;736ca06ecda3ab84a97d7cfe3d921c3c;c126515de2f5f0e4a9a807bb7e22c952;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;62;-3.615449,-17.85504;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;18;-1344,-784;Float;False;5;0;COLOR;0,0,0,0;False;1;COLOR;1,1,1,0;False;2;COLOR;0.9433962,0.9433962,0.9433962,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0.4433962,0,0.008694123,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;3;-1884.274,-1389.337;Float;True;Property;_Normal;Normal;2;0;Create;True;0;0;False;0;None;cb2e3802c2a42084c83ff10a23a95d12;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1132.594,-223.7014;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;FIREPOWER/LasertPyramid;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;39;1;37;0
WireConnection;13;0;41;0
WireConnection;40;0;39;0
WireConnection;40;2;38;0
WireConnection;19;0;13;0
WireConnection;19;1;23;2
WireConnection;56;0;40;0
WireConnection;56;1;19;0
WireConnection;63;0;66;0
WireConnection;10;1;56;0
WireConnection;68;0;63;0
WireConnection;36;1;40;0
WireConnection;62;0;68;0
WireConnection;62;1;4;0
WireConnection;18;0;10;0
WireConnection;3;1;40;0
WireConnection;0;0;36;0
WireConnection;0;1;3;0
WireConnection;0;2;18;0
WireConnection;0;9;62;0
ASEEND*/
//CHKSM=DB2F6D165C854F9B28C70155F29226F3077E8E28