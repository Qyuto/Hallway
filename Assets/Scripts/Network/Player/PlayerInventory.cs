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

        public UnityEvent<int> OnActiveSlotChanged;

        [SyncVar] private string _activeItem;
        [SyncVar] private NetworkIdentity _activeNetworkObject;
        
        private readonly SyncList<string> _inventoryString = new SyncList<string>();
        private int _activeSlot;

        private void Start()
        {
            BindInputActions();
            if (isLocalPlayer)
                OnActiveSlotChanged.AddListener(OnActiveSlotChange);
        }

        public override void OnStartClient()
        {
            base.OnStartClient();
            if(!string.IsNullOrEmpty(_activeItem))
                CmdSyncActiveItem(_activeItem);
        }

        private void OnEnable()
        {
            BindInputActions();
        }

        private void OnDisable()
        {
            UnbindInputActions();
        }

        private void UnbindInputActions()
        {
            if (!isLocalPlayer) return;
            interactableReference.action.performed -= InteractablePerformed;
            interactableReference.action.Disable();

            dropItemReference.action.performed -= DropItem;
            dropItemReference.action.Disable();

            firstSlotReference.action.performed -= SetActiveFirstSlot;
            secondSlotReference.action.performed -= SetActiveSecondSlot;
            thirdSlotReference.action.performed -= SetActiveThirdSlot;

            firstSlotReference.action.Disable();
            secondSlotReference.action.Disable();
            thirdSlotReference.action.Disable();
        }

        private void BindInputActions()
        {
            if (!isLocalPlayer) return;
            interactableReference.action.performed += InteractablePerformed;
            interactableReference.action.Enable();

            dropItemReference.action.performed += DropItem;
            dropItemReference.action.Enable();

            firstSlotReference.action.performed += SetActiveFirstSlot;
            secondSlotReference.action.performed += SetActiveSecondSlot;
            thirdSlotReference.action.performed += SetActiveThirdSlot;

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
            if(_inventoryString.Count >= _activeSlot && !string.IsNullOrEmpty(_inventoryString[_activeSlot]))
                CmdDropItem(_inventoryString[_activeSlot], viewTransform.position + viewTransform.forward);
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
                CmdSpawnInventoryItem(_inventoryString[newSlot]);
        }

        private void SetActiveFirstSlot(InputAction.CallbackContext obj)
        {
            _activeSlot = 0;
            OnActiveSlotChanged?.Invoke(_activeSlot);
        }

        private void SetActiveSecondSlot(InputAction.CallbackContext obj)
        {
            _activeSlot = 1;
            OnActiveSlotChanged?.Invoke(_activeSlot);
        }

        private void SetActiveThirdSlot(InputAction.CallbackContext obj)
        {
            _activeSlot = 2;
            OnActiveSlotChanged?.Invoke(_activeSlot);
        }

        [Command(requiresAuthority = false)]
        private void CmdSpawnInventoryItem(string itemKey)
        {
            if (_activeItem == itemKey) return;
            
            ItemInfo info = OnlineItemDatabase.ItemDatabase.GetItem(itemKey);
            GameObject newItem = Instantiate(info.Prefab, transform.position, Quaternion.identity, netIdentity.transform);
            NetworkServer.Spawn(newItem.gameObject);
            
            _activeNetworkObject = newItem.GetComponent<NetworkIdentity>();
            _activeItem = itemKey;
            CmdSyncActiveItem(itemKey);
        }

        [Command(requiresAuthority = false)]
        private void CmdSyncActiveItem(string itemKey)
        {
            SetItemParentLocally(netIdentity);
            CmdUpdateItemTransform(itemKey);
        }
        
        [Command(requiresAuthority = false)]
        private void CmdUpdateItemTransform(string itemKey)
        {
            if(_activeNetworkObject == null) return;
            ItemInfo info = OnlineItemDatabase.ItemDatabase.GetItem(itemKey);
            _activeNetworkObject.transform.localPosition = info.SpawnInventoryPosition;
            if (_activeNetworkObject.TryGetComponent(out Rigidbody rigidbody))
                rigidbody.isKinematic = true;
        }
        
        [ClientRpc]
        private void SetItemParentLocally(NetworkIdentity networkParent)
        {
            if(_activeNetworkObject == null) return;
            _activeNetworkObject.transform.SetParent(networkParent.transform);
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