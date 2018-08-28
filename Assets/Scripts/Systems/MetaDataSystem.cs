using UnityEngine;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Jobs;

using PerlinPlayground.Components;
using PerlinPlayground.Managers;
using PerlinPlayground.Systems;

namespace PerlinPlayground.Systems
{
    class MetaDataSystem : ComponentSystem
    {
        float[,] m_rawFloatData;
        int[,] m_entityIndexData;
        ComponentGroup m_mapDataGroup;
        EntityArray m_mapDataEntities;
        ComponentDataArray<MapIndexComponent> m_DataGroupIndex;
        ComponentDataArray<MapDataComponent> m_DataGroupData;

        protected override void OnCreateManager(int capacity)
        {
            //m_mapDataGroup = GetComponentGroup(typeof(MapIndexComponent), typeof(MapDataComponent));
            //m_rawFloatData = new float[128, 128];
            //m_entityIndexData = new int[128, 128];

            Unity.Entities.World.Active.GetExistingManager<MetaDataSystem>().Enabled = false;
        }

        protected override void OnUpdate()
        {
            m_mapDataEntities = m_mapDataGroup.GetEntityArray();
            m_DataGroupIndex = m_mapDataGroup.GetComponentDataArray<MapIndexComponent>();
            m_DataGroupData = m_mapDataGroup.GetComponentDataArray<MapDataComponent>();

            for (int i = 0; i < 128 * 128; i++)
            {
                m_rawFloatData[m_DataGroupIndex[i].Value.x, m_DataGroupIndex[i].Value.y] = m_DataGroupData[i].Value;
                m_entityIndexData[m_DataGroupIndex[i].Value.x, m_DataGroupIndex[i].Value.y] = i;
            }

            for (int w = 0; w < 128; w++)
            {
                for (int h = 0; h < 128; h++)
                {
                    //------ INNER LOOP ------
                    for (int wx = -1; wx <= 1; wx++)
                    {
                        for (int hx = -1; hx <= 1; hx++)
                        {
                            
                        }
                    }
                    //------ INNER LOOP ------



                }
            }

            m_mapDataGroup.Dispose();

            Unity.Entities.World.Active.GetExistingManager<MetaDataSystem>().Enabled = false;
        }
    }
}
