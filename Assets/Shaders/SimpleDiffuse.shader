/******************************************************************************
Copyright (C) 2016 Intel Corporation All Rights Reserved.
The source code, information and material ("Material") contained herein is
owned by Intel Corporation or its suppliers or licensors, and title to such
Material remains with Intel Corporation or its suppliers or licensors. The
Material contains proprietary information of Intel or its suppliers and
licensors. The Material is protected by worldwide copyright laws and treaty
provisions. No part of the Material may be used, copied, reproduced, modified,
published, uploaded, posted, transmitted, distributed or disclosed in any way
without Intel's prior express written permission. No license under any patent,
copyright or other intellectual property rights in the Material is granted to
or conferred upon you, either expressly, by implication, inducement, estoppel
or otherwise. Any license under such intellectual property rights must be
express and approved by Intel in writing.
Unless otherwise agreed by Intel in writing, you may not remove or alter this
notice or any other notice embedded in Materials by Intel or Intel's suppliers
or licensors in any way.
******************************************************************************/

Shader "Custom/SimpleDiffuse" {
	Properties{
		_DiffuseColor("Diffuse color", Color) = (.34, .85, .92, 1)
	}
	SubShader{
		Pass{
			CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#include "UnityCG.cginc"

			fixed4 _DiffuseColor;

			struct v2f {
				float4 pos : SV_POSITION;
			};

			// Must output SV_POSITION
			v2f vert(appdata_base v)
			{
				v2f o;
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target{ return _DiffuseColor; }
				ENDCG
		}
	}
}


