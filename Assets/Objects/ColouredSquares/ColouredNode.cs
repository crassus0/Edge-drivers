using UnityEngine;
using System.Collections;

public class ColouredNode : CustomObject
{
  public int color;
  public override void OnStart()
  {
    switch (color)
    {
      case 0:
        {
          Node.Tag = NodeTag.RedColoured;
          break;
        }
      case 1:
        {
          Node.Tag = NodeTag.GreenColoured;
          break;
        }
      case 2:
        {
          Node.Tag = NodeTag.GreenColoured;
          break;
        }
    }
    ChangeVisual();
  }
  public void ChangeVisual()
  {
    Color visColor;
    switch (color)
    {
      case 0:
        {
          visColor = Color.red;
          break;
        }
      case 1:
        {
          visColor = Color.green;
          break;
        }
      case 2:
        {
          visColor = Color.blue;
          break;
        }
      default:
        {
          visColor = Color.white;
          break;
        }
    }
    visColor.a = 0.5f;

      m_visualiser.renderer.sharedMaterial.color = visColor;

    transform.rotation = Quaternion.identity;
    transform.Rotate(new Vector3(0, (1 - Node.Index) * 180, 0));
  }
  new void OnDrawGizmos()
  {
    base.OnDrawGizmos();
    ChangeVisual();
  }
}
