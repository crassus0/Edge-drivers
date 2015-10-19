using UnityEngine;
using System.Collections;

public class WarmingVisualiser : CustomObjectVisualiser
{
  //WarmingControls parentWarming;
  int numMoves=0;
  //GameObject core;
  void Start()
  {
    GetComponent<Animation>()["WalkWarming"].layer = 1;
    GetComponent<Animation>()["WalkWarming"].speed = Random.Range(0.8f, 1.2f) * GetComponent<Animation>()["WalkWarming"].speed;
    //animation["Rotate"].layer = 2;
    //animation.Play("WalkWarming", AnimationPlayMode.Mix);
    //animation.Play("Rotate", AnimationPlayMode.Mix);
    GetComponent<Animation>()["MoveI1"].layer = 3;
    GetComponent<Animation>()["MoveI0"].layer = 3;
  }
  public void PassiveAnimations()
  {
      transform.Rotate(new Vector3(0, 2, 0));//animation.Play("Rotate", PlayMode.StopSameLayer);
      

  }
  public void Move(int index)
  {
    numMoves++;
    GetComponent<Animation>().Play("MoveI" + index, PlayMode.StopSameLayer);
    GetComponent<Animation>().Play("WalkWarming", PlayMode.StopSameLayer);
  }
  public void OnMoveFinished()
  {
    GetComponent<Animation>().Stop();
   
  }
  public void Rewind()
  {
    numMoves = 0;
  }
  
}
