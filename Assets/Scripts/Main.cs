using Unity.Entities;
using UnityEngine;

using PerlinPlayground.Systems;

namespace PerlinPlayground
{
    

    public partial class Main
    {

        public static Texture2D tempTextureTest;

        /// <summary>
        /// Static reference to the active world's EntityManager instance.
        /// </summary>
        public static EntityManager entityManager;

        /// <summary>
        /// Bootstrap entry point. Initializes entityManager and calls DefineArchetypes().
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Initialize()
        {
            entityManager = World.Active.GetOrCreateManager<EntityManager>();
            Managers.Archetype.Initialize(entityManager);

            //Unity.Entities.World.Active.GetExistingManager<MapBandingSystem>().Enabled = false;
        }

        /// <summary>
        /// Initialization to occur after the scene has loaded. 
        /// TODO: check if this is called only once or everytime
        /// a scene is loaded.
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        public static void InitializeWithScene()
        {
            CreateTextureViewer();
        }
    }
}