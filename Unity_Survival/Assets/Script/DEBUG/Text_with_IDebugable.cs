using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Text_with_IDebugable : MonoBehaviour {

    public Text text;
    public Camera cam;
	
	void Update () {
        Ray _ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit _hit;
        if (Physics.Raycast(_ray, out _hit))
        {
            IDebuguable _debugGameObject = _hit.collider.gameObject.GetComponent<IDebuguable>();
            if (_debugGameObject == null)
            {
                text.text = "null";
                return;
            }
            else
            {
                text.text = _debugGameObject.getName() + " : " + _debugGameObject.getDescription();
            }
        }
    }
}
