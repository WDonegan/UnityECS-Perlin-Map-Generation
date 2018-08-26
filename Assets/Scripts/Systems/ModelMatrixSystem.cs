using Unity.Entities;
using Unity.Collections;
using Unity.Mathematics;
using Unity.Jobs;
using Unity.Burst;

using PerlinPlayground.Components.Transform;

namespace PerlinPlayground.Systems {

	[UpdateInGroup(typeof(UpdateGroups.RenderingGroup))]
	[UpdateBefore(typeof(RendererSystem))]
	public class ModelMatrixSystem : JobComponentSystem 
	{
		struct TransGroup
		{
			public readonly int Length;
			public ComponentDataArray<ModelMatrix> Matrices;
			[ReadOnly] public ComponentDataArray<Pos> Positions;
		}
		[Inject] TransGroup m_TransGroup;

		struct RotTransGroup
		{
            public readonly int Length;
			public ComponentDataArray<ModelMatrix> Matrices;
			[ReadOnly] public ComponentDataArray<Pos> Positions;
			[ReadOnly] public ComponentDataArray<Rot> Rotations;
		}
		[Inject] RotTransGroup m_RotTransGroup;

		struct SclRotTransGroup
		{
            public readonly int Length;
			public ComponentDataArray<ModelMatrix> Matrices;
			[ReadOnly] public ComponentDataArray<Pos> Positions;
			[ReadOnly] public ComponentDataArray<Rot> Rotations;
			[ReadOnly] public ComponentDataArray<Scl> Scales;
		}
		[Inject] SclRotTransGroup m_SclRotTransGroup;
		///
		///------------------------------------------------------------
        [BurstCompile]
        struct TransToMatrix : IJobParallelFor
        {
			public ComponentDataArray<ModelMatrix> Matrices;
 			[ReadOnly] public ComponentDataArray<Pos> Positions;

            public void Execute(int index)
            {
				float3 position = Positions[index].Value;
				Matrices[index] = new ModelMatrix { Value = math.float4x4(quaternion.identity, position) };
            } 
        }
        ///
        ///------------------------------------------------------------
        [BurstCompile]
        struct RotTransToMatrix : IJobParallelFor
        {
			public ComponentDataArray<ModelMatrix> Matrices;
			[ReadOnly] public ComponentDataArray<Pos> Positions;
			[ReadOnly] public ComponentDataArray<Rot> Rotations;

            public void Execute(int index)
            {
                float3 position = Positions[index].Value;
				quaternion quat = Rotations[index].Value;
				Matrices[index] = new ModelMatrix { Value = math.float4x4(quat, position) };
            }
        }
        ///
        ///------------------------------------------------------------
        [BurstCompile]
        struct SclRotTransToMatrix : IJobParallelFor
        {
			public ComponentDataArray<ModelMatrix> Matrices;
			[ReadOnly] public ComponentDataArray<Pos> Positions;
			[ReadOnly] public ComponentDataArray<Rot> Rotations;
			[ReadOnly] public ComponentDataArray<Scl> Scales;

            public void Execute(int index)
            {
				float3 position = Positions[index].Value;
				quaternion quat = Rotations[index].Value;
				float3 scale = Scales[index].Value;

				float4x4 matrix = math.mul(math.float4x4(scale.x, 0.0f, 0.0f, 0.0f,
                                                         0.0f, scale.y, 0.0f, 0.0f,
                                                         0.0f, 0.0f, scale.z, 0.0f,
                                                         0.0f, 0.0f, 0.0f, 1.0f),
                            math.float4x4(quat, new float3 {x=0,y=0,z=0}));

				matrix = math.mul(math.float4x4(quaternion.identity, position), matrix);
				Matrices[index] = new ModelMatrix { Value = matrix };
            }
        }
		///
        ///------------------------------------------------------------
		protected override JobHandle OnUpdate(JobHandle _inpDeps)
		{
			var transToMatrixJob = new TransToMatrix
			{
				Matrices = m_TransGroup.Matrices,
				Positions = m_TransGroup.Positions
			};
			var transToMatrixJobHandle = transToMatrixJob.Schedule(m_TransGroup.Length, 64, _inpDeps);

			var rotTransToMatrixJob = new RotTransToMatrix
			{
				Matrices = m_RotTransGroup.Matrices,
				Positions = m_RotTransGroup.Positions,
				Rotations = m_RotTransGroup.Rotations
			};
			var rotTransToMatrixJobHandle = rotTransToMatrixJob.Schedule(m_RotTransGroup.Length, 64, transToMatrixJobHandle);

			var sclRotTransToMatrixJob = new SclRotTransToMatrix
			{
				Matrices = m_SclRotTransGroup.Matrices,
				Positions = m_SclRotTransGroup.Positions,
				Rotations = m_SclRotTransGroup.Rotations,
				Scales = m_SclRotTransGroup.Scales
			};
			return sclRotTransToMatrixJob.Schedule(m_SclRotTransGroup.Length, 64, rotTransToMatrixJobHandle);
		}
    } 

}