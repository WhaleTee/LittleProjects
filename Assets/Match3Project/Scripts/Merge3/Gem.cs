using System;
using UnityEngine;
using WhaleTee.Runtime.Extensions;

namespace Match3Project.Scripts.Merge3 {
  public struct GemDestroyedEvent {
    public int x;
    public int y;
  }

  [Serializable]
  public class Gem : IEquatable<Gem> {
    [SerializeField] private GemData gemData;
    
    public GemData GetGemData() => gemData;
    
    public void SetGemData(GemData gemData) => this.gemData = gemData;

    public Gem(GemData gemData) => this.gemData = gemData;

    public override bool Equals(object obj) => obj is Gem other && Equals(other);

    public bool Equals(Gem other) => other is not null && gemData == other.gemData;

    public override int GetHashCode() => gemData.OrNull()?.GetHashCode() ?? 0;

    public static bool operator ==(Gem left, Gem right) => left is not null && left.Equals(right);

    public static bool operator !=(Gem left, Gem right) => !(left == right);
  }
}