using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class Global : MonoBehaviour {

    #region staticAttributs
    [SerializeField]
    public bool GodMod { get; private set; }
    #endregion

    #region InventoryControlerInstance
    public static Global instance;
    void Awake() {
        if( instance != null ) {
            Debug.LogError( "There is more than 1 instance of Global !" );
            return;
        }
        instance = this;
    }
    #endregion
}
