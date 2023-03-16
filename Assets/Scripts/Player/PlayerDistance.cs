using Network;
using UnityEngine;

namespace Player
{
    public class PlayerDistance : MonoBehaviour
    {
        private float _timer;
        private const float Delay = 5f;

        private void Update()
        {
            _timer += Time.deltaTime;
            if (_timer >= Delay && LocalPlayer.Player.isLocalPlayer)
            {
                LocalPlayer.Player.CmdUpdatePlayerDistance(Vector3.Distance(transform.position, new Vector3(0, 0, 0)));
                _timer = 0f;
            }
        }
    }
}