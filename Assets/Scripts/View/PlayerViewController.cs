using UnityEngine;

public class PlayerViewController : MonoBehaviour {

  private Animator animator;
  private MovementBase movement;


  void Awake() {
    animator = GetComponent<Animator>();
    movement = GetComponentInParent<MovementBase>();
  }

  void Update() {
    animator.SetFloat(Constants.ANIMATION_SPEED, movement.Velocity.magnitude);
    animator.SetFloat(Constants.ANIMATION_VELOCITY_VERTICAL, movement.Velocity.y);
    animator.SetFloat(Constants.ANIMATION_VELOCITY_HORISONTAL, movement.Velocity.x);
    animator.SetFloat(Constants.ANIMATION_LOOK_VERTICAL, movement.LookDirection.y);
    animator.SetFloat(Constants.ANIMATION_LOOK_HORISONTAL, movement.LookDirection.x);
  }

}
