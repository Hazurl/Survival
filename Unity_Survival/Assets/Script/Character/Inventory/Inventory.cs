using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class Inventory
{
    #region Attributs
    //This is the display Inventory
    public Item[, ] virtualInventory { get; private set; }
    //The List of Item in the inventory, because a Item can take more than one space
    private Dictionary<int, InventoryPosition> itemPosition;

    public readonly InventorySpace inventorySpace;
    public string name;

    //OnItemChange
    Action<Inventory, Item, InventoryPosition> OnAddingItem;
    Action<Inventory, Item, InventoryPosition> OnRemovingItem;

    #endregion

    #region Constante
    #endregion

    #region staticAttributs
    private static Dictionary<string, GameObject> inventoryPanels = new Dictionary<string, GameObject>();
    private static Dictionary<string, Inventory> inventoryName = new Dictionary<string, Inventory>();

    public InventoryControler Controler = InventoryControler.instance;
    #endregion

    #region Constructors
    /// <summary>
    /// Constructors
    /// </summary>
    /// <param name="Space">The inventorySpace of the inventory, this is a class from <see cref="Inventory.InventorySpace"/></param>
    /// <param name="inventory">The inventory copied in the current, it must be smaller or of the same size</param>
    public Inventory (InventorySpace Space, string name, Inventory inventory = null, bool showInventory = false) {
        //I don't want to have a negative array, an inventory must be more or equals than 1 * 1 array
        if( Space.x < 1 || Space.y < 1 ) throw new System.Exception( "Inventory can't have negative or null space" );

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
        Controler.AddInventoryPanel( Controler.CreatePanel( name, this ), ref OnAddingItem, ref OnRemovingItem );
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

        OnAddingItem( this, item, pos );
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

        OnRemovingItem( this, item, pos );
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
	public class InventoryPosition {
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
