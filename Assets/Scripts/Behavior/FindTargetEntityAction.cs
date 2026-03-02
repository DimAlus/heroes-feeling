using System;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using Action = Unity.Behavior.Action;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Find Target Entity", story: "Try find [target] [entity] around [me] at [distance]", category: "_Components", id: "d5ed78f467b130e2c3268801fe187c22")]
public partial class FindTargetEntityAction : Action {


  [SerializeReference] public BlackboardVariable<Entity> entity;
  [SerializeReference] public BlackboardVariable<ETag> Target;
  [SerializeReference] public BlackboardVariable<Entity> Me;
  [SerializeReference] public BlackboardVariable<Vector2> Distance;

  protected override Status OnStart() {
    Entity found = Society.Instance.FindAnyEntityAtDistance(Me.Value.transform.position, Distance, Target.Value, Constants.ETAGS_ARRAY_EMPTY);
    if (found != null) {
      entity.Value = found;
      return Status.Success;
    }
    return Status.Failure;
  }

}

