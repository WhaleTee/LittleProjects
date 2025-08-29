using UnityEngine;

public abstract class Obstacle : MonoBehaviour {
  public abstract bool SolveObstacle(ElementData elementData);
}