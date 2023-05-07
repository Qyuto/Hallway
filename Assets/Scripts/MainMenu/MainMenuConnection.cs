using System;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuConnection : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputIpField;
    [SerializeField] private Button clientButton;
    [SerializeField] private Button hostButton;

    private void Awake()
    {
        clientButton.onClick.AddListener(ConnectToServer);
        hostButton.onClick.AddListener(CreateServer);
        string ip = PlayerPrefs.GetString("lastIp", "localhost");
        inputIpField.text = ip;
    }

    private void CreateServer()
    {
        if (string.IsNullOrEmpty(inputIpField.text)) return;
        PlayerPrefs.SetString("lastIp", inputIpField.text);
        NetworkManager.singleton.networkAddress = inputIpField.text;
        NetworkManager.singleton.StartHost();
    }

    private void ConnectToServer()
    {
        Uri serverIp = new Uri("kcp://localhost");
        if (!string.IsNullOrEmpty(inputIpField.text))
            serverIp = new Uri($"kcp://{inputIpField.text}");
        PlayerPrefs.SetString("lastIp", inputIpField.text);
        NetworkManager.singleton.networkAddress = inputIpField.text;
        NetworkManager.singleton.StartClient(serverIp);
    }
}