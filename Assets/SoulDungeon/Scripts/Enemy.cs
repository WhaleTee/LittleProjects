using UnityEngine;

[RequireComponent(typeof(CellMovement2D))]
public class Enemy : MonoBehaviour, PositionProvider2D, ActionReceiver {
  private CellMovement2D moveBehaviour;

  private void Awake() {
    TryGetComponent(out moveBehaviour);
  }

  public void Move(Vector2 direction) {
    var findPosition = GetPosition() + direction;
    Debug.Log(findPosition);
    var wall = SceneObjectFinder.FindObjectByPosition2D<Wall>(findPosition);

    if (wall) {
      PlayDeath();
    } else moveBehaviour.Move(direction);
  }

  private void PlayDeath() {
    Debug.Log("Death comes!");
    Destroy(gameObject);
  }

  public Vector2 GetPosition() => transform.position;

  public bool ReceiveAction(PlayerActionType actionType, object ctx) {
    if (actionType == GetAction() && ctx is PushActionContext c) {
      Move(c.direction);
      return true;
    }

    return false;
  }

  public PlayerActionType GetAction() => PlayerActionType.Push;
}