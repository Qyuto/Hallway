using System.Collections.Generic;
using Item;
using Mirror;
using UnityEngine;

public class OnlineItemDatabase : MonoBehaviour // Only server/host side
{
    [SerializeField] private List<ItemInfo> dataItem;

    public static OnlineItemDatabase ItemDatabase;
    private void Start()
    {
        if(ItemDatabase != null) Destroy(this);
        ItemDatabase = this;
        foreach (var item in dataItem)
        {
            if(!NetworkManager.singleton.spawnPrefabs.Contains(item.Prefab))
                NetworkManager.singleton.spawnPrefabs.Add(item.Prefab);
        }
    }

    public ItemInfo GetItem(string itemId)
    {
        return dataItem.Find((info => info.ItemId == itemId));
    }
}
