using UnityEngine;

public class Health : MonoBehaviour {

  private Entity entity;


  [SerializeField] private float maxHealth = 20;
  public float MaxHealth { get => maxHealth; }
  public float CurrentHealth { get; private set; }


  public event FFloatSignature OnHealthChanging;


  private void Awake() {
    entity = GetComponent<Entity>();
  }

  private void Start() {
    CurrentHealth = MaxHealth;
  }


  public void ApplyDamage(float damage) {
    CurrentHealth -= damage;
    OnHealthChanging?.Invoke(CurrentHealth);
    if (CurrentHealth <= 0) {
      entity.Destroy();
    }
  }
}
