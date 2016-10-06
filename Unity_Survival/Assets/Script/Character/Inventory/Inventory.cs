using UnityEngine;
using System;
using System.Collections.Generic;

public class Inventory
{
    #region Attributs
    //This is the display Inventory
    private Item[, ] virtualInventory;
    //The List of Item in the inventory, because a Item can take more than one space
    private List<Item> itemList;

    public readonly InventorySpace Space;
    #endregion

    #region Constructors
    /// <summary>
    /// Constructors
    /// </summary>
    /// <param name="Space">The Space of the inventory, this is a class from <see cref="Inventory.InventorySpace"/></param>
    /// <param name="inventory">The inventory copied in the current, it must be smaller or of the same size</param>
    public Inventory (InventorySpace Space, Inventory inventory = null) {
        //I don't want to have a negative array, an inventory must be more or equals than 1 * 1 array
        if( Space.x < 1 || Space.y < 1 ) throw new Exception( "Inventory can't have a negative space" );

        //Initialize Attribut
        this.Space = Space;
        virtualInventory = new Item[ Space.x, Space.y ];
        itemList = new List<Item>( Space.Lenght );

        if( inventory == null ) return;
        //Add Some Start Item in the inventory
        //But we can't do this if the space is less than this inventory
        if( Space.x < inventory.Space.x || Space.y < inventory.Space.y ) return;

        for( int i = 0; i < inventory.Space.x; ++i )
            for( int j = 0; j < inventory.Space.y; ++j )
                virtualInventory[ i, j ] = inventory.virtualInventory[ i, j ];
    }
    #endregion

    #region Methods
    #endregion

    #region Structures
    /// <summary>
    /// A class to represent the space maximum of an inventory
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
}
