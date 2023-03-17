using UnityEngine;
using UnityEngine.UI;

namespace Item
{
    [CreateAssetMenu(fileName = "Item_", menuName = "Room/Item", order = 0)]
    public class ItemInfo : ScriptableObject
    {
        [SerializeField] private Sprite itemInventorySprite;
        [SerializeField] private bool showInInventory;
        [SerializeField] private GameObject itemPrefab;
        [SerializeField] private string itemKeyId;
        [SerializeField] private Vector3 spawnInventoryPosition;

        public Sprite ItemInventorySprite => itemInventorySprite;
        public bool ShowInInventory => showInInventory;
        public GameObject Prefab => itemPrefab;
        public string ItemId => itemKeyId;
        public Vector3 SpawnInventoryPosition => spawnInventoryPosition;
    }
}