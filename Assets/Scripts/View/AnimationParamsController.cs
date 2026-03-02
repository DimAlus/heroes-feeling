using UnityEngine;
using UnityEngine.AI;

public class AnimationParamsController : MonoBehaviour {


  private NavMeshAgent agent;
  [SerializeField] private Animator animator;

  private Vector2 lookDirection;


  private void Awake() {
    agent = GetComponent<NavMeshAgent>();
  }

  private void FixedUpdate() {
    if (agent.velocity.x != 0 || agent.velocity.y != 0) {
      lookDirection = agent.velocity.normalized;
    }
    //animator.SetFloat(Constants.ANIMATION_SPEED, agent.velocity.magnitude);
    animator.SetFloat(Constants.ANIMATION_VELOCITY_HORISONTAL, agent.velocity.x);
    animator.SetFloat(Constants.ANIMATION_VELOCITY_VERTICAL, agent.velocity.y);
    animator.SetFloat(Constants.ANIMATION_LOOK_HORISONTAL, lookDirection.x);
    animator.SetFloat(Constants.ANIMATION_LOOK_VERTICAL, lookDirection.y);

  }
}
