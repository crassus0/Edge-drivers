using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class WebMine : CustomObject
{
  //public BasicMineVisualiser m_visualiser;
  int stunTime = 2;
  List<IPlanerLike> m_caughtList = new List<IPlanerLike>();
  //bool placed = false;
  protected new void Awake()
  {
    base.Awake();
    Interact = OnInteract;
    OnUpdate = OnUpdated;
  }
  public override void OnStart()
  {
    //throw new System.NotImplementedException();
  }
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
        if (x.HasObject(typeof(IPlanerLike)))
          i = -1;
      }
      else
      {
        i = -1;
      }
    }
    //placed = true;
    Node = x;
    (m_visualiser as WebCatapultVisualiser).Push(parent);
  }
  void OnUpdated()
  {
    if (stunTime-- <= 0)
    {
      foreach (IPlanerLike planer in m_caughtList)
      {
        planer.RemoveUpdateFunc(PlanerNewUpdater);
      }
      Destroy(gameObject);

    }
  }
  void OnInteract(CustomObject obj, InteractType type)
  {

    if (type == InteractType.Enter)
    {
      IPlanerLike planer = obj as IPlanerLike;
      if (planer != null)
      {
        //placed = true;
        planer.AddUpdateFunc(PlanerNewUpdater);
        m_caughtList.Add(planer);
      }
    }
  }
  void PlanerNewUpdater(IPlanerLike planer)
  {
  }
}

