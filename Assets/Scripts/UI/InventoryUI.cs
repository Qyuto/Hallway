using System;
using Item;
using Network.Player;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class InventoryUI : MonoBehaviour
    {
        [SerializeField] private Image[] uiImage;

        private PlayerInventory _localInventory;
        private Image _lastSelectedImage;

        private void Awake()
        {
            _localInventory = GetComponentInParent<PlayerInventory>();

            _localInventory.OnActiveSlotChanged.AddListener(UpdateActiveSlot);
            _localInventory.OnAddItem.AddListener(CreateNewImage);
            _localInventory.OnRemoveItem.AddListener(RemoveImage);
        }

        private void RemoveImage(string itemKey, int slotID)
        {
            if (uiImage.Length < slotID) return;
            uiImage[slotID].sprite = null;
        }

        private void CreateNewImage(string itemKey, int slotID)
        {
            ItemInfo itemInfo = OnlineItemDatabase.ItemDatabase.GetItem(itemKey);
            if (itemInfo?.ItemInventorySprite == null) return;
            if (uiImage.Length < slotID) return;

            uiImage[slotID].sprite = itemInfo.ItemInventorySprite;
        }

        private void UpdateActiveSlot(int slotId)
        {
            if (_lastSelectedImage != null)
                _lastSelectedImage.rectTransform.localScale = Vector3.one;

            _lastSelectedImage = uiImage[slotId];
            _lastSelectedImage.rectTransform.localScale = Vector3.one * 1.2f;
        }
    }
}