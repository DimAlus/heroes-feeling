using System;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using Action = Unity.Behavior.Action;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "InitializeBehavior", story: "Initialize behavior variables from [Entity]", category: "_Components", id: "f1634c462a448742e17c3547ca1b99a9")]
public partial class InitializeBehaviorAction : Action {

  [SerializeReference] public BlackboardVariable<Entity> entity;
  [SerializeReference] public BlackboardVariable<EAIState> aistate;
  [SerializeReference] public BlackboardVariable<string> keyAnomatorSpeed;
  [SerializeReference] public BlackboardVariable<float> movementSpeed;
  [SerializeReference] public BlackboardVariable<float> dashSpeed;
  [SerializeReference] public BlackboardVariable<Vector3> spawnLocation;

  protected override Status OnStart() {
    keyAnomatorSpeed.Value = Constants.ANIMATION_SPEED;
    movementSpeed.Value = entity.Value.Movement.MovementSpeed;
    dashSpeed.Value = entity.Value.Movement.RunSpeed;
    aistate.Value = EAIState.Idle;
    spawnLocation.Value = entity.Value.transform.position;
    return Status.Success;
  }

  protected override Status OnUpdate() {
    return Status.Success;
  }

  protected override void OnEnd() {
  }
}

