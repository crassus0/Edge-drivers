using UnityEngine;
using System.Collections;

public class WebCatapultActivator : ButtonObject
{
  int cooldown = 0;
  int number = -1;
  int range = 3;
  public static GameObject minePrefab;
  public static Texture2D mineTexture;
  public override void Activate()
  {
    //	Debug.Log("hit");
    if (cooldown > 0) return;
    if (number == 0) return;
    //	Debug.Log("hithit");
    minePrefab.GetComponent<CustomObject>().Level = ParentPlaner.Level;
    //	Debug.Log(ParentPlaner.transform.position);
    //	Debug.Log(ParentPlaner.prevNode.NodeCoords());
    GameObject mine = Instantiate(minePrefab) as GameObject;
    mine.transform.localScale = ParentPlaner.transform.localScale * 0.5f;
    
    mine.GetComponent<WebMine>().Init(ParentPlaner, range);
    if (HasGUI)
      Button.guiTexture.color = new Color(1, 0, 0, 1) * 0.5f;
    if (number > 0) number--;
    cooldown = 3;
    Activated = true;
  }

  public override Texture2D GetObjectTexture()
  {
    return mineTexture;
  }
  protected override void OnLoop()
  {
    //Debug.Log("Loop"+ cooldown);
    if (cooldown > 0) cooldown--;
    else Activated = false;
    if (HasGUI)
      Button.guiTexture.color = new Color(1, 1 - 0.33333f * cooldown, 1 - 0.33333f * cooldown, 1) * 0.5f;

  }

}
