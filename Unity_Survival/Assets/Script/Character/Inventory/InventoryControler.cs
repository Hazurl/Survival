using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
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

    #region Preset
    [Header("Where the inventory will be display")]
    public GameObject targetPanel;

    [Space( 5 )]
    public GameObject scrollBar;

    [Space(10)]
    [Header("Prefab used to build the inventory")]
    public GameObject panelPrefab;
    public GameObject slotPrefab;
    public GameObject itemPrefab;

    void Start () {
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

    public void HideInventory () {
        if( isHide )
            return;
        targetPanel.SetActive( true );
        isHide = true;

        if( targetPanel.GetComponent<RectTransform>().rect.height > ACTIVE_SCROLLBAR_HEIGHT && targetPanel.activeInHierarchy ) {
            scrollBar.SetActive( true );
            //scrollBar.GetComponent<Scrollbar>().size = 1;
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

    #region Inventory Panel Controler
    private Dictionary<string, GameObject> inventoryPanelRef = new Dictionary<string, GameObject>();
    private Vector2 offset = Vector3.zero;

    #region Constante
    [ Space(10)]
    [Header("Constante pour graphique :")]
    [SerializeField]
    private int SIZE_SLOT = 50;
    [SerializeField]
    private int SPACE_BETWEEN_PANEL = 50;
    [SerializeField]
    private Vector2 POS_DEFAULT_PANEL = Vector2.zero;
    [SerializeField]
    private int ACTIVE_SCROLLBAR_HEIGHT = 300;
    #endregion

    public void AddInventoryPanel ( GameObject panel, ref Action<Inventory, Item, Inventory.InventoryPosition> OnAddingItem, ref Action<Inventory, Item, Inventory.InventoryPosition> OnRemovingItem ) {
        inventoryPanelRef.Add( panel.name, panel );

        //Callback
        OnAddingItem += (inventory, item, pos) => {
            //AddItem( inventory, item, pos );
            if( !inventoryPanelRef.ContainsKey( inventory.name ) ) {
                Debug.LogError( "Cannot add an item sprite, because the inventory (" + inventory.name + ") is not register." );
                return;
            }

            //Create the sprite into the inventory Panel 
            GameObject sprite = Instantiate( itemPrefab, inventoryPanelRef[ inventory.name ].transform ) as GameObject;

            //Name
            sprite.name = item.id.ToString() + "_" + pos.x + "_" + pos.y;

            //Sprite
            sprite.GetComponent<Image>().sprite = Resources.Load<Sprite>( "Items/" + item.id.ToString() );

            RectTransform rect = sprite.GetComponent<RectTransform>();

            //Position
            rect.anchoredPosition = new Vector3( pos.x * SIZE_SLOT, -pos.y * SIZE_SLOT, 0 );

            //Size
            rect.SetSizeWithCurrentAnchors( RectTransform.Axis.Horizontal, SIZE_SLOT );
            rect.SetSizeWithCurrentAnchors( RectTransform.Axis.Vertical, SIZE_SLOT );
        };
        OnRemovingItem += ( inventory, item, pos ) => {
            //RemoveItem( inventory, item, pos );
            if( !inventoryPanelRef.ContainsKey( inventory.name ) ) {
                Debug.LogError( "Cannot remove an item sprite, because the inventory (" + inventory.name + ") is not register." );
                return;
            }

            //Remove the Sprite
            Transform curInv = inventoryPanelRef[ inventory.name ].transform;
            for( int i = curInv.childCount - 1; i >= 0; --i ) {
                Transform curChild = curInv.GetChild( i );
                if( curChild.name == item.id.ToString() + "_" + pos.x + "_" + pos.y ) {
                    Destroy( curChild.gameObject );
                    return;
                }
            }
        };
    }

    public void RemoveInventoryPanel( GameObject panel ) {
        if( !inventoryPanelRef.ContainsKey( name ) ) return;

        Destroy( inventoryPanelRef[ name ] );
        inventoryPanelRef.Remove( name );
    }

    public GameObject CreatePanel( string nameIdentifier, Inventory inventory, bool DisplayOnDebugLog = false) {
        Inventory.InventorySpace invSpace = inventory.inventorySpace;
        Item[,] virtualInv = inventory.virtualInventory;

        #region Display On Debug.Log
        if( DisplayOnDebugLog ) {
            string text = "Inventory : \n";
            for( int i = 0; i < invSpace.x; i++ ) {
                for( int j = 0; j < invSpace.y; j++ )
                    if( virtualInv[ i, j ] == null )
                        text += '▮';
                    else
                        text += virtualInv[ i, j ].uniqueId;
                text += '\n';
            }
            Debug.Log( text );
        }
        #endregion

        //The Panel which holding slots
        GameObject panel = Instantiate( panelPrefab, targetPanel.transform ) as GameObject;
        panel.name = nameIdentifier;
        panel.transform.localRotation = Quaternion.identity;
        panel.GetComponent<RectTransform>().anchoredPosition = POS_DEFAULT_PANEL + offset;

        //Update offset
        offset.y -= SPACE_BETWEEN_PANEL + invSpace.y * SIZE_SLOT;

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
        for( int x = 0; x < invSpace.x; ++x ) {
            for( int y = 0; y < invSpace.y; ++y ) {
                //Create the current Slot
                GameObject slot = Instantiate( slotPrefab, panel.transform ) as GameObject;
                RectTransform rectSlot = slot.GetComponent<RectTransform>();

                //Change her Position
                rectSlot.anchoredPosition = new Vector3( SIZE_SLOT * x, -SIZE_SLOT * y, 0 );

                //Change Size
                rectSlot.SetSizeWithCurrentAnchors( RectTransform.Axis.Horizontal, SIZE_SLOT );
                rectSlot.SetSizeWithCurrentAnchors( RectTransform.Axis.Vertical, SIZE_SLOT );

                //Update the name just to have a nice hierarchy
                slot.name = "Slot_" + x + "_" + y;
            }
        }

        return panel;
    }
    #endregion

    #region Item OnChange
    void AddItem ( Inventory inventory, Item item , Inventory.InventoryPosition pos) {
        if (!inventoryPanelRef.ContainsKey(inventory.name)) {
            Debug.LogError( "Cannot add an item sprite, because the inventory (" + inventory.name + ") is not register." );
            return;
        }

        //Create the sprite into the inventory Panel 
        GameObject sprite = Instantiate( itemPrefab, inventoryPanelRef[ inventory.name ].transform ) as GameObject;

        //Name
        sprite.name = item.id.ToString() + "_" + pos.x + "_" + pos.y;

        //Sprite
        sprite.GetComponent<Image>().sprite = Resources.Load<Sprite>( "Items/" + item.id.ToString() );

        RectTransform rect = sprite.GetComponent<RectTransform>();

        //Position
        rect.anchoredPosition = new Vector3( pos.x * SIZE_SLOT, -pos.y * SIZE_SLOT, 0 );

        //Size
        rect.SetSizeWithCurrentAnchors( RectTransform.Axis.Horizontal, SIZE_SLOT );
        rect.SetSizeWithCurrentAnchors( RectTransform.Axis.Vertical, SIZE_SLOT );
    }

    void RemoveItem( Inventory inventory, Item item, Inventory.InventoryPosition pos ) {
        if( !inventoryPanelRef.ContainsKey( inventory.name ) ) {
            Debug.LogError( "Cannot remove an item sprite, because the inventory (" + inventory.name + ") is not register." );
            return;
        }

        //Remove the Sprite
        Transform curInv = inventoryPanelRef[ inventory.name ].transform;
        for( int i = curInv.childCount - 1; i >= 0; --i ) {
            Transform curChild = curInv.GetChild( i );
            if( curChild.name == item.id.ToString() + "_" + pos.x + "_" + pos.y ) {
                Destroy( curChild.gameObject );
                return;
            }
        }
    }

    #endregion
}
