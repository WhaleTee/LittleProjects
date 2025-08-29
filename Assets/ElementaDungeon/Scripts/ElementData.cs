using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ElementData : ScriptableObject {
  [field: SerializeField] public ElementType Type { get; private set; }
  [field: SerializeField] public Color Color { get; private set; }
  [field: SerializeField] public GameObject EffectPrefab { get; private set; }
  
  [SerializeReference] private HashSet<AvatarForm> allowedForms;

  public bool IsFormAllowed(AvatarForm form) => allowedForms.Contains(form);
}