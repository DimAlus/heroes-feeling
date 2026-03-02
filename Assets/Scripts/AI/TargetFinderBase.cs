using UnityEngine;

public class TargetFinderBase : MonoBehaviour {

  protected MovementBase movement;

  protected virtual void Awake() {
    movement = GetComponent<MovementBase>();
  }


  public virtual Vector3 FindTarget(EAbilityTarget target, EAbilityTargetFinder finder) {
    if (finder == EAbilityTargetFinder.LookDirection) {
      return transform.position + (Vector3)movement.LookDirection;
    } else if (finder == EAbilityTargetFinder.MovementDirection) {
      return transform.position + (Vector3)movement.Velocity;
    }
    return Vector3.zero;
  }
}
