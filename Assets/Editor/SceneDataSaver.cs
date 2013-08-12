using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
public static class SceneDataSaver
{
  public static string[] ReadSceneNames()
  {
    List<string> temp = new List<string>();
    foreach (string x in Directory.GetFiles("Assets/Resources"))
    {
      string name = x.Substring(x.LastIndexOf('\\') + 1);
      if (name.EndsWith("xml"))
      {
				
        name = name.Substring(0, name.Length - 4);
				temp.Add(name);

      }
    }
    return temp.ToArray();
  }
}
