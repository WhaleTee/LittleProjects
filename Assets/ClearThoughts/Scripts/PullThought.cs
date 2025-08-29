using DG.Tweening;
using UnityEngine;
using WhaleTee.Reactive.Runtime.Input;
using WhaleTee.Runtime.Extensions;
using WhaleTee.Runtime.ServiceLocator;

[RequireComponent(typeof(Animator))]
public class PullThought : MonoBehaviour {
  public float offsetToMakeLong = .5f;
  private Animator animator;
  private UserInput userInput;
  private bool canDrag = true;
  private bool isDragging;
  private DirtThoughts dirtThoughts;
  private bool parentChanged;
  public Vector2 offset = Vector2.zero;

  private void Awake() {
    animator = GetComponent<Animator>();
    dirtThoughts = GetComponentInParent<DirtThoughts>();
    ServiceLocator.Global.TryGet(out userInput);
  }

  public void OnMouseDown() {
    if (canDrag) isDragging = true;
  }

  private void OnMouseDrag() {
    if (canDrag) transform.position = Vector2.Lerp(transform.position, userInput.GetMousePositionWorld(), .75f);

    if ((transform.position - dirtThoughts.transform.position).magnitude > offsetToMakeLong) {
      animator.Play(PullThoughtAnimations.longAnimation);
    }
  }

  public void OnMouseUp() {
    if (isDragging) {
      var shelf = PhysicsTools.GetOverlapComponent<Shelf>(userInput.GetMousePositionWorld(), LayerMask.NameToLayer("Shelf"));
      var localMove = new Vector3(0, 0f);

      if (shelf && shelf.canPlaceThoughts) {
        gameObject.ReParent(shelf.transform);
        localMove = new Vector3(0, 2f);
        dirtThoughts.thoughtsCount.Value--;
        parentChanged = true;
        shelf.canPlaceThoughts = false;
      }

      transform.DOLocalMove(localMove, 1f)
      .OnComplete(() => {
                    if (!parentChanged) {
                      animator.Play(PullThoughtAnimations.shortAnimation);
                      canDrag = true;
                    }
                  }
      );

      canDrag = false;
      isDragging = false;
    }
  }
}