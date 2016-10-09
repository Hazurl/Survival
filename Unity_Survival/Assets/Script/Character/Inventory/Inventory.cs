using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

public class Inventory
{
    #region Attributs
    //This is the display Inventory
    private Item[, ] virtualInventory;
    //The List of Item in the inventory, because a Item can take more than one space
    private Dictionary<int, InventoryPosition> itemPosition;

    public readonly InventorySpace inventorySpace;
    public string name;
    #endregion

    #region Constante
    private const int SIZE_SLOT = 50;
    #endregion

    #region staticAttributs
    private static Dictionary<string, GameObject> inventoryPanels = new Dictionary<string, GameObject>();
    private static Dictionary<string, Inventory> inventoryName = new Dictionary<string, Inventory>();
    #endregion

    #region Constructors
    /// <summary>
    /// Constructors
    /// </summary>
    /// <param name="Space">The inventorySpace of the inventory, this is a class from <see cref="Inventory.InventorySpace"/></param>
    /// <param name="inventory">The inventory copied in the current, it must be smaller or of the same size</param>
    public Inventory (InventorySpace Space, string name, Inventory inventory = null, bool showInventory = false) {
        //I don't want to have a negative array, an inventory must be more or equals than 1 * 1 array
        if( Space.x < 1 || Space.y < 1 ) throw new Exception( "Inventory can't have a negative space" );

        //Initialize Attribut
        this.inventorySpace = Space;
        virtualInventory = new Item[ Space.x, Space.y ];
        itemPosition = new Dictionary<int, InventoryPosition>( Space.Lenght );
        this.name = name;

        if( inventory != null ) {
            //Add Some Start Item in the inventory
            //But we can't do this if the space is less than this inventory
            if( Space.x < inventory.inventorySpace.x || Space.y < inventory.inventorySpace.y ) return;

            for( int i = 0; i < inventory.inventorySpace.x; ++i )
                for( int j = 0; j < inventory.inventorySpace.y; ++j )
                    virtualInventory[ i, j ] = inventory.virtualInventory[ i, j ];
        }

        //Add Inventory into the inventory Panel
        if( !showInventory ) return;
        AddInventoryPanel( CreateInventoryPanel(name) );
        inventoryName.Add( name, this );
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

        //Create the sprite into the inventory Panel :
        GameObject sprite = GameObject.Instantiate( InventoryControler.instance.itemPrefab , inventoryPanels[ name ].transform ) as GameObject;

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

        //Remove the Sprite
        GameObject curInv = inventoryPanels[ name ];
        for(int i = curInv.transform.childCount - 1; i >= 0; --i ) {
            Transform curChild = curInv.transform.GetChild( i );
            if ( curChild.name == item.id.ToString() + "_" + pos.x + "_" + pos.y ) {
                GameObject.Destroy( curChild.gameObject );
                return true;
            }
        }

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

        for( int j = 0; j < inventorySpace.y - space.y + 1; j++ ) {
            for( int i = 0; i < inventorySpace.x - space.x + 1; i++ ) {
                bool PosOk = true;
                for( int _j = 0; _j < space.y; _j++ ) {
                    for( int _i = 0; _i < space.x; _i++ ) {
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
        GameObject panel = GameObject.Instantiate( InventoryControler.instance.panelPrefab, InventoryControler.instance.targetPanel.transform ) as GameObject;
        panel.name = name;
        panel.transform.localRotation = Quaternion.identity;

        //Slots        
        for (int x = 0; x < inventorySpace.x; ++x ) {
            for (int y = 0; y < inventorySpace.y; ++y ) {
                //Create the current Slot
                GameObject slot = GameObject.Instantiate( InventoryControler.instance.slotPrefab, panel.transform ) as GameObject;
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
        #endregion
    }
    #endregion

    #region static
    private static void AddInventoryPanel (GameObject panel) {
        inventoryPanels.Add( panel.name, panel );
        panel.transform.SetParent( InventoryControler.instance.targetPanel.transform );
        panel.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
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
