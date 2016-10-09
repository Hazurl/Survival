using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class Global : MonoBehaviour {

    #region staticAttributs
    public bool _GodMod;
    public static bool GodMod;
    #endregion

    void Awake () {
        //Attributs
        GodMod = _GodMod;
    }
}