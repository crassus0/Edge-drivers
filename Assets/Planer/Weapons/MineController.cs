using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;


public class MineController : ScriptableObject
{
  //	public mineIncluded[] mines;
  PlanerCore m_planer;
  List<ButtonObject> m_mines;
  public List<ButtonObject> Mines { get { return m_mines; } }

  public void Init(PlanerCore planer)
  {
    //Debug.Log("init");
    m_planer = planer;
    m_mines = new List<ButtonObject>();
    int i = 0;
    foreach(string x in planer.Upgrades)
    {
      //Debug.Log(x);
      ButtonObject obj = ScriptableObject.CreateInstance(x) as ButtonObject;

      obj.Init(planer, i);
      m_mines.Add(obj);
      i++;
      //Debug.Log(i);
    }
    
  }
  public void OnUpdate()
  {
    for (int i = 0; i < m_mines.Count; i++)
    {
      m_mines[i].OnUpdate();
    }
  }
  public void RenewObjectList(string[] mines)
  {
    DestroyMines();
    m_mines = new List<ButtonObject>();
    for (int i = 0; i < mines.Length; i++)
    {
      //Debug.Log(mines[i]);
      ButtonObject x = ScriptableObject.CreateInstance(mines[i]) as ButtonObject;
      x.Init(m_planer, i);
      m_mines.Add(x);
    }
  }
  void OnDestroy()
  {
    //Debug.Log("adfsdf");
    DestroyMines();
  }
  void DestroyMines()
  {
    for (int i = 0; i < m_mines.Count; i++)
    {
      if(m_mines[i]!=null)
        Destroy(m_mines[i]);
    }
  }
  public static MineController GetMineController(string[] mines, PlanerCore planer)
  {
    MineController newController=ScriptableObject.CreateInstance<MineController>();
    newController.Init(planer);
    newController.RenewObjectList(mines);
    return newController;
  }
}