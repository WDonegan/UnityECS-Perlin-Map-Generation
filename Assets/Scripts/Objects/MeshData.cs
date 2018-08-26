using UnityEngine;

namespace PerlinPlayground.Objects
{
    [CreateAssetMenu]
    public class MeshData : ScriptableObject
    {
        [SerializeField]
        public Mesh Value;
    }
}
