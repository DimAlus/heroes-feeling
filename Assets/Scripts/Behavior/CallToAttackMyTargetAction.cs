using System;
using System.Collections.Generic;
using Unity.Behavior;
using Unity.Properties;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using Action = Unity.Behavior.Action;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Call to attack my target", story: "Call [neighbours] to attack [my] [target] at [radius]", category: "Action", id: "bc479e147e9b3f9710c6cb4f4436f875")]
public partial class CallToAttackMyTargetAction : Action {
  [SerializeReference] public BlackboardVariable<ETag> Neighbours;
  [SerializeReference] public BlackboardVariable<float> Radius;
  [SerializeReference] public BlackboardVariable<Entity> My;
  [SerializeReference] public BlackboardVariable<Entity> Target;

  protected override Status OnStart() {
    List<Entity> nb = Society.Instance.FindAllEntityAtDistance(My.Value.transform.position, new Vector2(0, Radius), Neighbours, Constants.ETAGS_ARRAY_EMPTY);
    foreach (Entity entity in nb) {
      entity.AI?.SetEnemyTarget(Target);
    }
    return Status.Success;
  }

}

