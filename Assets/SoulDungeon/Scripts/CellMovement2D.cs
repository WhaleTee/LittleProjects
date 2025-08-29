using DG.Tweening;
using UnityEngine;

public class CellMovement2D : MonoBehaviour, PositionProvider2D {
  [SerializeField] private float duration = .1f;
  [SerializeField] private bool snapMovement;
  private Vector2 nextMove;
  private bool moving;
  private Tween moveTween;

  public virtual void Move(Vector2 direction) {
    if (direction == Vector2.zero || direction.ToMoveDirection() == MoveDirection.NoDirection) return;

    if (moving) {
      nextMove = direction;
      return;
    }

    moving = true;

    moveTween = transform.DOMove(GetPosition() + direction, duration, snapMovement)
    .SetEase(Ease.InQuad)
    .OnComplete(() => {
                  moving = false;

                  if (nextMove != Vector2.zero) {
                    Move(nextMove);
                    nextMove = Vector2.zero;
                  }
                }
    );
  }
  
  public Vector2 GetPosition() => transform.position;
}