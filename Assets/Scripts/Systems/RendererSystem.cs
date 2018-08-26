using System.Collections.Generic;
///-----------------------------------------
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;
using UnityEngine.Assertions;
///-----------------------------------------
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Collections.LowLevel.Unsafe;
///-----------------------------------------
using PerlinPlayground.Components.Transform;

namespace PerlinPlayground.Systems {

    [UpdateInGroup(typeof(UpdateGroups.RenderingGroup))]
    [UpdateAfter(typeof(PreLateUpdate.ParticleSystemBeginUpdateAll))]
    public class RendererSystem : ComponentSystem
    {
        Matrix4x4[] m_matricesArray = new Matrix4x4[1023];
        List<MeshInstanceRenderer> m_cacheduniqueRenderTypes = new List<MeshInstanceRenderer>(10);
        ComponentGroup m_instanceRendererGroup;

        public unsafe static void CopyMatrices(ComponentDataArray<ModelMatrix> _transforms, int  _beginIndex, int _length, Matrix4x4[] _outMatrices)
        {
            fixed (Matrix4x4* matricesPtr = _outMatrices)
            {
                Assert.AreEqual(sizeof(Matrix4x4), sizeof(ModelMatrix));
                var matricesSlice = NativeSliceUnsafeUtility.ConvertExistingDataToNativeSlice<ModelMatrix>(matricesPtr, sizeof(Matrix4x4), _length);
#if ENABLE_UNITY_COLLECTIONS_CHECKS
                NativeSliceUnsafeUtility.SetAtomicSafetyHandle(ref matricesSlice, AtomicSafetyHandle.GetTempUnsafePtrSliceHandle());
#endif
                _transforms.CopyTo(matricesSlice, _beginIndex);
            }
        }

        protected override void OnCreateManager(int capacity)
        {
            m_instanceRendererGroup = GetComponentGroup(typeof(MeshInstanceRenderer), typeof(ModelMatrix), typeof(Pos), typeof(Rot), typeof(Scl));
        }

        protected override void OnUpdate()
        {
            EntityManager.GetAllUniqueSharedComponentDatas(m_cacheduniqueRenderTypes);
            
            for (int index = 0; index != m_cacheduniqueRenderTypes.Count; index++)
            {
                var renderer = m_cacheduniqueRenderTypes[index];
                m_instanceRendererGroup.SetFilter(renderer);
                var transforms = m_instanceRendererGroup.GetComponentDataArray<ModelMatrix>();
                int beginIndex = 0;

                while(beginIndex < transforms.Length)
                {
                    int length = math.min(m_matricesArray.Length, transforms.Length - beginIndex);
                    CopyMatrices(transforms, beginIndex, length, m_matricesArray);

                    if (renderer.mesh != null)
                        Graphics.DrawMeshInstanced(renderer.mesh, 0, renderer.material, m_matricesArray, length, null, renderer.castShadows, renderer.receiveShadows);
                    beginIndex += length;
                }
            }
            m_cacheduniqueRenderTypes.Clear();
        }
    }
}