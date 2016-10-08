using UnityEngine;
using System;
using System.Collections.Generic;

public class Inventory
{
    #region Attributs
    //This is the display Inventory
    private Item[, ] virtualInventory;
    //The List of Item in the inventory, because a Item can take more than one space
    private Dictionary<int, InventoryPosition> itemPosition;

    public readonly InventorySpace inventorySpace;
    #endregion

    #region Constante
    private const int SIZE_SLOT = 50;
    #endregion

    #region staticAttributs
    private static Dictionary<string, GameObject> inventoryPanels = new Dictionary<string, GameObject>();
    #endregion

    #region Constructors
    /// <summary>
    /// Constructors
    /// </summary>
    /// <param name="Space">The inventorySpace of the inventory, this is a class from <see cref="Inventory.InventorySpace"/></param>
    /// <param name="inventory">The inventory copied in the current, it must be smaller or of the same size</param>
    public Inventory (InventorySpace Space, Inventory inventory = null, bool addInventory = false, string name = "defaultName") {
        //I don't want to have a negative array, an inventory must be more or equals than 1 * 1 array
        if( Space.x < 1 || Space.y < 1 ) throw new Exception( "Inventory can't have a negative space" );

        //Initialize Attribut
        this.inventorySpace = Space;
        virtualInventory = new Item[ Space.x, Space.y ];
        itemPosition = new Dictionary<int, InventoryPosition>( Space.Lenght );

        if( inventory == null ) return;
        //Add Some Start Item in the inventory
        //But we can't do this if the space is less than this inventory
        if( Space.x < inventory.inventorySpace.x || Space.y < inventory.inventorySpace.y ) return;

        for( int i = 0; i < inventory.inventorySpace.x; ++i )
            for( int j = 0; j < inventory.inventorySpace.y; ++j )
                virtualInventory[ i, j ] = inventory.virtualInventory[ i, j ];

        //Add Inventory into the inventory Panel
        if( !addInventory ) return;

        AddInventoryPanel( CreateInventoryPanel(name) );
    }
	#endregion

	#region Methods
	/// <summary>
	/// Add an item into the inventory
	/// </summary>
	/// <param name="item">The Item to put it in</param>
	/// <returns>Return True if it can be put else return false</returns>
	public bool AddItem( Item item ) {
		InventoryPosition pos = FindPositionFor( item.spaceRequired );

		if( pos == null )
			return false;

		for( int i = pos.x; i < item.spaceRequired.x + pos.x; i++ )
			for( int j = pos.y; j < item.spaceRequired.y + pos.y; j++ )
				virtualInventory[ i, j ] = item;

		itemPosition[ item.uniqueId ] = pos;
		//Debug.Log( "Add " + item.id.ToString() + " at position (" + pos.x.ToString() + ", " + pos.y.ToString() + ")" + 
        //    " - Space (" + item.spaceRequired.x + ", " + item.spaceRequired.y+")");
		return true;
	}

	/// <summary>
	/// Remove an Item of this inventory
	/// </summary>
	/// <param name="item">The Item to remve</param>
	/// <returns>Retrn the success of this action</returns>
	public bool RemoveItem ( Item item ) {
		InventoryPosition pos;
		if((pos = itemPosition[ item.uniqueId ]) == null )
			return false;

		for( int i = pos.x; i < item.spaceRequired.x + pos.x; i++ )
			for( int j = pos.y; j < item.spaceRequired.y + pos.y; j++ )
				virtualInventory[ i, j ] = null;

		itemPosition.Remove( item.uniqueId );
		return true;
	}

	/// <summary>
	/// Method to search and find a space to pt an Item in the inventory
	/// TODO: Opti !
	/// </summary>
	/// <param name="space">The space of the item</param>
	/// <returns>The position of the item or null if he can't</returns>
	private InventoryPosition FindPositionFor( InventorySpace space ) {
		//Return null if the Inventory space is smaller than the item
		if( inventorySpace.x < space.x || inventorySpace.y < space.y )
			return null;

		for (int i = 0; i < inventorySpace.x - space.x + 1;  i++)
			for (int j = 0; j < inventorySpace.y - space.y + 1;  j++) {
				bool PosOk = true;
                for( int _i = 0; _i < space.x; _i++ ) {
                    for( int _j = 0; _j < space.y; _j++ ) {
                        if( virtualInventory[ _i + i, _j + j ] != null ) {
                            PosOk = false;
                            j += _j;
                            break;
                        }
                    }
                    if( !PosOk ) break;
                }
                    
				if( PosOk )
					return new InventoryPosition( i, j );
			}
        //if we are here, It's beacause there is no space for this Item
        Debug.LogError( "No Space for " + space.x + " : " + space.y );
		return null;
	}

    /// <summary>
    /// Display On screen The inventory
    /// </summary>
	public GameObject CreateInventoryPanel (string name) {
        #region Debug.Log
        string text = "Inventory : \n";
		for( int i = 0; i < inventorySpace.x; i++ ) {
			for( int j = 0; j < inventorySpace.y; j++ )
				if( virtualInventory[ i, j ] == null )
					text += '▮';
				else
					text += virtualInventory[i, j].uniqueId;
			text += '\n';
		}
		Debug.Log( text );
        #endregion
        #region OnScreen 
        //The Panel which holding slots
        GameObject panel = GameObject.Instantiate( Global.SlotsPanel, Global.InventoryPanel.transform ) as GameObject;
        panel.name = name;

        //Slots
        Vector3 offsetCornerUpperRight = new Vector3( 0, panel.GetComponent<RectTransform>().offsetMax.y );
        
        for (int x = 0; x < inventorySpace.x; ++x ) {
            for (int y = 0; y < inventorySpace.y; ++y ) {
                //Create the current Slot
                GameObject slot = GameObject.Instantiate( Global.Slot, panel.transform ) as GameObject;

                //Change her Position
                slot.transform.localPosition = new Vector3( SIZE_SLOT * x, -SIZE_SLOT * y, 0 ) + offsetCornerUpperRight;

                //Change Size (must be a size of 100 in the slot prefab)
                slot.GetComponent<RectTransform>().localScale = new Vector3( SIZE_SLOT / 100, SIZE_SLOT / 100, 0 );

                //Update the name just to have a nice hierarchy
                slot.name = "Slot_" + x + "_" + y;
           } 
        }

        return panel;
        #endregion
    }
    #endregion

    #region static
    public static void DisplayInventory () {
        Global.InventoryPanel.SetActive( true );
    }

    public static void HideInventory() {
        Global.InventoryPanel.SetActive( false );
    }

    public static void ToggleInventory () {
        Global.InventoryPanel.SetActive( !Global.InventoryPanel.activeSelf );
    }

    private static void AddInventoryPanel (GameObject panel) {
        inventoryPanels.Add( panel.name, panel );
        panel.transform.parent = Global.InventoryPanel.transform;
    }

    private static void RemoveInventoryPanel( string name ) {
        if( !inventoryPanels.ContainsKey( name ) ) return;

        GameObject.Destroy( inventoryPanels[ name ] );
        inventoryPanels.Remove( name );
    }
    #endregion

    #region Structures
    /// <summary>
    /// A structure to represent the space maximum of an inventory
    /// </summary>
    public struct InventorySpace {
        #region Attributs
        public readonly int x, y;
        public int Lenght { get { return x * y; } }
        #endregion

        #region Constructors
        public InventorySpace(int x, int y) {
            this.x = x;
            this.y = y;
        }
        #endregion
    }
	#endregion

	#region Class
	/// <summary>
	/// A Class to represent a position in an inventory
	/// <para>Not a structure beacause can be null</para>
	/// </summary>
	private class InventoryPosition {
		#region Attributs
		public readonly int x, y;
		#endregion

		#region Constructors
		public InventoryPosition( int x, int y ) {
			this.x = x;
			this.y = y;
		}
		#endregion
	}

	#endregion
}
