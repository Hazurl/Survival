using UnityEngine;
using System;
using System.Collections.Generic;

public class Inventory
{
    private Item[] inventory;

    /// <summary>
    /// Constructeur d'Inventaire
    /// </summary>
    /// <param name="capacity">La capacité maximale de l'Inventaire</param>
    /// <param name="items">La liste d'items à rajouter</param>
	public Inventory(uint capacity, Item[] items = null)
	{
        inventory = new Item[capacity];

        if (items != null)
            foreach (var item in items)
                if (!TryAddItem(item)) break;
	}

    /// <summary>
    /// Constructeur de copie
    /// </summary>
    /// <param name="inv">Une instance d'un inventaire à recopier</param>
    public Inventory (Inventory inv)
    {
        Inventory tmp = new Inventory((uint)inv.inventory.Length, inventory);
        inventory = tmp.inventory;
    }

    /// <summary>
    /// Affiche l'inventaire dans la fenêtre Unity
    /// </summary>
    public void display ()
    {
        //Affichage du tableau dans la fenêtre Unity, p-e avons nous besoin d'une reference vers la zone ou affiche ?
        Debug.Log("Inventory : ");
        for (int i = 0; i < inventory.Length; ++i)
        {
            Item item = inventory[i];
            if (item == null) continue;
            Debug.Log("ID : " + item.ID.ToString() + " * " + item.Amount.ToString());
        }
    }

    /// <summary>
    /// Essaye de rajouter plusieurs Items dans l'Inventaire, si le tableau en paramètre est vide c'est que tous les items ont été rajouté
    /// </summary>
    /// <para> Voici ça version List</para>
    /// <param name="items">La liste d'Item à rajouter</param>
    public void AddItems(ref List<Item> items)
    {
        for (int i = 0; i < items.Count; ++i)
            if (TryAddItem(items[i]))   //Il y a de la place pour cet Item
                items.RemoveAt(i);
    }

    /// <summary>
    /// Essaye de rajouter plusieurs Items dans l'Inventaire, si le tableau en paramètre est vide c'est que tous les items ont été rajouté
    /// </summary>
    /// <para> Voici ça version Array</para>
    /// <param name="items">La liste d'Item à rajouter</param>
    public void AddItems(ref Item[] items)
    {
        for (int i = 0; i < items.Length; ++i)
            if (TryAddItem(items[i]))   //Il y a de la place pour cet Item
                items[i] = null;
    }

    /// <summary>
    /// On essaye de rajouter un item, renvoie false si l'inventaire est plein ou d'un manière générale, que l'objet ne peut pas être rajouté
    /// </summary>
    /// <param name="item">L'Item à rajouté</param>
    /// <returns>Retourne false si l'Item n'as pas été inséré</returns>
    public bool TryAddItem(Item item)
    {
        for (int i = 0; i < inventory.Length; ++i)
        {
            Item invItem = inventory[i]; //Pour modifier la valeur
            if (invItem == null) inventory[i] = item;
            else if (item.ID == invItem.ID)
            {
                //On essaye de les empiler
                //FIXME : Pas de contrainte, on peux empiler à l'infini (ou pas)
                //        p -e instaurer un Item Type ou une valeur max
                inventory[i].Add(item.Amount);
            }
            else continue;
            return true;
        }
        return false;
    }

    /// <summary>
    /// Essaye d'enlever tous les items donné en paramètre de l'inventaire, si ce n'est pas possible renvoie false
    /// </summary>
    /// <param name="recipe">La liste des items à enlever</param>
    /// <returns>Renvoie false si tout les items n'ont pas été enlevé</returns>
    public bool TryRemoveRecipe (Item[] recipe)
    {
        Item[] backUp = inventory;

        foreach (Item item in recipe)
        {
            for (int i = 0; i < inventory.Length; ++i)
            {
                Item invItem = inventory[i]; //Pour modifier la valeur
                if (item.ID == invItem.ID)
                {
                    if (invItem.Amount >= item.Amount)
                    {
                        invItem.Remove(item.Amount);
                        if (invItem.Amount == 0) inventory[i] = null; //On libère la place
                        break;
                    }
                    else
                    { //L'item à enlever est en excès, il faut donc continuer pour tester les autres emplacements
                        item.Remove(invItem.Amount);
                        inventory[i] = null; //On libère la place
                    }
                }
            }

            if (item.Amount > 0)
            {
                inventory = backUp;
                return false; //There is no location to place the item in this inventory
            }
        }
        return true;
    }
}
