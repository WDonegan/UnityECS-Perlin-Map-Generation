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
    [UpdateInGroup(typeof(UpdateGroups.ProcessingGroup))]
    [UpdateBefore(typeof(UpdateTextureSystem))]
    [UpdateAfter(typeof(MapBandingSystem))]
    public class CellProcessingSystem : ComponentSystem
    {
        float[,] m_rawFloatData, m_newFloatData;
        ComponentGroup m_mapDataGroup;
        EntityArray m_mapDataEntities;
        ComponentDataArray<MapIndexComponent> m_mDataGroupIndex;
        ComponentDataArray<MapDataComponent> m_mDataGroupData;

        protected override void OnCreateManager(int capacity)
        {
            m_mapDataGroup = GetComponentGroup(typeof(MapIndexComponent), typeof(MapDataComponent));
            m_rawFloatData = new float[128, 128];
            m_newFloatData = new float[128, 128];
        }

        protected override void OnUpdate()
        {
            m_mapDataEntities = m_mapDataGroup.GetEntityArray();
            m_mDataGroupIndex = m_mapDataGroup.GetComponentDataArray<MapIndexComponent>();
            m_mDataGroupData = m_mapDataGroup.GetComponentDataArray<MapDataComponent>();

            for (int i = 0; i < 128 * 128; i ++)
            {
                m_rawFloatData[m_mDataGroupIndex[i].Value.x, m_mDataGroupIndex[i].Value.y] = m_mDataGroupData[i].Value;
            }

            m_newFloatData = (float[,])m_rawFloatData.Clone();

            for (int w = 0; w < 128; w ++)
            {
                for (int h = 0; h < 128; h++)
                {
                    var countOver = 0;
                    var countUnder = 0;

                    //------ INNER LOOP ------
                    for (int wx = -1; wx <= 1; wx++)
                    {
                        for (int hx = -1; hx <= 1; hx++)
                        {
                            if (w + wx > 127 || w + wx < 0 || h + hx > 127 || h + hx < 0)
                            {
                                continue;
                            }
                            if (m_rawFloatData[w + wx, h + hx] == m_rawFloatData[w, h])
                            {
                                continue;
                            }
                            else
                            if (m_rawFloatData[w + wx, h + hx] < m_rawFloatData[w, h])
                            {
                                countUnder++;
                            }
                            else
                            if (m_rawFloatData[w + wx, h + hx] > m_rawFloatData[w, h])
                            {
                                countOver++;
                            }
                        }
                    }
                    //------ End Loop, Chech Results -----
                    if (countUnder >= 2)
                    {
                        m_newFloatData[w, h] = 0;
                    }

                }
            }

            for (int i = 0; i < 128 * 128; i++)
            {
                Main.entityManager.SetComponentData(m_mapDataEntities[i], new MapDataComponent
                {
                    Value = m_newFloatData[m_mDataGroupIndex[i].Value.x, m_mDataGroupIndex[i].Value.y]
                });
            }

            Unity.Entities.World.Active.GetExistingManager<CellProcessingSystem>().Enabled = false;
        }
    }
}
