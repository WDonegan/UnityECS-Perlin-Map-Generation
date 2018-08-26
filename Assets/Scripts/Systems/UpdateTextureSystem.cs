using UnityEngine;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Rendering;

using PerlinPlayground.Components;
using PerlinPlayground.Components.Transform;

namespace PerlinPlayground.Systems
{
    [UpdateBefore(typeof(UpdateGroups.RenderingGroup))]
    [UpdateAfter(typeof(UpdateGroups.ProcessingGroup))]
    public class UpdateTextureSystem : ComponentSystem
    {
        Material material;
        Texture2D texture;

        ComponentGroup m_mapComponentGroup;

        ComponentDataArray<MapDataComponent> m_data;
        ComponentDataArray<MapIndexComponent> m_loc;

        protected override void OnCreateManager(int capacity)
        {
            m_mapComponentGroup = GetComponentGroup(typeof(MapDataComponent), typeof(MapIndexComponent));

            material = (Resources.Load("TextureViewerMaterial") as Objects.MaterialData).Value;

            texture = new Texture2D(128, 128);
            texture.filterMode = FilterMode.Point;
        }

        [BurstCompile]
        protected override void OnUpdate()
        {
            m_data = m_mapComponentGroup.GetComponentDataArray<MapDataComponent>();
            m_loc = m_mapComponentGroup.GetComponentDataArray<MapIndexComponent>();

            for (int w = 0; w < 128; w++)
            {
                for (int h = 0; h < 128; h++)
                {
                    int index = w * 128 + h;

                    texture.SetPixel(m_loc[index].Value.x, m_loc[index].Value.y,
                        new Color
                        {
                            r = m_data[index].Value,
                            g = m_data[index].Value,
                            b = m_data[index].Value,
                            a = 1.0f
                        });
                }
            }

            texture.Apply();

            material.mainTexture = texture;
        }
    }
}