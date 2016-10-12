using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class Global : MonoBehaviour {

    #region staticAttributs
    public bool GodMod = true;
    #endregion

    #region GlobalInstance
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
