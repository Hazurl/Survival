using UnityEngine;
using System.Collections.Generic;

public class Character : MonoBehaviour {

    //The camera following this character
    [SerializeField]
    private Camera cam;

    //FIXME : The damage on a three can't be just an integer
    public const int CHOP_DAMAGE_PER_SECOND = 20;
    [SerializeField]
    public float CHOP_DISTANCE = 30f;
    [SerializeField]
    public int INVENTORY_CAPACITY_X = 5 ,
               INVENTORY_CAPACITY_Y = 5 ;

    //Reload Time
    [SerializeField]
    private float RELOAD_TIME = 0.5f; //0.5 secondes
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

        //If 'I' KW is press, toggle the inventory
        if( Input.GetKeyDown( KeyCode.I ) )
            InventoryControler.instance.ToggleInventory();

        //Debug
        if( Global.instance.GodMod) {
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
        Ray _ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit _hit;
        if (Physics.Raycast(_ray, out _hit))
        {
            Quaternion _rot = Quaternion.LookRotation(_hit.point - tf.position);
            _rot.x = 0; _rot.z = 0;
            tf.rotation = _rot;
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
                RaycastHit _hit;
                if (Physics.Raycast(tf.position, transform.TransformDirection(Vector3.forward), out _hit, CHOP_DISTANCE))
                {
                    Debug.DrawRay(tf.position, transform.TransformDirection(Vector3.forward), Color.red);
                    Chopable _target = _hit.collider.gameObject.GetComponent<Chopable>();

                    if (_target == null || _target.isDead() || (_hit.collider.transform.position - tf.position).magnitude < CHOP_DISTANCE) return;

                    //Damage th three, get the drop list of items if he die, and finally updtae reload Time
                    List<Item> _drop = new List<Item>();
                    if (_target.Chop(CHOP_DAMAGE_PER_SECOND, out _drop))
                    {
                        Debug.Log("A three has been chop");
                        List<Item> _dropOverflow = new List<Item>( _drop );
                        foreach( Item item in _dropOverflow )
                            if( inventory.AddItem( item ) ) _dropOverflow.Remove( item );

                        if (_dropOverflow.Count > 0) Debug.Log("Inventory Overflow");
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
