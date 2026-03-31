using Unity.Behavior;
using UnityEngine;
using UnityEngine.AI;



public class EnemyAI : MonoBehaviour {

  private Entity Entity;
  private BehaviorGraphAgent Behavior;

  float forceAgressiveCooldown = -1f;
  [SerializeField] private float ForceAgressiveTime = 3f;

  private void Awake() {
    Entity = GetComponent<Entity>();
    Behavior = GetComponent<BehaviorGraphAgent>();
  }

  private void FixedUpdate() {
    if (forceAgressiveCooldown > 0) {
      if ((forceAgressiveCooldown -= Time.fixedDeltaTime) <= 0) {
        Behavior.BlackboardReference.SetVariableValue<bool>(Constants.BB_FORCE_AGRESSIVE, false);
      }
    }
  }

  public void SetEnemyTarget(Entity enemy) {
    Behavior.BlackboardReference.SetVariableValue<Entity>(Constants.BB_TARGET_ENEMY, enemy);
    SetForceAgressive();
  }


  public void SetForceAgressive() {
    forceAgressiveCooldown = ForceAgressiveTime;
    Behavior.BlackboardReference.SetVariableValue<EAIState>(Constants.BB_AI_STATE, EAIState.Aggressive);
    Behavior.BlackboardReference.SetVariableValue<bool>(Constants.BB_FORCE_AGRESSIVE, true);
  }
}
