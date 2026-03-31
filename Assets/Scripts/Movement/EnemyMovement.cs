using System;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MovementBase {

  private NavMeshAgent agent;
  //private Vector3 targetPosition;

  private float agentDisabledDuration = 0;

  protected override void Awake() {
    base.Awake();
    agent = GetComponent<NavMeshAgent>();
    agent.updateRotation = false;
    agent.updateUpAxis = false;
  }

  protected override void Start() {
    base.Start();
    agent.speed = MovementSpeed;
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


  public override void AddImpulse(Vector2 direction, float power) {
    agentDisabledDuration = 0.3f;
    if (agent.enabled) {
      StartCoroutine(DisableAgent());
    }
    base.AddImpulse(direction, power);
  }


  private System.Collections.IEnumerator DisableAgent() {
    agent.enabled = false;

    while ((agentDisabledDuration -= Time.deltaTime) > 0) {
      yield return null;
    }

    rb.totalForce = Vector2.zero;
    agent.enabled = true;
    agent.Warp(transform.position);
  }
}
