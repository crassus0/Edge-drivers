using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public static class Armory
{
  public static GameObject weaponPrototypePrefab;
  static WeaponSelectButton[] m_prototypes;
  static readonly string[] UpgradeNames = { "BasicMineActivator", "WebCatapultActivator", "LeftTerraformingMine"};
  static List<string> InstalledUpgrades;
  static int s_index = -1;

  public static void ShowUpgrades(int index)
  {
    InstalledUpgrades = new List<string>(PlayerSaveData.GetMines());
    m_prototypes = new WeaponSelectButton[UpgradeNames.Length];
    for (int k = 0; k < UpgradeNames.Length; k++)
    {
      s_index = index;
      GameObject x = GameObject.Instantiate(weaponPrototypePrefab) as GameObject;

      m_prototypes[k] = x.GetComponent<WeaponSelectButton>();
      ButtonObject t = ScriptableObject.CreateInstance(UpgradeNames[k]) as ButtonObject;
      m_prototypes[k].guiTexture.texture = (t).GetObjectTexture();
      m_prototypes[k].transform.position = GetCoords(k) + new Vector3(1f, 0.6f-index*0.2f);
      float radius = Screen.height / 10;
      m_prototypes[k].guiTexture.pixelInset = new Rect(0, radius / 2, radius, radius);
      m_prototypes[k].weaponName = UpgradeNames[k];
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
  public static void AddWeapon(string name)
  {
    InstalledUpgrades[s_index]=name;
    Creator.Player.MineController.RenewObjectList(InstalledUpgrades.ToArray());
  }
  static Vector3 GetCoords(int index)
  {
    float dist = (0.2f / 3) * UpgradeNames.Length;
    float ang = index * (Mathf.PI / (3 * (float)(UpgradeNames.Length-1))) + 5 * Mathf.PI / 6;
    Vector3 coords = new Vector3(Mathf.Cos(ang), Mathf.Sin(ang))*dist;
    return coords;
  }
}
