using System.Collections.Generic;
using Item;
using Mirror;
using UnityEngine;

public class OnlineItemDatabase : MonoBehaviour // Only server/host side
{
    [SerializeField] private List<ItemInfo> dataItem;

    public static OnlineItemDatabase ItemDatabase;

    private void Awake()
    {
        if(ItemDatabase != null) Destroy(this);
        ItemDatabase = this;
        NetworkManager manager = GetComponent<NetworkManager>();
        
        
        foreach (var item in dataItem)
        {
            if(!manager.spawnPrefabs.Contains(item.Prefab))
                manager.spawnPrefabs.Add(item.Prefab);
        }
    }

    public ItemInfo GetItem(string itemId)
    {
        return dataItem.Find((info => info.ItemId == itemId));
    }
}
