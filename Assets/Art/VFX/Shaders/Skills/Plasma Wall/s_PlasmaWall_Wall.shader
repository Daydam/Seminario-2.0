// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Gastón Zabala/Skills/PlasmaWall/Wall"
{
	Properties
	{
		_MainTexture("Main Texture", 2D) = "white" {}
		[HDR]_MainColor("Main Color", Color) = (0,0,0,0)
		_ImpactTime("ImpactTime", Range( 0 , 50)) = 0
		[HDR]_ImpactColor("ImpactColor", Color) = (0,0,0,0)
		[HDR]_Emission_Border("Emission_Border", Color) = (0,0,0,0)
		_ImpactRadius("ImpactRadius", Float) = 0
		_FallOff("FallOff", Float) = 0
		_Cracks("Cracks", 2D) = "white" {}
		_Cracks_Intensity("Cracks_Intensity", Range( 0 , 9)) = 0
		[HDR]_Cracks_Color("Cracks_Color", Color) = (0,0,0,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Off
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.5
		#pragma surface surf Unlit alpha:fade keepalpha noshadow exclude_path:deferred 
		struct Input
		{
			float2 uv_texcoord;
			float3 worldPos;
		};

		uniform float4 _MainColor;
		uniform sampler2D _MainTexture;
		uniform sampler2D _Cracks;
		uniform float4 _Cracks_ST;
		uniform float _Cracks_Intensity;
		uniform float4 myObjects[30];
		uniform float _ImpactRadius;
		uniform float _FallOff;
		uniform float _ImpactTime;
		uniform float4 _ImpactColor;
		uniform float4 _Emission_Border;
		uniform float4 _Cracks_Color;


		float MyCustomExpression146( float3 worldPos , float3 positions , float radius )
		{
			float c = 10000;
			float z = 0;
			for (int i = 0; i< myObjects.Length; i++)
			{
				z = distance(worldPos, myObjects[i]);
				z /= radius;
				if (z < c) c = z;
			}
			return c;
		}


		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float2 uv_TexCoord4 = i.uv_texcoord * float2( 2,2 );
			float2 uv_Cracks = i.uv_texcoord * _Cracks_ST.xy + _Cracks_ST.zw;
			float temp_output_161_0 = saturate( ( tex2D( _Cracks, uv_Cracks ).r * _Cracks_Intensity ) );
			float Cracks165 = temp_output_161_0;
			float2 temp_output_168_0 = ( uv_TexCoord4 + Cracks165 );
			float2 panner7 = ( 1.0 * _Time.y * float2( -0.7,0 ) + temp_output_168_0);
			float2 panner3 = ( 1.0 * _Time.y * float2( 0.5,0 ) + temp_output_168_0);
			float Borders_Alpha62 = ( ( ( ( i.uv_texcoord.x + 0.0 ) * ( ( 1.0 - i.uv_texcoord.x ) + 0.0 ) ) * ( ( i.uv_texcoord.y + 0.0 ) * ( ( 1.0 - i.uv_texcoord.y ) + 0.0 ) ) ) * 6.93 );
			float3 ase_worldPos = i.worldPos;
			float3 worldPos146 = ase_worldPos;
			float3 positions146 = myObjects[0].xyz;
			float radius146 = _ImpactRadius;
			float localMyCustomExpression146 = MyCustomExpression146( worldPos146 , positions146 , radius146 );
			float temp_output_79_0 = saturate( pow( localMyCustomExpression146 , _FallOff ) );
			float temp_output_85_0 = saturate( (0.0 + (_ImpactTime - 0.0) * (1.0 - 0.0) / (100.0 - 0.0)) );
			float lerpResult151 = lerp( 0.0 , temp_output_79_0 , temp_output_85_0);
			float temp_output_6_0 = saturate( ( ( ( tex2D( _MainTexture, panner7 ).g + tex2D( _MainTexture, panner3 ).r ) * Borders_Alpha62 ) + lerpResult151 ) );
			float4 lerpResult92 = lerp( ( _MainColor * temp_output_6_0 ) , ( temp_output_79_0 * _ImpactColor ) , temp_output_85_0);
			float Borders_Emisison134 = saturate( ( ( saturate( ( ( i.uv_texcoord.x - 0.95 ) * 3.0 ) ) + saturate( ( ( ( 1.0 - i.uv_texcoord.x ) - 0.95 ) * 3.0 ) ) ) + ( saturate( ( ( i.uv_texcoord.y - 0.95 ) * 3.0 ) ) + saturate( ( ( ( 1.0 - i.uv_texcoord.y ) - 0.95 ) * 3.0 ) ) ) ) );
			float4 lerpResult135 = lerp( lerpResult92 , _Emission_Border , Borders_Emisison134);
			float4 lerpResult162 = lerp( lerpResult135 , ( Cracks165 * _Cracks_Color ) , Cracks165);
			o.Emission = lerpResult162.rgb;
			float Opacity139 = ( temp_output_6_0 + temp_output_161_0 );
			float lerpResult141 = lerp( Opacity139 , 1.0 , Borders_Emisison134);
			o.Alpha = lerpResult141;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16700
25;397;1266;743;1250.704;442.1609;3.725622;True;False
Node;AmplifyShaderEditor.RangedFloatNode;156;-550.3973,1357.902;Float;False;Property;_Cracks_Intensity;Cracks_Intensity;8;0;Create;True;0;0;False;0;0;9;0;9;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;152;-570.4914,1146.202;Float;True;Property;_Cracks;Cracks;7;0;Create;True;0;0;False;0;None;7fd6d0a44f6c5e842b6b40f8e57c0082;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;63;-1694.985,-1458.015;Float;False;1602.693;554.0742;;12;58;46;47;48;45;59;50;49;60;61;57;62;Borders Alpha;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;157;-180.9227,1173.748;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;58;-1644.985,-1240.715;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;161;3.710159,1172.805;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;47;-1303.796,-1279.45;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;49;-1303.384,-1034.047;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;165;204.6418,1167.366;Float;False;Cracks;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;48;-1102.451,-1281.794;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;133;-1688.703,-769.837;Float;False;1914.083;613.2893;;20;134;142;132;118;131;129;130;122;123;115;127;110;128;126;114;113;125;111;124;107;Borders;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleAddOpNode;45;-1304.862,-1146.89;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;50;-1102.466,-1033.941;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;46;-1303.884,-1404.015;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;167;-1848.603,346.0815;Float;False;165;Cracks;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;107;-1638.703,-535.1035;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;59;-932.8701,-1145.855;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;57;-888.5099,-1305.645;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;4;-1903.51,203.0931;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;2,2;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;60;-695.6823,-1167.793;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;124;-1283.897,-277.4168;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;111;-1276.522,-553.817;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;168;-1609.159,202.1837;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GlobalArrayNode;144;-1551.33,906.4695;Float;False;myObjects;0;30;2;False;False;0;1;False;Object;-1;4;0;INT;0;False;2;INT;0;False;1;INT;0;False;3;INT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;148;-1480.839,1023.966;Float;False;Property;_ImpactRadius;ImpactRadius;5;0;Create;True;0;0;False;0;0;0.08;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;147;-1511.825,740.4666;Float;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleSubtractOpNode;126;-1276.295,-439.3015;Float;False;2;0;FLOAT;0;False;1;FLOAT;0.95;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;61;-531.599,-1167.977;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;6.93;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;114;-1070.522,-553.817;Float;False;2;0;FLOAT;0;False;1;FLOAT;0.95;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;125;-1077.896,-277.4168;Float;False;2;0;FLOAT;0;False;1;FLOAT;0.95;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;113;-1268.92,-715.7016;Float;False;2;0;FLOAT;0;False;1;FLOAT;0.95;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;7;-1382.494,70.27918;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;-0.7,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;150;-1137.313,963.0993;Float;False;Property;_FallOff;FallOff;6;0;Create;True;0;0;False;0;0;-16.77;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CustomExpressionNode;146;-1184.882,813.0615;Float;False;float c = 10000@$float z = 0@$$for (int i = 0@ i< myObjects.Length@ i++)${$	z = distance(worldPos, myObjects[i])@$	z /= radius@$	if (z < c) c = z@$}$$return c@$;1;False;3;True;worldPos;FLOAT3;0,0,0;In;;Float;False;True;positions;FLOAT3;0,0,0;In;;Float;False;True;radius;FLOAT;0;In;;Float;False;My Custom Expression;True;False;0;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;3;-1373.705,332.0597;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0.5,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;68;-1246.402,608.8264;Float;False;Property;_ImpactTime;ImpactTime;2;0;Create;True;0;0;False;0;0;50;0;50;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;115;-873.5226,-553.817;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;3;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;110;-1065.228,-718.4147;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;3;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;127;-880.8965,-277.4168;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;3;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;128;-1072.603,-442.0145;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;3;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;1;-1084.864,40.75089;Float;True;Property;_MainTexture;Main Texture;0;0;Create;True;0;0;False;0;None;f303f13b0db3fa74488dace53e9790fa;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TFHCRemapNode;76;-931.7244,614.4499;Float;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;100;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;2;-1083.467,306.398;Float;True;Property;_TextureSample0;Texture Sample 0;0;0;Create;True;0;0;False;0;None;f303f13b0db3fa74488dace53e9790fa;True;0;False;white;Auto;False;Instance;1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;62;-358.2908,-1173.116;Float;False;Borders_Alpha;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;149;-902.3134,812.0994;Float;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;130;-895.87,-442.4368;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;123;-890.496,-719.837;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;122;-671.0984,-551.9523;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;129;-678.4725,-275.5521;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;9;-666.671,191.9617;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;79;-621.4506,813.5032;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;85;-642.6534,615.0017;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;64;-698.3228,321.7827;Float;False;62;Borders_Alpha;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;131;-511.8759,-437.1537;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;118;-504.502,-713.5538;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;66;-395.0042,191.6251;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;151;-433.9534,699.8395;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;132;-353.3605,-584.2141;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;90;-259.766,391.9344;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;142;-201.5198,-583.7393;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;74;-752.2958,928.6085;Float;False;Property;_ImpactColor;ImpactColor;3;1;[HDR];Create;True;0;0;False;0;0,0,0,0;178.9082,27.16407,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;15;-138.0725,129.2056;Float;False;Property;_MainColor;Main Color;1;1;[HDR];Create;True;0;0;False;0;0,0,0,0;0,4.392381,8.47419,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;6;-63.04612,393.5225;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;134;-22.36011,-587.3989;Float;False;Borders_Emisison;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;73;-406.0451,906.6085;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;28;247.0863,132.8127;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;160;125.7586,395.1282;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;166;534.5545,120.1026;Float;False;165;Cracks;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;92;574.7159,508.8155;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;159;508.0334,222.4685;Float;False;Property;_Cracks_Color;Cracks_Color;9;1;[HDR];Create;True;0;0;False;0;0,0,0,0;0,4.015687,23.96863,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;139;271.4072,391.897;Float;False;Opacity;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;137;509.6084,640.6289;Float;False;Property;_Emission_Border;Emission_Border;4;1;[HDR];Create;True;0;0;False;0;0,0,0,0;83.46362,13.508,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;136;507.4043,866.7948;Float;False;134;Borders_Emisison;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;135;895.4384,515.0793;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;158;857.8729,126.6332;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;140;1078.091,698.9001;Float;False;139;Opacity;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;162;1249.985,381.8608;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;141;1293.729,700.1017;Float;False;3;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1541.149,475.3011;Float;False;True;3;Float;ASEMaterialInspector;0;0;Unlit;Gastón Zabala/Skills/PlasmaWall/Wall;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;False;0;False;Transparent;;Transparent;ForwardOnly;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;6;2;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;157;0;152;1
WireConnection;157;1;156;0
WireConnection;161;0;157;0
WireConnection;47;0;58;1
WireConnection;49;0;58;2
WireConnection;165;0;161;0
WireConnection;48;0;47;0
WireConnection;45;0;58;2
WireConnection;50;0;49;0
WireConnection;46;0;58;1
WireConnection;59;0;45;0
WireConnection;59;1;50;0
WireConnection;57;0;46;0
WireConnection;57;1;48;0
WireConnection;60;0;57;0
WireConnection;60;1;59;0
WireConnection;124;0;107;2
WireConnection;111;0;107;1
WireConnection;168;0;4;0
WireConnection;168;1;167;0
WireConnection;126;0;107;2
WireConnection;61;0;60;0
WireConnection;114;0;111;0
WireConnection;125;0;124;0
WireConnection;113;0;107;1
WireConnection;7;0;168;0
WireConnection;146;0;147;0
WireConnection;146;1;144;0
WireConnection;146;2;148;0
WireConnection;3;0;168;0
WireConnection;115;0;114;0
WireConnection;110;0;113;0
WireConnection;127;0;125;0
WireConnection;128;0;126;0
WireConnection;1;1;7;0
WireConnection;76;0;68;0
WireConnection;2;1;3;0
WireConnection;62;0;61;0
WireConnection;149;0;146;0
WireConnection;149;1;150;0
WireConnection;130;0;128;0
WireConnection;123;0;110;0
WireConnection;122;0;115;0
WireConnection;129;0;127;0
WireConnection;9;0;1;2
WireConnection;9;1;2;1
WireConnection;79;0;149;0
WireConnection;85;0;76;0
WireConnection;131;0;130;0
WireConnection;131;1;129;0
WireConnection;118;0;123;0
WireConnection;118;1;122;0
WireConnection;66;0;9;0
WireConnection;66;1;64;0
WireConnection;151;1;79;0
WireConnection;151;2;85;0
WireConnection;132;0;118;0
WireConnection;132;1;131;0
WireConnection;90;0;66;0
WireConnection;90;1;151;0
WireConnection;142;0;132;0
WireConnection;6;0;90;0
WireConnection;134;0;142;0
WireConnection;73;0;79;0
WireConnection;73;1;74;0
WireConnection;28;0;15;0
WireConnection;28;1;6;0
WireConnection;160;0;6;0
WireConnection;160;1;161;0
WireConnection;92;0;28;0
WireConnection;92;1;73;0
WireConnection;92;2;85;0
WireConnection;139;0;160;0
WireConnection;135;0;92;0
WireConnection;135;1;137;0
WireConnection;135;2;136;0
WireConnection;158;0;166;0
WireConnection;158;1;159;0
WireConnection;162;0;135;0
WireConnection;162;1;158;0
WireConnection;162;2;166;0
WireConnection;141;0;140;0
WireConnection;141;2;136;0
WireConnection;0;2;162;0
WireConnection;0;9;141;0
ASEEND*/
//CHKSM=562208082BC67A81B4F5AAAA42FA0884A144DB78