using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
public static class SceneDataSaver
{
  public static void SaveSceneData()
  {
    string name = EditorApplication.currentScene;
    string path = name;
   // Debug.Log(path);
    path = path.Remove(path.Length - 5, 5) + "pdt";
    //Debug.Log(path);
    

    FileStream fout = File.Open(path, FileMode.Create);
    BinaryFormatter bFormatter = new BinaryFormatter();
    List<DistantPortalExit.DistantPortalSaveData> data = new List<DistantPortalExit.DistantPortalSaveData>();
    foreach (Object x in Resources.FindObjectsOfTypeAll(typeof(DistantPortalExit)))
    {
      DistantPortalExit portal = x as DistantPortalExit;
      if (!portal.name.Contains("Prefab"))
        data.Add(portal.GetData());
    }
    bFormatter.Serialize(fout, data);
    fout.Close();
  }
  public static List<DistantPortalExit.DistantPortalSaveData> LoadSceneData(string sceneName)
  {

    string path = "Assets/Levels/" + sceneName + "/" + sceneName + ".pdt";
    FileStream fin = File.Open(path, FileMode.Open);
    BinaryFormatter bFormatter = new BinaryFormatter();
    List<DistantPortalExit.DistantPortalSaveData> data;
    data = bFormatter.Deserialize(fin) as List<DistantPortalExit.DistantPortalSaveData>;
    fin.Close();
    return data;
  }

  public static string[] ReadSceneNames()
  {
    List<string> temp = new List<string>();
    foreach (UnityEditor.EditorBuildSettingsScene S in UnityEditor.EditorBuildSettings.scenes)
    {
      string name = S.path.Substring(S.path.LastIndexOf('/') + 1);
      name = name.Substring(0, name.Length - 6);
      if (!name.Equals("BasicScene"))
      {
        temp.Add(name);

      }
    }
    return temp.ToArray();
  }
}
