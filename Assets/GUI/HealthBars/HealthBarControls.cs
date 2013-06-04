using UnityEngine;
using System.Collections;

public class HealthBarControls : MonoBehaviour
{
    public GUITexture indicator;
	public GUITexture background;
	float m_healthValue;
	int m_allign;
	float m_width;
	float m_heigth;
	float m_top;
	Vector2 m_ratio;
	// Use this for initialization
	public void Init (Allign allign) 
	{
	  m_ratio = new Vector2(0.1f, 0.3f);
	  m_allign=Utility.AllignToIntHorizontal(allign);
	  int numTextures = GUIButtonControls.numColumnTextures;
	  m_width=1.2f*Screen.height/numTextures;
	  m_heigth=0.6f*Screen.height/numTextures;
	  m_top=(Screen.height/numTextures-m_heigth)/2;
	  Rect pixelInsert= new Rect(-m_allign*m_width,m_top,m_width,m_heigth);
	  background.pixelInset=pixelInsert;
//	  Debug.Log("Init");
	}
	public void SetValue(float newValue)
	{
	  //Debug.Log(newValue);
//	  int numTextures = GUIButtonControls.numTextures;
	  Rect pixelInsert= new Rect(-m_allign*m_width+m_ratio.x*m_width,m_top+m_heigth*m_ratio.y, newValue*m_width*(1-2*m_ratio.x),m_heigth*(1-2*m_ratio.y)) ;
	  Color newColor = new Color(1-newValue, newValue, 0,1);
	  indicator.pixelInset=pixelInsert;
	  indicator.color=newColor;
	}

}
