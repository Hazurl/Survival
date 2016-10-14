using UnityEngine;
using System.Collections;

public class DroppedItem : MonoBehaviour {

    [SerializeField]
    private ItemData itemData;

    public ItemData TakeItem () {
        return itemData;
    }
}
