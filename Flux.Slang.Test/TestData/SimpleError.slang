[shader("vertex")]
float4 vertexMain(float3 position): SV_Position
{
    return float4(position
}

[shader("fragment")]
float4 fragmentMain() : SV_Target
{
    return float4(1.0, 0.0, 1.0, 1.0);
}