using UnityEngine;
using System.Collections;

public interface IAutoMove 
{
  GraphNode GetNode();
  int Direction { get; set; }
  bool CanRotateWithTag(NodeTag tag);
}
