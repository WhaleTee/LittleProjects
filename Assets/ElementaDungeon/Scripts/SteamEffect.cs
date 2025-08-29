using UnityEngine;

[CreateAssetMenu]
public class SteamEffect : InteractionEffect {
  [SerializeField] private ElementData steamElement;
  public override void ApplyEffect(ElementalCell targetCell) {
    // var effect = Instantiate(steamElement.effectPrefab, targetCell.transform);
  }
}