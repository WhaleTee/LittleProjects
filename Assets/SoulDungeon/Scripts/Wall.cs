using UnityEngine;

[RequireComponent(typeof(CellMovement2D))]
public class Wall : MonoBehaviour, PositionProvider2D, ActionReceiver {
  private CellMovement2D moveBehaviour;

  private void Awake() {
    TryGetComponent(out moveBehaviour);
  }

  private bool Move(Vector2 direction) {
    var movable = SceneObjectFinder.FindObjectByPosition2D<CellMovement2D>(GetPosition() + direction);

    if (movable != null) {
      PlayPushNoMove();
      return false;
    }

    moveBehaviour.Move(direction);
    return true;
  }

  private void PlayPushNoMove() {
    Debug.Log("Push no move!");
  }

  public Vector2 GetPosition() => transform.position;
  public virtual PlayerActionType GetAction() => PlayerActionType.Push;

  public virtual bool ReceiveAction(PlayerActionType actionType, object ctx) {
    if (actionType == GetAction() && ctx is PushActionContext c) {
      return Move(c.direction);
    }

    return false;
  }
}