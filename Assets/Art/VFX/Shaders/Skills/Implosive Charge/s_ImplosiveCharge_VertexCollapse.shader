// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Gastón Zabala/Skills/ImplosiveCharge_VertexCollapse"
{
	Properties
	{
		_MainColor("Main Color", Color) = (0,0,0,0)
		[HDR]_EmissionColor("Emission Color", Color) = (0,0,0,0)
		_Smoothness("Smoothness", Float) = 0
		_BlackHolePos("BlackHolePos", Vector) = (0,0,0,0)
		_Radius("Radius", Float) = 0
		_FallOff("FallOff", Float) = 0
		_PerlinNoise("Perlin Noise", 2D) = "white" {}
		_NoiseIntensity("Noise Intensity", Float) = 0
		_Fresnel_Color("Fresnel_Color", Color) = (0,0,0,0)
		_Fresnel_Bias("Fresnel_Bias", Float) = 0
		_Fresnel_Scale("Fresnel_Scale", Float) = 0
		_Fresnel_Power("Fresnel_Power", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		struct Input
		{
			float3 worldPos;
			float3 worldNormal;
			float2 uv_texcoord;
		};

		uniform float3 _BlackHolePos;
		uniform float _Radius;
		uniform float _FallOff;
		uniform sampler2D _PerlinNoise;
		uniform float4 _PerlinNoise_ST;
		uniform float _NoiseIntensity;
		uniform float4 _MainColor;
		uniform float4 _EmissionColor;
		uniform float4 _Fresnel_Color;
		uniform float _Fresnel_Bias;
		uniform float _Fresnel_Scale;
		uniform float _Fresnel_Power;
		uniform float _Smoothness;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float3 ase_worldPos = mul( unity_ObjectToWorld, v.vertex );
			float2 uv_PerlinNoise = v.texcoord * _PerlinNoise_ST.xy + _PerlinNoise_ST.zw;
			float temp_output_26_0 = ( tex2Dlod( _PerlinNoise, float4( uv_PerlinNoise, 0, 0.0) ).r * _NoiseIntensity );
			float temp_output_12_0 = saturate( ( pow( ( distance( ase_worldPos , _BlackHolePos ) / _Radius ) , _FallOff ) - saturate( temp_output_26_0 ) ) );
			float3 lerpResult17 = lerp( float3( 0,0,0 ) , ( ( ase_worldPos + ( 1.0 - _BlackHolePos ) ) - float3( 1,1,1 ) ) , ( temp_output_12_0 - 1.0 ));
			float4 transform18 = mul(unity_WorldToObject,float4( lerpResult17 , 0.0 ));
			float4 VertexOffset44 = transform18;
			v.vertex.xyz += VertexOffset44.xyz;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			o.Albedo = _MainColor.rgb;
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = i.worldNormal;
			float fresnelNdotV24 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode24 = ( _Fresnel_Bias + _Fresnel_Scale * pow( 1.0 - fresnelNdotV24, _Fresnel_Power ) );
			float4 Fresnel_Color42 = ( _Fresnel_Color * fresnelNode24 );
			float Fresnel_Mask43 = saturate( fresnelNode24 );
			float4 lerpResult52 = lerp( _EmissionColor , Fresnel_Color42 , Fresnel_Mask43);
			float2 uv_PerlinNoise = i.uv_texcoord * _PerlinNoise_ST.xy + _PerlinNoise_ST.zw;
			float temp_output_26_0 = ( tex2D( _PerlinNoise, uv_PerlinNoise ).r * _NoiseIntensity );
			float temp_output_12_0 = saturate( ( pow( ( distance( ase_worldPos , _BlackHolePos ) / _Radius ) , _FallOff ) - saturate( temp_output_26_0 ) ) );
			float4 lerpResult22 = lerp( lerpResult52 , float4( 0,0,0,0 ) , temp_output_12_0);
			float4 Emission53 = lerpResult22;
			o.Emission = Emission53.rgb;
			o.Smoothness = _Smoothness;
			o.Alpha = 1;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard keepalpha fullforwardshadows vertex:vertexDataFunc 

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
				vertexDataFunc( v, customInputData );
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
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
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
7;7;1266;948;2994.822;-387.8463;1;True;False
Node;AmplifyShaderEditor.WorldPosInputsNode;3;-2794.543,108.4221;Float;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.Vector3Node;4;-2788.691,300.1143;Float;False;Property;_BlackHolePos;BlackHolePos;3;0;Create;True;0;0;False;0;0,0,0;-0.24,0.416,6.011426;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.DistanceOpNode;5;-2394.4,206.2989;Float;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;6;-2379.583,324.0508;Float;False;Property;_Radius;Radius;4;0;Create;True;0;0;False;0;0;1.3;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;27;-2749.841,822.445;Float;False;Property;_NoiseIntensity;Noise Intensity;7;0;Create;True;0;0;False;0;0;10.59;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;25;-2834.349,618.8328;Float;True;Property;_PerlinNoise;Perlin Noise;6;0;Create;True;0;0;False;0;None;b59197fee5b624c4f80793ec93eb2347;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleDivideOpNode;10;-2192.064,199.0604;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;9;-2202.798,329.5645;Float;False;Property;_FallOff;FallOff;5;0;Create;True;0;0;False;0;0;-3.12;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;32;-2711.981,1097.348;Float;False;Property;_Fresnel_Bias;Fresnel_Bias;9;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;26;-2502.496,645.3451;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;34;-2720.975,1315.986;Float;False;Property;_Fresnel_Power;Fresnel_Power;11;0;Create;True;0;0;False;0;0;1.08;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;33;-2716.752,1196.625;Float;False;Property;_Fresnel_Scale;Fresnel_Scale;10;0;Create;True;0;0;False;0;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FresnelNode;24;-2490.004,1126.786;Float;False;Standard;WorldNormal;ViewDir;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;40;-2406.482,919.9212;Float;False;Property;_Fresnel_Color;Fresnel_Color;8;0;Create;True;0;0;False;0;0,0,0,0;0.3576933,0.002758995,0.5849056,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;30;-2056.651,643.9265;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;11;-1994.063,203.0604;Float;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;41;-2128.448,1089.134;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;15;-2295.961,488.1499;Float;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;31;-1768.477,205.0217;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;35;-2128.314,1208.135;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;14;-1714.935,463.6444;Float;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SaturateNode;12;-1568.99,204.7958;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;43;-1961.427,1201.729;Float;False;Fresnel_Mask;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;42;-1968.79,1084.142;Float;False;Fresnel_Color;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;46;-1833.731,70.66113;Float;False;43;Fresnel_Mask;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;13;-1372.348,205.0313;Float;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;16;-1399.116,464.115;Float;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;1,1,1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;47;-1832.731,-22.33887;Float;False;42;Fresnel_Color;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;21;-1827.597,-211.2628;Float;False;Property;_EmissionColor;Emission Color;1;1;[HDR];Create;True;0;0;False;0;0,0,0,0;11.98431,4.956863,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;17;-1133.201,439.6319;Float;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;52;-1512.074,-56.25105;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.WorldToObjectTransfNode;18;-915.2654,441.7528;Float;False;1;0;FLOAT4;0,0,0,1;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;22;-1305.748,-54.68712;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;44;-685.7002,436.1694;Float;False;VertexOffset;-1;True;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;53;-1102.008,-61.64546;Float;False;Emission;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;1;-213.958,131.3726;Float;False;Property;_Smoothness;Smoothness;2;0;Create;True;0;0;False;0;0;0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;29;-2250.758,641.6184;Float;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;2;-333.9764,-196.9906;Float;False;Property;_MainColor;Main Color;0;0;Create;True;0;0;False;0;0,0,0,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;45;-225.1379,270.2099;Float;False;44;VertexOffset;1;0;OBJECT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.GetLocalVarNode;54;-257.3428,33.83746;Float;False;53;Emission;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;Gastón Zabala/Skills/ImplosiveCharge_VertexCollapse;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;5;0;3;0
WireConnection;5;1;4;0
WireConnection;10;0;5;0
WireConnection;10;1;6;0
WireConnection;26;0;25;1
WireConnection;26;1;27;0
WireConnection;24;1;32;0
WireConnection;24;2;33;0
WireConnection;24;3;34;0
WireConnection;30;0;26;0
WireConnection;11;0;10;0
WireConnection;11;1;9;0
WireConnection;41;0;40;0
WireConnection;41;1;24;0
WireConnection;15;0;4;0
WireConnection;31;0;11;0
WireConnection;31;1;30;0
WireConnection;35;0;24;0
WireConnection;14;0;3;0
WireConnection;14;1;15;0
WireConnection;12;0;31;0
WireConnection;43;0;35;0
WireConnection;42;0;41;0
WireConnection;13;0;12;0
WireConnection;16;0;14;0
WireConnection;17;1;16;0
WireConnection;17;2;13;0
WireConnection;52;0;21;0
WireConnection;52;1;47;0
WireConnection;52;2;46;0
WireConnection;18;0;17;0
WireConnection;22;0;52;0
WireConnection;22;2;12;0
WireConnection;44;0;18;0
WireConnection;53;0;22;0
WireConnection;29;0;26;0
WireConnection;0;0;2;0
WireConnection;0;2;54;0
WireConnection;0;4;1;0
WireConnection;0;11;45;0
ASEEND*/
//CHKSM=009E39C159F2CD8E574C1DB0266EBCBA8E543B0B