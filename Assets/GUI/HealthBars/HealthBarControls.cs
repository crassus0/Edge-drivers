using UnityEngine;
using System.Collections;

public class HealthBarControls : MonoBehaviour
{
  public Texture2D acttiveHPTexture;
  public Texture2D lostHPTexture;
  public Texture2D hpCooldownBackcround;
  public Texture2D hpCooldownArrow;
  static float width;
  float curentCooldown;
  float maxCooldown;
  int hp = 0;
  void OnGUI()
  {
    if (Event.current.type != EventType.Repaint) return;
    width = Screen.height / 5;

    DrawHP();
    DrawCooldown();
  }
  void DrawHP()
  {
    GUI.BeginGroup(new Rect(0, 0, width, width));
    {
      int numHP = Creator.Player.m_hitPoints;

      int maxHP = 3;
      float itemWidth = width / 5;
      Texture2D texture = acttiveHPTexture;
      for (int i = 0; i < maxHP; i++)
      {
        if (i == numHP)
          texture = lostHPTexture;

        GUI.DrawTexture(new Rect(itemWidth * (i + 1.2f), width * 0.2f, itemWidth * 0.6f, width * 0.6f), texture);
      }
    }
    GUI.EndGroup();
  }
  void DrawCooldown()
  {
    float smallWidth = 0.05f * width;
    maxCooldown = Creator.Player.GetStepCount() * 0.25f;
    int step = PlanerCore.MaxRegenCooldown - Creator.Player.m_regenCooldown;
    step = step % PlanerCore.MaxRegenCooldown;
    hp = hp % PlanerCore.MaxRegenCooldown;
    if (hp != step)
    {
      curentCooldown += Time.deltaTime;
    }

    if (curentCooldown >= maxCooldown)
    {
      curentCooldown = 0;
      hp++;
    }

    GUI.BeginGroup(new Rect(Screen.width - width, 0, width, width));
    {

      float angle = ((hp + curentCooldown / maxCooldown) / PlanerCore.MaxRegenCooldown) * 360;

      GUIUtility.RotateAroundPivot(angle, new Vector2(width / 2, width / 2));
      GUI.DrawTexture(new Rect(width / 2 - smallWidth, smallWidth, 2 * smallWidth, width / 2 - 2 * smallWidth), hpCooldownArrow);
      GUIUtility.RotateAroundPivot(-angle, new Vector2(width / 2, width / 2));
      GUI.DrawTexture(new Rect(smallWidth, smallWidth, width - 2 * smallWidth, width - 2 * smallWidth), hpCooldownBackcround);
    }
    GUI.EndGroup();

  }
}
