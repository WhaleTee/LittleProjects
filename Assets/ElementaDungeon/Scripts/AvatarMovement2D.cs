using R3;
using Sirenix.OdinInspector;
using UnityEngine;
using WhaleTee.Reactive.Runtime.Input;
using WhaleTee.Runtime.ServiceLocator;

[RequireComponent(typeof(PlayerAvatar))]
public class AvatarMovement2D : CellMovement2D {
  [SerializeField] private MoveDirection sightDirectionAtStart = MoveDirection.E;
  [ShowInInspector] private MoveDirection currentSightDirection;
  private PlayerAvatar playerAvatar;
  private UserInput userInput;

  private void Awake() {
    currentSightDirection = sightDirectionAtStart;
    TryGetComponent(out playerAvatar);
    ServiceLocator.Global.TryGet(out userInput);
    userInput.Move.Where(value => value != Vector2.zero).Subscribe(Move);
  }

  public override void Move(Vector2 direction) {
    var moveDirection = direction.ToMoveDirection();

    if (moveDirection != currentSightDirection) {
      currentSightDirection = moveDirection;
      // todo: rotate avatar
      return;
    }
    
    var position = GetPosition() + direction;
    var cell = SceneObjectFinder.FindObjectByPosition2D<ElementalCell>(position);

    if (cell && cell.currentElement.IsFormAllowed(playerAvatar.form)) base.Move(direction);
  }

  public MoveDirection GetSightDirection() => currentSightDirection;
}