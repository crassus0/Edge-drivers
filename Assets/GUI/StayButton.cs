using UnityEngine;
using System.Collections;

public class StayButton : ButtonObject
{
  public static Texture2D objectTexture;

  public override Texture2D GetObjectTexture()
  {
    return objectTexture;
  }

  protected override void InitGUI(int index)
  {
    if (!Application.isPlaying) return;
    Button = (GUITexture)Instantiate(buttonPrefab);
    Button.transform.parent = Creator.GUIFolder.transform;

    Button.transform.position = new Vector3(1, 1 - (float)((index + 2)) / GUIButtonControls.numColumnTextures, 0);
    Button.GetComponent<GUIButtonControls>().Init(this, Allign.Left, index);
    Button.name = "ControlButton";
  }
  protected override void OnLoop()
  {
    
  }
  public override string GetName()
  {
    return "CancelAction";
  }
  public override void Activate(bool isUp)
  {
    if (!isUp) return;
    ParentPlaner.Stay();
  }
}
