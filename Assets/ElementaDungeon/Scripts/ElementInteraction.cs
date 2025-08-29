using UnityEngine;

[CreateAssetMenu]
public class ElementInteraction : ScriptableObject {
  public ElementData sourceElement;
  public ElementData appliedElement;
  public ElementData resultElement;
  public InteractionEffect[] effects;
}