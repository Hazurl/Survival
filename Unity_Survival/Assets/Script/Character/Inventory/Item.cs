using System;

public class Item
{
    /// <summary>
    /// Constructeur d'Item
    /// </summary>
    /// <param name="ID">ID de l'Item, voir <see cref="Item.ItemID"/></param>
    /// <param name="amount">Quantité initiale de l'Item</param>
    public Item(ItemID ID, uint amount)
    {
        this.ID = ID;
        this.Amount = amount;
    }

    //The Item is defined by an ID, an amount and some data 

    /// <summary>
    /// ID de l'Item
    /// <para>L'ID est en read only</para>
    /// </summary>
    public readonly ItemID ID;

    private uint _amount;
    /// <summary>
    /// Quantité de l'objet
    /// </summary>
    public uint Amount
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

    //private Data data; FIXME : What is 'Data' ?

    /// <summary>
    /// Ajoute la quantité à l'item en question
    /// </summary>
    /// <param name="amount">La quantité à ajouter</param>
    /// <returns>Retourne la nouvel quantité <paramref name="amount"/></returns>
    public uint Add (uint amount)
    {
        return Amount += amount;
    }

    /// <summary>
    /// Enleve la quantité à l'item en question
    /// </summary>
    /// <param name="amount">La quantité à enlevé</param>
    /// <returns>Retourne la nouvel quantité <paramref name="amount"/></returns>
    public uint Remove(uint amount)
    {
        return Amount -= Math.Min(amount, Amount);
    }

    /// <summary>
    /// Essaye de convertir un integer en un Item
    /// <para>Retourne true si la conversion à réussie</para>
    /// </summary>
    /// <param name="ID">L'ID à convertir</param>
    /// <param name="item">L'Item converti si l'ID existe</param>
    /// <returns>Retourne true si la conversion à reussi</returns>
    static public bool TryConvertIdToItem(int ID, out ItemID item)
    {
        item = default(ItemID);
        bool success = Enum.IsDefined(typeof(ItemID), ID);
        if (success)
        {
            item = (ItemID)Enum.ToObject(typeof(ItemID), ID);
        }
        return success;
    }

    /// <summary>
    /// Liste de tout les items du jeu
    /// </summary>
    public enum ItemID
    {
        INVALID = 0,        //Case of nothing

        LOG,            //Buche
        BRANCH,         //Branche
        BARK,           //Ecorce
        STONE_SHARP,    //Pierre pointu
        STONE_FLAT,     //Pierre plate
        PEBBLE          //Caillou
    }
}
