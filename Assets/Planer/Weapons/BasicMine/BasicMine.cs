using UnityEngine;
using System;
using System.Collections;

public class BasicMine : CustomObject
{
  //public BasicMineVisualiser m_visualiser;
  int damage = 5;
  public void Init(PlanerCore parent)
  {
    //Destroy(GetComponent<CustomObjectEditorSupply>());
    Node = parent.prevNode;
    gameObject.SetActive(true);
    //Debug.Log(Node);
    //Debug.Log(gameObject.activeInHierarchy);
    (m_visualiser as BasicMineVisualiser).OnInstall(parent);
  }
  public override void OnUpdate()
  {

  }
  public override void Interact(CustomObject obj, InteractType type)
  {
    if (type == InteractType.Stay)
    {
      PlanerCore planer = obj as PlanerCore;
      if (planer == null) return;
      planer.OnDamageDealt(damage);
      (m_visualiser as BasicMineVisualiser).OnDestroy();
    }
  }

}
