using UnityEngine;
using System.Collections.Generic;

public class Character : MonoBehaviour {

    //The camera following this character
    public Camera cam;

    //FIXME : The damage on a three can't be just an integer
    public const int CHOP_DAMAGE_PER_SECOND = 20;
    public const float CHOP_DISTANCE = 20f;
    public const int INVENTORY_CAPACITY_X = 5 ,
                     INVENTORY_CAPACITY_Y = 5 ;

    //Reload Time
    public const float RELOAD_TIME = 0.5f; //0.5 secondes
    private float TimeToReload = 0;

    //Transform
    private Transform tf;
    private Transform camTf;
    private Vector3 offset;

    //Inventory
    private Inventory inventory;

    void Start () {
        //Initialize Tansform
        tf = GetComponent<Transform>();
        camTf = cam.GetComponent<Transform>();
        offset = camTf.position - tf.position;

        //Initialize inventory, later we should initialize from a save
        inventory = new Inventory( new Inventory.InventorySpace( INVENTORY_CAPACITY_X, INVENTORY_CAPACITY_Y ), "MyInventory", null, true );
    }

    void Update () {
        //Move the Character when press ZQSD
        //The camera has to follow too
        InputMovement();

        //Now the character has to look the mouse
        SetLookRotation();

        //Choping
        Chop();

        //If 'E' KW is press, display the inventory
        if( Input.GetKeyDown( KeyCode.I ) )
            Inventory.ToggleInventory();

        //Debug
        if( Global.GodMod ) {
            //Add a log in your inventory
            if( Input.GetKeyDown( KeyCode.L ) )
                inventory.AddItem( new Item( Item.ItemID.LOG, new Inventory.InventorySpace( 1, 1 ) ) );

            //Destroy Random Item
            if( Input.GetKeyDown( KeyCode.K ) )
                inventory.RemoveItem( Item.getItemfromUniqueId( Random.Range( 0, 5 ) ) );
        }
    }

    /// <summary>
    /// Vérifie Les touches de déplacement et déplace le personnage si l'une d'elle est enfoncé
    /// </summary>
    void InputMovement ()
    {
        if (Input.anyKey)
        {
            if (Input.GetKey(KeyCode.Z))
            { //Up
                tf.Translate(Vector3.forward, Space.World);
            }
            if (Input.GetKey(KeyCode.D))
            { //Rigth
                tf.Translate(Vector3.right, Space.World);
            }
            if (Input.GetKey(KeyCode.Q))
            { //Left
                tf.Translate(Vector3.left, Space.World);
            }
            if (Input.GetKey(KeyCode.S))
            { //Down
                tf.Translate(Vector3.back, Space.World);
            }
            //Update Camera position
            camTf.position = tf.position + offset;
        }
    }

    /// <summary>
    /// Update la rotation du personnage en fonction du pointeur de la souris
    /// </summary>
    void SetLookRotation ()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Quaternion rot = Quaternion.LookRotation(hit.point - tf.position);
            rot.x = 0; rot.z = 0;
            tf.rotation = rot;
        }
    }

    /// <summary>
    /// Essaye de couper du bois
    /// <para>Ne coupe du bois que si le rechargement du coup est à 0, que le bouton gauche de la souris est activé et qu'un arbre se trouve devant</para>
    /// </summary>
    void Chop ()
    {
        if (TimeToReload <= 0)
        {
            if (Input.GetMouseButton(0)) //left button
            {
                RaycastHit hit;
                if (Physics.Raycast(tf.position, transform.TransformDirection(Vector3.forward), out hit, CHOP_DISTANCE))
                {
                    Debug.DrawRay(tf.position, transform.TransformDirection(Vector3.forward), Color.red);
                    Chopable target = hit.collider.gameObject.GetComponent<Chopable>();

                    if (target == null || target.isDead()) return;

                    //Damage th three, get the drop list of items if he die, and finally updtae reload Time
                    List<Item> drop = new List<Item>();
                    if (target.Chop(CHOP_DAMAGE_PER_SECOND, out drop))
                    {
                        Debug.Log("A three has been chop");
                        List<Item> _drop = new List<Item>(drop);
                        foreach( Item item in drop )
                            if( inventory.AddItem( item ) ) _drop.Remove( item );

                        if (_drop.Count > 0) Debug.Log("Inventory Overflow");
                    }

                    TimeToReload = RELOAD_TIME;
                }
            }
        } else
        {
            TimeToReload -= Time.deltaTime;
        }
    }
}
