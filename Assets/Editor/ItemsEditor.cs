using UnityEngine;
using System.Collections;
using UnityEditor;
public class ItemsEditor : MonoBehaviour
{

  [MenuItem("EdgeDrivers/AddLevel")]
  static void AddLevel()
  {
    GameObject levelPrefab = AssetDatabase.LoadAssetAtPath("Assets/Terrain/BareerLevels/LevelPrefab.prefab", typeof(GameObject)) as GameObject;
    BareerLevelControls level = (Instantiate(levelPrefab) as GameObject).GetComponent<BareerLevelControls>();
    //	  Debug.Log(EditorAdditionalGUI.EditorOptions);
    level.name = "Level" + EditorAdditionalGUI.EditorOptions.levels.Count;
    EditorAdditionalGUI.EditorOptions.levels.Add(level);
    level.transform.parent = GameObject.Find("Creator").transform;
    level.Init();
  }
  [MenuItem("EdgeDrivers/ShowHidden")]
  static void ShowHidden()
  {
    Object[] x = Resources.FindObjectsOfTypeAll(typeof(Object));
    foreach (Object a in x)
    {
      a.hideFlags = 0;
    }
  }
  [MenuItem("EdgeDrivers/HideSelected")]
  static void HideSelected()
  {
    foreach (GameObject x in Selection.objects)
    {
      x.gameObject.hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector;
    }
  }
}
