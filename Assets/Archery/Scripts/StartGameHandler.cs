using R3;
using UnityEngine;
using WhaleTee.Reactive.Runtime.Input;
using WhaleTee.Runtime.ServiceLocator;

public class StartGameHandler : MonoBehaviour {
  private UserInput userInput;

  private void Awake() {
    ServiceLocator.Global.TryGet(out userInput);

    userInput.Cancel
    .Where(cancel => cancel)
    .Subscribe(_ => userInput.SetCursorVisible(true, false));
  }

  private void Start() {
    userInput.SetCursorVisible(false, true);
  }
}