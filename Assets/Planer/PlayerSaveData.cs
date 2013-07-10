using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization;
[Serializable]
public static class PlayerSaveData
{
  public static void Save(PlanerCore planer, GraphNode node, bool onPlaySave)
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
      PlayerPrefs.SetString("Mine" + i, planer.MineController.Mines[i].GetType().ToString());
    }
    if (onPlaySave)
    {
      PlayerPrefs.SetInt("XCoord", node.X);
      PlayerPrefs.SetInt("YCoord", node.Y);
      PlayerPrefs.SetInt("IndexCoord", node.Index);
      PlayerPrefs.SetInt("LevelCoord", node.Level);
      PlayerPrefs.SetInt("Direction", planer.Direction);
    }
  }
  public static void Clear()
  {
    PlayerPrefs.DeleteAll();
  }
  public static void SaveDiscoveredScene(string sceneName, int sceneStatus)
  {
    
      
    PlayerPrefs.SetInt(sceneName, sceneStatus);
  }
  public static int GetSceneStatus(string sceneName)
  {
    if (!PlayerPrefs.HasKey(sceneName))
      PlayerPrefs.SetInt(sceneName, 0);
    return PlayerPrefs.GetInt(sceneName);
  }
  public static string[] GetMines()
  {
    int mineCount = PlayerPrefs.GetInt("MineCount");
    string[] mines = new string[mineCount];
    for (int i = 0; i < mineCount; i++)
    {
      mines[i] = PlayerPrefs.GetString("Mine" + i);
    }
    return mines;
  }


  public static bool SetPlanerData(PlanerCore planer, bool firstLoad)
  {

    if (!PlayerPrefs.HasKey("CurrentLevel") || !PlayerPrefs.HasKey("Concentration") || !PlayerPrefs.HasKey("MaxConcentration"))
      return false;
    if (PlayerPrefs.GetString("CurrentLevel") != Application.loadedLevelName && firstLoad)
      Application.LoadLevel(PlayerPrefs.GetString("CurrentLevel"));
    if (!firstLoad)
    {
      try
      {
        int x = PlayerPrefs.GetInt("XCoord");
        int y = PlayerPrefs.GetInt("YCoord");
        int index = PlayerPrefs.GetInt("IndexCoord");
        int level = PlayerPrefs.GetInt("LevelCoord");
        int direction = PlayerPrefs.GetInt("Direction");
        planer.Node = GraphNode.GetNodeByParameters(x, y, index, level).GetNodeByDirection(direction);
        planer.Direction = direction;
      }
      catch (System.Exception)
      {

      }
    }
    planer.Concentration = PlayerPrefs.GetFloat("Concentration");
    planer.MaxConcentration = PlayerPrefs.GetFloat("MaxConcentration");
    string[] mines = GetMines();
    ScriptableObject.Destroy(planer.MineController);
    planer.MineController = MineController.GetMineController(mines, planer);
    return true;

  }
}