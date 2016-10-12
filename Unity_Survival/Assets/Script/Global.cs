using UnityEngine;
using UnityEngine.UI;
public class Global : MonoBehaviour {

    public bool GodMod = true;
    public bool ActiveGetKey = true;

    [SerializeField]
    private Character character;
    [SerializeField]
    private InputField inputField;
    [SerializeField]
    private Text cmd;

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

        if( Input.GetKeyDown( KeyCode.Return ) && cmd.text != "" )  {
            ValidCmd( cmd.text );
            cmd.text = "";
        }
    }

    void ValidCmd( string _cmd ) {
        string [] _cmds = _cmd.Split( ' ' );
        if( _cmds.Length < 6 )
            return;

        if( _cmds[ 0 ] != "give" )
            return;

        int _id;
        if( !int.TryParse( _cmds[ 1 ], out _id ) )
            return;

        ItemData.ItemID _itemID;
        if( ( _itemID = ItemData.ConvertIdToItem( _id ) ) == ItemData.ItemID.INVALID ) 
            return;

        int _x;
        if( !int.TryParse( _cmds[ 2 ], out _x ) )
            return;

        int _y;
        if( !int.TryParse( _cmds[ 3 ], out _y ) )
            return;

        int _w;
        if( !int.TryParse( _cmds[ 4 ], out _w ) )
            return;

        int _h;
        if( !int.TryParse( _cmds[ 5 ], out _h ) )
            return;

        character.giveItem( new ItemRect( _x, _y, _w, _h, new ItemData( _itemID ) ) );
    }
}
