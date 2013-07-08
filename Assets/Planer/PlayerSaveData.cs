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

    planer.Concentration = PlayerPrefs.GetFloat("Concentration");
    planer.MaxConcentration = PlayerPrefs.GetFloat("MaxConcentration");
    string[] mines = GetMines();
    ScriptableObject.Destroy(planer.MineController);
    planer.MineController = MineController.GetMineController(mines, planer);
    return true;

  }
}
