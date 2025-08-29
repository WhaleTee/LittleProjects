using System;
using UnityEngine;

namespace Match3Project.Scripts.Merge3 {
  [Serializable]
  public class LevelDataCell : Cell {
    [SerializeReference] private Gem gem;
    private int x, y;

    public LevelDataCell(int x, int y) {
      this.x = x;
      this.y = y;
    }

    public int GetX() => x;

    public int GetY() => y;

    // serialization purpose
    public void SetX(int x) => this.x = x;

    // serialization purpose
    public void SetY(int y) => this.y = y;
    
    public void SetXY(int x, int y) {
      this.x = x;
      this.y = y;
    }

    public void SetGem(Gem gem) => this.gem = gem;

    public Gem GetGem() => gem;

    public void DestroyGem() { }

    public Vector3 GetWorldPosition() => new Vector3(x, y);
  }
}