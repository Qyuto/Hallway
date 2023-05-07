using Mirror;
using Network;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace MainMenu
{
    public class PlayerMenu : MonoBehaviour
    {
        [SerializeField] private CanvasGroup playerMenu;
        [SerializeField] private InputActionReference toggleMenuReference;

        [Header("Mouse")] [SerializeField] private MouseRotation mouseRotation;
        [SerializeField] private Slider mouseSense;
        [SerializeField] private TextMeshProUGUI mouseSenseText;

        [Header("Network")] [SerializeField] private Button disconnectButton;

        private void Awake()
        {
            playerMenu.gameObject.SetActive(false);
            mouseSense.onValueChanged.AddListener(ChangeMouseSense);
            disconnectButton.onClick.AddListener(Disconnect);
        }

        private void Disconnect()
        {
            if (NetworkServer.active && NetworkClient.isConnected)
                NetworkManager.singleton.StopHost();
            else
                NetworkManager.singleton.StopClient();
        }

        private void OnEnable()
        {
            toggleMenuReference.action.started += ToggleMenu;
            toggleMenuReference.action.Enable();
        }

        private void OnDisable()
        {
            toggleMenuReference.action.started -= ToggleMenu;
            toggleMenuReference.action.Disable();
        }

        private void ToggleMenu(InputAction.CallbackContext obj)
        {
            
            if (!playerMenu.gameObject.activeSelf)
            {
                playerMenu.gameObject.SetActive(true);
                mouseRotation.enabled = false;
            }
            else
            {
                playerMenu.gameObject.SetActive(false);
                mouseRotation.enabled = true;
            }
        }

        private void ChangeMouseSense(float arg0)
        {
            mouseSenseText.text = $"Sensitivity: {arg0:0.0}";
            mouseRotation.MouseSense = arg0 / 10;
        }
    }
}