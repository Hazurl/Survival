using System.Collections.Generic;
using System;
using UnityEngine;

public class Item
{
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
}
