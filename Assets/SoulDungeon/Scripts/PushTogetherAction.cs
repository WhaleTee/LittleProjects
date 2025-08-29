using System;
using R3;
using UnityEngine;
using WhaleTee.Reactive.Runtime.Input;
using WhaleTee.Runtime.ServiceLocator;

public class PushTogetherAction : PlayerAction<Vector2> {
  private UserInput userInput;
  public PushTogetherAction(PlayerActionType actionType, Action<Vector2> subscription) : base(actionType, subscription) {
    ServiceLocator.Global.TryGet(out userInput);
    userInput.Move.Where(value => value != Vector2.zero).Subscribe(subscription);
  }
}