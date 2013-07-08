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
      WarmingControls warm= (Instantiate(m_warmingPrefab) as GameObject).GetComponent<WarmingControls>();
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
}
