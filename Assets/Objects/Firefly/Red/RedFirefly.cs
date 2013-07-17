using UnityEngine;
using System.Collections;

public class RedFirefly : ScriptableObject
{
  int action=0;
  ArcadeControlButton[] buttons = new ArcadeControlButton[2];
  PlanerCore m_planer;
  Material defaultPlanerMaterial;
  public void Rotate(int ang)
  {
    action=System.Math.Sign(-ang);
  }
  public void Activate(PlanerCore planer)
  {
    m_planer = planer;
    defaultPlanerMaterial =new Material( m_planer.Visualiser.body.renderer.material);
    m_planer.Visualiser.body.renderer.material.color = Color.red;
    planer.AddUpdateFunc(UpdatePlaner);
    planer.State=1;
    planer.CutInterface();
    planer.Interact = OnInteract;
    AddButtons();
  }
  void UpdatePlaner(IPlanerLike planer)
  {
    
    PlanerCore x = planer as PlanerCore;
    if (x.State != 1)
    {
      Destroy(this);

    }
    else
    {
      x.MoveControls.Rotate(action);
      x.MoveControls.OnUpdate();
      action = 0;
      if (x.MoveControls.isHit)
        Destroy(this);
    }
  }
  void OnDestroy()
  {
    m_planer.RemoveUpdateFunc(UpdatePlaner);
    m_planer.State = 0;
    m_planer.InitInterface();
    m_planer.Visualiser.body.renderer.material = defaultPlanerMaterial;
    m_planer.DefaultInteract();
    Destroy(buttons[0]);
    Destroy(buttons[1]);
    Destroy(this);
  }
  void AddButtons()
  {
    buttons[0]=ScriptableObject.CreateInstance<ArcadeControlButton>();
    buttons[0].InitButton(this, -1);
    buttons[0].Init(m_planer, 1);
    buttons[1] = ScriptableObject.CreateInstance<ArcadeControlButton>();
    buttons[1].InitButton(this, 1);
    buttons[1].Init(m_planer, 1);
  }
  void OnInteract(CustomObject obj, InteractType type)
  {
    DistantPortalEnter x = obj as DistantPortalEnter;
    if (x != null&&x.Status==1)
    {
      x.Status = 2;
    }

  }
}
