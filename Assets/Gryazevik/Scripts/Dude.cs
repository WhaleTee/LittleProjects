using DG.Tweening;
using R3;
using UnityEngine;
using WhaleTee.Reactive.Runtime.Input;
using WhaleTee.Runtime.Extensions;
using WhaleTee.Runtime.ServiceLocator;

public class Dude : MonoBehaviour {
  public Animator animator;
  private UserInput userInput;
  private Vector3 nextMove;
  private Tween moveTween;

  private void Awake() {
    ServiceLocator.Global.TryGet(out userInput);

    userInput.MoveUpdate.Where(value => value != default).Subscribe(value => nextMove = value.normalized.ToMoveDirection().ToVector3());
  }

  private void Update() {
    if (nextMove != default) {
      var nextPosition = transform.position + nextMove;

      var cell = PhysicsTools.GetCell(nextPosition);

      if (cell && cell.isFree && !moveTween.IsActive()) {
        var snowBalls = cell.GetSnowBalls();
        var nextCell = PhysicsTools.GetCell(nextPosition + nextMove);
        var nextSnowBall = nextCell ? nextCell.GetSnowBall() : null;
        var nextSnowBalls = nextCell ? nextCell.GetSnowBalls() : null;

        if (snowBalls is { Length: 1 }) {
          if (nextCell && nextCell.isFree) {
            var snowBall = snowBalls[0];

            if (nextSnowBalls is { Length: 2 } && nextSnowBalls[1].size > snowBall.size) {
              moveTween = transform.DOMove(nextPosition, .5f);
              snowBall.MoveToCell(nextCell, .5f);
            } else if (nextSnowBall && nextSnowBall.size > snowBall.size) {
              moveTween = transform.DOMove(nextPosition, .5f);
              snowBall.MoveToCell(nextCell, .5f);
            } else if (!nextSnowBall) {
              moveTween = transform.DOMove(nextPosition, .5f);
              snowBall.MoveToCell(nextCell, .5f);
            }
          }
        } else if (snowBalls is { Length: 2 }) {
          if (nextCell && nextCell.isFree && !nextSnowBall) {
            snowBalls[1].MoveToCell(nextCell, .5f);
          }
        } else moveTween = transform.DOMove(nextPosition, .5f);

        nextMove = default;
      }
    }
  }
}