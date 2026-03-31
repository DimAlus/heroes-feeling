using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour {

  [SerializeField]
  private Transform healthBarTransform;

  public FAbilityContextSignature OnDead;


  [SerializeField]
  private string entityName = "None";
  public string EntityName { get => entityName; }
  public EntityData EntityData { get; private set; }

  public TagContainer TagContainer { get; private set; }
  public Health Health { get; private set; }
  public MovementBase Movement { get; private set; }
  public EffectHaver EffectHaver { get; private set; }
  public EnemyAI AI { get; private set; }
  public Transform HealthBarTransform { get => healthBarTransform; }

  private LinkedListNode<Entity> myNode;


  private void Awake() {
    EntityData = GameData.GetEntityData(EntityName);
    if (EntityData == null) {
      Debug.LogError($"Entity data not found for [{EntityName}]");
    }
    TagContainer = GetComponent<TagContainer>();
    Health = GetComponent<Health>();
    Movement = GetComponent<MovementBase>();
    EffectHaver = GetComponent<EffectHaver>();
    AI = GetComponent<EnemyAI>();
  }

  private void Start() {
    myNode = Society.Instance.AddEntity(this);
  }


  public void Dead(FAbilityContext context) {
    OnDead?.Invoke(context);
    Society.Instance.RemoveEntity(myNode);
    Destroy(gameObject);
  }

  public void OnKill(Entity killed, FAbilityContext context) {

  }

}
