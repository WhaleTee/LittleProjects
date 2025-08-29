public class WallObstacle : Obstacle {
  public override bool SolveObstacle(ElementData elementData) => elementData.Type == ElementType.Electricity;
}