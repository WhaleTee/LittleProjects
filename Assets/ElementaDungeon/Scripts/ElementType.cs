public enum ElementType {
  Fire,
  Water,
  Air,
  Soil,
  Electricity,
  Lava,
  Ice,
  Steam
}

public static class ElementTypeExtensions {
  public static AvatarForm ToAvatarForm(this ElementType type) {
    return type switch {
             ElementType.Fire => AvatarForm.Fire,
             ElementType.Water => AvatarForm.Water,
             ElementType.Air => AvatarForm.Air,
             ElementType.Soil => AvatarForm.Soil,
             ElementType.Electricity => AvatarForm.Fire,
             ElementType.Lava => AvatarForm.Fire,
             ElementType.Ice => AvatarForm.Water,
             var _ => AvatarForm.Soil
           };
  }
}