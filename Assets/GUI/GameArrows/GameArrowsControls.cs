using UnityEngine;
using System.Collections;

public class GameArrowsControls : MonoBehaviour
{
  public GameObject[] m_arrows;
  int m_active = -1;
  public bool IsActive = false;

  void Start()
  {

  }
  public void Appear()
  {
   IsActive = true;
    animation.Play("Appear");
  }
  public void Disappear()
  {
    IsActive = false;
    animation.Play("Disappear");
  }
  public void SetActive(int index)
  {
    //Debug.Log("activated");
    if (m_active >= 0 && m_active != index && index >= 0)
    {
      m_arrows[m_active].animation.Play("Deactivate");
      m_arrows[index].animation.Play("Activate");
      m_active = index;
    }
    if (m_active < 0 && index >= 0)
    {
      m_arrows[index].animation.Play("Activate");
      m_active = index;
    }
    if (index == -1 && m_active >= 0)
    {
      m_arrows[m_active].animation.Play("Deactivate");
      m_active = -1;
    }
  }
  void OnDisapperaed()
  {
    if (m_active >= 0)
      m_arrows[m_active].animation.Play("Deactivate");
    m_active = -1;
  }
}
