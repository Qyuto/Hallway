using Mirror;
using Network.Player;
using UnityEngine;

namespace Item
{
    public class ItemMain : NetworkBehaviour, IInteractable
    {
        [SerializeField] private ItemInfo info;

        public void OnSelect(NetworkIdentity identity)
        {
            if (identity.TryGetComponent(out PlayerInventory inventory))
                inventory.CmdAddNewItem(inventory.LocalActiveSlot, netIdentity, info.ItemId);
        }
    }
}