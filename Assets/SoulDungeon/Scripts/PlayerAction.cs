using System;

public class PlayerAction<T> : ActionProvider {
  private PlayerActionType actionType;

  public PlayerAction(PlayerActionType actionType, Action<T> subscription) {
    this.actionType = actionType;
  }

  public PlayerActionType GetAction() => actionType;
}