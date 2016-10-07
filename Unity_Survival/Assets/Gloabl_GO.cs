using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class Gloabl_GO : MonoBehaviour {
    public GameObject _SlotsPanel;
    public static RectTransform SlotsPanel { get; private set; }

    public GameObject _Slot;
    public static GameObject Slot { get; private set; }


    void Awake () {
        SlotsPanel = _SlotsPanel.GetComponent<RectTransform>();
        Slot = _Slot;
    }
}
