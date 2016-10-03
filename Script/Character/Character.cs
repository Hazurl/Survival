using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour {

    public Camera cam;

    //FIXME : The damage on a three can't be just an integer
    public const int CHOP_DAMAGE_PER_SECOND = 20;
    public const float CHOP_DISTANCE = 20f;


    private Transform tf;
    private Transform camTf;
    private Vector3 offset;

    void Start () {
        tf = GetComponent<Transform>();
        camTf = cam.GetComponent<Transform>();
        offset = camTf.position - tf.position;
    }

    void Update () {
        //Move the Character when press ZQSD
        //The camera has to follow too
        InputMovement();

        //Now the character has to look the mouse
        SetLookRotation();

        //Choping
        Chop();
    }

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

    void Chop ()
    {
        if (Input.GetMouseButton(0)) //left button
        {
            RaycastHit hit;
            if (Physics.Raycast(tf.position, transform.TransformDirection(Vector3.forward), out hit, CHOP_DISTANCE))
            {
                Debug.DrawRay(tf.position, transform.TransformDirection(Vector3.forward), Color.red);
                IChopable target = hit.collider.gameObject.GetComponent<IChopable>();

                if (target == null || target.isDead()) return;

                if (target.Damage(CHOP_DAMAGE_PER_SECOND)) Debug.Log("The three is dead !!!");
            }
        }
    }
}
