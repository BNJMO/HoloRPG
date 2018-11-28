Shader "Custom/ProductGizmoOutline" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
	}
	SubShader {
		Tags { "RenderType"="Transparent" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Lambert fullforwardshadows alpha

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
		};

		fixed4 _Color;

		void surf (Input IN, inout SurfaceOutput o) {
			fixed4 c = _Color;
			o.Albedo = c.rgb;
			float t = 0.01;
			float x = IN.uv_MainTex.x;
			float y = IN.uv_MainTex.y;

			o.Alpha = c.a *  (x <= t || x >= 1 - t || y <= t || y >= 1 - t) ? 1 : 0;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
