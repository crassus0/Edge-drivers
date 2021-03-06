using UnityEngine;
using System.Collections;

public class HomePortal : ButtonObject
{
  public static Texture2D homeTexture;
  public override Texture2D GetObjectTexture ()
  {
    return homeTexture;
  }
  public override void Activate (bool isUp)
  {
    if (!isUp) return;
    if(Creator.creator.SceneName =="SafeHouse")
      Creator.creator.LoadPrev();
    else
      Creator.creator.LoadHome();
  }
  public override string GetName ()
  {
    return "HomePortal";
  }
  protected override void OnLoop ()
  {
    
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
}

