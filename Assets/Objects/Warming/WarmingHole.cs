using UnityEngine;
using System.Collections;

public class WarmingHole : CustomObject
{
  public GameObject m_warmingPrefab;
  public WarmingHole PairHole;
  public int cooldown;
  int turn=0;
  int step = 0;

  public float[] intensity;
  public int direction;
  public override void OnStart()
  {
    OnUpdate = OnUpdated;
  }
  void OnUpdated()
  {
    if (turn++ == cooldown)
    {
      Warming warm= (Instantiate(m_warmingPrefab) as GameObject).GetComponent<Warming>();
      turn = 0;
      
      warm.m_warmingConcentration=intensity[(step++)%intensity.Length];
      warm.Direction = direction;
      warm.Node = Node.GetNodeByDirection(direction);
      warm.MaxRotateAngle = 3;
      warm.Init();
      warm.m_ai.SetTarget(PairHole.Node);
      warm.OnUpdate();
    }
  }
	public override CustomObjectInfo SerializeObject ()
	{
		WarmingHoleInfo x = new WarmingHoleInfo();
		x.PairHole=PairHole.ObjectID;
		x.cooldown=cooldown;
		x.BasicSerialization(this);
		return x;
	}
	public override System.Type SerializedType ()
	{
		return typeof (WarmingHoleInfo);
	}
}
[System.Serializable]
public class WarmingHoleInfo:CustomObjectInfo
{
  public int PairHole;
  public int cooldown;
	[System.NonSerialized]
	WarmingHole hole;
	public override CustomObject Deserialize ()
	{
		hole=CreateInstance() as WarmingHole;
		hole.cooldown=cooldown;
		return hole;
	}
	public override void EstablishConnections ()
	{
		hole.PairHole=GetObjectByID(PairHole) as WarmingHole;
	}
	public override string GetName ()
	{
		return "WarmingHole";
	}
}