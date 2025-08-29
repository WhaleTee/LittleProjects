using System.Collections.Generic;
using R3;
using UnityEngine;
using WhaleTee.Reactive.Runtime.Input;
using WhaleTee.Runtime.ServiceLocator;

public class ClearThoughts : MonoBehaviour {
  public GameObject slider;
  public Vector2[] upPoints;
  public Vector2[] downPoints;
  public Vector2[] leftPoints;
  public Vector2[] rightPoints;

  public ReactiveProperty<float> percentage = new ReactiveProperty<float>(0);
  private Queue<MoveDirection> moveDirections = new Queue<MoveDirection>();
  private UserInput userInput;
  public MoveDirection activeDirection;

  private void Awake() {
    ServiceLocator.Global.TryGet(out userInput);
  }

  private void Start() {
    SetUpDirections();
    activeDirection = moveDirections.Dequeue();
    percentage.Subscribe(value => { Debug.Log($"{activeDirection} {value}"); });
  }

  private void SetUpDirections() {
    moveDirections.Enqueue(MoveDirection.E);
    moveDirections.Enqueue(MoveDirection.S);
    moveDirections.Enqueue(MoveDirection.W);
    moveDirections.Enqueue(MoveDirection.N);
  }

  private float time;
  private void Update() {
    if (userInput.Move.Value.ToMoveDirection() != activeDirection) percentage.Value = Mathf.Clamp01(percentage.Value - Time.deltaTime);
    else if (userInput.Move.Value.ToMoveDirection() == activeDirection) percentage.Value = Mathf.Clamp01(percentage.Value + Time.deltaTime);

    if (percentage.Value >= 1) {
      percentage.Value = 0;

      if (moveDirections.Count == 0) {
        SetUpDirections();
      }

      activeDirection = moveDirections.Dequeue();
    }

    GetDestination(out var one, out var two);
    slider.transform.position = GetPositionByPercentage(one, two);

    if (userInput.Move.Value != Vector2.zero) {
      time += Time.deltaTime;
      Debug.Log(time);
    }
    else time = 0f;
  }

  private void GetDestination(out Vector2 pointOne, out Vector2 pointTwo) {
    pointOne = activeDirection switch {
                 MoveDirection.N => upPoints[0],
                 MoveDirection.S => downPoints[0],
                 MoveDirection.W => leftPoints[0],
                 MoveDirection.E => rightPoints[0],
                 _ => Vector2.zero
               };

    pointTwo = activeDirection switch {
                 MoveDirection.N => upPoints[1],
                 MoveDirection.S => downPoints[1],
                 MoveDirection.W => leftPoints[1],
                 MoveDirection.E => rightPoints[1],
                 _ => Vector2.zero
               };
  }

  private Vector2 GetPositionByPercentage(Vector2 pointOne, Vector2 pointTwo) {
    var x = Mathf.Lerp(pointOne.x, pointTwo.x, percentage.Value);
    var y = Mathf.Lerp(pointOne.y, pointTwo.y, percentage.Value);
    return new Vector2(x, y);
  }

  private void OnDrawGizmos() {
    Gizmos.color = Color.red;

    foreach (var point in upPoints) {
      Gizmos.DrawSphere(point, .1f);
    }

    foreach (var point in downPoints) {
      Gizmos.DrawSphere(point, .1f);
    }

    foreach (var point in leftPoints) {
      Gizmos.DrawSphere(point, .1f);
    }

    foreach (var point in rightPoints) {
      Gizmos.DrawSphere(point, .1f);
    }
  }
}