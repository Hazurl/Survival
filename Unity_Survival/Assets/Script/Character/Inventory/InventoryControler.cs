using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;

[DisallowMultipleComponent]
public class InventoryControler : MonoBehaviour {

    #region InventoryControlerInstance
    public static InventoryControler instance;
    void Awake() {
        if( instance != null ) {
            Debug.LogError( "There is more than 1 instance of InventoryControler !" );
            return;
        }
        instance = this;
    }
    #endregion

    #region Constante
    [Space( 10 )]
    [Header( "Constante pour graphique :" )]
    [SerializeField]
    private int PANEL_SIZE = 50;
    [SerializeField]
    private int SPACE_BETWEEN_PANEL = 50;
    [SerializeField]
    private Vector2 POS_DEFAULT_PANEL = Vector2.zero;
    [SerializeField]
    private int ACTIVE_SCROLLBAR_HEIGHT = 300;
    #endregion

    #region Preset
    [Header( "Where the inventory will be display" )]
    public Transform targetItemOnDragPanel;
    public GameObject targetInventoryPanel;
    public GameObject scrollBar;


    [Space( 10 )]
    [Header( "Prefab used to build the inventory" )]
    public GameObject panelPrefab;
    public GameObject slotPrefab;
    public GameObject itemPrefab;

    void Start() {
        if( panelPrefab == null || slotPrefab == null || itemPrefab == null )
            Debug.LogError( "Inventory Prefab not initialized" );
        if( targetInventoryPanel == null )
            Debug.LogError( "Panel holding the inventory system isn't referenced" );
        if( scrollBar == null )
            Debug.LogError( "Inventory system need a reference to a Scroll Bar" );
        //Get the current hide state
        isHide = targetInventoryPanel.activeSelf;
    }

    #endregion

    #region Toggle Hide
    public bool isHide { get; private set; }

    public void HideInventory() {
        if( isHide )
            return;
        targetInventoryPanel.SetActive( true );
        isHide = true;

        if( targetInventoryPanel.GetComponent<RectTransform>().rect.height > ACTIVE_SCROLLBAR_HEIGHT && targetInventoryPanel.activeInHierarchy ) {
            scrollBar.SetActive( true );
        } else {
            scrollBar.SetActive( false );
        }

    }

    public void ShowInventory() {
        if( !isHide )
            return;
        targetInventoryPanel.SetActive( false );
        isHide = false;

        if( targetInventoryPanel.GetComponent<RectTransform>().rect.height > ACTIVE_SCROLLBAR_HEIGHT && targetInventoryPanel.activeInHierarchy ) {
            scrollBar.SetActive( true );
            scrollBar.GetComponent<Scrollbar>().size = 1;
        } else {
            scrollBar.SetActive( false );
        }

    }

    public void ToggleInventory() {
        if( isHide )
            ShowInventory();
        else
            HideInventory();
    }

    #endregion

    //      ( Cle : Valeur ) -> ( Inventaire : Panel ) 
    private Dictionary<Inventory, GameObject> InventoriesPanels = new Dictionary<Inventory, GameObject>();
    private Vector2 offset = Vector3.zero;

    #region Add or remove Item from inventory panel
    public void AddItemOnPanel( Inventory _inventory, ItemRect _itemRect ) {
        GameObject _panel;

        if( !InventoriesPanels.TryGetValue( _inventory, out _panel ) )
            return;

        Debug.Log( "Display it !" );

        //Create the sprite into the inventory Panel 
        GameObject _sprite = Instantiate( itemPrefab, _panel.transform ) as GameObject;

        //Name
        string _name = _itemRect.data.Id.ToString();
        _sprite.name = _name + "_" + _itemRect.X + "_" + _itemRect.Y;

        //Sprite
        _sprite.GetComponent<Image>().sprite = Resources.Load<Sprite>( "Items/" + _name );

        RectTransform _rect = _sprite.GetComponent<RectTransform>();

        //Position
        _rect.anchoredPosition = new Vector3( _itemRect.X * PANEL_SIZE, -_itemRect.Y * PANEL_SIZE, 0 );

        //Size
        _rect.SetSizeWithCurrentAnchors( RectTransform.Axis.Horizontal, PANEL_SIZE * _itemRect.Width );
        _rect.SetSizeWithCurrentAnchors( RectTransform.Axis.Vertical, PANEL_SIZE * _itemRect.Height );
        
        //DragComponent
        _sprite.AddComponent<DragAndDropUI>().Parameter( this, _itemRect );

    }

    public void RemoveItemOnPanel( Inventory _inventory, ItemRect _itemRect ) {
        Debug.Log( "Try to remove : " + _itemRect.ToString() + " from : " + _inventory.ToString());
        GameObject _panel;

        if( !InventoriesPanels.TryGetValue( _inventory, out _panel ) )
            return;

        //TODO : for optimization hold in memory a list for each panel or inventory the list of the Item's sprite

        for (int index = _panel.transform.childCount - 1; index >= 0 ; --index ) {
            Transform child = _panel.transform.GetChild( index );
            if( child.name == _itemRect.data.Id.ToString() + "_" + _itemRect.X + "_" + _itemRect.Y ) {
                //Remove it
                Destroy( child.gameObject );
                return;
            }
        }

    }

    public bool TryDropItemOn( GameObject _target, DragAndDropUI _itemToDrop ) {
        Inventory _inv = InventoriesPanels.FirstOrDefault( x => x.Value == _target ).Key;

        if( _inv == null )
            return false;

        Debug.Log( "Try to drop at " + _itemToDrop.itemRect.rect.position );
        if( _inv.AddItem(_itemToDrop.itemRect)) {
            return true;
        }

        return false;
    }

    #endregion

    #region CreatePanel
    public void CreatePanel( Inventory _inventory ) {
        CreatePanel( _inventory, targetInventoryPanel );
    }

    public void CreatePanel( Inventory _inventory, GameObject target) {
        //The Panel which holding slots
        GameObject _panel = Instantiate( panelPrefab, target.transform ) as GameObject;
        _panel.name = targetInventoryPanel.name + '_' + targetInventoryPanel.transform.childCount.ToString();
        _panel.transform.localRotation = Quaternion.identity;
        _panel.GetComponent<RectTransform>().anchoredPosition = POS_DEFAULT_PANEL + offset;

        InventoriesPanels.Add( _inventory, _panel );
        
        //Update offset
        offset.y -= SPACE_BETWEEN_PANEL + _inventory.Height * PANEL_SIZE;

        //Update targetPanel height
        target.GetComponent<RectTransform>().sizeDelta = new Vector2( 0, -offset.y - SPACE_BETWEEN_PANEL );
        
        //Update Scrollbar
        if( target.GetComponent<RectTransform>().rect.height > ACTIVE_SCROLLBAR_HEIGHT && target.activeInHierarchy ) {
            scrollBar.SetActive( true );
        } else {
            scrollBar.SetActive( false );
        }

        //Create the current Slot
        GameObject _slot = Instantiate( slotPrefab, _panel.transform ) as GameObject;
        RectTransform _rectSlot = _slot.GetComponent<RectTransform>();

        //Change her Position
        _rectSlot.anchoredPosition = Vector3.zero;

        //Change Size
        _rectSlot.SetSizeWithCurrentAnchors( RectTransform.Axis.Horizontal, _inventory.Width * PANEL_SIZE );
        _rectSlot.SetSizeWithCurrentAnchors( RectTransform.Axis.Vertical, _inventory.Height * PANEL_SIZE );
    }
    #endregion

    #region OnDrag
    
    private GameObject onDragSprite;
    private ItemRect onDragItemRect;
    private Inventory lastContainer;

    private Vector2 dragOffset = Vector2.zero;

    public void BeginDrag( GameObject _itemSprite, ItemRect _itemrect, PointerEventData e ) {
        Debug.Log( "BeginDrag !" );

        dragOffset = (Vector2)_itemSprite.transform.position - e.position;

        onDragItemRect = _itemrect;
        onDragSprite = _itemSprite;
        _itemSprite.transform.SetParent( targetItemOnDragPanel );

        lastContainer = _itemrect.InventoryContainer;
        _itemrect.InventoryContainer = null;
        /*
         _itemSprite.SetActive( true );
         _itemSprite.transform.SetParent( targetItemOnDragPanel );
         _itemSprite.transform.localPosition = Input.mousePosition;*/
    }

    public void UpdateDrag ( PointerEventData e  ) {
        onDragSprite.transform.position = e.position + dragOffset;
    }

    public void EndDrag( PointerEventData e ) {
        Debug.Log( "EndDrag !" );

        GameObject _targetPanel = null;
        Inventory _targetInv = null;

        List<RaycastResult> _outputRaycast = new List<RaycastResult>();
        EventSystem.current.RaycastAll( e, _outputRaycast );

        foreach( RaycastResult _ray in _outputRaycast ) {
            GameObject curPanel = _ray.gameObject;
            Inventory _inv = InventoriesPanels.FirstOrDefault( x => x.Value == curPanel ).Key;
            if( _inv != null ) {
                _targetInv = _inv;
                _targetPanel = curPanel;
                break;
            }
        }

        if( _targetPanel != null ) {
            Vector3 _offsetPos = onDragSprite.transform.position - _targetPanel.transform.position;
            _offsetPos /= PANEL_SIZE;
            Debug.Log( "_offsetPos : " + _offsetPos );

        }
        /*onDragItemRect.InventoryContainer = lastContainer;

        onDragSprite.transform.SetParent( InventoriesPanels[ lastContainer ].transform );

        onDragSprite.GetComponent<RectTransform>().anchoredPosition = new Vector3( onDragItemRect.X * PANEL_SIZE, -onDragItemRect.Y * PANEL_SIZE, 0 );
        */
    }

    #endregion
}
