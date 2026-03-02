using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MovementBase {


  protected override void MovementTick(float deltaTime) {
    base.MovementTick(deltaTime);

    if (!effectHaver.HasEffect(EEffectType.MovementLock)) {
      Vector2 movementDirection = Inputs.Instance.PlayerMove;
      Velocity = movementDirection * MovementSpeed;
      rb.MovePosition(rb.position + Velocity * deltaTime);
    }
    if (!effectHaver.HasEffect(EEffectType.LookLock)) {
      LookDirection = (Mouse.current.position.ReadValue() - (Vector2)Camera.main.WorldToScreenPoint(rb.position)).normalized;
    }
  }
}
