using UnityEngine;
using System.Collections;

public class PlanerVisualControls : CustomObjectVisualiser
{
  PlanerCore m_parentPlaner;
  bool m_isHit = false;
  public GameObject halo;
  public GameObject body;
  public Texture2D normalHaloTexture;
  public Texture2D hitHaloTexture;
  public void Init(PlanerCore parent)
  {
    m_parentPlaner = parent;

  }

  public void Move(int rotation)
  {
    
    string anim = "MoveR" + rotation + "I" + (m_parentPlaner.GetNode().Index + m_parentPlaner.Direction % 2) % 2;
    //Debug.Log("Planer");
    //Debug.Log(animation[anim].normalizedSpeed);
    animation.Play(anim);
    //animation[anim].speed = 1/Creator.turnDuration;
  }
  public void Hit(int rotation)
  {
    string anim = "HitR" + rotation + "I" + (m_parentPlaner.GetNode().Index + m_parentPlaner.Direction % 2) % 2;
    animation.Play(anim);
  }
  public void OnHit()
  {
    if (m_isHit)
    {
      halo.renderer.material.mainTexture = normalHaloTexture;
      m_isHit = false;
    }
    else
    {
      halo.renderer.material.mainTexture = hitHaloTexture;
      m_isHit = true;
    }
  }
}
