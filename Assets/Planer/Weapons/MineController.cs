using UnityEngine;
using System.Collections;

public class MineController : ScriptableObject 
{
//	public mineIncluded[] mines;
	PlanerCore m_planer;
    ButtonObject[] m_mines;
    public ButtonObject[] Mines{get{return m_mines;}}
	
	public void Init(PlanerCore planer)
	{
	  m_planer=planer;
	  m_mines=new ButtonObject[2];
	  m_mines[0]=ScriptableObject.CreateInstance<BasicMineActivator>();
	  m_mines[0].Init(m_planer, 0);
	  m_mines[1]=ScriptableObject.CreateInstance<WebCatapultActivator>();
	  m_mines[1].Init(m_planer, 1);
	}
	public void OnUpdate()
	{
	  for(int i=0; i<m_mines.Length; i++)
	  {
		m_mines[i].OnUpdate();
	  }
	}

}