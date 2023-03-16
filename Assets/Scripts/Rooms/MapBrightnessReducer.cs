using Mirror;
using UnityEngine;

namespace Rooms
{
    public class MapBrightnessReducer : NetworkBehaviour
    {
        private const float minBrightness = 0.05f;
        
        private Light[] _roomLights;
        private void Awake()
        {
            _roomLights = GetComponentsInChildren<Light>();
        }

        [Command (requiresAuthority = false)]
        public void CmdChangeBrightness(float currentDistance)
        {
            UpdateClientBrightness(currentDistance);
        }

        [ClientRpc]
        private void UpdateClientBrightness(float currentDistance)
        {
            float newIntensity = 1 - (currentDistance * 0.001f);
            Debug.Log($"UpdateClientBrightness: {newIntensity}");
            if (newIntensity < minBrightness)
                newIntensity = minBrightness;
            
            foreach (var light in _roomLights)
                light.intensity = newIntensity;
        }
    }
}