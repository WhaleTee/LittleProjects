using DG.Tweening;
using UnityEngine;
using WhaleTee.Runtime.Extensions;

public class SnowBall : MonoBehaviour {
  public float step = 1.5f;
  public int size = 1;

  public void MoveToCell(Cell cell, float duration) {
    if (cell.GetSnowBall()) {
      if (cell.GetSnowBalls() is { Length: 2 }) {
        gameObject.ReParent(cell.transform);
        transform.DOLocalMove(new Vector3(0, .75f), duration);
        cell.isFree = false;
      } else {
        gameObject.ReParent(cell.transform);
        transform.DOLocalMove(new Vector3(0,.5f), duration); 
      }
    } else {
      gameObject.ReParent(cell.transform);
      if (cell.hasSnow && size < 3) {
        transform.DOScale(transform.localScale * step, duration);
        size++;
      }
      transform.DOLocalMove(Vector3.zero, duration);
    }

    cell.hasSnow = false;
  }
}