using UnityEngine;
using UnityEngine.AI;

public enum AIState {
  Idle,
  Walk
}


public class EnemyAI : MonoBehaviour {

  private EnemyMovement movement;

  [SerializeField] private AIState startState;
  [SerializeField] private Vector2 roamingDistance = new Vector2(3f, 7f);
  [SerializeField] private float roamingTimeMax = 2f;

  private AIState state;
  private float currentRoamingTime = 0f;
  private Vector3 roamingTargetPosition;

  private void Awake() {
    movement = GetComponent<EnemyMovement>();
    state = startState;
  }

  private void Update() {

    switch (state) {
    case AIState.Idle:
      break;

    case AIState.Walk:
      currentRoamingTime -= Time.deltaTime;
      if (currentRoamingTime < 0) {
        CalculateRoaming();
        currentRoamingTime = roamingTimeMax;
      }
      break;

    default:
      break;
    }
  }

  private void CalculateRoaming() {
    roamingTargetPosition = CalculateRoamingPosition();
    movement.MoveTo(roamingTargetPosition);
  }

  private Vector3 CalculateRoamingPosition() {
    return transform.position + Lib.RandomDirection() * Random.Range(roamingDistance.x, roamingDistance.y);
  }
}
