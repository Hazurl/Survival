using UnityEngine;
using System;
using System.Collections.Generic;

public class Inventory
{
    private Item[] inventory;

	public Inventory(uint capacity, Item[] items = null)
	{
        inventory = new Item[capacity];
        if (items != null)
        {
            foreach (var item in items)
            {
                if(!TryAddItem(item)) break;
            }
        }
	}

    public void display ()
    {
        //At this point, we want to display the inventory into the unity windows
    }

    public void AddItems (Item[] items)
    {
        foreach (Item item in items)
        {
            if (!TryAddItem(item)) break;
        }
    }

    public bool TryAddItem(Item item)
    {
        return false;
    }

    public bool haveCompleteRecipe(Item[] recipeList)
    {
        foreach (Item item in recipeList)
        {

        }
        return true;
    }

    public void RemoveRecipe (Item[] recipe)
    {
        foreach (Item item in recipe)
        {

        }
    }
}
