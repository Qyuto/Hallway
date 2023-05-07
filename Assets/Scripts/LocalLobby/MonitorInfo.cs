using System;
using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace LocalLobby
{
    public class MonitorInfo : MonoBehaviour
    {
        [SerializeField] private Button showInfoLayerButton;
        [SerializeField] private CanvasGroup infoLayer;
        [SerializeField] private TextMeshProUGUI senderNameGUI;
        [SerializeField] private TextMeshProUGUI textGUI;
        [SerializeField] private MonitorTextInfo textInfo;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip typingClip;

        private void Awake()
        {
            showInfoLayerButton.onClick.AddListener(ToggleCanvasGroup);
            audioSource.clip = typingClip;
            infoLayer.alpha = 0;
        }

        private void ToggleCanvasGroup()
        {
            infoLayer.alpha = infoLayer.alpha == 0 ? 1f : 0f;
            audioSource.Stop();
            StopAllCoroutines();
            if (infoLayer.alpha == 1)
                StartCoroutine(WriteTextInfo());
            else
                textGUI.text = String.Empty;
        }

        IEnumerator WriteTextInfo()
        {
            audioSource.Play();
            
            textGUI.text = String.Empty;
            senderNameGUI.text = String.Empty;
            StringBuilder currentText = new StringBuilder(textInfo.Text.Length);
            StringBuilder currentSenderName = new StringBuilder(textInfo.NameSender.Length);
            
            for (int i = 0; i < textInfo.NameSender.Length; i++)
            {
                char symbol = MonitorTextInfo.Symbols[Random.Range(0, MonitorTextInfo.Symbols.Length)];
                currentSenderName.Insert(i, symbol);
                senderNameGUI.text += symbol;
                yield return new WaitForSeconds(0.15f);
                currentSenderName.Replace(symbol, textInfo.NameSender[i], i, 1);
                senderNameGUI.text = currentSenderName.ToString();
            }
            
            for (int i = 0; i < textInfo.Text.Length; i++)
            {
                char symbol = MonitorTextInfo.Symbols[Random.Range(0, MonitorTextInfo.Symbols.Length)];
                currentText.Insert(i, symbol);
                textGUI.text += symbol;
                yield return new WaitForSeconds(0.15f);
                currentText.Replace(symbol, textInfo.Text[i], i, 1);
                textGUI.text = currentText.ToString();
            }

            audioSource.Stop();
        }
    }
}