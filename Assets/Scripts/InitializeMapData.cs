using PerlinPlayground.Components;
using PerlinPlayground.Managers;

using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

using UnityEngine;

namespace PerlinPlayground
{
    public partial class Main
    {
        private static void InitializeMapData()
        {
            int width = 128, height = 128, cells = width * height;

            Entity cell = entityManager.CreateEntity(Archetype.GridData);

            NativeArray<Entity> map = new NativeArray<Entity>(cells, Allocator.Temp);
            entityManager.Instantiate(cell, map);

            entityManager.DestroyEntity(cell);

            for (int w = 0; w < width; w++)
            {
                for (int h = 0; h < height; h++)
                {
                    var perlinValue1 = Mathf.PerlinNoise((float)w * 0.025f, (float)h * 0.025f) * 0.3333f;

                    var perlinValue2 = Mathf.PerlinNoise((float)w * 0.050f, (float)h * 0.050f) * 0.3333f;

                    var perlinValue3 = Mathf.PerlinNoise((float)(w + 54) * 0.025f, (float)(h + 54) * 0.025f) * 0.3333f;

                    var colorValue = perlinValue1 + perlinValue2 + perlinValue3;
                    colorValue = math.clamp(math.round(colorValue * 10), 0, 10);
                    colorValue *= 0.1f;

                    entityManager.SetComponentData(map[w * width + h],
                        new MapIndexComponent
                        {
                            Value = new int2(w, h)
                        });

                    entityManager.SetComponentData(map[w * width + h],
                        new MapDataComponent
                        {
                            Value = colorValue
                        });
                }
            }
            
            map.Dispose();
        }
    }
}