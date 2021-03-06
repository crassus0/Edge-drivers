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
      if (GraphTagMachine.GetDirections(x)[direction]!=WayStatus.Blocked)
      {

        x = x.GetNodeByDirection(direction);

        i--;
        if (x.HasObjectOfType(typeof(IPlanerLike)))
          i = -1;
      }
      else
      {
        i = -1;
      }
    }
    //placed = true;
    Node = x;
    (m_visualiser.GetComponent<WebCatapultVisualiser>()).Push(parent);
  }
  void OnUpdated()
  {
    if (stunTime-- <= 0)
    {
      foreach (IPlanerLike planer in m_caughtList)
      {
        planer.RemoveUpdateFunc(PlanerNewUpdater);
      }
      Creator.DestroyObject(this);

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
	public override CustomObjectInfo SerializeObject ()
	{
		throw new System.NotImplementedException ();
	}
	public override System.Type SerializedType ()
	{
		throw new System.NotImplementedException ();
	}
}

