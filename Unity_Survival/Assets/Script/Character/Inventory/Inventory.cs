using UnityEngine;
using System.Collections.Generic;
using System;

[System.Serializable]
public class Inventory
{

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
