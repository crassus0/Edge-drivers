using UnityEngine;
using System.Collections;

public class Whirlwind : CustomObject
{
  public int spin;
  public override void OnStart()
  {
    if (Node.Tag != NodeTag.None)
    {
      Destroy(gameObject);
      return;
    }
    Node.Tag = NodeTag.Whirlwind;
    Node.TagModifier = spin;
    Interact = OnInteract;
    m_visualiser.animation["Rotate"].speed = -0.2f*spin;
  }
  public void OnInteract(CustomObject obj, InteractType type)
  {
    if (type == InteractType.Stay)
    {
      if (obj as IAutoMove != null)
        (obj as IAutoMove).Direction += spin;
    }
  }
  protected new void OnDestroy()
  {
    base.OnDestroy();

  }
}
