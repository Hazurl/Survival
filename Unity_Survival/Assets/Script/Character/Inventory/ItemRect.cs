using System.Collections.Generic;
using System;
using UnityEngine;

[System.Serializable]
public class ItemRect
{
    #region Last Version
    /*
    #region static
    private static int UniqueId = 0;
	private static List<Item> ItemUniqueId = new List<Item>();

	public static Item getItemfromUniqueId (int _uniqueId) {
		return ItemUniqueId[ _uniqueId ];
	}
	#endregion

	#region Attributs
	public readonly int uniqueId;
    public readonly ItemID id;
    public Inventory.InventorySpace spaceRequired { get; protected set; }
    #endregion

    #region Constructors
    public Item( ItemID _id ) {
        id = _id;
        spaceRequired = new Inventory.InventorySpace( 1, 1);

		ItemUniqueId.Add( this );
		this.uniqueId = UniqueId++;
	}

	public Item (ItemID _id, Inventory.InventorySpace _spaceRequired ) {
        id = _id;
        spaceRequired = _spaceRequired;

		ItemUniqueId.Add( this );
		this.uniqueId = UniqueId++;
    }
    #endregion

    #region Methods

    #endregion

    #region Enums
    /// <summary>
    /// Convert The integer into an Item ID, if the ID is not existing, return ItemID.INVALID
    /// </summary>
    /// <param name="_id">The integer to convert into the ItemID</param>
    /// <returns>return the ItemID corresponding to the integer or, if he's not defined, return ItemID.INVALID</returns>
    static public ItemID ConvertIdToItem( int _id ) {
        return (Enum.IsDefined( typeof( ItemID ), _id )) ? 
            (ItemID)Enum.ToObject( typeof( ItemID ), _id ) : 
            ItemID.INVALID;
    }

    /// <summary>
    /// ID of each Item actually implemented
    /// </summary>
    public enum ItemID { 
        INVALID = 0,
        LOG = 1
    }
    #endregion
    */
    #endregion
    
    [SerializeField]
    public Rect rect;
    [SerializeField]
    public ItemData data;

    [NonSerialized]
    public Inventory InventoryContainer;

    // Easy Accessor
    public float X {
        get {
            return rect.x;
        }
    }
    public float Y {
        get {
            return rect.y;
        }
    }
    public float Width {
        get {
            return rect.width;
        }
    }
    public float Height {
        get {
            return rect.height;
        }
    }

    #region Constructor
    public ItemRect (Rect _rect, ItemData _data ) {
        rect = _rect;
        data = _data;
    }

    public ItemRect (float _x, float _y, float _w, float _h, ItemData _data ) {
        rect = new Rect( _x, _y, _w, _h );
        data = _data;
    }

    public ItemRect( Vector2 _position, Vector2 _size, ItemData _data ) {
        rect = new Rect( _position, _size );
        data = _data;
    }
    #endregion

    public bool CollideWith( ItemRect other ) {
        return !(
            X >= other.X + other.Width
         || X + Width <= other.X
         || Y >= other.Y + other.Height
         || Y + Height <= other.Y
         );
    }
}
