using UnityEngine;
using UnityEngine.UI;
public class Global : MonoBehaviour {

    public bool GodMod = true;
    public bool ActiveGetKey = true;

    [SerializeField]
    private Character character;
    [SerializeField]
    private InputField inputField;

    private string LastCmd = "";

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

    public void Update () {
        ActiveGetKey = !inputField.isFocused;

        if( Input.GetKeyDown( KeyCode.UpArrow ) && !ActiveGetKey) {
            inputField.text = LastCmd;
        }

        if( Input.GetKeyDown( KeyCode.Return ) && inputField.text != "" )  {
            ValidCmd( inputField.text );
            inputField.text = "";
        }
    }

    void ValidCmd( string _cmd ) {
        string [] _cmds = _cmd.Split( ' ' );
        if( _cmds.Length < 6 )
            return;

        if( _cmds[ 0 ] != "give" )
            return;

        ItemData.ItemID _itemID;
        int _id, _x, _y, _w, _h;
        if( ( !int.TryParse( _cmds[ 1 ], out _id ) )
         || ( (_itemID = ItemData.ConvertIdToItem( _id ) ) == ItemData.ItemID.INVALID )
         || ( !int.TryParse( _cmds[ 2 ], out _x ) )
         || ( !int.TryParse( _cmds[ 3 ], out _y ) )
         || ( !int.TryParse( _cmds[ 4 ], out _w ) )
         || ( !int.TryParse( _cmds[ 5 ], out _h ) ) )
            return;

        LastCmd = _cmd;

        character.giveItem( new ItemRect( _x, _y, _w, _h, new ItemData( _itemID ) ) );
    }
}
