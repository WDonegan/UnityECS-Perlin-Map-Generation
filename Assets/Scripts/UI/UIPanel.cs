using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

using PerlinPlayground.Components;
using PerlinPlayground.Managers;
using PerlinPlayground.Systems;

namespace PerlinPlayground.UI {
    public class UIPanel : MonoBehaviour {

        public Image Preview;
        Dropdown LayerSelection;
        public SliderElement Scale;
        public OffsetElement Offset;
        Toggle BandToggle;

        struct LayerInfo
        {
            public float scale;
            public float offsetX;
            public float offsetY;

            public float[] mapData;
            public Texture2D texture;
        }

        LayerInfo[] layerData;

        public int width, height;

        int activeLayer = 0;
        bool isDirty = true;

        void Start() {
            LayerSelection = GetComponentInChildren<Dropdown>();
            BandToggle = GetComponentInChildren<Toggle>();

            layerData = new LayerInfo[3];

            for (int i = 0; i < 3; i++)
            {
                layerData[i] = new LayerInfo
                {
                    scale = Scale.Slider.minValue,
                    offsetX = 0,
                    offsetY = 0,
                    mapData = new float[width * height],
                    texture = new Texture2D(width, height) { filterMode = FilterMode.Point }
                };
                UpdateMap(layerData[i]);
            }
            
            InitializeEntityData();
        }

        void Update()
        {
            if (isDirty)
            {
                layerData[activeLayer].scale = Scale.Value;
                layerData[activeLayer].offsetX = Offset.X;
                layerData[activeLayer].offsetY = Offset.Y;

                UpdateMap(layerData[activeLayer]);

                UpdateEntityData();

                Unity.Entities.World.Active.GetExistingManager<MapBandingSystem>().Enabled = bandMap;
                Unity.Entities.World.Active.GetExistingManager<CellProcessingSystem>().Enabled = outlineMap;
            }

            
        }

        void UpdateMap(LayerInfo data)
        {
            var f = 0f;

            for (int w = 0; w < width; w++)
            {
                for (int h = 0; h < height; h++)
                {
                    f = Mathf.PerlinNoise((float)(w + data.offsetX) * data.scale, (float)(h + data.offsetY) * data.scale);

                    data.mapData[(w * width) + h] = f;
                    data.texture.SetPixel(w, h, new Color(f, f, f, 1.0f));
                }
            }
            data.texture.Apply();

            Preview.sprite = Sprite.Create(
                                        data.texture,
                                        new Rect(0, 0, width, height),
                                        Vector2.zero);

            isDirty = false;
        }

        public void UpdateActiveLayer(int newActiveLayer)
        {
            activeLayer = LayerSelection.value;

            Scale.SetValue(layerData[activeLayer].scale);
            Offset.SetValue(layerData[activeLayer].offsetX, layerData[activeLayer].offsetY);
        }

        public void MarkDirty()
        {
            isDirty = true;
        }


        NativeArray<Entity> map;
        public void InitializeEntityData()
        {
            Entity cell = Main.entityManager.CreateEntity(Archetype.GridData);
            
            map = new NativeArray<Entity>(width * height, Allocator.Persistent);
            Main.entityManager.Instantiate(cell, map);

            UpdateEntityData();

            Main.entityManager.DestroyEntity(cell);
        }

        public void UpdateEntityData()
        {
            float color;
            int i = 0;
            for (int w = 0; w < width; w++)
            {
                for (int h = 0; h < height; h++)
                {
                    i = w * width + h;

                    color = math.clamp(
                        ((layerData[0].mapData[i] * 0.33333f) +
                         (layerData[1].mapData[i] * 0.33333f) +
                         (layerData[2].mapData[i] * 0.33333f)),
                          0, 1);

                    Main.entityManager.SetComponentData(map[i],
                        new MapIndexComponent
                        {
                            Value = new int2(w, h)
                        });

                    Main.entityManager.SetComponentData(map[i],
                        new MapDataComponent
                        {
                            Value = color
                        });
                }
            }
        }

        bool bandMap = true;
        public void onBandToggle (bool b)
        {
            bandMap = b;
            isDirty = true;
        }

        bool outlineMap = true;
        public void onOutlineToggle(bool b)
        {
            outlineMap = b;
            isDirty = true;
        }

        public void OnDestroy()
        {
            map.Dispose();
        }
    }
}
//math.clamp(math.round(color * 10f), 0, 10) * 0.1f;