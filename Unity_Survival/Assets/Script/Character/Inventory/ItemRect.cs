using System.Collections.Generic;
using System;
using UnityEngine;

[System.Serializable]
public class ItemRect
{
    
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

    public bool CollideWithBorder( float _width, float _height ) {
        return !(
            X >= 0
         && X + Width <= _width
         && Y >= 0
         && Y + Height <= _height
         );
    }
}
