using UnityEngine;
using Unity.Collections;
using Unity.Mathematics;
using Unity.Rendering;
using PerlinPlayground.Components;
using PerlinPlayground.Components.Transform;
using Unity.Entities;

namespace PerlinPlayground
{
    public partial class Main
    {
        private static void CreateTextureViewer()
        {
            var Quad = entityManager.CreateEntity(Managers.Archetype.Base);

            entityManager.SetComponentData(Quad, new Pos
            {
                Value = new float3
                {
                     x = 0, y = 0, z = 0
                }
            });

            entityManager.SetComponentData(Quad, new Rot
            {
                Value = quaternion.eulerXYZ(new float3 (0, 0, 0))
            });

            entityManager.SetComponentData(Quad, new Scl
            {
                Value = new float3
                {
                    x = 10, y = 10, z = 1
                }
            });

            entityManager.SetComponentData(Quad, new ModelMatrix { Value = float4x4.identity });
            
            entityManager.SetSharedComponentData(Quad, new MeshInstanceRenderer
            {
                mesh = (Resources.Load("QuadMesh") as Objects.MeshData).Value,
                material = (Resources.Load("TextureViewerMaterial") as Objects.MaterialData).Value,
                castShadows = UnityEngine.Rendering.ShadowCastingMode.Off,
                receiveShadows = false
            });

        }
    }
}