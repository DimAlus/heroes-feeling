using System;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using UnityEngine.AI;
using Action = Unity.Behavior.Action;


[Serializable, GeneratePropertyBag]
[NodeDescription(
        name: "Navigate To Target 2D",
        description: "Navigates a GameObject towards another GameObject using NavMeshAgent.",
        story: "[Agent] navigates to [Target]",
        category: "Action/Navigation",
        id: "9aa5120ba4ea66807cc71d7f9255036f")]
public partial class NavigateToTarget2DAction : Action {

  [SerializeReference] public BlackboardVariable<GameObject> Agent;
  [SerializeReference] public BlackboardVariable<GameObject> Target;
  [SerializeReference] public BlackboardVariable<float> Speed = new BlackboardVariable<float>(1.0f);
  [SerializeReference] public BlackboardVariable<float> DistanceThreshold = new BlackboardVariable<float>(0.2f);

  // This will only be used in movement without a navigation agent.
  [SerializeReference] public BlackboardVariable<float> SlowDownDistance = new BlackboardVariable<float>(1.0f);

  private NavMeshAgent m_NavMeshAgent;
  private Animator m_Animator;
  private Vector3 m_LastTargetPosition;
  private Vector3 m_ColliderAdjustedTargetPosition;
  [CreateProperty] private float m_OriginalStoppingDistance = -1f;
  [CreateProperty] private float m_OriginalSpeed = -1f;
  private float m_ColliderOffset;
  private float m_CurrentSpeed;

  private float previousSpeed;

  protected override Status OnStart() {
    if (Agent.Value == null || Target.Value == null) {
      return Status.Failure;
    }

    return Initialize();
  }

  protected override Status OnUpdate() {
    if (Agent.Value == null || Target.Value == null) {
      return Status.Failure;
    }

    if (previousSpeed != Speed) {
      m_NavMeshAgent.speed = previousSpeed = Speed;
    }

    // Check if the target position has changed.
    bool boolUpdateTargetPosition = !Mathf.Approximately(m_LastTargetPosition.x, Target.Value.transform.position.x)
                || !Mathf.Approximately(m_LastTargetPosition.y, Target.Value.transform.position.y)
                || !Mathf.Approximately(m_LastTargetPosition.z, Target.Value.transform.position.z);

    if (boolUpdateTargetPosition) {
      m_LastTargetPosition = Target.Value.transform.position;
      m_ColliderAdjustedTargetPosition = GetPositionColliderAdjusted();
    }

    float distance = GetDistanceXY();
    bool destinationReached = distance <= (DistanceThreshold + m_ColliderOffset);

    if (destinationReached && (m_NavMeshAgent == null || !m_NavMeshAgent.pathPending)) {
      return Status.Success;
    } else if (m_NavMeshAgent == null) // transform-based movement
      {
      m_CurrentSpeed = SimpleMoveTowardsLocation(distance);
    } else if (boolUpdateTargetPosition) // navmesh-based destination update (if needed)
      {
      m_NavMeshAgent.SetDestination(m_ColliderAdjustedTargetPosition);
    }

    UpdateAnimatorSpeed();

    return Status.Running;
  }

  protected override void OnEnd() {
    UpdateAnimatorSpeed(0f);

    if (m_NavMeshAgent != null) {
      if (m_NavMeshAgent.isOnNavMesh) {
        m_NavMeshAgent.ResetPath();
      }
      m_NavMeshAgent.speed = m_OriginalSpeed;
      m_NavMeshAgent.stoppingDistance = m_OriginalStoppingDistance;
    }

    m_NavMeshAgent = null;
    m_Animator = null;
  }

  protected override void OnDeserialize() {
    // If using a navigation mesh, we need to reset default value before Initialize.
    m_NavMeshAgent = Agent.Value.GetComponentInChildren<NavMeshAgent>();
    if (m_NavMeshAgent != null) {
      if (m_OriginalSpeed >= 0f)
        m_NavMeshAgent.speed = m_OriginalSpeed;
      if (m_OriginalStoppingDistance >= 0f)
        m_NavMeshAgent.stoppingDistance = m_OriginalStoppingDistance;

      m_NavMeshAgent.Warp(Agent.Value.transform.position);
    }

    Initialize();
  }

  private Status Initialize() {
    m_LastTargetPosition = Target.Value.transform.position;
    m_ColliderAdjustedTargetPosition = GetPositionColliderAdjusted();

    // Add the extents of the colliders to the stopping distance.
    m_ColliderOffset = 0.0f;
    Collider agentCollider = Agent.Value.GetComponentInChildren<Collider>();
    if (agentCollider != null) {
      Vector3 colliderExtents = agentCollider.bounds.extents;
      m_ColliderOffset += Mathf.Max(colliderExtents.x, colliderExtents.z);
    }

    if (GetDistanceXY() <= (DistanceThreshold + m_ColliderOffset)) {
      return Status.Success;
    }

    // If using a navigation mesh, set target position for navigation mesh agent.
    m_NavMeshAgent = Agent.Value.GetComponentInChildren<NavMeshAgent>();
    if (m_NavMeshAgent != null) {
      if (m_NavMeshAgent.isOnNavMesh) {
        m_NavMeshAgent.ResetPath();
      }

      m_OriginalSpeed = m_NavMeshAgent.speed;
      m_NavMeshAgent.speed = previousSpeed = Speed;
      m_OriginalStoppingDistance = m_NavMeshAgent.stoppingDistance;
      m_NavMeshAgent.stoppingDistance = DistanceThreshold + m_ColliderOffset;
      m_NavMeshAgent.SetDestination(m_ColliderAdjustedTargetPosition);
    }

    m_Animator = Agent.Value.GetComponentInChildren<Animator>();
    UpdateAnimatorSpeed(0f);

    return Status.Running;
  }

  private Vector3 GetPositionColliderAdjusted() {
    return Target.Value.transform.position;
  }

  private float GetDistanceXY() {
    Vector3 agentPosition = new Vector3(Agent.Value.transform.position.x, Agent.Value.transform.position.y, m_ColliderAdjustedTargetPosition.z);
    return Vector3.Distance(agentPosition, m_ColliderAdjustedTargetPosition);
  }

  private void UpdateAnimatorSpeed(float explicitSpeed = -1) {
    if (m_Animator == null) {
      return;
    }

    float speedValue = 0;
    if (explicitSpeed >= 0) {
      speedValue = explicitSpeed;
    } else if (m_NavMeshAgent != null) {
      speedValue = m_NavMeshAgent.velocity.magnitude;
    } else {
      speedValue = m_CurrentSpeed;
    }

    if (speedValue <= 0.1f) {
      speedValue = 0;
    }

    m_Animator.SetFloat(Constants.ANIMATION_SPEED, speedValue);
  }

  private float SimpleMoveTowardsLocation(float distance, float minSpeedRatio = 0.1f) {
    if (Agent.Value.transform == null) {
      return 0f;
    }

    Vector3 agentPosition = Agent.Value.transform.position;
    float movementSpeed = Speed;

    // Slowdown
    if (SlowDownDistance > 0.0f && distance < SlowDownDistance) {
      float ratio = distance / SlowDownDistance;
      movementSpeed = Mathf.Max(Speed * minSpeedRatio, Speed * ratio);
    }

    Vector3 toDestination = m_ColliderAdjustedTargetPosition - agentPosition;
    toDestination.y = 0.0f;

    if (toDestination.sqrMagnitude > 0.0001f) {
      toDestination.Normalize();

      // Apply movement
      agentPosition += toDestination * (movementSpeed * Time.deltaTime);
      Agent.Value.transform.position = agentPosition;

      // Look at the target
      Agent.Value.transform.forward = toDestination;
    }

    return movementSpeed;
  }
}


