using UnityEngine;

namespace Item
{
    [CreateAssetMenu(fileName = "Item_", menuName = "Room/Item", order = 0)]
    public class ItemInfo : ScriptableObject
    {
        [SerializeField] private Vector3 spawnInventoryPosition;
        [SerializeField] private bool showInInventory;
        [SerializeField] private GameObject itemPrefab;
        [SerializeField] private string itemKeyId;

        public Vector3 SpawnInventoryPosition => spawnInventoryPosition;
        public bool ShowInInventory => showInInventory;
        public GameObject Prefab => itemPrefab;
        public string ItemId => itemKeyId;
    }
}