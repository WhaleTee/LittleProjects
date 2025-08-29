using UnityEngine;

namespace Match3Project.Scripts.Merge3 {
  [CreateAssetMenu(fileName = "Gem", menuName = "Scriptable Objects/Gem", order = 0)]
  public class GemData : ScriptableObject {
    [field: SerializeField] public Sprite Sprite { get; private set; }
  }
}