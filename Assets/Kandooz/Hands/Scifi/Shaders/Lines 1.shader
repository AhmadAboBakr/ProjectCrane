// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "EyeShader"
{
	Properties
	{
		_Color("Color ", Color) = (1,1,1,0)
		_Value("value",Range(0,1))=0
	}
	
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100
		CGINCLUDE
		#pragma target 3.0
		ENDCG
	
		
		

		Pass
		{
			Name "Unlit"
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_instancing
			#include "UnityCG.cginc"
			#include "UnityShaderVariables.cginc"


			struct appdata
			{
				float4 vertex : POSITION;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				
			};
			
			struct v2f
			{
				float4 vertex : SV_POSITION;
				float value: VALUE;
				UNITY_VERTEX_OUTPUT_STEREO
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			uniform float4 _Color;
			uniform float _Value;
			
			v2f vert ( appdata v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.value=abs(v.vertex.y);
				return o;
			}
			
			fixed4 frag (v2f i ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID(i);
				if(i.value<=_Value)discard;
				return _Color;
			}
			ENDCG
		}
	}
}