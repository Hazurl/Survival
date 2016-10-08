using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class Global : MonoBehaviour {

    #region staticAttributs
    public bool _GodMod;
    public static bool GodMod;
    #endregion

    #region staticPrefabs
    public GameObject _InventoryPanel;
    public static GameObject InventoryPanel { get; private set; }

    public GameObject _SlotsPanel;
    public static GameObject SlotsPanel { get; private set; }

    public GameObject _Slot;
    public static GameObject Slot { get; private set; }

    public GameObject _ItemSprite;
    public static GameObject ItemSprite { get; private set; }
    #endregion

    void Awake () {
        //Prefabs
        SlotsPanel = _SlotsPanel;
        Slot = _Slot;
        InventoryPanel = _InventoryPanel;
        ItemSprite = _ItemSprite;

        //Attributs
        GodMod = _GodMod;
    }
}
