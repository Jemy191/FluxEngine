#version 330 core
layout (location = 0) in vec3 vPos;
layout (location = 1) in vec3 vNormal;
layout (location = 2) in vec3 vTangent;
layout (location = 3) in vec3 vBiTangent;
layout (location = 4) in vec2 vUv;
layout (location = 5) in vec3 vColor;

out vec3 fPos;
out vec3 fCol;
out vec2 fUv;
out mat3 tbn;

uniform mat4 uModel;
uniform mat4 uView;
uniform mat4 uProjection;

void main()
{
    gl_Position = uProjection * uView * uModel * vec4(vPos, 1.0);
    fPos = vec3(uModel * vec4(vPos, 1.0));
    
    vec3 n = normalize( ( uModel * vec4( vNormal, 0.0 ) ).xyz );
    vec3 t = normalize( ( uModel * vec4( vTangent, 0.0 ) ).xyz );
    vec3 b = normalize( ( uModel * vec4( vBiTangent, 0.0 ) ).xyz );
    tbn = mat3( t, b, n );

    fCol = vColor;
    fUv = vUv;
}
