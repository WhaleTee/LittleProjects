using UnityEngine;
using WhaleTee.Runtime.Extensions;

public static class PhysicsTools {
  private static readonly int cellLayer = LayerMask.NameToLayer("Cell");
  private static readonly int snowBallLayer = LayerMask.NameToLayer("SnowBall");

  public static Cell GetCell(Vector2 position) {
    var ray = Camera.main.ScreenPointToRay(Camera.main.WorldToScreenPoint(position));
    var hit = Physics2D.GetRayIntersection(ray, float.MaxValue, cellLayer.GetMask());
    return hit ? hit.collider.GetComponent<Cell>() : null;
  }

  public static SnowBall GetSnowBall(Vector2 position) {
    var hit = Physics2D.OverlapArea(position, Vector2.positiveInfinity, snowBallLayer.GetMask());
    return hit ? hit.GetComponent<SnowBall>() : null;
  }
  
  public static T GetOverlapComponent<T>(Vector2 worldPosition, int layerMask) where T : Component {
    var hit = Physics2D.OverlapPoint(worldPosition, layerMask.GetMask());
    return hit ? hit.GetComponent<T>() : null;
  }
}