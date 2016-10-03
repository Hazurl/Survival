using UnityEngine;
using System;
using System.Collections.Generic;

public class Inventory
{
    private Dictionary<Item.ItemID, int> listItem;

    const int MAX_STACKABLE_ITEMS = 128;

	public Inventory()
	{
        listItem = new Dictionary<Item.ItemID, int>();
	}

    public void display ()
    {
        //At this point, we want to display the inventory into the unity windows
    }

    public void AddItem (List<Item> items)
    {
        foreach (Item item in items)
        {
            if (listItem.ContainsKey(item.ID) == false) listItem[item.ID] = item.Amount;
            else listItem[item.ID] += item.Amount;
            if (listItem[item.ID] > MAX_STACKABLE_ITEMS)
                listItem[item.ID] = MAX_STACKABLE_ITEMS;
        }
    }

    public bool haveCompleteRecipe(List<Item> recipeList)
    {
        foreach (Item item in recipeList)
        {
            if (!listItem.ContainsKey(item.ID) || listItem[item.ID] < item.Amount) return false;
        }
        return true;
    }

    public void RemoveRecipe (List<Item> recipe)
    {
        foreach (Item item in recipe)
        {
            listItem[item.ID] -= item.Amount;
            if (listItem[item.ID] < 0) throw new Exception("Not Enought Item");
        }
    }
}
