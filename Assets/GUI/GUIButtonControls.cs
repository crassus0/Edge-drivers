using UnityEngine;
using System.Collections;

public class GUIButtonControls : MonoBehaviour {
	private ButtonObject m_parent;
	// Use this for initialization
	public static readonly int numColumnTextures=5;
	public void OnPressed()
	{
	  m_parent.Activate();
	  
	}
	public void Init(ButtonObject parent, Allign allign, int index)
	{
   
	  m_parent=parent;
	  guiTexture.texture=parent.GetObjectTexture();
	  
    int allignNumeric=Utility.AllignToIntHorizontal(allign);
    transform.position = new Vector3(allignNumeric, 1 - (float)((index + 2)) / GUIButtonControls.numColumnTextures, 0);
    Rect pixelInsert = new Rect(-allignNumeric * Screen.height / numColumnTextures, 0, Screen.height / numColumnTextures, Screen.height / numColumnTextures);
	  guiTexture.pixelInset=pixelInsert;
	}
  public void Init(ButtonObject parent)
	{
	  m_parent=parent;
	}
  void OnGUI()
  {

  }

}
