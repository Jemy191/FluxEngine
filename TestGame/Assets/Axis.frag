#version 330 core
in vec3 fPos;
in vec2 fUv;
in mat3 tbn;
in vec3 fCol;

uniform vec3 lightDirection;

out vec4 FragColor;

void main()
{
      vec3 pixelNormal = tbn[2].rgb;

      vec3 normalizedLightDirection = normalize(lightDirection);
      float lambert = max(0.3, dot(pixelNormal, normalizedLightDirection));

      FragColor = vec4(fCol * lambert, 1.0 );
      //FragColor = vec4(lambert, lambert, lambert, 1.0 );
      //FragColor = vec4(pixelNormal, 1.0 );
}