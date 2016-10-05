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

    public int Add (int amount)
    {
        if (amount < 0) throw new Exception("Can't add a negative number");
        return Amount += amount;
    }

    public int Remove(int amount)
    {
        if (amount < 0) throw new Exception("Can't remove a negative number");
        return Amount -= Math.Min(amount, Amount);
    }

    public enum ItemID
    {
        WOOD,
        STONE
    }
}
