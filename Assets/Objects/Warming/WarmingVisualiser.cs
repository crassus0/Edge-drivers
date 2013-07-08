using UnityEngine;
using System.Collections;

public class WarmingVisualiser : CustomObjectVisualiser
{
  //WarmingControls parentWarming;
  int numMoves=0;
  //GameObject core;
  void Start()
  {
    animation["WalkWarming"].layer = 1;
    animation["WalkWarming"].speed = Random.Range(0.8f, 1.2f) * animation["WalkWarming"].speed;
    //animation["Rotate"].layer = 2;
    //animation.Play("WalkWarming", AnimationPlayMode.Mix);
    //animation.Play("Rotate", AnimationPlayMode.Mix);
    animation["MoveI1"].layer = 3;
    animation["MoveI0"].layer = 3;
  }
  public void PassiveAnimations()
  {
      transform.Rotate(new Vector3(0, 2, 0));//animation.Play("Rotate", PlayMode.StopSameLayer);
      

  }
  public void Move(int index)
  {
    numMoves++;
    animation.Play("MoveI" + index, PlayMode.StopSameLayer);
    animation.Play("WalkWarming", PlayMode.StopSameLayer);
  }
  public void OnMoveFinished()
  {
    animation.Stop();
   
  }
  public void Rewind()
  {
    numMoves = 0;
  }
  
}
