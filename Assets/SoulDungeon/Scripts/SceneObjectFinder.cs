using UnityEngine;
using WhaleTee.Runtime.Extensions;

public static class SceneObjectFinder {
  public static T FindObjectByPosition2D<T>(Vector2 position, float approximation = 0.1f) where T : Object {
    foreach (var obj in Object.FindObjectsByType<T>(FindObjectsSortMode.None)) {
      if (obj is PositionProvider2D pp && pp.GetPosition().Approximately(position, approximation)) return obj;
    }

    return null;
  }
  
  public static T FindObjectByPosition2DAndType<T>(Vector2 position, float approximation = 0.1f) {
    foreach (var obj in Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)) {
      if (obj is PositionProvider2D pp && pp.GetPosition().Approximately(position, approximation) && obj is T t) return t;
    }

    return default;
  }
}