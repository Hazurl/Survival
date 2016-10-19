using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDropUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

    public ItemRect itemRect { get; private set; }
    public InventoryControler controler { get; private set; }
    public Transform dragPanel { get; private set; }

    private bool wasParametered = false;

    public void Parameter ( InventoryControler _controler, ItemRect _itemRect, Transform _dragPanel) {
        itemRect = _itemRect;
        controler = _controler;
        dragPanel = _dragPanel;

        wasParametered = true;
    }

    private Vector3 lastPosition;
    private Transform lastParent;
    private Vector2 lastPositionRect;
    private Vector3 offset = Vector3.zero;

    public void OnBeginDrag( PointerEventData eventData ) {
        if( !wasParametered )
            return;

        lastPosition = transform.position;
        lastParent = transform.parent;
        lastPositionRect = itemRect.rect.position;
        transform.SetParent( dragPanel );

        offset = transform.position - (Vector3)eventData.position;
    }

    public void OnDrag( PointerEventData eventData ) {
        if( !wasParametered )
            return;

        transform.position = (Vector3)eventData.position + offset;

    }

    public void OnEndDrag( PointerEventData eventData ) {
        if( !wasParametered )
            return;

        List<RaycastResult> _outputRaycast = new List<RaycastResult>();
        EventSystem.current.RaycastAll( eventData, _outputRaycast );

        foreach (RaycastResult _ray in _outputRaycast) {
            if( controler.TryDropItemOn( _ray.gameObject, this ) ) {

            }
        }

        transform.position = lastPosition;
        transform.SetParent( lastParent );
        itemRect.rect.position = lastPositionRect;
    }
}
