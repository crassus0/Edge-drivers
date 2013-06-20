using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class WebMine : CustomObject
{
  //public BasicMineVisualiser m_visualiser;
  int stunTime = 2;
  List<PlanerCore> m_caughtList = new List<PlanerCore>();
  bool placed = false;
  public void Init(PlanerCore parent, int range)
  {
    GraphNode x = parent.GetNode();
    gameObject.SetActive(true);
    int direction = parent.Direction;
    int i = range;
    while (i > 0)
    {
      if (x.GetDirections()[direction])
      {
        x = x.GetNodeByDirection(direction);
        i--;
      }
      else
      {
        i = -1;
      }
    }
    placed = true;
    Node = x;

    (m_visualiser as WebCatapultVisualiser).Push(parent);
  }
  public override void OnUpdate()
  {
    if (stunTime-- <= 0)
    {
      foreach (PlanerCore planer in m_caughtList)
      {
        planer.RemoveUpdateFunc(PlanerNewUpdater);
      }
      Destroy(gameObject);

    }
  }
  public override void Interact(CustomObject obj, InteractType type)
  {
    if (!placed) return;
    if (type == InteractType.Enter)
    {
      PlanerCore planer = obj as PlanerCore;
      if (planer != null)
      {
        planer.AddUpdateFunc(PlanerNewUpdater);
        m_caughtList.Add(planer);
      }
    }
  }
  void PlanerNewUpdater(PlanerCore planer)
  {
  }
}

