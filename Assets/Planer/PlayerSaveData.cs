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
    PlayerPrefs.DeleteAll();
    PlayerPrefs.SetFloat("Concentration", planer.Concentration);
    PlayerPrefs.SetInt("MineCount", planer.MineController.Mines.Count);
    for (int i = 0; i < planer.MineController.Mines.Count; i++)
    {
      PlayerPrefs.SetString("Mine" + i, planer.MineController.Mines[i].GetType().ToString());
    }
  }



  public static bool SetPlanerData(PlanerCore planer)
  {
    try
    {
      //Debug.LogError(level);
      
      planer.Concentration = PlayerPrefs.GetFloat("Concentration");
      int mineCount = PlayerPrefs.GetInt("MineCount");
      string[] mines = new string[mineCount];
      for (int i = 0; i < mineCount; i++)
      {
        mines[i] = PlayerPrefs.GetString("Mine" + i); 
      }
      ScriptableObject.Destroy(planer.MineController);
      planer.MineController=MineController.GetMineController(mines, planer);
      return true;
    }
    catch (System.Exception a)
    {
      Debug.LogError(a.Message);
      return false;
    }
  }
}
