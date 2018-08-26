using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

namespace PerlinPlayground.Components.Transform 
{
    [System.Serializable]
    public struct ModelMatrix : IComponentData
    {
        public float4x4 Value;
    }

    [System.Serializable]
    public struct Pos : IComponentData
    {
        public float3 Value;
    }

    [System.Serializable]
    public struct Rot : IComponentData
    {
        public quaternion Value;
    }

    [System.Serializable]
    public struct Scl : IComponentData
    {
        public float3 Value;
    }
}