using UnityEngine;

namespace Scripts.Models
{
    [CreateAssetMenu]
    public class ItemSO : ScriptableObject
    {
        [field: SerializeField]
        public bool IsStackable { get; set; }

        public int ID => GetInstanceID();

        [field: SerializeField]
        public int MaxStackSize { get; set; } = 1;

        [field: SerializeField]
        public Sprite ItemSprite { get; set; }

        [field: SerializeField]
        [field: Min(0)]
        public int AttackStats { get; set; } = 0;

        [field: SerializeField]
        [field: Min(0)]
        public int DefenseStats { get; set; } = 0;

        [field: SerializeField]
        public string ItemName { get; set; }

        [field: SerializeField]
        [field: TextArea]
        public string ItemDescription { get; set; }

        [field: SerializeField]
        [field: TextArea]
        public string Quirk { get; set; }

        [field: SerializeField]
        [field: TextArea]
        public string QuirkDescription { get; set; }
    }
}