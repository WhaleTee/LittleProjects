using UnityEngine;
using UnityEngine.Serialization;
using WhaleTee.Runtime.Extensions;

[RequireComponent(typeof(CellMovement2D))]
public class Friend : MonoBehaviour, PositionProvider2D, ActionReceiver {
  [FormerlySerializedAs("playerAction")] [SerializeField] private PlayerActionType playerActionType;
  private CellMovement2D moveBehaviour;
  private Player player;

  private void Awake() {
    TryGetComponent(out moveBehaviour);
  }

  public void Move(Vector2 direction) {
    if (player.OrNull() != null) moveBehaviour.Move(direction);
  }

  public Vector2 GetPosition() => transform.position;

  public PlayerActionType GetAction() => PlayerActionType.MakeFriendship;

  public bool ReceiveAction(PlayerActionType actionType, object ctx) {
    if (actionType is PlayerActionType.MakeFriendship && ctx is Player p) {
      player = p;
      p.friend = this;
      return true;
    }

    return false;
  }
}