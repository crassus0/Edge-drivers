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
      if (!x.EndsWith("xml")) continue;
      string name = x.Substring(x.LastIndexOf('\\') + 1);
      name = name.Substring(name.LastIndexOf('/') + 1);
      name = name.Substring(0, name.Length - 4);
			temp.Add(name);

    }
    return temp.ToArray();
  }
}
