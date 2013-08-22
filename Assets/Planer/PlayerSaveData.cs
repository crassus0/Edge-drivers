using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization;
[Serializable]
public static class PlayerSaveData
{
  public static void Save(PlanerCore planer, GraphNode node, int direction, bool onPlaySave)
  {
    //Debug.Log("OnSave");
    //PlayerPrefs.DeleteAll();
    PlayerPrefs.SetFloat("Concentration", planer.Concentration);
    PlayerPrefs.SetFloat("MaxConcentration", planer.MaxConcentration);
    PlayerPrefs.SetInt("MineCount", planer.MineController.Mines.Count);
    //Debug.Log(Application.loadedLevelName);
    string currentLevel = Application.loadedLevelName;
    if (currentLevel == "GlobalMap")
      currentLevel = Creator.PreviousLevel;
    PlayerPrefs.SetString("CurrentLevel", currentLevel);
		
    for (int i = 0; i < planer.MineController.Mines.Count; i++)
    {
      PlayerPrefs.SetInt("Mine" + i, Armory.WeaponIndex(planer.MineController.Mines[i].GetType().Name));
    }
    if (onPlaySave)
    {
      PlayerPrefs.SetInt("XCoord", node.X);
      PlayerPrefs.SetInt("YCoord", node.Y);
      PlayerPrefs.SetInt("IndexCoord", node.Index);
      PlayerPrefs.SetInt("LevelCoord", node.Level);
      if(direction<0)
        direction=planer.Direction;
      PlayerPrefs.SetInt("Direction", direction);
    }
  }
	public static void SaveCharges(string weaponName, int charges)
	{
		PlayerPrefs.SetInt("Weapon"+weaponName, charges);
	}
  public static void Clear()
  {
		Debug.Log("Clear");
    PlayerPrefs.DeleteAll();
  }
	public static List<int> WeaponList(int category)
	{
		List<int> categoryList=new List<int>();
		for(int i=0; i<Armory.UpgradeNames[category].Length; i++)
		{
			string index="Weapon"+Armory.UpgradeNames[category][i];
			if (!PlayerPrefs.HasKey(index))
			{
				PlayerPrefs.SetInt(index, 0);
			}
			
			categoryList.Add(PlayerPrefs.GetInt(index));
		}
		return categoryList;
	}
  public static void SaveDiscoveredScene(string sceneName, int sceneStatus)
  {
    
      
    PlayerPrefs.SetInt(sceneName, sceneStatus);
  }
  public static int GetSceneStatus(string sceneName)
  {
    if (!PlayerPrefs.HasKey(sceneName))
      PlayerPrefs.SetInt(sceneName, 0);
		int status=PlayerPrefs.GetInt(sceneName);
    return status;
  }
  public static int[] GetMines()
  {
    int mineCount = PlayerPrefs.GetInt("MineCount");
    int[] mines = new int[mineCount];
    for (int i = 0; i < mineCount; i++)
    {
      mines[i] = PlayerPrefs.GetInt("Mine" + i);
    }
    return mines;
  }


  public static bool SetPlanerData(PlanerCore planer, bool firstLoad)
  {
    planer.InitInterface();

    if ("SafeHouse" != Application.loadedLevelName && firstLoad)
    {
      if (!(Creator.creator.testBuild && Application.isEditor))
      {

        Application.LoadLevel("SafeHouse");
        return false;
      }
    }
    EnterPortal(planer);
    if (!PlayerPrefs.HasKey("CurrentLevel") || !PlayerPrefs.HasKey("Concentration") || !PlayerPrefs.HasKey("MaxConcentration"))
    {
      return true;
    }
    planer.prevNode = planer.Node.GetNodeByDirection((planer.Direction + 3) % 6);
    planer.Concentration = PlayerPrefs.GetFloat("Concentration");
    planer.MaxConcentration = PlayerPrefs.GetFloat("MaxConcentration");
    planer.m_visualiser.transform.position=planer.transform.position;
    int[] mines = GetMines();
    ScriptableObject.Destroy(planer.MineController);
    planer.MineController = MineController.GetMineController(mines, planer);
    return true;
  }
  static void EnterPortal(PlanerCore planer)
  {
    IPortalExit enter = Creator.creator.defaultPortal;
    if (Creator.creator.testBuild && Application.isEditor )
    {
      if(Creator.creator.testEnter != null)
        enter = Creator.creator.testEnter;
      planer.EnteredPortal = true;
    }

    planer.OnEnterPortal(enter.GetNode(), enter.Direction);
  }
	
  public static bool GetColorStatus(string color)
  {

    try
    {
      return PlayerPrefs.GetInt(color)==1;
    }
    catch
    {
      PlayerPrefs.SetInt(color, 0);
      return false;
    }
  }
}
