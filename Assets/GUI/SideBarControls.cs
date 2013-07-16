using UnityEngine;
using System.Collections;
[ExecuteInEditMode]
public class SideBarControls : MonoBehaviour {

  public Allign allign;
  // Use this for initialization
	void Update () 
  {
    int modifier = allign == Allign.Right ? 1 : 0;
    guiTexture.pixelInset = new Rect(-modifier * Screen.height / 5f, 0, Screen.height / 5f, Screen.height);
    transform.position = new Vector3(modifier, 0, -0.5f);
    guiTexture.border=new RectOffset(-4,-4,0,0);
    

	}
	
}
