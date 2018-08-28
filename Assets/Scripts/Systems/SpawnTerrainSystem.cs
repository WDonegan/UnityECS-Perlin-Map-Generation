using UnityEngine;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Jobs;
using Unity.Rendering;

using PerlinPlayground;
using PerlinPlayground.Components;
using PerlinPlayground.Managers;
using PerlinPlayground.Systems;
using PerlinPlayground.UpdateGroups;
using PerlinPlayground.Objects;

namespace PerlinPlayground.Systems
{
    [UpdateInGroup(typeof(SpawnGroup))]
    [UpdateBefore(typeof(ProcessingGroup))]
    class SpawnTerrainSystem : ComponentSystem
    {
        Entity m_prefabEntity;
        NativeArray<Entity> m_entityArray;
        
        protected override void OnUpdate()
        {
            m_entityArray = new NativeArray<Entity>(128 * 128, Allocator.Temp);

            CreatePrefabEntity();
            
            Main.entityManager.Instantiate(m_prefabEntity, m_entityArray);

            int i = 0;
            for (int x = 0; x < 128; x++)
            {
                for (int y = 0; y < 128; y++)
                {
                    Main.entityManager.SetComponentData(m_entityArray[i], new MapIndexComponent
                    {
                        Value = new int2 { x = x, y = y }
                    });

                    Main.entityManager.SetComponentData(m_entityArray[i], new Components.Transform.Pos
                    {
                        Value = new float3
                        {
                            x = x,
                            y = 0,
                            z = y
                        }
                    });

                    i++;
                }
            }

            m_entityArray.Dispose();
            Main.entityManager.DestroyEntity(m_prefabEntity);

            Unity.Entities.World.Active.GetExistingManager<SpawnTerrainSystem>().Enabled = false;
        }

        void CreatePrefabEntity()
        {
            m_prefabEntity = Main.entityManager.CreateEntity(Archetype.Terrain);

            Main.entityManager.SetComponentData(m_prefabEntity, new MapIndexComponent
            {
                Value = new int2 { x = 0, y = 0 }
            });

            Main.entityManager.SetComponentData(m_prefabEntity, new Components.Transform.Pos
            {
                Value = new float3 { x = 0, y = 0, z = 0 }
            });

            Main.entityManager.SetComponentData(m_prefabEntity, new Components.Transform.Rot
            {
                Value = quaternion.identity
            });

            Main.entityManager.SetComponentData(m_prefabEntity, new Components.Transform.Scl
            {
                Value = new float3 { x = 1, y = 1, z = 1 }
            });

            Main.entityManager.SetComponentData(m_prefabEntity, new Components.Transform.ModelMatrix
            {
                Value = float4x4.identity
            });

            Main.entityManager.SetSharedComponentData(m_prefabEntity, new MeshInstanceRenderer
            {
                mesh = (Resources.Load("Terrain_Flat_Mesh") as MeshData).Value,
                material = (Resources.Load("Terrain_Flat_Mat") as MaterialData).Value
            });
        }
    }
}
