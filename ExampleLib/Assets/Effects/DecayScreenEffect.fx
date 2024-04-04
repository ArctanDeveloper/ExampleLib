sampler uImage0 : register(s0);
sampler uImage1 : register(s1);
sampler uImage2 : register(s2);
sampler uImage3 : register(s3);
float3 uColor;
float3 uSecondaryColor;
float2 uScreenResolution;
float2 uScreenPosition;
float2 uTargetPosition;
float2 uDirection;
float uOpacity;
float uTime;
float uIntensity;
float uProgress;
float2 uImageSize1;
float2 uImageSize2;
float2 uImageSize3;
float2 uImageOffset;
float uSaturation;
float4 uSourceRect;
float2 uZoom;

// This is a shader. You are on your own with shaders. Compile shaders in an XNB project.

const float maxProgress = 400.0f;
float4 DecayScreenEffectOne(float2 coords : TEXCOORD0) : COLOR0
{
    //Red Tint
    float4 color = tex2D(uImage0, coords);

    color.r = tanh(color.r) / sin(uTime + coords.x);
    color.g = tanh(color.g) / sin(uTime + coords.x);
    color.b = tanh(color.b) / sin(uTime + coords.x);

    

	return color;
}

float4 DecayScreenEffectTwo(float2 coords : TEXCOORD0) : COLOR0 {
    //Red Tint
    float4 color = tex2D(uImage0, coords);

    float4 tempcolor = color;

    tempcolor.r += sin(color.g);

    color = tempcolor;

    tempcolor.rgb *= sin((uTime * 6) + coords.x);

    color.r = lerp(color.r, tempcolor.r, 0.4);

    return color;
}

float4 DecayScreenEffectThree(float2 coords : TEXCOORD0) : COLOR0 {
    //Red Tint
    float4 color = tex2D(uImage0, coords);

    float2 targetCoords = (uTargetPosition - uScreenPosition) / uScreenResolution;

    float2 vec = targetCoords - coords;
    float2 offset = -vec * 0.25;
    vec.x *= uScreenResolution.x;
    vec.y *= uScreenResolution.y;
    float distance = length(vec) / 1.33f;


    float distFromShockwave = pow(((distance - uProgress) / maxProgress) + 1, 2);

    color.g = distFromShockwave * sin(uTime + color.g);

    return color;
}

float4 DecayScreenEffectFour(float2 coords : TEXCOORD0) : COLOR0 {
    //Red Tint
    float4 color = tex2D(uImage0, coords);

    color = lerp(
        lerp(
            tex2D(uImage0, coords),
            tex2D(uImage0, 1 - coords),
            0.5),
        lerp(
            tex2D(uImage0, (1 - coords.x, coords.y)),
            tex2D(uImage0, (coords.x, 1 - coords.y)),
            0.5),
        0.5);

    return color;
}

float4 DecayScreenEffectFive(float2 coords : TEXCOORD0) : COLOR0 {
    float2 newCoords = coords;

    if (coords.x >= 0.5) 
    {
        newCoords.y += (uScreenResolution * frac(uTime * 0.2)) / uScreenResolution;

        if (newCoords.y > 1)
            newCoords.y--;
    }
    else 
    {
        newCoords.y -= (uScreenResolution * frac(uTime * 0.2)) / uScreenResolution;

        if (newCoords.y < 0)
            newCoords.y++;
    }

    float4 color = tex2D(uImage0, newCoords);

    return color;
}

float4 DecayScreenEffectSix(float2 coords : TEXCOORD0) : COLOR0 {
    float2 newCoords = coords;

    if (coords.x >= 0.5)
    {
        newCoords.y += 5 / uScreenResolution;

        if (newCoords.y > 1)
            newCoords.y--;
    }
    else
    {
        newCoords.y -= 5 / uScreenResolution;

        if (newCoords.y < 0)
            newCoords.y++;
    }

    float4 color = tex2D(uImage0, newCoords);

    return color;
}

technique Technique1
{
    pass DecayScreenEffect
    {
        PixelShader = compile ps_2_0 DecayScreenEffectOne();
        PixelShader = compile ps_2_0 DecayScreenEffectTwo();
        PixelShader = compile ps_2_0 DecayScreenEffectThree();
        PixelShader = compile ps_2_0 DecayScreenEffectFour();
        PixelShader = compile ps_2_0 DecayScreenEffectFive();
        PixelShader = compile ps_2_0 DecayScreenEffectSix();
    }
}