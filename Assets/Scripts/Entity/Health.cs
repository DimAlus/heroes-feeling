using UnityEngine;

public class Health : MonoBehaviour {

  private Entity entity;


  [SerializeField] private float maxHealth = 20;
  public float MaxHealth { get => maxHealth; }
  public float CurrentHealth { get; private set; }

  public bool bDead = false;


  public event FFloatSignature OnHealthChanging;


  private void Awake() {
    entity = GetComponent<Entity>();
  }

  private void Start() {
    maxHealth = entity.EntityData.Health.MaxHealth;
    CurrentHealth = MaxHealth;
  }


  public void ApplyDamage(float damage, FAbilityContext context) {
    if (bDead) {
      return;
    }
    CurrentHealth = Mathf.Min(CurrentHealth - damage, MaxHealth);
    OnHealthChanging?.Invoke(CurrentHealth);
    if (CurrentHealth <= 0) {
      bDead = true;
      if (context.Ability != null) {
        context.Ability.OnKill(entity, context);
      }
      if (context.Owner != null) {
        context.Owner.OnKill(entity, context);
      }
      entity.Dead(context);
    }
  }
}
