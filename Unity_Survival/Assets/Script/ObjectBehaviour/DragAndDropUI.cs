using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDropUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

    private ItemRect itemRect;
    private InventoryControler controler;

    public void Parameter ( InventoryControler _controler, ItemRect _itemRect) {
        itemRect = _itemRect;
        controler = _controler;
    }

    public void OnBeginDrag( PointerEventData eventData ) {
        controler.BeginDrag( gameObject, itemRect );
    }

    public void OnDrag( PointerEventData eventData ) {
        controler.UpdateDrag();
    }

    public void OnEndDrag( PointerEventData eventData ) {
        controler.EndDrag();
    }
}
