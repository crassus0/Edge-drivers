using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class WeaponShop : MonoBehaviour
{
  public GUISkin m_shopSkin;
  public static List<ButtonObject> m_allWeapons;
  List<ButtonObject> m_unlockedWeapons = null;
  List<bool> m_activeWeapons;
  public bool m_isActive = false;

  Vector2 m_scrollPosition;
  void Awake()
  {
    m_allWeapons = new List<ButtonObject>();
    m_allWeapons.Add(ScriptableObject.CreateInstance<BasicMineActivator>() as ButtonObject);
    m_allWeapons.Add(ScriptableObject.CreateInstance<WebCatapultActivator>() as ButtonObject);
  }
  public void OpenShop()
  {
    //Debug.Log("Opening");
    GUI.skin = m_shopSkin;
    m_isActive = true;

    InitWeaponList();
  }
  void OnGUI()
  {
    if (!m_isActive) return;
    Rect x = new Rect(0.1f * Screen.width, 0.1f * Screen.height, 0.8f * Screen.width, 0.8f * Screen.height);
    if (m_unlockedWeapons == null)
      OpenShop();
   
    GUILayout.BeginArea(x, "adasdfasd", m_shopSkin.GetStyle("window"));
    {
      m_scrollPosition = GUILayout.BeginScrollView(m_scrollPosition);
      {
        for (int i = 0; i < m_unlockedWeapons.Count; i++)
          DrawItemSelection(i);
      } GUILayout.EndScrollView();
      if (GUILayout.Button("OK"))
        OnConfigChange();
    } GUILayout.EndArea();
  }
  bool DrawItemSelection(int i)
  {
    //Debug.Log(i);
    GUILayout.BeginHorizontal();
    {
      GUILayout.Box(m_unlockedWeapons[i].GetName());
      m_activeWeapons[i] = GUILayout.Toggle(m_activeWeapons[i], "");
    } GUILayout.EndHorizontal();
    return false;
  }
  void InitWeaponList()
  {
    //Debug.Log("init");
    m_unlockedWeapons = m_allWeapons;
    m_activeWeapons = new List<bool>(m_unlockedWeapons.Count);

    foreach (Object z in m_unlockedWeapons)
    {
      ButtonObject x = z as ButtonObject;

      bool y = false;
      if (Creator.Player.Upgrades.Contains(x.GetType().Name))
        y = true;
      m_activeWeapons.Add(y);

    }
  }
  void OnConfigChange()
  {
    Creator.Player.Upgrades=new List<string>();
    for(int i=0; i<m_activeWeapons.Count; i++)
    {
      if(m_activeWeapons[i])
        Creator.Player.Upgrades.Add(m_unlockedWeapons[i].GetType().Name);
    }
    Creator.Player.OnWeaponsChanged();
  }
}
