using UnityEngine;
using System.Collections.Generic;
using System;

[System.Serializable]
public class Inventory
{
    #region LastVersion
    /*
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

    #region staticAttributs
    public InventoryControler Controler = InventoryControler.instance;
    #endregion

    #region Constructors
    /// <summary>
    /// Constructors
    /// </summary>
    /// <param name="_invSpace">The inventorySpace of the inventory, this is a class from <see cref="Inventory.InventorySpace"/></param>
    /// <param name="_invForCopy">The inventory copied in the current, it must be smaller or of the same size</param>
    public Inventory (InventorySpace _invSpace, string _name, Inventory _invForCopy = null, bool _showInventory = false) {
        //I don't want to have a negative array, an inventory must be more or equals than 1 * 1 array
        if( _invSpace.x < 1 || _invSpace.y < 1 ) throw new System.Exception( "Inventory can't have negative or null space" );

        //Initialize Attribut
        inventorySpace = _invSpace;
        virtualInventory = new Item[ _invSpace.x, _invSpace.y ];
        itemPosition = new Dictionary<int, InventoryPosition>( _invSpace.Lenght );
        name = _name;

        if( _invForCopy != null ) {
            //Add Some Start Item in the inventory
            //But we can't do this if the space is less than this inventory
            if( _invSpace.x < _invForCopy.inventorySpace.x || _invSpace.y < _invForCopy.inventorySpace.y ) return;

            for( int i = 0; i < _invForCopy.inventorySpace.x; ++i )
                for( int j = 0; j < _invForCopy.inventorySpace.y; ++j )
                    virtualInventory[ i, j ] = _invForCopy.virtualInventory[ i, j ];
        }

        //Add Inventory into the inventory Panel
        if( !_showInventory ) return;
        Controler.AddInventoryPanel( Controler.CreatePanel( _name, this ), ref OnAddingItem, ref OnRemovingItem );
    }
	#endregion

	#region Methods
	/// <summary>
	/// Add an item into the inventory
	/// </summary>
	/// <param name="_item">The Item to put it in</param>
	/// <returns>Return True if it can be put else return false</returns>
	public bool AddItem( Item _item ) {
		InventoryPosition _pos = FindPositionFor( _item.spaceRequired );

		if( _pos == null )
			return false;

		for( int i = _pos.x; i < _item.spaceRequired.x + _pos.x; i++ )
			for( int j = _pos.y; j < _item.spaceRequired.y + _pos.y; j++ )
				virtualInventory[ i, j ] = _item;

		itemPosition[ _item.uniqueId ] = _pos;

        OnAddingItem( this, _item, _pos );
        return true;
	}

	/// <summary>
	/// Remove an Item of this inventory
	/// </summary>
	/// <param name="_item">The Item to remve</param>
	/// <returns>Retrn the success of this action</returns>
	public bool RemoveItem ( Item _item ) {
        InventoryPosition _pos = itemPosition[ _item.uniqueId ];

        if( _pos == null )
			return false;

		for( int i = _pos.x; i < _item.spaceRequired.x + _pos.x; i++ )
			for( int j = _pos.y; j < _item.spaceRequired.y + _pos.y; j++ )
				virtualInventory[ i, j ] = null;

		itemPosition.Remove( _item.uniqueId );

        OnRemovingItem( this, _item, _pos );
        return true;
	}

	/// <summary>
	/// Method to search and find a space to pt an Item in the inventory
	/// TODO: Opti !
	/// </summary>
	/// <param name="_space">The space of the item</param>
	/// <returns>The position of the item or null if he can't</returns>
	private InventoryPosition FindPositionFor( InventorySpace _space ) {
		//Return null if the Inventory space is smaller than the item
		if( inventorySpace.x < _space.x || inventorySpace.y < _space.y )
			return null;

        for( int _invSpaceX = 0; _invSpaceX < inventorySpace.y - _space.y + 1; _invSpaceX++ ) {
            for( int _invSpaceY = 0; _invSpaceY < inventorySpace.x - _space.x + 1; _invSpaceY++ ) {
                bool _posOK = true;
                for( int _internSpaceY = 0; _internSpaceY < _space.y; _internSpaceY++ ) {
                    for( int _internSpaceX = 0; _internSpaceX < _space.x; _internSpaceX++ ) {
                        if( virtualInventory[ _internSpaceX + _invSpaceY, _internSpaceY + _invSpaceX ] != null ) {
                            _posOK = false;
                            _invSpaceX += _internSpaceY;
                            break;
                        }
                    }
                    if( !_posOK ) break;
                }

                if( _posOK )
                    return new InventoryPosition( _invSpaceY, _invSpaceX );
            }
        }
        //if we are here, It's beacause there is no space for this Item
        Debug.LogError( "No Space for " + _space.x + " : " + _space.y );
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
        public InventorySpace(int _x, int _y) {
            x = _x;
            y = _y;
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
		public InventoryPosition( int _x, int _y ) {
			x = _x;
			y = _y;
		}
		#endregion
	}

    #endregion
    */
    #endregion

    #region Constructors
    public Inventory (float _width, float _height) {
        width = _width;
        height = _height;

        InventoryControler.instance.CreatePanel( this );
    }
    #endregion


    //Liste des items dans l'inventaire
    [SerializeField]
    private List<ItemRect> Items = new List<ItemRect>();
    [SerializeField]
    private float width;
    [SerializeField]
    private float height;

    public float Width {
        get {
            return width;
        }
    }
    public float Height {
        get {
            return height;
        }
    }

    public bool AddItem( ItemRect _itemRect ) {
        //On test la collision avec ch  aque item de notre inventaire
        Debug.Log( "Add item" );

        //If colliding with the inventory border
        if( _itemRect.CollideWithBorder( Width, Height ) )
            return false;

        foreach( ItemRect item in Items) {
            if( item.CollideWith( _itemRect ) )
                return false;
        }
        Debug.Log( "Not colliding" );

        //On ajoute l'item
        Items.Add( _itemRect );
        _itemRect.InventoryContainer = this;

        //On l'affiche
        InventoryControler.instance.AddItemOnPanel(this, _itemRect );

        return true;
    }

    public bool RemoveItem( ItemRect _itemRect ) {
        //int index = Items.FindIndex( new Predicate<ItemRect>( i => { return i.X == _itemRect.X && i.Y == _itemRect.Y; } ) );

        InventoryControler.instance.RemoveItemOnPanel( this, _itemRect );

        Items.Remove( _itemRect );
        _itemRect.InventoryContainer = null;

        return false;
    }

}
