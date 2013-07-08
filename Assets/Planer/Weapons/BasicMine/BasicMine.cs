using UnityEngine;
using System;
using System.Collections;

public class BasicMine : CustomObject
{
  //public BasicMineVisualiser m_visualiser;
  int damage = 5;
  public override void OnStart()
  {
    Interact = OnInteract;
  }
  public void Init(PlanerCore parent)
  {
    //Destroy(GetComponent<CustomObjectEditorSupply>());
    Node = parent.prevNode;
    gameObject.SetActive(true);
    
    (m_visualiser as BasicMineVisualiser).OnInstall(parent);
  }

  void OnInteract(CustomObject obj, InteractType type)
  {
    //Debug.Log("interact");
    if (type == InteractType.Stay)
    {
      PlanerCore planer = obj as PlanerCore;
      if (planer == null) return;
      planer.OnDamageDealt(damage);
      (m_visualiser as BasicMineVisualiser).OnDestroy();
    }
  }

}
