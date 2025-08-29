using System;
using UnityEngine;

[Serializable]
public class DudeAnimations {
  public static readonly int idleUp = Animator.StringToHash("IdleUp");
  public static readonly int idleDown = Animator.StringToHash("IdleDown");
  public static readonly int idleLeft = Animator.StringToHash("IdleLeft");
  public static readonly int idleRight = Animator.StringToHash("IdleRight");
  public static readonly int moveUp = Animator.StringToHash("MoveUp");
  public static readonly int moveDown = Animator.StringToHash("MoveDown");
  public static readonly int moveLeft = Animator.StringToHash("MoveLeft");
  public static readonly int moveRight = Animator.StringToHash("MoveRight");
  public static readonly int rollUp = Animator.StringToHash("RollUp");
  public static readonly int rollDown = Animator.StringToHash("RollDown");
  public static readonly int rollLeft = Animator.StringToHash("RollLeft");
  public static readonly int rollRight = Animator.StringToHash("RollRight");

  public static int GetIdleAnimation(Vector2Int direction) {
    return direction.ToMoveDirection() switch {
             MoveDirection.N => idleUp,
             MoveDirection.S => idleDown,
             MoveDirection.W => idleLeft,
             MoveDirection.E => idleRight,
             var _ => idleDown
           };
  }

  public static int GetMoveAnimation(Vector2Int direction) {
    return direction.ToMoveDirection() switch {
             MoveDirection.N => moveUp,
             MoveDirection.S => moveDown,
             MoveDirection.W => moveLeft,
             MoveDirection.E => moveRight,
             var _ => moveDown
           };
  }

  public static int GetRollAnimation(Vector2Int direction) {
    return direction.ToMoveDirection() switch {
             MoveDirection.N => rollUp,
             MoveDirection.S => rollDown,
             MoveDirection.W => rollLeft,
             MoveDirection.E => rollRight,
             var _ => rollDown
           };
  }
}