Shader "Unlit/NewUnlitShader"
{
	Properties
	{
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100
		Pass
		{
			Blend One One
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			#include "UnityCG.cginc"
			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};
			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};
#define hlsl_atan(x,y) atan2(x, y)
#define mod(x,y) ((x)-(y)*floor((x)/(y)))
inline float4 textureLod(sampler2D tex, float2 uv, float lod) {
    return tex2D(tex, uv);
}
inline float2 tofloat2(float x) {
    return float2(x, x);
}
inline float2 tofloat2(float x, float y) {
    return float2(x, y);
}
inline float3 tofloat3(float x) {
    return float3(x, x, x);
}
inline float3 tofloat3(float x, float y, float z) {
    return float3(x, y, z);
}
inline float3 tofloat3(float2 xy, float z) {
    return float3(xy.x, xy.y, z);
}
inline float3 tofloat3(float x, float2 yz) {
    return float3(x, yz.x, yz.y);
}
inline float4 tofloat4(float x, float y, float z, float w) {
    return float4(x, y, z, w);
}
inline float4 tofloat4(float x) {
    return float4(x, x, x, x);
}
inline float4 tofloat4(float x, float3 yzw) {
    return float4(x, yzw.x, yzw.y, yzw.z);
}
inline float4 tofloat4(float2 xy, float2 zw) {
    return float4(xy.x, xy.y, zw.x, zw.y);
}
inline float4 tofloat4(float3 xyz, float w) {
    return float4(xyz.x, xyz.y, xyz.z, w);
}
inline float4 tofloat4(float2 xy, float z, float w) {
    return float4(xy.x, xy.y, z, w);
}
inline float2x2 tofloat2x2(float2 v1, float2 v2) {
    return float2x2(v1.x, v1.y, v2.x, v2.y);
}
// EngineSpecificDefinitions
float rand(float2 x) {
    return frac(cos(mod(dot(x, tofloat2(13.9898, 8.141)), 3.14)) * 43758.5453);
}
float2 rand2(float2 x) {
    return frac(cos(mod(tofloat2(dot(x, tofloat2(13.9898, 8.141)),
						      dot(x, tofloat2(3.4562, 17.398))), tofloat2(3.14))) * 43758.5453);
}
float3 rand3(float2 x) {
    return frac(cos(mod(tofloat3(dot(x, tofloat2(13.9898, 8.141)),
							  dot(x, tofloat2(3.4562, 17.398)),
                              dot(x, tofloat2(13.254, 5.867))), tofloat3(3.14))) * 43758.5453);
}
float param_rnd(float minimum, float maximum, float seed) {
	return minimum+(maximum-minimum)*rand(tofloat2(seed));
}
float3 rgb2hsv(float3 c) {
	float4 K = tofloat4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
	float4 p = c.g < c.b ? tofloat4(c.bg, K.wz) : tofloat4(c.gb, K.xy);
	float4 q = c.r < p.x ? tofloat4(p.xyw, c.r) : tofloat4(c.r, p.yzx);
	float d = q.x - min(q.w, q.y);
	float e = 1.0e-10;
	return tofloat3(abs(q.z + (q.w - q.y) / (6.0 * d + e)), d / (q.x + e), q.x);
}
float3 hsv2rgb(float3 c) {
	float4 K = tofloat4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
	float3 p = abs(frac(c.xxx + K.xyz) * 6.0 - K.www);
	return c.z * lerp(K.xxx, clamp(p - K.xxx, 0.0, 1.0), c.y);
}
float pingpong(float a, float b)
{
  return (b != 0.0) ? abs(frac((a - b) / (b * 2.0)) * b * 2.0 - b) : 0.0;
}
float2 transform2_clamp(float2 uv) {
	return clamp(uv, tofloat2(0.0), tofloat2(1.0));
}
float2 transform2(float2 uv, float2 translate, float rotate, float2 scale) {
 	float2 rv;
	uv -= translate;
	uv -= tofloat2(0.5);
	rv.x = cos(rotate)*uv.x + sin(rotate)*uv.y;
	rv.y = -sin(rotate)*uv.x + cos(rotate)*uv.y;
	rv /= scale;
	rv += tofloat2(0.5);
	return rv;	
}
float3 kal_rotate(float2 uv, float count, float offset, float seed) {
	float pi = 3.14159265359;
	offset *= pi/180.0;
	offset += pi*(1.0/count+0.5);
	uv -= tofloat2(0.5);
	float l = length(uv);
	float angle = hlsl_atan(uv.y, uv.x)+offset;
	angle += (1.0-sign(angle))*pi;
	float a = mod(angle, 2.0*pi/count)-offset;
	return tofloat3(tofloat2(0.5)+l*tofloat2(cos(a), sin(a)), rand(tofloat2(seed))+floor(0.5*angle*count/pi));
}
static const float seed_o320075 = 0.699641049;
static const float p_o320075_count = 8.000000000;
static const float p_o320075_offset = 180.000000000;
static const float p_o320088_d_in1_x = 0.000000000;
static const float p_o320088_d_in1_y = 0.000000000;
static const float p_o320088_d_in1_z = 0.000000000;
static const float p_o320088_d_in2_x = 0.000000000;
static const float p_o320088_d_in2_y = 0.000000000;
static const float p_o320088_d_in2_z = 0.000000000;
static const float p_o320072_d_in1_x = 0.000000000;
static const float p_o320072_d_in1_y = 0.000000000;
static const float p_o320072_d_in1_z = 0.000000000;
static const float p_o320072_d_in2_x = 0.000000000;
static const float p_o320072_d_in2_y = 0.000000000;
static const float p_o320072_d_in2_z = 0.000000000;
static const float p_o320078_default_in1 = 0.000000000;
static const float p_o320078_default_in2 = 0.830000000;
static const float p_o320071_default_in1 = 0.120000000;
static const float p_o320071_default_in2 = 0.290000000;
static const float p_o320068_default_in1 = 1.000000000;
static const float p_o320068_default_in2 = 0.000000000;
static const float p_o320067_default_in1 = 0.000000000;
static const float p_o320067_default_in2 = 0.620000000;
static const float p_o320064_default_in1 = 1.000000000;
static const float p_o320064_default_in2 = 1.000000000;
static const float p_o320070_default_in1 = 1.000000000;
static const float p_o320070_default_in2 = 0.000000000;
static const float p_o320066_default_in1 = 0.000000000;
static const float p_o320066_default_in2 = 1.000000000;
static const float p_o320065_color = 12.000000000;
static const float p_o320063_d_in1_x = 0.000000000;
static const float p_o320063_d_in1_y = 0.000000000;
static const float p_o320063_d_in1_z = 0.000000000;
static const float p_o320063_d_in2_x = 0.000000000;
static const float p_o320063_d_in2_y = 0.000000000;
static const float p_o320063_d_in2_z = 0.000000000;
static const float p_o320055_r = 0.500000000;
static const float p_o320055_g = 0.500000000;
static const float p_o320055_b = 0.500000000;
static const float p_o320062_d_in1_x = 0.000000000;
static const float p_o320062_d_in1_y = 0.000000000;
static const float p_o320062_d_in1_z = 0.000000000;
static const float p_o320062_d_in2_x = 0.000000000;
static const float p_o320062_d_in2_y = 0.000000000;
static const float p_o320062_d_in2_z = 0.000000000;
static const float p_o320053_r = 0.500000000;
static const float p_o320053_g = 0.500000000;
static const float p_o320053_b = 0.500000000;
static const float p_o320061_d_in1_x = 0.000000000;
static const float p_o320061_d_in1_y = 0.000000000;
static const float p_o320061_d_in1_z = 0.000000000;
static const float p_o320061_d_in2_x = 0.000000000;
static const float p_o320061_d_in2_y = 0.000000000;
static const float p_o320061_d_in2_z = 0.000000000;
static const float p_o320060_color = 6.283180000;
static const float p_o320059_d_in1_x = 0.000000000;
static const float p_o320059_d_in1_y = 0.000000000;
static const float p_o320059_d_in1_z = 0.000000000;
static const float p_o320059_d_in2_x = 0.000000000;
static const float p_o320059_d_in2_y = 0.000000000;
static const float p_o320059_d_in2_z = 0.000000000;
static const float p_o320056_r = 0.263000000;
static const float p_o320056_g = 0.416000000;
static const float p_o320056_b = 0.557000000;
static const float p_o320057_d_in1_x = 0.000000000;
static const float p_o320057_d_in1_y = 0.000000000;
static const float p_o320057_d_in1_z = 0.000000000;
static const float p_o320057_d_in2_x = 0.000000000;
static const float p_o320057_d_in2_y = 0.000000000;
static const float p_o320057_d_in2_z = 0.000000000;
static const float p_o320054_r = 1.000000000;
static const float p_o320054_g = 1.000000000;
static const float p_o320054_b = 1.000000000;
static const float p_o320058_default_in2 = 0.000000000;
static const float p_o320077_default_in1 = 0.000000000;
static const float p_o320077_default_in2 = 0.000000000;
static const float p_o320086_d_in1_x = 0.000000000;
static const float p_o320086_d_in1_y = 0.000000000;
static const float p_o320086_d_in1_z = 0.000000000;
static const float p_o320086_d_in2_x = 0.000000000;
static const float p_o320086_d_in2_y = 0.000000000;
static const float p_o320086_d_in2_z = 0.000000000;
static const float p_o320089_translate_x = 0.000000000;
static const float p_o320089_translate_y = 0.000000000;
static const float p_o320089_rotate = 45.000000000;
static const float p_o320089_scale_x = 1.000000000;
static const float p_o320089_scale_y = 1.000000000;
static const float p_o320087_default_in1 = 0.000000000;
static const float p_o320087_default_in2 = 0.710000000;
static const float p_o320085_default_in1 = 0.120000000;
static const float p_o320085_default_in2 = 0.000000000;
static const float p_o320084_default_in1 = 1.000000000;
static const float p_o320084_default_in2 = 0.000000000;
static const float p_o320083_default_in1 = 0.000000000;
static const float p_o320083_default_in2 = 0.620000000;
static const float p_o320082_default_in1 = 1.000000000;
static const float p_o320082_default_in2 = 1.000000000;
static const float p_o320080_default_in1 = 1.000000000;
static const float p_o320080_default_in2 = 0.000000000;
static const float p_o320081_default_in1 = 0.000000000;
static const float p_o320081_default_in2 = 1.000000000;
static const float p_o320093_translate_x = 0.250000000;
static const float p_o320093_translate_y = 0.250000000;
static const float p_o320093_rotate = 0.000000000;
static const float p_o320093_scale_x = 0.500000000;
static const float p_o320093_scale_y = 0.500000000;
float4 o320075_input_i(float2 uv, float _seed_variation_) {
float o320074_0_1_f = (length( frac((uv)) - tofloat2(0.5, 0.5)));
float o320065_0_1_f = p_o320065_color;
float o320066_0_clamp_false = o320074_0_1_f*o320065_0_1_f;
float o320066_0_clamp_true = clamp(o320066_0_clamp_false, 0.0, 1.0);
float o320066_0_1_f = o320066_0_clamp_false;
float o320069_0_1_f = (_Time.y);
float o320070_0_clamp_false = o320066_0_1_f+o320069_0_1_f;
float o320070_0_clamp_true = clamp(o320070_0_clamp_false, 0.0, 1.0);
float o320070_0_1_f = o320070_0_clamp_false;
float o320064_0_clamp_false = sin(o320070_0_1_f*p_o320064_default_in2);
float o320064_0_clamp_true = clamp(o320064_0_clamp_false, 0.0, 1.0);
float o320064_0_2_f = o320064_0_clamp_false;
float o320067_0_clamp_false = o320064_0_2_f/p_o320067_default_in2;
float o320067_0_clamp_true = clamp(o320067_0_clamp_false, 0.0, 1.0);
float o320067_0_2_f = o320067_0_clamp_false;
float o320068_0_clamp_false = abs(o320067_0_2_f);
float o320068_0_clamp_true = clamp(o320068_0_clamp_false, 0.0, 1.0);
float o320068_0_1_f = o320068_0_clamp_false;
float o320071_0_clamp_false = p_o320071_default_in1/o320068_0_1_f;
float o320071_0_clamp_true = clamp(o320071_0_clamp_false, 0.0, 1.0);
float o320071_0_2_f = o320071_0_clamp_false;
float o320078_0_clamp_false = pow(o320071_0_2_f,p_o320078_default_in2);
float o320078_0_clamp_true = clamp(o320078_0_clamp_false, 0.0, 1.0);
float o320078_0_2_f = o320078_0_clamp_false;
float3 o320055_0_1_rgb = tofloat3(p_o320055_r,p_o320055_g,p_o320055_b);
float3 o320053_0_1_rgb = tofloat3(p_o320053_r,p_o320053_g,p_o320053_b);
float o320060_0_1_f = p_o320060_color;
float3 o320056_0_1_rgb = tofloat3(p_o320056_r,p_o320056_g,p_o320056_b);
float3 o320054_0_1_rgb = tofloat3(p_o320054_r,p_o320054_g,p_o320054_b);
float o320076_0_1_f = (exp(-length(tofloat2(0.5, 0.5) - (uv))));
float o320073_0_1_f = (length(tofloat2(0.5, 0.5) - (uv)));
float o320077_0_clamp_false = pow(o320076_0_1_f,o320073_0_1_f);
float o320077_0_clamp_true = clamp(o320077_0_clamp_false, 0.0, 1.0);
float o320077_0_1_f = o320077_0_clamp_false;
float o320058_0_clamp_false = (_Time.y * 0.4)+o320077_0_1_f;
float o320058_0_clamp_true = clamp(o320058_0_clamp_false, 0.0, 1.0);
float o320058_0_2_f = o320058_0_clamp_false;
float3 o320057_0_clamp_false = o320054_0_1_rgb*tofloat3(o320058_0_2_f);
float3 o320057_0_clamp_true = clamp(o320057_0_clamp_false, tofloat3(0.0), tofloat3(1.0));
float3 o320057_0_1_rgb = o320057_0_clamp_false;
float3 o320059_0_clamp_false = o320056_0_1_rgb+o320057_0_1_rgb;
float3 o320059_0_clamp_true = clamp(o320059_0_clamp_false, tofloat3(0.0), tofloat3(1.0));
float3 o320059_0_1_rgb = o320059_0_clamp_false;
float3 o320061_0_clamp_false = cos(tofloat3(o320060_0_1_f)*o320059_0_1_rgb);
float3 o320061_0_clamp_true = clamp(o320061_0_clamp_false, tofloat3(0.0), tofloat3(1.0));
float3 o320061_0_1_rgb = o320061_0_clamp_false;
float3 o320062_0_clamp_false = o320053_0_1_rgb*o320061_0_1_rgb;
float3 o320062_0_clamp_true = clamp(o320062_0_clamp_false, tofloat3(0.0), tofloat3(1.0));
float3 o320062_0_1_rgb = o320062_0_clamp_false;
float3 o320063_0_clamp_false = o320055_0_1_rgb+o320062_0_1_rgb;
float3 o320063_0_clamp_true = clamp(o320063_0_clamp_false, tofloat3(0.0), tofloat3(1.0));
float3 o320063_0_1_rgb = o320063_0_clamp_true;
float3 o320072_0_clamp_false = tofloat3(o320078_0_2_f)*o320063_0_1_rgb;
float3 o320072_0_clamp_true = clamp(o320072_0_clamp_false, tofloat3(0.0), tofloat3(1.0));
float3 o320072_0_1_rgb = o320072_0_clamp_false;
float o320074_0_3_f = (length( frac((frac(transform2((transform2((uv), tofloat2(p_o320089_translate_x*(2.0*1.0-1.0), p_o320089_translate_y*(2.0*1.0-1.0)), p_o320089_rotate*0.01745329251*(2.0*1.0-1.0), tofloat2(p_o320089_scale_x*(2.0*1.0-1.0), p_o320089_scale_y*(2.0*1.0-1.0)))), tofloat2(p_o320093_translate_x*(2.0*1.0-1.0), p_o320093_translate_y*(2.0*1.0-1.0)), p_o320093_rotate*0.01745329251*(2.0*1.0-1.0), tofloat2(p_o320093_scale_x*(2.0*1.0-1.0), p_o320093_scale_y*(2.0*1.0-1.0)))))) - tofloat2(0.5, 0.5)));
float4 o320093_0_1_rgba = tofloat4(tofloat3(o320074_0_3_f), 1.0);
float o320065_0_3_f = p_o320065_color;
float o320081_0_clamp_false = (dot((o320093_0_1_rgba).rgb, tofloat3(1.0))/3.0)*o320065_0_3_f;
float o320081_0_clamp_true = clamp(o320081_0_clamp_false, 0.0, 1.0);
float o320081_0_1_f = o320081_0_clamp_false;
float o320079_0_1_f = (_Time.y);
float o320080_0_clamp_false = o320081_0_1_f+o320079_0_1_f;
float o320080_0_clamp_true = clamp(o320080_0_clamp_false, 0.0, 1.0);
float o320080_0_1_f = o320080_0_clamp_false;
float o320082_0_clamp_false = sin(o320080_0_1_f*p_o320082_default_in2);
float o320082_0_clamp_true = clamp(o320082_0_clamp_false, 0.0, 1.0);
float o320082_0_2_f = o320082_0_clamp_false;
float o320083_0_clamp_false = o320082_0_2_f/p_o320083_default_in2;
float o320083_0_clamp_true = clamp(o320083_0_clamp_false, 0.0, 1.0);
float o320083_0_2_f = o320083_0_clamp_false;
float o320084_0_clamp_false = abs(o320083_0_2_f);
float o320084_0_clamp_true = clamp(o320084_0_clamp_false, 0.0, 1.0);
float o320084_0_1_f = o320084_0_clamp_false;
float o320085_0_clamp_false = p_o320085_default_in1/o320084_0_1_f;
float o320085_0_clamp_true = clamp(o320085_0_clamp_false, 0.0, 1.0);
float o320085_0_2_f = o320085_0_clamp_false;
float o320087_0_clamp_false = pow(o320085_0_2_f,p_o320087_default_in2);
float o320087_0_clamp_true = clamp(o320087_0_clamp_false, 0.0, 1.0);
float o320087_0_2_f = o320087_0_clamp_false;
float4 o320089_0_1_rgba = tofloat4(tofloat3(o320087_0_2_f), 1.0);
float3 o320086_0_clamp_false = ((o320089_0_1_rgba).rgb)*o320063_0_1_rgb;
float3 o320086_0_clamp_true = clamp(o320086_0_clamp_false, tofloat3(0.0), tofloat3(1.0));
float3 o320086_0_1_rgb = o320086_0_clamp_false;
float3 o320088_0_clamp_false = o320072_0_1_rgb+o320086_0_1_rgb;
float3 o320088_0_clamp_true = clamp(o320088_0_clamp_false, tofloat3(0.0), tofloat3(1.0));
float3 o320088_0_1_rgb = o320088_0_clamp_false;
return tofloat4(o320088_0_1_rgb, 1.0);
}
		
			v2f vert (appdata v) {
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			fixed4 frag (v2f i) : SV_Target {
				float _seed_variation_ = 0.0;
				float2 uv = i.uv;
float3 o320075_0_kal = kal_rotate((uv), p_o320075_count, p_o320075_offset, (seed_o320075+frac(_seed_variation_)));float4 o320075_0_1_rgba = o320075_input_i(o320075_0_kal.xy, true ? o320075_0_kal.z : 0.0);

				// sample the generated texture
				fixed4 col = o320075_0_1_rgba;

				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}
	}
}



