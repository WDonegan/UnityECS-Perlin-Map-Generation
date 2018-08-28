using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

namespace PerlinPlayground.Components
{
    public struct MapDataComponent : IComponentData
    {
        public float Value;
    }

    public struct MapIndexComponent : IComponentData
    {
        public int2 Value;
    }

    public struct MapLayerComponent : IComponentData
    {
        public int Value;
    }

    public struct MapHeightComponent : IComponentData
    {
        public float Value;
    }
}
