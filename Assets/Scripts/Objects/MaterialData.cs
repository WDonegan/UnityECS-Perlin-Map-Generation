using UnityEngine;

namespace PerlinPlayground.Objects
{
    [CreateAssetMenu]
    public class MaterialData : ScriptableObject
    {
        [SerializeField]
        public Material Value;
    }
}