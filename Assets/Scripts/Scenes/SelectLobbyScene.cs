using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectLobbyScene : MonoBehaviour
{
    void Start()
    {
#if IGNORE_LOBBYROOM
        SceneManager.LoadScene("MainMenu");
#else
        SceneManager.LoadScene("LocalLobby");
#endif
    }
}