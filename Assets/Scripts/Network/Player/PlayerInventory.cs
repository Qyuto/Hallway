using System;
using Item;
using Mirror;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Network.Player
{
    public class PlayerInventory : NetworkBehaviour
    {
        [SerializeField] private LayerMask itemMask;
        [SerializeField] private Transform viewTransform;
        [SerializeField] private InputActionReference interactableReference;
        [SerializeField] private InputActionReference dropItemReference;
        [SerializeField] private InputActionReference firstSlotReference;
        [SerializeField] private InputActionReference secondSlotReference;
        [SerializeField] private InputActionReference thirdSlotReference;

        public SyncList<string> _inventoryString = new SyncList<string>();

        public UnityEvent<int> OnActiveSlotChanged;

        [SyncVar] private string _activeItem;
        private int _activeSlot;

        private void Start()
        {
            SubscribeInputReference();
            if (isLocalPlayer)
                OnActiveSlotChanged.AddListener(OnActiveSlotChange);
        }

        private void OnEnable()
        {
            SubscribeInputReference();
        }

        private void OnDisable()
        {
            UnSubscribeInputReference();
        }

        private void UnSubscribeInputReference()
        {
            if (!isLocalPlayer) return;
            interactableReference.action.performed -= InteractablePerformed;
            interactableReference.action.Disable();

            dropItemReference.action.performed -= DropItem;
            dropItemReference.action.Disable();

            firstSlotReference.action.performed -= ChangeActiveFirstSlot;
            secondSlotReference.action.performed -= ChangeActiveSecondSlot;
            thirdSlotReference.action.performed -= ChangeActiveThirdSlot;

            firstSlotReference.action.Disable();
            secondSlotReference.action.Disable();
            thirdSlotReference.action.Disable();
        }

        private void SubscribeInputReference()
        {
            if (!isLocalPlayer) return;
            interactableReference.action.performed += InteractablePerformed;
            interactableReference.action.Enable();

            dropItemReference.action.performed += DropItem;
            dropItemReference.action.Enable();

            firstSlotReference.action.performed += ChangeActiveFirstSlot;
            secondSlotReference.action.performed += ChangeActiveSecondSlot;
            thirdSlotReference.action.performed += ChangeActiveThirdSlot;

            firstSlotReference.action.Enable();
            secondSlotReference.action.Enable();
            thirdSlotReference.action.Enable();
        }

        private void InteractablePerformed(InputAction.CallbackContext obj)
        {
            TryPickUp();
        }

        private void DropItem(InputAction.CallbackContext obj)
        {
            CmdDropItem(_inventoryString[0], viewTransform.position + viewTransform.forward);
        }

        private void TryPickUp()
        {
            if (viewTransform == null)
                viewTransform = Camera.main.transform;

            if (Physics.SphereCast(viewTransform.position, 0.3f, viewTransform.forward, out RaycastHit hitInfo, 2f,
                    itemMask, QueryTriggerInteraction.Ignore))
            {
                IInteractable interactable = hitInfo.collider.GetComponentInParent<IInteractable>();
                interactable.OnSelect(netIdentity);
            }
        }

        private void OnActiveSlotChange(int newSlot)
        {
            if (_inventoryString.Count > newSlot && _inventoryString[newSlot] != null)
                CmdSpawnInventoryItem(_inventoryString[newSlot], transform.forward);
        }

        private void ChangeActiveFirstSlot(InputAction.CallbackContext obj)
        {
            _activeSlot = 0;
            OnActiveSlotChanged?.Invoke(_activeSlot);
        }

        private void ChangeActiveSecondSlot(InputAction.CallbackContext obj)
        {
            _activeSlot = 1;
            OnActiveSlotChanged?.Invoke(_activeSlot);
        }

        private void ChangeActiveThirdSlot(InputAction.CallbackContext obj)
        {
            _activeSlot = 2;
            OnActiveSlotChanged?.Invoke(_activeSlot);
        }

        [Command(requiresAuthority = false)]
        private void CmdSpawnInventoryItem(string itemKey, Vector3 direction) // Todo: Change name for method
        {
            if (_activeItem == itemKey) return;
            ItemInfo info = OnlineItemDatabase.ItemDatabase.GetItem(itemKey);
            GameObject newItem = Instantiate(info.Prefab, transform.position, Quaternion.identity);

            Rigidbody rigidbody = newItem.GetComponent<Rigidbody>();
            if (rigidbody != null) rigidbody.isKinematic = true;
            _activeItem = itemKey;
            NetworkServer.Spawn(newItem);
            
            SetParentForNewObject(netIdentity, newItem.GetComponent<NetworkIdentity>());
            newItem.transform.localPosition = info.SpawnInventoryPosition;
            newItem.transform.localRotation = Quaternion.LookRotation(direction, Vector3.forward);
        }

        [ClientRpc]
        private void SetParentForNewObject(NetworkIdentity newParent, NetworkIdentity newObject)
        {
            // newObject.transform.parent = newParent.transform;
        }

        [Command(requiresAuthority = false)]
        private void CmdDropItem(string dropItemId, Vector3 dropPosition)
        {
            ItemInfo info = OnlineItemDatabase.ItemDatabase.GetItem(dropItemId);
            GameObject newItem = Instantiate(info.Prefab, dropPosition, Quaternion.identity);
            NetworkServer.Spawn(newItem);
            _inventoryString.Remove(dropItemId);
            NotifyDropItem(netIdentity.connectionToClient, dropItemId);
        }

        [Command(requiresAuthority = false)]
        public void CmdAddNewItem(NetworkIdentity itemIdentity, string itemKey)
        {
            Debug.Log($"Try add new item for player: {NetworkClient.localPlayer.name}");
            _inventoryString.Add(itemKey);
            NetworkServer.Destroy(itemIdentity.gameObject);
            NotifyNewItem(netIdentity.connectionToClient, itemKey);
        }

        [TargetRpc]
        private void NotifyNewItem(NetworkConnectionToClient target, string newItemKey)
        {
            Debug.Log($"You have new item: {newItemKey}");
        }

        [TargetRpc]
        private void NotifyDropItem(NetworkConnectionToClient target, string newItemKey)
        {
            Debug.Log($"You have drop item: {newItemKey}");
        }
    }
}