using System;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using Action = Unity.Behavior.Action;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "RandomPointAround", story: "Find random [point] around [Target] at [length]", category: "Utils", id: "28df6fcd442df9a75ecb3720524b2ba2")]
public partial class RandomPointAroundAction : Action {
  [SerializeReference] public BlackboardVariable<Vector3> Point;
  [SerializeReference] public BlackboardVariable<Vector3> Target;
  [SerializeReference] public BlackboardVariable<Vector2> Length;

  protected override Status OnStart() {
    Point.Value = Target.Value + Lib.RandomDirection() * UnityEngine.Random.Range(Length.Value.x, Length.Value.y);
    return Status.Success;
  }

}

