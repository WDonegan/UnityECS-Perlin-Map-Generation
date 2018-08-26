using UnityEditor;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

using PerlinPlayground.Components;

namespace PerlinPlayground.Systems
{
    [UpdateInGroup(typeof(UpdateGroups.ProcessingGroup))]
    [UpdateBefore(typeof(CellProcessingSystem))]
    public class MapBandingSystem : JobComponentSystem
    {
        struct MapData
        {
            public readonly int Length;
            public ComponentDataArray<MapDataComponent> Data;
        }
        [Inject] MapData m_MapData;

        [BurstCompile]
        struct BandMap : IJobParallelFor
        {
            public ComponentDataArray<MapDataComponent> Data;

            public void Execute(int index)
            {
                var value = Data[index].Value;

                value = math.round(value * 10f);
                value *= 0.1f;

                Data[index] = new MapDataComponent { Value = value };
            }
        }

        struct DisableSystem : IJob
        {
            public void Execute()
            {
                Unity.Entities.World.Active.GetExistingManager<MapBandingSystem>().Enabled = false;
            }
        }


        protected override JobHandle OnUpdate(JobHandle _inpDeps)
        {
            var MapBandingJob = new BandMap
            {
                Data = m_MapData.Data
            };
            var MapBandingJobHandle =  MapBandingJob.Schedule(m_MapData.Length, 64, _inpDeps);

            var DisableSystemJob = new DisableSystem();

            return DisableSystemJob.Schedule(MapBandingJobHandle);
        }
    }
}
