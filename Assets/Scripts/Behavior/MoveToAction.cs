using System;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using Action = Unity.Behavior.Action;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "MoveTo", story: "MoveTo", category: "_Components/Movement", id: "04a7fd2ba5f32f5a6b0515fc5c1d0e36")]
public partial class MoveToAction : Action {

  [SerializeReference] public BlackboardVariable<Entity> entity;


  protected override Status OnStart() {
    return Status.Running;
  }

  protected override Status OnUpdate() {
    return Status.Success;
  }

  protected override void OnEnd() {
  }
}

