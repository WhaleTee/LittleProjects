using UnityEngine;

public abstract class InteractionEffect : ScriptableObject {
  public abstract void ApplyEffect(ElementalCell targetCell);
}