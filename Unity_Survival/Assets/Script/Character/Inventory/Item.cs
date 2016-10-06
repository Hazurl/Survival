using System;
using UnityEngine;

public class Item
{
    #region Attributs
    public readonly ItemID id;
    public Vector2 spaceRequired { get; protected set; }
    #endregion

    #region Constructors
    public Item( ItemID id ) {
        this.id = id;
        this.spaceRequired = new Vector2 (1, 1);
    }

    public Item (ItemID id, Vector2 spaceRequired) {
        this.id = id;
        this.spaceRequired = spaceRequired;
    }
    #endregion

    #region Methods

    #endregion

    #region Enums
    /// <summary>
    /// Convert The integer into an Item ID, if the ID is not existing, return ItemID.INVALID
    /// </summary>
    /// <param name="ID">The integer to convert into the ItemID</param>
    /// <returns>return the ItemID corresponding to the integer or, if he's not defined, return ItemID.INVALID</returns>
    static public ItemID ConvertIdToItem( int ID ) {
        return (Enum.IsDefined( typeof( ItemID ), ID )) ? 
            (ItemID)Enum.ToObject( typeof( ItemID ), ID ) : 
            ItemID.INVALID;
    }

    /// <summary>
    /// ID of each Item actually implemented
    /// </summary>
    public enum ItemID { 
        INVALID = 0,
        LOG
    }
    #endregion
}
