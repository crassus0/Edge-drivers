using UnityEngine;
using System.Collections;

public class WeaponSelectButton : MonoBehaviour, IButton
{
  public int  index;
  public void OnPressed(bool isUp)
  {

    if (isUp)
    {
      Armory.AddWeapon(index);
    }
  }
}
