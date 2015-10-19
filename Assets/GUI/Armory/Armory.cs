using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public static class Armory
{
  public static GameObject weaponPrototypePrefab;
  static WeaponSelectButton[] m_prototypes;
  public static readonly string[][] UpgradeNames = new string[3][]
	{
		new string[1]{ "BasicMineActivator"},
		new string[1]{"WebCatapultActivator"},
		new string[1]{"LeftTerraformingMine"}
	}; 
	static List<int> [] unlockedUpgrades=new List<int>[3];
	static List<int> InstalledUpgrades;
  static int s_index = -1;

  public static void ShowUpgrades(int index)
  {
    InstalledUpgrades = new List<int>(PlayerSaveData.GetMines());
    unlockedUpgrades[index]=PlayerSaveData.WeaponList(index);
		s_index = index;
		List<int>upgrades=new List<int>();
		for(int i=0; i<unlockedUpgrades[index].Count; i++)
		{
			if(unlockedUpgrades[index][i]>0)
				upgrades.Add(i);
		}
		m_prototypes = new WeaponSelectButton[upgrades.Count];
	
    for (int k = 0; k < upgrades.Count; k++)
    {
      
      GameObject x = GameObject.Instantiate(weaponPrototypePrefab) as GameObject;

      m_prototypes[k] = x.GetComponent<WeaponSelectButton>();
      ButtonObject t = ScriptableObject.CreateInstance(UpgradeNames[index][upgrades[k]]) as ButtonObject;
      m_prototypes[k].GetComponent<GUITexture>().texture = (t).GetObjectTexture();
      m_prototypes[k].transform.position = GetCoords(k, upgrades.Count) + new Vector3(1f, 0.6f-index*0.2f);
      float radius = Screen.height / 10;
      m_prototypes[k].GetComponent<GUITexture>().pixelInset = new Rect(0, radius / 2, radius, radius);
      m_prototypes[k].index = upgrades[k];
      ScriptableObject.Destroy(t);
      
    }
  }
  public static void HideUpgrades()
  {
    if (m_prototypes == null) return;
    foreach (WeaponSelectButton x in m_prototypes)
    {
      GameObject.Destroy(x.gameObject);
    }
    m_prototypes = null;
  }
  public static void AddWeapon(int index)
  {
    InstalledUpgrades[s_index]=index;
    Creator.Player.MineController.RenewObjectList(InstalledUpgrades.ToArray());
  }
  static Vector3 GetCoords(int index, int length)
  {
    float dist = (0.2f / 3) * length;
    float ang;
		if(length>1)
			ang= index * (Mathf.PI / (3 * (float)(length-1))) + 5 * Mathf.PI / 6;
		else
		{
			dist=0.4f/3;
		  ang=Mathf.PI;	
		}
    Vector3 coords = new Vector3(Mathf.Cos(ang), Mathf.Sin(ang))*dist;
		//Debug.Log(coords);
    return coords;
  }
	public static int GetNumCharges(int category, int index)
	{
		unlockedUpgrades[category]=PlayerSaveData.WeaponList(category);
		
		try
		{
			return unlockedUpgrades[category][index];
		}
		catch
		{
			Debug.Log(category);
			Debug.Log(index);
			throw;
		}
	}
	public static int WeaponIndex(string name, out int category)
	{
		for(int i=0; i<3; i++)
		{
			List<string> list=new List<string>(UpgradeNames[i]);
			int index=list.IndexOf(name);
			if(index>=0)
			{
				category=i;
				return index;
			}
		}
		category=-1;
	  return -1;
	}
	public static int WeaponIndex(string name)
	{
    int cat;
		return WeaponIndex(name, out cat);
	}
}
