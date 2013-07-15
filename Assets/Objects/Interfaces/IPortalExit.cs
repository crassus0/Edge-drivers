using UnityEngine;
using System.Collections;

public interface IPortalExit
{
  int Direction { get; }
  GraphNode GetNode();
}
