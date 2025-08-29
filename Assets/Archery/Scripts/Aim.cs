using System.Collections;
using R3;
using UnityEngine;
using WhaleTee.Reactive.Runtime.Input;
using WhaleTee.Runtime.Coroutine;
using WhaleTee.Runtime.Extensions;
using WhaleTee.Runtime.ServiceLocator;
using Random = UnityEngine.Random;

public class Aim : MonoBehaviour {
  [SerializeField] private float mouseFollowModifier = 1;
  [SerializeField] private float rechargeTime = 7;
  [SerializeField] private Vector2 initPosition = new Vector2(1, -6.5f);
  [SerializeField] private Vector2 centerPosition = new Vector2(.93f, -1.23f);

  private UserInput userInput;

  private float rechargeTimeElapsed;

  private void Awake() {
    ServiceLocator.Global.TryGet(out userInput);

    userInput.Click.Where(click => click)
    .Subscribe(
      _ => {
        if (rechargeTimeElapsed != 0f) return;

        StartCoroutine(MoveToPosition(centerPosition, 1.7f));
        StartCoroutine(MoveRandomVectorForTime(10f));
      }
    );

    userInput.Click.Where(click => !click)
    .Subscribe(
      _ => {
        if (transform.position.Approximately(initPosition, .01f)) return;
        StartCoroutine(RechargeShoot());
        StartCoroutine(MoveToPosition(initPosition, 2f));
      }
    );
  }

  private IEnumerator MoveRandomVectorForTime(float time) {
    var moveVector = new Vector3(Random.Range(-.5f, 1), Random.Range(-.5f, 1));
    Debug.Log(moveVector);

    yield return CoroutineTool.Lerp(
      this,
      time,
      _ => {
        if (userInput.Click.Value) {
          transform.position += moveVector * (mouseFollowModifier * Time.deltaTime * time)
                                + userInput.MousePositionDeltaUpdate.Value.With(z: 0) * (mouseFollowModifier * Time.deltaTime);
        }
      }
    );

    if (userInput.Click.Value) StartCoroutine(MoveRandomVectorForTime(time));
  }

  private IEnumerator MoveToPosition(Vector2 position, float time) {
    Vector2 currentPosition = transform.position;

    yield return CoroutineTool.Lerp(
      this,
      time,
      t => { transform.position = Vector2.Lerp(currentPosition, position, t); }
    );
  }

  private IEnumerator RechargeShoot() {
    while (rechargeTimeElapsed < rechargeTime) {
      rechargeTimeElapsed += Time.deltaTime;
      yield return null;
    }

    rechargeTimeElapsed = 0f;
  }
}