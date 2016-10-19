using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDropUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

    public ItemRect itemRect { get; private set; }
    public InventoryControler controler { get; private set; }

    private bool wasParametered = false;

    public void Parameter ( InventoryControler _controler, ItemRect _itemRect) {
        itemRect = _itemRect;
        controler = _controler;

        wasParametered = true;
    }

    private Vector3 lastPosition;
    private Vector2 lastPositionRect;
    private Vector3 offset = Vector3.zero;

    public void OnBeginDrag( PointerEventData eventData ) {
        if( !wasParametered )
            return;


        controler.BeginDrag( this.gameObject, itemRect, eventData );
        /*
        lastPosition = transform.position;
        lastPositionRect = itemRect.rect.position;

        offset = transform.position - (Vector3)eventData.position;*/
    }

    public void OnDrag( PointerEventData eventData ) {
        if( !wasParametered )
            return;

        controler.UpdateDrag(eventData );
        /*
        transform.position = (Vector3)eventData.position + offset;*/
    }

    public void OnEndDrag( PointerEventData eventData ) {
        if( !wasParametered )
            return;

        controler.EndDrag( eventData );
        /*
        List<RaycastResult> _outputRaycast = new List<RaycastResult>();
        EventSystem.current.RaycastAll( eventData, _outputRaycast );

        foreach (RaycastResult _ray in _outputRaycast) {
            //Debug.Log( _ray.gameObject.name + " has been raycasted !" );
            itemRect.rect.position = transform.position - lastPosition;
            if( controler.TryDropItemOn( _ray.gameObject, this ) ) {
                Debug.Log( "drop on " + _ray.gameObject.name );
                return ;
            }
        }
        Debug.Log( "My last rectPosition " + lastPositionRect );
        transform.position = lastPosition;
        itemRect.rect.position = lastPositionRect;*/
    }
}
