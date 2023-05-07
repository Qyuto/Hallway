using UnityEngine;

namespace LocalLobby
{
    [CreateAssetMenu(fileName = "MonitorTextInfo", menuName = "Room/MonitorText", order = 0)]
    public class MonitorTextInfo : ScriptableObject
    {
        public static readonly char[] Symbols = new[] { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0','/','`','d','n','a','z','u','k','x','[',']' };

        [SerializeField] private string nameSender = "Unknown";
        [SerializeField] private string text;

        public string NameSender => nameSender;
        public string Text => text;
    }
}