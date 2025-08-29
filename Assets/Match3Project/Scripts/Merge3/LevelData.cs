using System.Collections.Generic;
using UnityEngine;

namespace Match3Project.Scripts.Merge3 {
  [CreateAssetMenu(fileName = "Match3LevelData", menuName = "Scriptable Objects/Match3LevelData", order = 0)]
  public class LevelData : ScriptableObject {
    [field: SerializeField] public int MaxMoves { get; private set; }
    [field: SerializeField] public int Width { get; private set; }
    [field: SerializeField] public int Height { get; private set; }
    [field: SerializeReference] public List<GemData> GemList { get; private set; } = new List<GemData>();
    [field: SerializeReference] public List<LevelDataCell> Cells { get; private set; } = new List<LevelDataCell>();
  }
}