using R3;
using UnityEngine;
using WhaleTee.Reactive.Runtime.Input;
using WhaleTee.Runtime.Coroutine;
using WhaleTee.Runtime.Extensions;
using WhaleTee.Runtime.ServiceLocator;

[RequireComponent(typeof(AvatarMovement2D))]
[RequireComponent(typeof(ElementHolder))]
public class PlayerAvatar : MonoBehaviour, PositionProvider2D {
  private AvatarMovement2D moveBehaviour;
  private ElementHolder elementHolder;
  private UserInput userInput;
  public AvatarForm form;

  private void Awake() {
    TryGetComponent(out moveBehaviour);
    TryGetComponent(out elementHolder);
    ServiceLocator.Global.TryGet(out userInput);
    userInput.RightClick.Where(value => value).Subscribe(_ => ApplyElement());
  }

  private void Start() {
    CoroutineTool.Lerp(
      this,
      1,
      t => {
        if (t >= 1) {
          var cell = SceneObjectFinder.FindObjectByPosition2DAndType<ElementalCell>(transform.position);
          if (cell.OrNull() != null) form = cell.currentElement.Type.ToAvatarForm();
        }
      }
    );
  }

  private void ApplyElement() {
    var cell = SceneObjectFinder.FindObjectByPosition2D<ElementalCell>(GetPosition() + moveBehaviour.GetSightDirection().ToVector2());
    var element = elementHolder.PopElement();

    if (cell) cell.ApplyElement(element);
  }

  public Vector2 GetPosition() => transform.position;
}