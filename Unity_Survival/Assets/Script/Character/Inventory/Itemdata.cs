using System;
using UnityEngine;

[System.Serializable]
public class ItemData {

    [SerializeField]
    private ItemID id;
    [SerializeField]
    DataStructure extraData;

    // Property
    public DataStructure ExtraData {
        set {
            this.extraData = value;
        }
    }
    public ItemID Id {
        get {
            return id;
        }
    }

    #region Constructors
    public ItemData (ItemID _id, DataStructure _extraData = null) {
        id = _id;
        extraData = _extraData;
    }
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

[System.Serializable]
public class DataStructure {

}