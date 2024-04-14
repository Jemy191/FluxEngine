#version 330 core
in vec3 fPos;
in vec2 fUv;
in mat3 tbn;

uniform sampler2D albedo;
uniform sampler2D normal;
uniform vec3 lightDirection;

out vec4 FragColor;

void main()
{
      vec3 pixelNormal = tbn * normalize(texture2D(normal, fUv.st).xyz * 2.0 - 1.0);

      vec3 normalizedLightDirection = normalize(lightDirection);
      float lambert = max(0.0, dot(pixelNormal, normalizedLightDirection));

      FragColor = vec4(texture(albedo, fUv).rgb * lambert, 1.0 );
      //FragColor = vec4(lambert, lambert, lambert, 1.0 );
      //FragColor = vec4(pixelNormal, 1.0 );
}