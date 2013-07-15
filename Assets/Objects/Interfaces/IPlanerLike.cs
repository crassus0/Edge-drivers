using UnityEngine;
using System.Collections;

public interface IPlanerLike:IAutoMove
{
  int MaxRotateAngle { get; }
  void SetNewDirection(int newDirection);
  void AddUpdateFunc(System.Action<IPlanerLike> action);
  void RemoveUpdateFunc(System.Action<IPlanerLike> action);
  float EntityValue(CustomObject entity);
}
