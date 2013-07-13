using UnityEngine;
using System.Collections;

public class ArcadeControlButton : ButtonObject
{
  RedFirefly m_controlled;
  int direction;
  public static Texture2D[] arcadeControlsTexture=new Texture2D[2];
  static ArcadeControlButton[] buttons=new ArcadeControlButton[2];
  public override void Activate()
  {

    m_controlled.Rotate(direction);
  }
  public override Texture2D GetObjectTexture()
  {
    int index = direction > 0 ? 1 : 0;
    return arcadeControlsTexture[index];
  }
  public override string GetName()
  {
    throw new System.NotImplementedException();
  }
  protected override void OnLoop()
  {
  }
  public void InitButton(RedFirefly parent, int basicDirection)
  {
    if (basicDirection == -1)
      buttons[0] = this;
    else
      buttons[1] = this;
    m_controlled = parent;
    direction = basicDirection;
  }
  protected override void InitGUI(int index)
  {
    if (!Application.isPlaying) return;
    Allign z = direction > 0 ? Allign.Right : Allign.Left;
    Button = (GUITexture)Instantiate(buttonPrefab);
    DontDestroyOnLoad(Button);
    Button.GetComponent<GUIButtonControls>().Init(this, z, index);
    Button.name = "ControlButton";
  }
  public static void KeyPressed(int key)
  {
    if (key == -1)
      buttons[0].Activate();
    else
      buttons[1].Activate();
  }
}
