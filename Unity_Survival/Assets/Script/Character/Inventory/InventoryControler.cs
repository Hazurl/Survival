using UnityEngine;
using System.Collections.Generic;

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
    }
    #endregion

    #region Toggle Hide
    public bool isHide { get; private set; }

    public void HideInventory () {
        if( isHide )
            return;
        targetPanel.SetActive( false );
    }

    public void ShowInventory() {
        if( !isHide )
            return;
        targetPanel.SetActive( true );
    }

    public void ToggleInventory() {
        if( isHide )
            ShowInventory();
        else
            HideInventory();
    }

    #endregion

    #region Add / Remove inventory Panel
    private List<GameObject> listInventory = new List<GameObject>();

    public void AddInventoryPanel ( GameObject panel ) {
        
    }

    public void RemoveInventoryPanel( GameObject panel ) {

    }

    #endregion
}
