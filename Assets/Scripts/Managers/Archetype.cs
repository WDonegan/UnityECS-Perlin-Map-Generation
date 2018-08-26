using Unity.Entities;

namespace PerlinPlayground.Managers
{
    public static class Archetype
    {
        public static EntityArchetype GridData;


        public static EntityArchetype Base;
        


        public static void Initialize(EntityManager entityManager)
        {
            Base = entityManager.CreateArchetype(
                typeof(Components.Transform.Pos),
                typeof(Components.Transform.Rot),
                typeof(Components.Transform.Scl),
                typeof(Components.Transform.ModelMatrix),
                typeof(Unity.Rendering.MeshInstanceRenderer));


            GridData = entityManager.CreateArchetype(
                typeof(Components.MapDataComponent),
                typeof(Components.MapIndexComponent));
        }


    }
}