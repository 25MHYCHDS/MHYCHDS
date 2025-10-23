// Made with Amplify Shader Editor v1.9.1.5
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "SwordBladeStream"
{
	Properties
	{
		_TintColor ("Tint Color", Color) = (0.5,0.5,0.5,0.5)
		_MainTex ("Particle Texture", 2D) = "white" {}
		_InvFade ("Soft Particles Factor", Range(0.01,3.0)) = 1.0
		_Main("Main", 2D) = "white" {}
		_Opacity("Opacity", Float) = 20
		_Dissovle("Dissovle", 2D) = "white" {}
		_EmessionTexture("Emession Texture", 2D) = "white" {}
		_Emission("Emission", Float) = 2
		_end("end", Color) = (0.2222657,0.09803923,0.7490196,0)
		_Desaturate("Desaturate", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}

	}


	Category 
	{
		SubShader
		{
		LOD 0

			Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" }
			Blend SrcAlpha OneMinusSrcAlpha
			ColorMask RGB
			Cull Off
			Lighting Off 
			ZWrite Off
			ZTest LEqual
			
			Pass {
			
				CGPROGRAM
				
				#ifndef UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX
				#define UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input)
				#endif
				
				#pragma vertex vert
				#pragma fragment frag
				#pragma target 2.0
				#pragma multi_compile_instancing
				#pragma multi_compile_particles
				#pragma multi_compile_fog
				#include "UnityShaderVariables.cginc"
				#define ASE_NEEDS_FRAG_COLOR


				#include "UnityCG.cginc"

				struct appdata_t 
				{
					float4 vertex : POSITION;
					fixed4 color : COLOR;
					float4 texcoord : TEXCOORD0;
					UNITY_VERTEX_INPUT_INSTANCE_ID
					float4 ase_texcoord2 : TEXCOORD2;
				};

				struct v2f 
				{
					float4 vertex : SV_POSITION;
					fixed4 color : COLOR;
					float4 texcoord : TEXCOORD0;
					UNITY_FOG_COORDS(1)
					#ifdef SOFTPARTICLES_ON
					float4 projPos : TEXCOORD2;
					#endif
					UNITY_VERTEX_INPUT_INSTANCE_ID
					UNITY_VERTEX_OUTPUT_STEREO
					float4 ase_texcoord3 : TEXCOORD3;
				};
				
				
				#if UNITY_VERSION >= 560
				UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
				#else
				uniform sampler2D_float _CameraDepthTexture;
				#endif

				//Don't delete this comment
				// uniform sampler2D_float _CameraDepthTexture;

				uniform sampler2D _MainTex;
				uniform fixed4 _TintColor;
				uniform float4 _MainTex_ST;
				uniform float _InvFade;
				uniform float4 _end;
				uniform sampler2D _EmessionTexture;
				uniform float4 _EmessionTexture_ST;
				uniform float _Desaturate;
				uniform float _Emission;
				uniform sampler2D _Main;
				uniform float4 _Main_ST;
				uniform float _Opacity;
				uniform sampler2D _Dissovle;
				uniform float4 _Dissovle_ST;
				float3 HSVToRGB( float3 c )
				{
					float4 K = float4( 1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0 );
					float3 p = abs( frac( c.xxx + K.xyz ) * 6.0 - K.www );
					return c.z * lerp( K.xxx, saturate( p - K.xxx ), c.y );
				}
				
				float3 RGBToHSV(float3 c)
				{
					float4 K = float4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
					float4 p = lerp( float4( c.bg, K.wz ), float4( c.gb, K.xy ), step( c.b, c.g ) );
					float4 q = lerp( float4( p.xyw, c.r ), float4( c.r, p.yzx ), step( p.x, c.r ) );
					float d = q.x - min( q.w, q.y );
					float e = 1.0e-10;
					return float3( abs(q.z + (q.w - q.y) / (6.0 * d + e)), d / (q.x + e), q.x);
				}


				v2f vert ( appdata_t v  )
				{
					v2f o;
					UNITY_SETUP_INSTANCE_ID(v);
					UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
					UNITY_TRANSFER_INSTANCE_ID(v, o);
					o.ase_texcoord3 = v.ase_texcoord2;

					v.vertex.xyz +=  float3( 0, 0, 0 ) ;
					o.vertex = UnityObjectToClipPos(v.vertex);
					#ifdef SOFTPARTICLES_ON
						o.projPos = ComputeScreenPos (o.vertex);
						COMPUTE_EYEDEPTH(o.projPos.z);
					#endif
					o.color = v.color;
					o.texcoord = v.texcoord;
					UNITY_TRANSFER_FOG(o,o.vertex);
					return o;
				}

				fixed4 frag ( v2f i  ) : SV_Target
				{
					UNITY_SETUP_INSTANCE_ID( i );
					UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( i );

					#ifdef SOFTPARTICLES_ON
						float sceneZ = LinearEyeDepth (SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos)));
						float partZ = i.projPos.z;
						float fade = saturate (_InvFade * (sceneZ-partZ));
						i.color.a *= fade;
					#endif

					float2 uv_EmessionTexture = i.texcoord.xy * _EmessionTexture_ST.xy + _EmessionTexture_ST.zw;
					float3 hsvTorgb44 = RGBToHSV( tex2D( _EmessionTexture, uv_EmessionTexture ).rgb );
					float4 texCoord33 = i.ase_texcoord3;
					texCoord33.xy = i.ase_texcoord3.xy * float2( 1,1 ) + float2( 0,0 );
					float3 hsvTorgb45 = HSVToRGB( float3(( hsvTorgb44.x + texCoord33.z ),hsvTorgb44.y,hsvTorgb44.z) );
					float3 desaturateInitialColor47 = hsvTorgb45;
					float desaturateDot47 = dot( desaturateInitialColor47, float3( 0.299, 0.587, 0.114 ));
					float3 desaturateVar47 = lerp( desaturateInitialColor47, desaturateDot47.xxx, _Desaturate );
					float4 _Vector1 = float4(0,1,-2,1);
					float3 temp_cast_1 = (_Vector1.x).xxx;
					float3 temp_cast_2 = (_Vector1.y).xxx;
					float3 temp_cast_3 = (_Vector1.z).xxx;
					float3 temp_cast_4 = (_Vector1.w).xxx;
					float3 clampResult38 = clamp( (temp_cast_3 + (desaturateVar47 - temp_cast_1) * (temp_cast_4 - temp_cast_3) / (temp_cast_2 - temp_cast_1)) , float3( 0,0,0 ) , float3( 1,1,1 ) );
					float2 uv_Main = i.texcoord.xy * _Main_ST.xy + _Main_ST.zw;
					float clampResult4 = clamp( ( tex2D( _Main, uv_Main ).a * _Opacity ) , 0.0 , 1.0 );
					float4 _SpeedMainTexUV = float4(0,0,1,0);
					float2 appendResult22 = (float2(_SpeedMainTexUV.z , _SpeedMainTexUV.w));
					float4 uvs4_Dissovle = i.texcoord;
					uvs4_Dissovle.xy = i.texcoord.xy * _Dissovle_ST.xy + _Dissovle_ST.zw;
					float2 panner27 = ( 1.0 * _Time.y * appendResult22 + uvs4_Dissovle.xy);
					float2 break30 = panner27;
					float2 appendResult31 = (float2(break30.x , ( texCoord33.w + break30.y )));
					float w26 = uvs4_Dissovle.z;
					float t25 = uvs4_Dissovle.w;
					float3 _Vector0 = float3(1,1,0);
					float ifLocalVar12 = 0;
					if( ( tex2D( _Dissovle, appendResult31 ).r * w26 ) >= t25 )
					ifLocalVar12 = _Vector0.y;
					else
					ifLocalVar12 = _Vector0.z;
					float4 appendResult5 = (float4(( ( _end * i.color ) + ( float4( clampResult38 , 0.0 ) * _Emission * i.color ) ).rgb , ( i.color.a * clampResult4 * ifLocalVar12 )));
					

					fixed4 col = appendResult5;
					UNITY_APPLY_FOG(i.fogCoord, col);
					return col;
				}
				ENDCG 
			}
		}	
	}
	CustomEditor "ASEMaterialInspector"
	
	Fallback Off
}
/*ASEBEGIN
Version=19105
Node;AmplifyShaderEditor.RangedFloatNode;3;-1056.945,100.8759;Inherit;False;Property;_Opacity;Opacity;1;0;Create;True;0;0;0;False;0;False;20;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;2;-840.0833,30.80811;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;6;-448.4049,7.877393;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;4;-633.4235,33.32737;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;5;-271.7963,-63.76568;Inherit;False;FLOAT4;4;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;0;-94.88762,-62.18701;Float;False;True;-1;2;ASEMaterialInspector;0;11;SwordBladeStream;0b6a9f8b4f707c74ca64c0be8e590de0;True;SubShader 0 Pass 0;0;0;SubShader 0 Pass 0;2;False;True;2;5;False;;10;False;;0;1;False;;0;False;;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;;False;True;True;True;True;False;0;False;;False;False;False;False;False;False;False;False;False;True;2;False;;True;3;False;;False;True;4;Queue=Transparent=Queue=0;IgnoreProjector=True;RenderType=Transparent=RenderType;PreviewType=Plane;False;False;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;0;;0;0;Standard;0;0;1;True;False;;False;0
Node;AmplifyShaderEditor.ConditionalIfNode;12;-535.011,320.9784;Inherit;True;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;10;-774.6278,298.8898;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;1;-1243.727,-116.1321;Inherit;True;Property;_Main;Main;0;0;Create;True;0;0;0;False;0;False;-1;844d4b5c63743ee40a6f25def2f361b0;844d4b5c63743ee40a6f25def2f361b0;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector4Node;20;-3136.54,-119.9351;Inherit;False;Constant;_SpeedMainTexUV;SpeedMainTexU/V;3;0;Create;True;0;0;0;False;0;False;0,0,1,0;0,0,0,0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector4Node;37;-1307.089,-474.0012;Inherit;False;Constant;_Vector1;Vector 1;4;0;Create;True;0;0;0;False;0;False;0,1,-2,1;0,0,0,0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;40;-703.1255,-273.5166;Inherit;False;Property;_Emission;Emission;4;0;Create;True;0;0;0;False;0;False;2;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;39;-496.8417,-302.1532;Inherit;False;3;3;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TFHCRemapNode;35;-1011.718,-469.9391;Inherit;False;5;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;1,1,1;False;3;FLOAT3;0,0,0;False;4;FLOAT3;1,1,1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ClampOpNode;38;-785.8315,-475.9218;Inherit;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;1,1,1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;43;-532.8903,-544.6;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.VertexColorNode;7;-763.4918,-163.8468;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;42;-315.9948,-309.7513;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.PannerNode;27;-2281.855,224.7494;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;21;-2765.84,-89.81749;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;22;-2929.461,236.4429;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RGBToHSVNode;44;-2315.158,-500.5757;Inherit;False;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.HSVToRGBNode;45;-2023.273,-475.6248;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleAddOpNode;46;-2120.124,-296.8443;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;34;-2739.254,-505.0888;Inherit;True;Property;_EmessionTexture;Emession Texture;3;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DesaturateOpNode;47;-1663.799,-464.7388;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;48;-1832.641,-307.0611;Inherit;False;Property;_Desaturate;Desaturate;6;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector3Node;11;-977.5511,504.9308;Inherit;False;Constant;_Vector0;Vector 0;3;0;Create;True;0;0;0;False;0;False;1,1,0;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.BreakToComponentsNode;30;-2089.873,221.8423;Inherit;False;FLOAT2;1;0;FLOAT2;0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.SimpleAddOpNode;32;-1910.819,106.3329;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;25;-2556.45,197.2038;Float;False;t;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;26;-2553.796,108.1496;Float;False;w;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;8;-1337.549,217.8662;Inherit;True;Property;_Dissovle;Dissovle;2;0;Create;True;0;0;0;False;0;False;-1;5995e197a2393c74092f92dbafff4375;1d0ac62dfa8b109459287d46fd29cddc;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;17;-997.3879,308.5128;Inherit;False;26;w;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;16;-979.4359,399.1895;Inherit;False;25;t;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;29;-2534.132,-93.82541;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;28;-2885.944,44.99026;Inherit;False;0;8;4;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;33;-2341.372,-25.57021;Inherit;False;2;-1;4;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;31;-1728.8,224.5363;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ColorNode;41;-1007.95,-491.7656;Inherit;False;Property;_end;end;5;0;Create;True;0;0;0;False;0;False;0.2222657,0.09803923,0.7490196,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
WireConnection;2;0;1;4
WireConnection;2;1;3;0
WireConnection;6;0;7;4
WireConnection;6;1;4;0
WireConnection;6;2;12;0
WireConnection;4;0;2;0
WireConnection;5;0;42;0
WireConnection;5;3;6;0
WireConnection;0;0;5;0
WireConnection;12;0;10;0
WireConnection;12;1;16;0
WireConnection;12;2;11;2
WireConnection;12;3;11;2
WireConnection;12;4;11;3
WireConnection;10;0;8;1
WireConnection;10;1;17;0
WireConnection;39;0;38;0
WireConnection;39;1;40;0
WireConnection;39;2;7;0
WireConnection;35;0;47;0
WireConnection;35;1;37;1
WireConnection;35;2;37;2
WireConnection;35;3;37;3
WireConnection;35;4;37;4
WireConnection;38;0;35;0
WireConnection;43;0;41;0
WireConnection;43;1;7;0
WireConnection;42;0;43;0
WireConnection;42;1;39;0
WireConnection;27;0;28;0
WireConnection;27;2;22;0
WireConnection;21;0;20;1
WireConnection;21;1;20;2
WireConnection;22;0;20;3
WireConnection;22;1;20;4
WireConnection;44;0;34;0
WireConnection;45;0;46;0
WireConnection;45;1;44;2
WireConnection;45;2;44;3
WireConnection;46;0;44;1
WireConnection;46;1;33;3
WireConnection;47;0;45;0
WireConnection;47;1;48;0
WireConnection;30;0;27;0
WireConnection;32;0;33;4
WireConnection;32;1;30;1
WireConnection;25;0;28;4
WireConnection;26;0;28;3
WireConnection;8;1;31;0
WireConnection;29;0;21;0
WireConnection;31;0;30;0
WireConnection;31;1;32;0
ASEEND*/
//CHKSM=7060DE11B85EC763B437D66239A4E5612338422A