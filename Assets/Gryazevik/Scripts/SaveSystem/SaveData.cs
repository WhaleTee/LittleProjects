using UnityEngine;

public class SaveData {
  public PlayerData playerData;
  public SurfaceData surfaceData;
  
  
  
  public class PlayerData {
    public Vector3 position;
    public MoveDirection moveDirection;
  }
  
  public class SurfaceData {
    public bool[,] sticky;
  }
}