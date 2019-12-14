// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Gastón Zabala/Skills/ImplosiveCharge_Proyectile"
{
	Properties
	{
		_Flowmap("Flowmap", 2D) = "white" {}
		_Flowmap_Speed_Y("Flowmap_Speed_Y", Float) = 0
		_Flowmap_Speed_X("Flowmap_Speed_X", Float) = 0
		_Intensity("Intensity", Float) = 0
		_Float0("Float 0", Float) = 0
		_Fresnel_Power("Fresnel_Power", Float) = 0
		_Fresnel_Scale("Fresnel_Scale", Float) = 0
		_Fresnel_Bias("Fresnel_Bias", Float) = 0
		[HDR]_Fresnel_Color("Fresnel_Color", Color) = (0,0,0,0)
		[HDR]_MainColor("MainColor", Color) = (0,0,0,0)
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 4.6
		struct Input
		{
			float3 worldPos;
			float3 worldNormal;
		};

		uniform sampler2D _Flowmap;
		uniform float _Flowmap_Speed_X;
		uniform float _Flowmap_Speed_Y;
		uniform float _Float0;
		uniform float _Intensity;
		uniform float4 _MainColor;
		uniform float4 _Fresnel_Color;
		uniform float _Fresnel_Bias;
		uniform float _Fresnel_Scale;
		uniform float _Fresnel_Power;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float2 appendResult11 = (float2(_Flowmap_Speed_X , _Flowmap_Speed_Y));
			float2 panner8 = ( 1.0 * _Time.y * appendResult11 + v.texcoord.xy);
			float4 tex2DNode1 = tex2Dlod( _Flowmap, float4( panner8, 0, 0.0) );
			float3 appendResult14 = (float3(0.0 , ( ( tex2DNode1.g * v.texcoord.xy.y ) * _Float0 ) , ( ( tex2DNode1.r * v.texcoord.xy.x ) * _Intensity )));
			v.vertex.xyz += appendResult14;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = i.worldNormal;
			float fresnelNdotV20 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode20 = ( _Fresnel_Bias + _Fresnel_Scale * pow( 1.0 - fresnelNdotV20, _Fresnel_Power ) );
			float4 lerpResult28 = lerp( _MainColor , _Fresnel_Color , saturate( fresnelNode20 ));
			o.Emission = lerpResult28.rgb;
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
			#pragma target 4.6
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
				float3 worldPos : TEXCOORD1;
				float3 worldNormal : TEXCOORD2;
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
7;7;1266;948;1777.551;867.9586;1.366107;True;False
Node;AmplifyShaderEditor.RangedFloatNode;10;-1524.647,213.4305;Float;False;Property;_Flowmap_Speed_Y;Flowmap_Speed_Y;1;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;9;-1527.647,118.4305;Float;False;Property;_Flowmap_Speed_X;Flowmap_Speed_X;2;0;Create;True;0;0;False;0;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;5;-1375.243,353.0332;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;11;-1298.647,178.4305;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;8;-1133.647,151.4305;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;23;-1434.747,-177.4406;Float;False;Property;_Fresnel_Power;Fresnel_Power;5;0;Create;True;0;0;False;0;0;5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;1;-910.7549,124.0716;Float;True;Property;_Flowmap;Flowmap;0;0;Create;True;0;0;False;0;None;920b2289f820f91489558539f21b1313;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;21;-1421.43,-397.1725;Float;False;Property;_Fresnel_Bias;Fresnel_Bias;7;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;22;-1425.637,-287.4163;Float;False;Property;_Fresnel_Scale;Fresnel_Scale;6;0;Create;True;0;0;False;0;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;13;-560.8999,369.538;Float;False;Property;_Intensity;Intensity;3;0;Create;True;0;0;False;0;0;1.64;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;7;-571.3031,226.948;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;19;-469.3584,80.45155;Float;False;Property;_Float0;Float 0;4;0;Create;True;0;0;False;0;0;0.17;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FresnelNode;20;-1191.439,-357.1828;Float;False;Standard;WorldNormal;ViewDir;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;17;-504.5052,-89.38017;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;12;-354.3011,224.7376;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;29;-999.5682,-716.6899;Float;False;Property;_MainColor;MainColor;9;1;[HDR];Create;True;0;0;False;0;0,0,0,0;0.005684472,0.003248495,0.009433985,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;18;-256.1484,38.51424;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;24;-927.9691,-357.2942;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;26;-1003.427,-539.6163;Float;False;Property;_Fresnel_Color;Fresnel_Color;8;1;[HDR];Create;True;0;0;False;0;0,0,0,0;0.06674222,0,0.1037736,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;28;-601.9135,-366.8661;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.DynamicAppendNode;14;-189.7732,218.8027;Float;False;FLOAT3;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;3,-57;Float;False;True;6;Float;ASEMaterialInspector;0;0;Standard;Gastón Zabala/Skills/ImplosiveCharge_Proyectile;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;18.2;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;11;0;9;0
WireConnection;11;1;10;0
WireConnection;8;0;5;0
WireConnection;8;2;11;0
WireConnection;1;1;8;0
WireConnection;7;0;1;1
WireConnection;7;1;5;1
WireConnection;20;1;21;0
WireConnection;20;2;22;0
WireConnection;20;3;23;0
WireConnection;17;0;1;2
WireConnection;17;1;5;2
WireConnection;12;0;7;0
WireConnection;12;1;13;0
WireConnection;18;0;17;0
WireConnection;18;1;19;0
WireConnection;24;0;20;0
WireConnection;28;0;29;0
WireConnection;28;1;26;0
WireConnection;28;2;24;0
WireConnection;14;1;18;0
WireConnection;14;2;12;0
WireConnection;0;2;28;0
WireConnection;0;11;14;0
ASEEND*/
//CHKSM=87813E0495EB146E116AA0222C8F313B3FA20EC0