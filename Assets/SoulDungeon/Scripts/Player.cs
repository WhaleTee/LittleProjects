using System.Collections;
using R3;
using UnityEngine;
using WhaleTee.Reactive.Runtime.Input;
using WhaleTee.Runtime.Extensions;
using WhaleTee.Runtime.ServiceLocator;

[RequireComponent(typeof(CellMovement2D))]
public class Player : MonoBehaviour, PositionProvider2D, ActionProvider {
  private CellMovement2D moveBehaviour;
  private UserInput userInput;
  public Friend friend;

  private void Awake() {
    TryGetComponent(out moveBehaviour);
    ServiceLocator.Global.TryGet(out userInput);
    userInput.Move.Where(value => value != Vector2.zero).Subscribe(TryMove);
  }

  private void TryMove(Vector2 direction) {
    var position = GetPosition() + direction;
    var surface = SceneObjectFinder.FindObjectByPosition2D<Surface>(position);
    var receiver = SceneObjectFinder.FindObjectByPosition2DAndType<ActionReceiver>(position);

    if (surface.OrNull() != null) return;

    if (receiver != null) {
      if (receiver.GetAction() == GetAction()) {
        if (receiver.ReceiveAction(GetAction(), new PushActionContext { direction = direction })) MoveDirection(direction);
      } else if (receiver.GetAction() == PlayerActionType.PushTogether) {
        StartCoroutine(PushTogether(receiver, direction));
      }
    } else MoveDirection(direction);
  }

  private IEnumerator PushTogether(ActionReceiver receiver, Vector2 direction) {
    const float DURATION = 2f;
    var time = 0f;

    while (time < DURATION) {
      if (userInput.Move.Value != direction) yield break;

      var delta = Time.deltaTime;
      time += delta;
      Debug.Log(time);
      yield return null;
    }

    if (receiver.ReceiveAction(PlayerActionType.PushTogether, new PushActionContext { direction = direction })) MoveDirection(direction);
  }

  private void MoveDirection(Vector2 direction) {
    friend?.Move(direction);
    moveBehaviour.Move(direction);
  }

  public Vector2 GetPosition() => transform.position;

  public PlayerActionType GetAction() => PlayerActionType.Push;
}