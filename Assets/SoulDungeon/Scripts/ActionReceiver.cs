public interface ActionReceiver : ActionProvider {
  bool ReceiveAction(PlayerActionType actionType, object ctx);
}