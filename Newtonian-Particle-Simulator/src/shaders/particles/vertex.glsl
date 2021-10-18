#version 430 core
#define EPSILON 0.0001

struct Particle
{
    vec3 Position;
    vec3 Velocity;
};

layout(std430, binding = 0) restrict buffer ParticlesSSBO
{
    Particle[] particles;
} ssbo;

layout(location = 0) uniform float dT;
layout(location = 1) uniform vec3 pointOfMass;
layout(location = 2) uniform float isActive;
layout(location = 3) uniform float isRunning;
layout(location = 4) uniform mat4 projViewMatrix;

out vec4 Color;
void main()
{
    Particle particle = ssbo.particles[gl_VertexID];

    vec3 toMass = pointOfMass - particle.Position;
    float dist = max(length(toMass), EPSILON);
    
    particle.Velocity *= mix(1.0, 0.998, isRunning); 
    particle.Velocity += isRunning * isActive / dist * (toMass / dist);
    particle.Position = particle.Position + dT * particle.Velocity * isRunning;
    ssbo.particles[gl_VertexID] = particle;


    float red = mix(0.0, 1.0, 0.0045 * dot(particle.Velocity, particle.Velocity));
    float green = clamp(0.08 * max(particle.Velocity.x, max(particle.Velocity.y, particle.Velocity.z)), 0.2, 0.5);
    float blue = 0.7 - red;

    Color = vec4(red, green, blue, 0.25);
    gl_Position = projViewMatrix * vec4(particle.Position, 1.0);
}