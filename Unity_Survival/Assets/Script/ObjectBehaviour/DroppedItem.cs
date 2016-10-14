using UnityEngine;
using System.Collections;

public class DroppedItem : MonoBehaviour {

    [SerializeField]
    private ItemData itemData;

    public ItemData TakeItem () {
        return itemData;
    }

    public void OnMouseDown () {
        Debug.Log( "We click on this dropped item !" );
    }
}
