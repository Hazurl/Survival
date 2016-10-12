using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(MeshRenderer))] //TODO : Mettre une anim à la place d'effacer le mesh
public class Chopable : MonoBehaviour, IDebuguable
{
    [Header("Life :")]
    [SerializeField]
    private int maxLife = 100;

    [SerializeField]
    private int currentLife;

    [Space(10)]
    [Header("Part of the three growable")]
    [SerializeField]
    private MeshRenderer ThreeGrowable;

    [SerializeField]
    private float TIME_TO_GROW = 5; //2 minutes
    private float timeToGrow = 0;

    private CapsuleCollider colliderBase;
    private Vector3 POS_COLLIDER_CHOP = new Vector3(0, 20, 0);
    private Vector3 POS_COLLIDER_NOTCHOP = new Vector3(0, -18, 0);

    // Use this for initialization
    void Start()
    {
        currentLife = maxLife;
        colliderBase = GetComponent<CapsuleCollider>();
        foreach (var child in gameObject.GetComponentsInChildren<Transform>())
            if (child.name == "Arbre")
            {
                ThreeGrowable = child.GetComponent<MeshRenderer>();
                break;
            }
    }

    void Update()
    {
        //Control the growing
        if (timeToGrow > 0)
        {
            timeToGrow -= Time.deltaTime;
            if (timeToGrow <= 0)
            {
                ThreeGrowable.enabled = true;
                currentLife = maxLife;
                colliderBase.center = POS_COLLIDER_CHOP;
            }
        }

    }

    /// <summary>
    /// Fait prendre des dégâts à l'Objet
    /// </summary>
    /// <param name="_dam">Le nombre de dégâts</param>
    /// <param name="_items">La liste des items droppables si l'Objet en drop</param>
    /// <returns>True si L'objet drop des items</returns>
    public bool Chop(int _dam, out List<ItemRect> _items)
    {
        _items = new List<ItemRect>();
        if( (currentLife -= _dam) > 0) return false; //The three is still alive

        //At this point the three has been entirely chop, so play the animation and update drop list
        BreakThree();
        //Drop 1 to 3 Wood (maybe it should be a paremeter ?)
        //FIXME : We have to drop Item on the floor, not giving them to the character immediately

        //_items.Add( new ItemRect( ItemRect.ItemID.LOG, new Inventory.InventorySpace( 1, 1 ) ) );

        return true; //The three is dead
    }

    /// <summary>
    /// Retourne true si l'Objet est mort (si sa vie est inférieur à 0)
    /// </summary>
    /// <returns>Retourne true si l'Objet est mort</returns>
    public bool isDead()
    {
        return currentLife <= 0;
    }

    /// <summary>
    /// Joue l'animation et active son décompteur de croissance
    /// </summary>
    private void BreakThree()
    {
        //On enleve l'affichage du tronc
        ThreeGrowable.enabled = false;

        //On met le temps de pousse
        timeToGrow = TIME_TO_GROW;

        //On bouge le collider
        colliderBase.center = POS_COLLIDER_NOTCHOP;
    }

    #region IDebuguable

    /// <summary>
    /// Retourne la description de l'Objet
    /// </summary>
    /// <returns>Retourne la description de l'Objet</returns>
    string IDebuguable.getDescription()
    {
        return currentLife.ToString() + '/' + maxLife.ToString();
    }

    /// <summary>
    /// Retourne le nom de l'Objet
    /// </summary>
    /// <returns>Retourne le nom de l'Objet</returns>
    string IDebuguable.getName()
    {
        return "Three";
    }

    #endregion
}
