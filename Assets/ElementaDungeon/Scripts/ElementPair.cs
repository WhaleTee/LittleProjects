public struct ElementPair {
  public readonly ElementData source;
  public readonly ElementData applied;

  public ElementPair(ElementData source, ElementData applied) {
    this.source = source;
    this.applied = applied;
  }
}