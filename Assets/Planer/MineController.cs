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
	public static readonly int MaxNumCharges=10000;
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
  public void RenewObjectList(int[] mines)
  {
    DestroyMines();
    m_mines = new List<ButtonObject>();
    for (int i = 0; i < mines.Length; i++)
    {
      string name;
			int index;
			if(mines[i]>0)
			{
				name=Armory.UpgradeNames[i][mines[i]];
				index=mines[i];
			}
			else
			{
				name=m_planer.Upgrades[i];
				index=Armory.WeaponIndex(name);
			}
			ButtonObject x = ScriptableObject.CreateInstance(name) as ButtonObject;
      x.Init(m_planer, i);
			(x as IWeaponActivator).NumCharges=Armory.GetNumCharges(i, index);
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
			{
				//PlayerSaveData.SaveCharges(m_mines[i].GetType().Name, (m_mines[i] as IWeaponActivator).NumCharges);
				Destroy(m_mines[i]);
			}
    }
  }
  public static MineController GetMineController(int[] mines, PlanerCore planer)
  {
    MineController newController=ScriptableObject.CreateInstance<MineController>();
    newController.Init(planer);
    newController.RenewObjectList(mines);
    return newController;
  }
}