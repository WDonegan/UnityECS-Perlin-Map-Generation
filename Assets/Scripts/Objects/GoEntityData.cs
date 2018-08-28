using UnityEngine;

namespace PerlinPlayground.Objects
{
    [CreateAssetMenu]
    public class GoEntityData : ScriptableObject
    {
        [SerializeField]
        public GameObject Value;
    }
}