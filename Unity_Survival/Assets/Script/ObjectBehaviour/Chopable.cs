using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(MeshRenderer))]
public class Chopable : MonoBehaviour, IDebuguable
{

    public int maxLife = 100;
    private int currentLife;

    private const float TIME_BETWEEN_DAMAGE = 1;
    private float timeFromLastHit = 0;

    private const float TIME_TO_GROW = 5; //2 minutes
    private float timeToGrow = 0;
    private MeshRenderer ThreeGrowable;

    private new CapsuleCollider collider;
    private Vector3 POS_COLLIDER_CHOP = new Vector3(0, 20, 0);
    private Vector3 POS_COLLIDER_NOTCHOP = new Vector3(0, -18, 0);

    // Use this for initialization
    void Start()
    {
        currentLife = maxLife;
        collider = GetComponent<CapsuleCollider>();
        foreach (var child in gameObject.GetComponentsInChildren<Transform>())
            if (child.name == "Arbre")
            {
                ThreeGrowable = child.GetComponent<MeshRenderer>();
                break;
            }
    }

    void Update()
    {
        //Control waiting time between two hit
        if (timeFromLastHit > 0)
            timeFromLastHit -= Time.deltaTime;

        //Control the growing
        if (timeToGrow > 0)
        {
            timeToGrow -= Time.deltaTime;
            if (timeToGrow <= 0)
            {
                ThreeGrowable.enabled = true;
                currentLife = maxLife;
                collider.center = POS_COLLIDER_CHOP;
            }
        }

    }

    //Currently, this return just a boolean, maybe return a list of item ?
    public bool Damage(int dam, out List<Item> items)
    {
        items = new List<Item>();

        if (timeFromLastHit <= 0)
        {
            timeFromLastHit = TIME_BETWEEN_DAMAGE;
            currentLife -= dam;
        }

        if (currentLife > 0) return false;

        BreakThree();

        //Drop 1 to 3 Wood (maybe it should be a paremeter ?)
        items.Add(new Item(Item.ItemID.WOOD, UnityEngine.Random.Range(1, 3)));
        return true;
    }

    public bool isDead()
    {
        return currentLife <= 0;
    }

    private void BreakThree()
    {
        //On enleve l'affichage du tronc
        ThreeGrowable.enabled = false;

        //On met le temps de pousse
        timeToGrow = TIME_TO_GROW;

        //On bouge le collider
        collider.center = POS_COLLIDER_NOTCHOP;
    }

    string IDebuguable.getDescription()
    {
        return currentLife.ToString() + '/' + maxLife.ToString();
    }

    string IDebuguable.getName()
    {
        return "Three";
    }
}
