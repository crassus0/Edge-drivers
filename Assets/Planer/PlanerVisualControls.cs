using UnityEngine;
using System.Collections;

public class PlanerVisualControls : MonoBehaviour 
{
  PlanerCore m_parentPlaner;
  public void Init(PlanerCore parent)
  {
	  m_parentPlaner=parent;
	  Bounds newBounds=animation.localBounds;
      animation.localBounds=newBounds;
  }	
  public void Move(int rotation)
  {
	string anim="MoveR"+rotation+"I"+(m_parentPlaner.GetNode().Index+m_parentPlaner.Direction%2)%2;
//	Debug.Log(anim);
	animation.Play(anim);
  }
}
