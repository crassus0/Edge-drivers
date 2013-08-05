using UnityEngine;
using System.Collections;

public interface IActivatable
{
  void Activate();
  bool ActivateOnStart { get; }
}
