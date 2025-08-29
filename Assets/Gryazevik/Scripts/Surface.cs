using UnityEngine;

public enum SurfaceType {
  Fire, Water, Air, Soil, Electricity, Lava, Ice
}

public class Surface : MonoBehaviour, PositionProvider2D {
  [SerializeField] private SurfaceType type;
  
  public Vector2 GetPosition() => transform.position;
}