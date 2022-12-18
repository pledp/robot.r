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

float4 MainPS(VertexShaderOutput input) : COLOR
{
    input.TextureCoordinates -= 0.5;
    float r = input.TextureCoordinates.x * input.TextureCoordinates.x + input.TextureCoordinates.y * input.TextureCoordinates.y;
    input.TextureCoordinates *= 8.2 + r;
    input.TextureCoordinates *= 0.12;
    input.TextureCoordinates += 0.5;

    float4 color = tex2D(SpriteTextureSampler, input.TextureCoordinates);
    float4 col = tex2D(SpriteTextureSampler, input.TextureCoordinates);
    col.r = tex2D(SpriteTextureSampler, float2(input.TextureCoordinates.x + 0.0008f, input.TextureCoordinates.y + 0.0008f)).x;
    col.g = tex2D(SpriteTextureSampler, float2(input.TextureCoordinates.x, input.TextureCoordinates.y + 0.0008f*2)).y;
    col.b = tex2D(SpriteTextureSampler, float2(input.TextureCoordinates.x + 0.0008f*2, input.TextureCoordinates.y)).z;

    float vignette = (16 * input.TextureCoordinates.x * input.TextureCoordinates.y * (1 - input.TextureCoordinates.x) * (1 - input.TextureCoordinates.y));
    vignette = pow(vignette, 0.1f);
    col *= vignette;

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