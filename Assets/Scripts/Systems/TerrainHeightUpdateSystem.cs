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
    [UpdateInGroup(typeof(TerrainGroup))]
    [UpdateBefore(typeof(RenderingGroup))]
    [UpdateAfter(typeof(ProcessingGroup))]
    class TerrainHeightUpdateSystem : ComponentSystem
    {
        ComponentGroup m_mapDataGroup;
        ComponentGroup m_terrainGroup;
        EntityArray m_mapDataEntities;
        EntityArray m_terrainEntities;

        ComponentDataArray<MapIndexComponent> m_DataGroupIndex;
        ComponentDataArray<MapDataComponent> m_DataGroupData;

        ComponentDataArray<PerlinPlayground.Components.Transform.Pos> m_terrainPos;
        ComponentDataArray<PerlinPlayground.Components.MapIndexComponent> m_terrainIndex;

        protected override void OnCreateManager(int capacity)
        {
            m_mapDataGroup = GetComponentGroup(typeof(MapIndexComponent), typeof(MapDataComponent));
            m_terrainGroup = GetComponentGroup(typeof(Components.Transform.Pos), typeof(MapIndexComponent));
        }

        protected override void OnUpdate()
        {
            m_DataGroupIndex = m_mapDataGroup.GetComponentDataArray<MapIndexComponent>();
            m_DataGroupData = m_mapDataGroup.GetComponentDataArray<MapDataComponent>();

            m_terrainPos = m_terrainGroup.GetComponentDataArray<Components.Transform.Pos>();
            m_terrainIndex = m_terrainGroup.GetComponentDataArray<MapIndexComponent>();

            int2 index2D = new int2 { x = 0, y = 0 };
            float3 pos3D = new float3 { x = 0, y = 0, z = 0 };

            for (int i = 0; i < m_DataGroupIndex.Length; i++)
            {
                m_terrainIndex[i] = m_DataGroupIndex[i];
                index2D = m_DataGroupIndex[i].Value;
                pos3D = new float3 { x = index2D.x, y = m_DataGroupData[i].Value * 10, z = index2D.y };

                m_terrainPos[i] = new Components.Transform.Pos
                {
                    Value = pos3D
                };
            }
        }
    }
}