using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Cell : MonoBehaviour {

  public bool isFree = true;
  public bool hasSnow = true;
  private SpriteRenderer spriteRenderer;

  private void Awake() {
    spriteRenderer = GetComponent<SpriteRenderer>();
  }

  public SnowBall GetSnowBall() => GetComponentInChildren<SnowBall>();
  public SnowBall[] GetSnowBalls() => GetComponentsInChildren<SnowBall>();
}