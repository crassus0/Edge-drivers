using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface IAutoMove 
{
  GraphNode GetNode();
  int Direction { get; set; }
  List<int> CanRotateWithTag(GraphNode node, int direction);
}
