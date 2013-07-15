using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ArmoryGenerator : CustomObject
{
  public GameObject weaponPrototypePrefab;
  WeaponPrototype[] m_prototypes;
  static readonly string[] UpgradeNames = { "BasicMineActivator", "WebCatapultActivator", "LeftTerraformingMine" };
  List<string> InstalledUpgrades;

  public override void OnStart()
  {
    //Debug.Log("start");
    InstalledUpgrades = new List<string>(PlayerSaveData.GetMines());
    m_prototypes = new WeaponPrototype[UpgradeNames.Length];
    //Debug.Log(UpgradeNames.Length);
    int maxLength = 15;
    int i = 2;
    int j = 1;

    for (int k = 0; k < UpgradeNames.Length; k++)
    {
      //Debug.Log(k);
      GameObject x = GameObject.Instantiate(weaponPrototypePrefab) as GameObject;

      m_prototypes[k] = x.GetComponent<WeaponPrototype>();
      m_prototypes[k].Node = GraphNode.GetNodeByParameters(i, j, 0, Node.Level);
      ButtonObject t = ScriptableObject.CreateInstance(UpgradeNames[k]) as ButtonObject;
      m_prototypes[k].transform.GetChild(0).renderer.material.mainTexture = (t).GetObjectTexture();
      Destroy(t);
      m_prototypes[k].UpgradeName = UpgradeNames[k];
      m_prototypes[k].IsActive = 0;
      m_prototypes[k].Armory = this;
      m_prototypes[k].gameObject.SetActive(true);
      m_prototypes[k].Activate = m_prototypes[k].OnActivate;
      if (InstalledUpgrades.Contains(UpgradeNames[k]))
        m_prototypes[k].Activate();
      j = j + 2 * (i / maxLength);
      i = (i + 2) % maxLength;
      if (i == 0)
        i = 2;
    }

  }
  public void AddWeapon(string weapon)
  {
    if (!InstalledUpgrades.Contains(weapon))
      InstalledUpgrades.Add(weapon);
    Destroy(Creator.Player.MineController);
    Creator.Player.MineController = MineController.GetMineController(InstalledUpgrades.ToArray(), Creator.Player);
  }
  public void RemoveWeapon(string weapon)
  {
    InstalledUpgrades.Remove(weapon);
    Destroy(Creator.Player.MineController);
    Creator.Player.MineController = MineController.GetMineController(InstalledUpgrades.ToArray(), Creator.Player);
  }
  void OnDisable()
  {
    if (m_prototypes != null)
      foreach (WeaponPrototype x in m_prototypes)
      {
        Destroy(x);
      }
  }
}
