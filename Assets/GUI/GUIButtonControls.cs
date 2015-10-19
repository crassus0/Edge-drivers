using UnityEngine;
using System.Collections;

public class GUIButtonControls : MonoBehaviour, IButton
{
	private ButtonObject m_parent;
	// Use this for initialization
	public static readonly int numColumnTextures=5;
	public void OnPressed(bool isUp)
	{
    
	    m_parent.Activate(isUp);
	  
	}
	public void Init(ButtonObject parent, Allign allign, int index)
	{
   
	  m_parent=parent;
	  GetComponent<GUITexture>().texture=parent.GetObjectTexture();
	  
    int allignNumeric=Utility.AllignToIntHorizontal(allign);
    transform.position = new Vector3(allignNumeric, 1 - (float)((index + 2)) / GUIButtonControls.numColumnTextures, 0);
    Rect pixelInsert = new Rect(-allignNumeric * Screen.height / (numColumnTextures + 0.2f), 0, Screen.height / (numColumnTextures + 0.2f), Screen.height / (numColumnTextures + 0.2f));
	  GetComponent<GUITexture>().pixelInset=pixelInsert;
	}
  public void Init(ButtonObject parent)
	{
	  m_parent=parent;
	}


}
