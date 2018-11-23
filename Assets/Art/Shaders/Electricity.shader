// Shader created with Shader Forge v1.38 
// Shader Forge (c) Freya Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:Dissolve,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:0,dpts:2,wrdp:True,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:False,qofs:0,qpre:2,rntp:3,fgom:False,fgoc:True,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:True,fnfb:True,fsmp:False;n:type:ShaderForge.SFN_Final,id:4795,x:33328,y:32578,varname:node_4795,prsc:2|emission-2393-OUT,clip-1262-OUT;n:type:ShaderForge.SFN_Tex2d,id:6074,x:32781,y:32451,ptovrint:False,ptlb:MainTex,ptin:_MainTex,varname:_MainTex,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:276429d5546ced742adb7717bb67cdcc,ntxv:0,isnm:False|UVIN-8888-OUT;n:type:ShaderForge.SFN_Multiply,id:2393,x:33089,y:32677,varname:node_2393,prsc:2|A-6074-RGB,B-2053-RGB,C-797-RGB,D-3883-OUT;n:type:ShaderForge.SFN_VertexColor,id:2053,x:32781,y:32653,varname:node_2053,prsc:2;n:type:ShaderForge.SFN_Color,id:797,x:32781,y:32808,ptovrint:True,ptlb:Tint,ptin:_TintColor,varname:_TintColor,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0,c2:0.3793104,c3:1,c4:1;n:type:ShaderForge.SFN_ValueProperty,id:8796,x:30466,y:32673,ptovrint:False,ptlb:V Speed,ptin:_VSpeed,varname:node_8796,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.2;n:type:ShaderForge.SFN_ValueProperty,id:9326,x:30480,y:32544,ptovrint:False,ptlb:U Speed,ptin:_USpeed,varname:node_9326,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.2;n:type:ShaderForge.SFN_Append,id:4084,x:30734,y:32544,varname:node_4084,prsc:2|A-9326-OUT,B-8796-OUT;n:type:ShaderForge.SFN_Multiply,id:7095,x:30979,y:32524,varname:node_7095,prsc:2|A-4084-OUT,B-1469-T;n:type:ShaderForge.SFN_TexCoord,id:7983,x:30968,y:32703,varname:node_7983,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Add,id:5660,x:31194,y:32524,varname:node_5660,prsc:2|A-7095-OUT,B-7983-UVOUT;n:type:ShaderForge.SFN_Tex2d,id:9893,x:31393,y:32524,varname:node_9893,prsc:2,tex:28c7aad1372ff114b90d330f8a2dd938,ntxv:0,isnm:False|UVIN-5660-OUT,TEX-1020-TEX;n:type:ShaderForge.SFN_Tex2dAsset,id:1020,x:31194,y:32721,ptovrint:False,ptlb:Noise,ptin:_Noise,varname:node_1020,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:28c7aad1372ff114b90d330f8a2dd938,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Append,id:4131,x:30748,y:32923,varname:node_4131,prsc:2|A-275-OUT,B-9028-OUT;n:type:ShaderForge.SFN_Time,id:4652,x:30748,y:33102,varname:node_4652,prsc:2;n:type:ShaderForge.SFN_Multiply,id:2893,x:30986,y:32912,varname:node_2893,prsc:2|A-4131-OUT,B-4652-T;n:type:ShaderForge.SFN_TexCoord,id:7968,x:30986,y:33092,varname:node_7968,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_ValueProperty,id:275,x:30466,y:32903,ptovrint:False,ptlb:2U Speed,ptin:_2USpeed,varname:node_275,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:-0.2;n:type:ShaderForge.SFN_ValueProperty,id:9028,x:30466,y:33024,ptovrint:False,ptlb:2V Speed,ptin:_2VSpeed,varname:node_9028,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:-0.05;n:type:ShaderForge.SFN_Add,id:9780,x:31205,y:32948,varname:node_9780,prsc:2|A-2893-OUT,B-7968-UVOUT;n:type:ShaderForge.SFN_Tex2d,id:7262,x:31420,y:32930,varname:node_7262,prsc:2,tex:28c7aad1372ff114b90d330f8a2dd938,ntxv:0,isnm:False|UVIN-9780-OUT,TEX-1020-TEX;n:type:ShaderForge.SFN_Slider,id:8010,x:30707,y:32292,ptovrint:False,ptlb:Dissolve,ptin:_Dissolve,varname:node_8010,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.3762974,max:1;n:type:ShaderForge.SFN_OneMinus,id:1814,x:31063,y:32291,varname:node_1814,prsc:2|IN-8010-OUT;n:type:ShaderForge.SFN_RemapRange,id:5717,x:31243,y:32291,varname:node_5717,prsc:2,frmn:0,frmx:1,tomn:-0.9,tomx:0.9|IN-1814-OUT;n:type:ShaderForge.SFN_Add,id:7619,x:31545,y:32311,varname:node_7619,prsc:2|A-5717-OUT,B-9893-R;n:type:ShaderForge.SFN_Add,id:8927,x:31627,y:32601,varname:node_8927,prsc:2|A-5717-OUT,B-7262-R;n:type:ShaderForge.SFN_Multiply,id:1602,x:31798,y:32451,varname:node_1602,prsc:2|A-7619-OUT,B-8927-OUT;n:type:ShaderForge.SFN_Time,id:1469,x:30723,y:32732,varname:node_1469,prsc:2;n:type:ShaderForge.SFN_RemapRange,id:3689,x:32010,y:32451,varname:node_3689,prsc:2,frmn:0,frmx:1,tomn:-10,tomx:10|IN-1602-OUT;n:type:ShaderForge.SFN_Clamp01,id:3029,x:32189,y:32451,varname:node_3029,prsc:2|IN-3689-OUT;n:type:ShaderForge.SFN_OneMinus,id:4775,x:32373,y:32451,varname:node_4775,prsc:2|IN-3029-OUT;n:type:ShaderForge.SFN_Vector1,id:5866,x:32373,y:32614,varname:node_5866,prsc:2,v1:0;n:type:ShaderForge.SFN_Append,id:8888,x:32586,y:32451,varname:node_8888,prsc:2|A-4775-OUT,B-5866-OUT;n:type:ShaderForge.SFN_ValueProperty,id:3883,x:32781,y:32999,ptovrint:False,ptlb:Opacity,ptin:_Opacity,varname:node_3883,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:2;n:type:ShaderForge.SFN_Multiply,id:1262,x:32980,y:32386,varname:node_1262,prsc:2|A-5170-OUT,B-6074-R;n:type:ShaderForge.SFN_ValueProperty,id:5170,x:32759,y:32314,ptovrint:False,ptlb:Strech,ptin:_Strech,varname:node_5170,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.8;proporder:6074-797-8796-9326-1020-8010-275-9028-3883-5170;pass:END;sub:END;*/

Shader "Shader Forge/Electricity" {
    Properties {
        _MainTex ("MainTex", 2D) = "white" {}
        _TintColor ("Tint", Color) = (0,0.3793104,1,1)
        _VSpeed ("V Speed", Float ) = 0.2
        _USpeed ("U Speed", Float ) = 0.2
        _Noise ("Noise", 2D) = "white" {}
        _Dissolve ("Dissolve", Range(0, 1)) = 0.3762974
        _2USpeed ("2U Speed", Float ) = -0.2
        _2VSpeed ("2V Speed", Float ) = -0.05
        _Opacity ("Opacity", Float ) = 2
        _Strech ("Strech", Float ) = 0.8
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "Queue"="AlphaTest"
            "RenderType"="TransparentCutout"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend One One
            
			Cull Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 metal d3d11_9x 
            #pragma target 3.0
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform float4 _TintColor;
            uniform float _VSpeed;
            uniform float _USpeed;
            uniform sampler2D _Noise; uniform float4 _Noise_ST;
            uniform float _2USpeed;
            uniform float _2VSpeed;
            uniform float _Dissolve;
            uniform float _Opacity;
            uniform float _Strech;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 vertexColor : COLOR;
                UNITY_FOG_COORDS(1)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                float node_5717 = ((1.0 - _Dissolve)*1.8+-0.9);
                float4 node_1469 = _Time;
                float2 node_5660 = ((float2(_USpeed,_VSpeed)*node_1469.g)+i.uv0);
                float4 node_9893 = tex2D(_Noise,TRANSFORM_TEX(node_5660, _Noise));
                float4 node_4652 = _Time;
                float2 node_9780 = ((float2(_2USpeed,_2VSpeed)*node_4652.g)+i.uv0);
                float4 node_7262 = tex2D(_Noise,TRANSFORM_TEX(node_9780, _Noise));
                float2 node_8888 = float2((1.0 - saturate((((node_5717+node_9893.r)*(node_5717+node_7262.r))*20.0+-10.0))),0.0);
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(node_8888, _MainTex));
                clip((_Strech*_MainTex_var.r) - 0.5);
////// Lighting:
////// Emissive:
                float3 emissive = (_MainTex_var.rgb*i.vertexColor.rgb*_TintColor.rgb*_Opacity);
                float3 finalColor = emissive;
                fixed4 finalRGBA = fixed4(finalColor,1);
                UNITY_APPLY_FOG_COLOR(i.fogCoord, finalRGBA, fixed4(0.5,0.5,0.5,1));
                return finalRGBA;
            }
            ENDCG
        }
    }
	CustomEditor "ASEMaterialInspector"
}
