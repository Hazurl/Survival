using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Text_with_IDebugable : MonoBehaviour {

    public Text text;
    public Camera cam;
	
	void Update () {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            IDebuguable debugGameObject = hit.collider.gameObject.GetComponent<IDebuguable>();
            if (debugGameObject == null)
            {
                text.text = "null";
                return;
            }
            else
            {
                text.text = debugGameObject.getName() + " : " + debugGameObject.getDescription();
            }
        }
    }
}
