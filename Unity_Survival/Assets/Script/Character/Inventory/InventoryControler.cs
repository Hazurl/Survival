using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class InventoryControler : MonoBehaviour {
    #region LastVersion
    /*


    #region Inventory Panel Controler
    private Dictionary<string, GameObject> inventoryPanelRef = new Dictionary<string, GameObject>();
    private Vector2 offset = Vector3.zero;


    public void AddInventoryPanel ( GameObject _panel, ref Action<Inventory, ItemRect, Inventory.InventoryPosition> _OnAddingItem, ref Action<Inventory, ItemRect, Inventory.InventoryPosition> _OnRemovingItem ) {
        inventoryPanelRef.Add( _panel.name, _panel );

        //Callback
        _OnAddingItem += (_inventory, _item, _pos) => {
            if( !inventoryPanelRef.ContainsKey( _inventory.name ) ) {
                Debug.LogError( "Cannot add an item sprite, because the inventory (" + _inventory.name + ") is not register." );
                return;
            }

            //Create the sprite into the inventory Panel 
            GameObject _sprite = Instantiate( itemPrefab, inventoryPanelRef[ _inventory.name ].transform ) as GameObject;

            //Name
            _sprite.name = _item.id.ToString() + "_" + _pos.x + "_" + _pos.y;

            //Sprite
            _sprite.GetComponent<Image>().sprite = Resources.Load<Sprite>( "Items/" + _item.id.ToString() );

            RectTransform _rect = _sprite.GetComponent<RectTransform>();

            //Position
            _rect.anchoredPosition = new Vector3( _pos.x * SIZE_SLOT, -_pos.y * SIZE_SLOT, 0 );

            //Size
            _rect.SetSizeWithCurrentAnchors( RectTransform.Axis.Horizontal, SIZE_SLOT );
            _rect.SetSizeWithCurrentAnchors( RectTransform.Axis.Vertical, SIZE_SLOT );
        };
        _OnRemovingItem += ( _inventory, _item, _pos ) => {
            //RemoveItem( inventory, item, pos );
            if( !inventoryPanelRef.ContainsKey( _inventory.name ) ) {
                Debug.LogError( "Cannot remove an item sprite, because the inventory (" + _inventory.name + ") is not register." );
                return;
            }

            //Remove the Sprite
            Transform _curInv = inventoryPanelRef[ _inventory.name ].transform;
            for( int _indexChild = _curInv.childCount - 1; _indexChild >= 0; --_indexChild ) {
                Transform _curChild = _curInv.GetChild( _indexChild );
                if( _curChild.name == _item.id.ToString() + "_" + _pos.x + "_" + _pos.y ) {
                    Destroy( _curChild.gameObject );
                    return;
                }
            }
        };
    }

    public void RemoveInventoryPanel( GameObject _panel ) {
        if( !inventoryPanelRef.ContainsKey( _panel.name ) ) return;

        Destroy( inventoryPanelRef[ _panel.name ] );
        inventoryPanelRef.Remove( _panel.name );
    }

    public GameObject CreatePanel( string _nameIdentifier, Inventory _inventory, bool _DisplayOnDebugLog = false) {
        Inventory.InventorySpace _invSpace = _inventory.inventorySpace;
        ItemRect[,] _virtualInv = _inventory.virtualInventory;

        if( _DisplayOnDebugLog ) {
            #region Display On Debug.Log
            string _text = "Inventory : \n";
            for( int _x = 0; _x < _invSpace.x; _x++ ) {
                for( int _y = 0; _y < _invSpace.y; _y++ )
                    if( _virtualInv[ _x, _y ] == null )
                        _text += '▮';
                    else
                        _text += _virtualInv[ _x, _y ].uniqueId;
                _text += '\n';
            }
            Debug.Log( _text );
            #endregion
        }

        //The Panel which holding slots
        GameObject _panel = Instantiate( panelPrefab, targetPanel.transform ) as GameObject;
        _panel.name = _nameIdentifier;
        _panel.transform.localRotation = Quaternion.identity;
        _panel.GetComponent<RectTransform>().anchoredPosition = POS_DEFAULT_PANEL + offset;

        //Update offset
        offset.y -= SPACE_BETWEEN_PANEL + _invSpace.y * SIZE_SLOT;

        //Update targetPanel height
        targetPanel.GetComponent<RectTransform>().sizeDelta = new Vector2( 0, -offset.y - SPACE_BETWEEN_PANEL );

        //Update Scrollbar
        if( targetPanel.GetComponent<RectTransform>().rect.height > ACTIVE_SCROLLBAR_HEIGHT && targetPanel.activeInHierarchy) {
            scrollBar.SetActive( true );
        }
        else {
            scrollBar.SetActive( false );
        }

        //Slots        
        for( int _x = 0; _x < _invSpace.x; ++_x ) {
            for( int _y = 0; _y < _invSpace.y; ++_y ) {
                //Create the current Slot
                GameObject _slot = Instantiate( slotPrefab, _panel.transform ) as GameObject;
                RectTransform _rectSlot = _slot.GetComponent<RectTransform>();

                //Change her Position
                _rectSlot.anchoredPosition = new Vector3( SIZE_SLOT * _x, -SIZE_SLOT * _y, 0 );

                //Change Size
                _rectSlot.SetSizeWithCurrentAnchors( RectTransform.Axis.Horizontal, SIZE_SLOT );
                _rectSlot.SetSizeWithCurrentAnchors( RectTransform.Axis.Vertical, SIZE_SLOT );

                //Update the name just to have a nice hierarchy
                _slot.name = "Slot_" + _x + "_" + _y;
            }
        }

        return _panel;
    }
    #endregion
    */
    #endregion

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
    public GameObject targetPanel;

    [Space( 5 )]
    public GameObject scrollBar;

    [Space( 10 )]
    [Header( "Prefab used to build the inventory" )]
    public GameObject panelPrefab;
    public GameObject slotPrefab;
    public GameObject itemPrefab;

    void Start() {
        if( panelPrefab == null || slotPrefab == null || itemPrefab == null )
            Debug.LogError( "Inventory Prefab not initialized" );
        if( targetPanel == null )
            Debug.LogError( "Panel holding the inventory system isn't referenced" );
        if( scrollBar == null )
            Debug.LogError( "Inventory system need a reference to a Scroll Bar" );
        //Get the current hide state
        isHide = targetPanel.activeSelf;
    }
    #endregion

    #region Toggle Hide
    public bool isHide { get; private set; }

    public void HideInventory() {
        if( isHide )
            return;
        targetPanel.SetActive( true );
        isHide = true;

        if( targetPanel.GetComponent<RectTransform>().rect.height > ACTIVE_SCROLLBAR_HEIGHT && targetPanel.activeInHierarchy ) {
            scrollBar.SetActive( true );
        } else {
            scrollBar.SetActive( false );
        }

    }

    public void ShowInventory() {
        if( !isHide )
            return;
        targetPanel.SetActive( false );
        isHide = false;

        if( targetPanel.GetComponent<RectTransform>().rect.height > ACTIVE_SCROLLBAR_HEIGHT && targetPanel.activeInHierarchy ) {
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

    //Nouveaux attributs

    //      ( Cle : Valeur ) -> ( Inventaire : Panel ) 
    Dictionary<Inventory, GameObject> InventoriesPanels = new Dictionary<Inventory, GameObject>();
    private Vector2 offset = Vector3.zero;

    //Nouvelle methodes :
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

        //Create the button component
        Button button = _sprite.AddComponent<Button>();

        button.onClick.AddListener( () => {
            RemoveSpriteOnPanel( _sprite );
        } );
    }

    public void RemoveItemOnPanel( Inventory _inventory, ItemRect _itemRect ) {
        GameObject _panel;

        if( !InventoriesPanels.TryGetValue( _inventory, out _panel ) )
            return;

        //TODO : for optimization hold in memory a list for each panel or inventory the list of the Item's sprite

        for (int index = _panel.transform.childCount - 1; index >= 0 ; --index ) {
            Transform child = _panel.transform.GetChild( index );
            if( child.name == _itemRect.data.Id.ToString() + "_" + _itemRect.X + "_" + _itemRect.Y ) {
                //Remove it
                Destroy( child );
                return;
            }
        }

    }

    public void RemoveSpriteOnPanel (GameObject _itemSprite) {
        Debug.Log( "Try to remove : " + _itemSprite.name );
    }

    public void CreatePanel( Inventory _inventory ) {
        CreatePanel( _inventory, targetPanel );
    }

    public void CreatePanel( Inventory _inventory, GameObject target) {
        //The Panel which holding slots
        GameObject _panel = Instantiate( panelPrefab, target.transform ) as GameObject;
        _panel.name = targetPanel.name + targetPanel.transform.childCount.ToString();
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
}
