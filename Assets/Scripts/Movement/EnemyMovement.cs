using System;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MovementBase {

  private NavMeshAgent agent;
  //private Vector3 targetPosition;

  protected override void Awake() {
    base.Awake();
    agent = GetComponent<NavMeshAgent>();
    agent.updateRotation = false;
    agent.updateUpAxis = false;
    agent.speed = MovementSpeed;
  }

  protected override void Start() {
    base.Start();
    //targetPosition = transform.position;
  }

  protected override void MovementTick(float deltaTime) {
    //base.MovementTick(deltaTime);
    //if (effectHaver.HasEffect(EEffectType.MovementLock)) {
    //  Velocity = Vector2.zero;
    //} else {
    //  Velocity = (targetPosition - transform.position);
    //  if (Velocity.magnitude > 0.01f) {
    //    Velocity = Velocity.normalized * MovementSpeed;
    //  }
    //}
  }

  internal void MoveTo(Vector3 roamingTargetPosition) {
    //targetPosition = roamingTargetPosition;
    //agent.SetDestination(roamingTargetPosition);
    //LookDirection = (targetPosition - transform.position).normalized;
  }
}
