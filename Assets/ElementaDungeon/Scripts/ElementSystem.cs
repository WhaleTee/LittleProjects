using System.Collections.Generic;
using UnityEngine;

public class ElementSystem : PersistentSingleton<ElementSystem> {
  [SerializeField] private List<ElementInteraction> interactions;
  private Dictionary<ElementPair, ElementInteraction> interactionMap;

  protected override void Awake() {
    base.Awake();
    InitializeDictionary();
  }

  private void InitializeDictionary() {
    interactionMap = new Dictionary<ElementPair, ElementInteraction>();

    foreach (var interaction in interactions) {
      var key = new ElementPair(interaction.sourceElement, interaction.appliedElement);
      interactionMap[key] = interaction;
    }
  }

  public ElementInteraction GetInteraction(ElementData source, ElementData applied) {
    interactionMap.TryGetValue(new ElementPair(source, applied), out var result);
    return result;
  }
}