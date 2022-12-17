#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0 // This was ps_4_0_level_9_1, do that for DirectX

Texture2D SpriteTexture;

sampler2D SpriteTextureSampler = sampler_state
{
    Texture = <SpriteTexture>;
};

struct VertexShaderOutput
{
    float4 Position : SV_POSITION;
    float4 Color : COLOR0;
    float2 TextureCoordinates : TEXCOORD0;
};

// I don't understand the stuff before here
float4 MainPS(VertexShaderOutput input) : COLOR
{
    // Applying our cool effect. What it does is: when drawing pixel X:Y, instead of taking the
    // pixel from texture position X:Y, take it from (X+Y*0.2:Y) to create a slanted effected
      input.TextureCoordinates -= 0.5;				// offcenter screen
      float r = input.TextureCoordinates.x * input.TextureCoordinates.x + input.TextureCoordinates.y * input.TextureCoordinates.y; 	// get ratio
      input.TextureCoordinates *= 4.2 + r;				// apply ratio
      input.TextureCoordinates *= 0.22;				// zoom
      input.TextureCoordinates += 0.5;

    float4 color = tex2D(SpriteTextureSampler, input.TextureCoordinates);
    float4 col = tex2D(SpriteTextureSampler, input.TextureCoordinates);
    col.r = tex2D(SpriteTextureSampler, float2(input.TextureCoordinates.x + 0.0005f, input.TextureCoordinates.y + 0.0005f)).x;
    col.g = tex2D(SpriteTextureSampler, float2(input.TextureCoordinates.x, input.TextureCoordinates.y + 0.0005f*2)).y;
    col.b = tex2D(SpriteTextureSampler, float2(input.TextureCoordinates.x + 0.0005f*2, input.TextureCoordinates.y)).z;

    return col;
}
// Here comes the rest of the things I don't understand
technique SpriteDrawing
{
    pass P0
    {
        PixelShader = compile PS_SHADERMODEL MainPS();
    }
};