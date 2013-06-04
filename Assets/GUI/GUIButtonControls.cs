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
	public void Init(ButtonObject parent, Allign allign)
	{

	  m_parent=parent;
	  guiTexture.texture=parent.GetObjectTexture();
	  int allignNumeric=Utility.AllignToIntHorizontal(allign);
	  Rect pixelInsert= new Rect(-allignNumeric*Screen.height/numColumnTextures,0,Screen.height/numColumnTextures,Screen.height/numColumnTextures);
	  guiTexture.pixelInset=pixelInsert;
	}
    public void Init(ButtonObject parent)
	{
	  m_parent=parent;
	}


}
