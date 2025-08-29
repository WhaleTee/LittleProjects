using UnityEngine;
using WhaleTee.Runtime.Extensions;

[RequireComponent(typeof(SpriteRenderer))]
public class ElementalCell : MonoBehaviour, PositionProvider2D {
  [SerializeField] private bool eraseInitElementOnInteraction;
  public ElementData initElement;
  public ElementData currentElement;
  private SpriteRenderer cellRenderer;

  private void Awake() {
    cellRenderer = GetComponent<SpriteRenderer>();
  }

  private void Start() => UpdateVisuals();

  public void ApplyElement(ElementData appliedElement) {
    if (appliedElement == null) return;

    var interaction = ElementSystem.Instance.GetInteraction(currentElement, appliedElement);

    if (interaction.OrNull() == null) return;

    if (eraseInitElementOnInteraction) initElement = null;
    currentElement = interaction.resultElement;

    foreach (var effect in interaction.effects) {
      effect.ApplyEffect(this);
    }

    UpdateVisuals();
  }

  private void UpdateVisuals() {
    cellRenderer.color = currentElement?.Color ?? Color.white;
  }

  public Vector2 GetPosition() => transform.position;
}