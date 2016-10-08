using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class Global : MonoBehaviour {
    public GameObject _SlotsPanel;
    public static RectTransform SlotsPanel { get; private set; }

    public GameObject _Slot;
    public static GameObject Slot { get; private set; }

    public GameObject _InventoryPanel;
    public static GameObject InventoryPanel;


    void Awake () {
        SlotsPanel = _SlotsPanel.GetComponent<RectTransform>();
        Slot = _Slot;
        InventoryPanel = _InventoryPanel;
    }
}
