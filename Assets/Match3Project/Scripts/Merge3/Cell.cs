using UnityEngine;

namespace Match3Project.Scripts.Merge3 {
  public interface Cell {
    public bool IsEmpty => GetGem() == null;
    public bool HasGem => !IsEmpty;
    
    public int GetX();
    
    public int GetY();
    
    public void SetXY(int x, int y);
    
    public void SetGem(Gem gem);

    public Gem GetGem();

    public Vector3 GetWorldPosition();
  }
}