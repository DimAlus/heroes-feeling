using NUnit.Framework;
using System.Runtime.CompilerServices;
using UnityEngine;


public enum EAbilityTargetFinder {
  MovementDirection, NearestEnemy, LookDirection
}



[System.Serializable]
public struct FAbilityData {
  public float Cooldown;
  public FEffectData[] Effects;
  public FEffectData[] SelfEffects;
  public EAbilityTargetFinder TargetFinderType;
  public ETag TagsInclude;
  public ETag[] TagsExclude;
}

public class AbilityBase : MonoBehaviour {

  private AbilityAnimation aanimation;

  private Entity Owner;

  public FAbilityData data { get; private set; }

  [SerializeField]
  private bool forceInitialize = false;


  public event FTouchSignature OnCooldownEnd;


  public void Initialize(ref FAbilityData abilityData) {
    data = abilityData;
  }

  protected virtual void Awake() {
    if (forceInitialize) {
      FAbilityData dt = data;
      Initialize(ref dt);
    }
    aanimation = GetComponent<AbilityAnimation>();
    Owner = GetComponentInParent<Entity>();
  }


  protected virtual void OnTriggerEnter2D(Collider2D collision) {
    if (collision.transform.TryGetComponent(out Entity entity)) {
      ApplyEffects(entity);
    }
  }


  protected virtual void FixedUpdate() {
    UpdateCooldown(Time.fixedDeltaTime);
  }



  protected void UpdateCooldown(float deltaTime) {
    if (cooldown > 0) {
      cooldown -= deltaTime;
      if (cooldown < 0) {
        cooldown = 0;
        OnCooldownEnd?.Invoke();
      }
    }
  }



  protected float cooldown = 0;



  public float GetCooldown() {
    return cooldown;
  }

  public float GetCooldownPercentage() {
    return GetCooldownMax() == 0 ? 0 : cooldown / GetCooldownMax();
  }

  public float GetCooldownMax() {
    return data.Cooldown;
  }



  public virtual bool CanActivate() {
    return GetCooldown() == 0;
  }


  public virtual void TryActivate() {
    if (CanActivate()) {
      Activate();
    }
  }


  public virtual void Activate() {
    cooldown = data.Cooldown;
    aanimation?.StartAnimation();
    foreach (FEffectData data in data.SelfEffects) {
      Entity.ApplyEffect(Owner, data);
    }
  }


  public virtual void ApplyEffects(Entity entity) {
    if (entity.TagContainer.TagsAgreement(data.TagsInclude, data.TagsExclude)) {
      foreach (FEffectData data in data.Effects) {
        Entity.ApplyEffect(entity, data);
      }
    }
  }



}
