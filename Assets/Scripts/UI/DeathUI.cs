using Network;
using UnityEngine;

namespace UI
{
    public class DeathUI : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;
        
        private void Awake()
        {
            canvasGroup.alpha = 0;
        }

        private void Start()
        {
            LocalPlayer.Player.OnPlayerDeath.AddListener(ShowDeathUI);
        }

        private void ShowDeathUI()
        {
            canvasGroup.gameObject.SetActive(false);
        }
    }
}