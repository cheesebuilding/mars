using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ItemType{
    None,
    Iron
}
public class InventoryManager: MonoBehaviour
{
    public int itemsInInventory = 0;
    public ItemType itemType;
    public int maxItemsInInventory = 10;
    

}
