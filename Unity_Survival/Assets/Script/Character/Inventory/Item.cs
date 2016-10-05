using System;

public class Item
{
    public Item(ItemID ID, int amount)
    {
        this.ID = ID;
        this.Amount = amount;
    }

    //The Item is defined by an ID and an amount
    private ItemID _ID;
    private int _amount;

    public ItemID ID
    {
        get
        {
            return _ID;
        }

        private set
        {
            _ID = value;
        }
    }

    public int Amount
    {
        get
        {
            return _amount;
        }

        private set //We prefer method like 'add' or 'remove'
        {
            _amount = value;
        }
    }

    public int Add (uint amount)
    {
        return Amount += (int)amount;
    }

    public int Remove(uint amount)
    {
        return Amount -= Math.Min((int)amount, Amount);
    }

    public enum ItemID
    {
        LOG,            //Buche
        BRANCH,         //Branche
        BARK,           //Ecorce
        STONE_SHARP,    //Pierre pointu
        STONE_FLAT,     //Pierre plate
        PEBBLE          //Caillou
    }
}
