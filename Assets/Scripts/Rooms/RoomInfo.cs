﻿using UnityEngine;

namespace Rooms
{
    [CreateAssetMenu(fileName = "RoomInfo", menuName = "Room", order = 0)]
    public class RoomInfo : ScriptableObject
    {
        public string RoomId;
        public GameObject RoomPrefab;
    }
}