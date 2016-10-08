using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class Global : MonoBehaviour {
    public GameObject _SlotsPanel;
    public static GameObject SlotsPanel { get; private set; }

    public GameObject _Slot;
    public static GameObject Slot { get; private set; }

    public GameObject _InventoryPanel;
    public static GameObject InventoryPanel { get; private set; }


    void Awake () {
        SlotsPanel = _SlotsPanel;
        Slot = _Slot;
        InventoryPanel = _InventoryPanel;
    }
}
