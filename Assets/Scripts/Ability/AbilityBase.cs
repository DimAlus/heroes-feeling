using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;
using static UnityEngine.EventSystems.EventTrigger;


public enum EAbilityTargetFinder {
  MovementDirection, NearestEnemy, LookDirection
}




public class AbilityBase : MonoBehaviour {

  [SerializeField]
  protected Collider2D Collider;

  private AbilityAnimation aanimation;

  public Entity Owner { get; private set; }

  protected List<Entity> HitedEntities = new List<Entity>();

  protected List<FPrefabInfo> AbilityPrefabs;

  protected AbilityBase OuterAbility { get; private set; }

  public AbilityInfoData ActivatedAbilityInfo { get; private set; }
  public AbilityObjData AbilityData { get; private set; }

  public bool IsInitialized { get; private set; }

  public event FTouchSignature OnCooldownEnd;
  public event FTouchSignature OnInitialized;
  public event FTouchSignature OnHit;

  public void SetActivatedAbility(AbilityInfoData info) {
    ActivatedAbilityInfo = info;
  }


  public void Initialize(AbilityObjData abilityData, List<FPrefabInfo> prefabs, AbilityBase outerAbility = null) {
    AbilityData = abilityData;
    AbilityPrefabs = prefabs;
    OuterAbility = outerAbility;
    if (ActivatedAbilityInfo != null && ActivatedAbilityInfo.Autoactivate) {
      Activate();
    } else {
      Deactivate();
    }
    if (outerAbility != null) {
      Owner = outerAbility.Owner;
    } else {
      Owner = GetComponentInParent<Entity>();
    }
    IsInitialized = true;
    OnInitialized?.Invoke();
  }


  protected virtual void Awake() {
    Active = false;
    CooldownActivated = true;
    aanimation = GetComponent<AbilityAnimation>();

    if (aanimation != null) {
      aanimation.OnAnimationStateChanging += OnAnimationStateChanging;
    }
  }


  private void OnAnimationStateChanging(AbilityAnimation.EAbilityAnimationState state) {
    switch (state) {
    case AbilityAnimation.EAbilityAnimationState.Started:
      ApplySelfEffects(EAbilityEffectApplyContext.SelfActivate);
      break;

    case AbilityAnimation.EAbilityAnimationState.Activated:
      SpawnProjectile();
      ApplySelfEffects(EAbilityEffectApplyContext.SelfAffect);
      break;

    case AbilityAnimation.EAbilityAnimationState.Ended:
    case AbilityAnimation.EAbilityAnimationState.Canceled:
      RemoveSelfEffects();
      ApplySelfEffects(EAbilityEffectApplyContext.SelfDeactivate);
      break;

    default:
      break;
    }
  }


  protected virtual void OnTriggerEnter2D(Collider2D collision) {
    if (collision.transform.TryGetComponent(out Entity entity)) {
      if (!AbilityData.HitOnce || !HitedEntities.Contains(entity)) {
        if (ApplyEffects(entity)) {
          OnHit?.Invoke();
        }
        if (AbilityData.HitOnce) {
          HitedEntities.Add(entity);
        }
      }
    }
  }


  protected virtual void FixedUpdate() {
    UpdateCooldown(Time.fixedDeltaTime);
  }



  protected void UpdateCooldown(float deltaTime) {
    if (CooldownActivated && cooldown > 0) {
      cooldown -= deltaTime;
      if (cooldown < 0) {
        cooldown = 0;
        OnCooldownEnd?.Invoke();
      }
    }
  }



  protected float cooldown = 0;
  protected bool CooldownActivated { get; set; }
  protected bool Active { get; set; }



  public float GetCooldown() {
    return cooldown;
  }

  public float GetCooldownPercentage() {
    return GetCooldownMax() == 0 ? 0 : cooldown / GetCooldownMax();
  }

  public float GetCooldownMax() {
    return ActivatedAbilityInfo == null ? 0 : ActivatedAbilityInfo.Cooldown;
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
    if (AbilityData.HitOnce) {
      HitedEntities.Clear();
    }
    cooldown = GetCooldownMax();
    aanimation?.StartAnimation();
    if (aanimation == null) {
      SpawnProjectile();
    }
  }


  private void SpawnProjectile() {
    if (AbilityData.Projectile != null && AbilityData.Projectile != null) {
      FPrefabInfo prefab = AbilityPrefabs.Find((FPrefabInfo el) => el.Name == AbilityData.Projectile);
      if (prefab == null || prefab.GameObject == null) {
        Debug.LogError($"Prefab [{AbilityData.Projectile}] not found for Projectile of Object [{Owner.EntityName}]");
      } else {
        GameObject newObject = Instantiate(prefab.GameObject, transform.position, new Quaternion());
        AbilityBase ability = newObject.GetComponent<AbilityBase>();
        ability.Initialize(AbilityData.ProjectileObj, AbilityPrefabs, OuterAbility == null ? this : OuterAbility);
        newObject.SetActive(true);
      }
    }
  }


  public virtual void Deactivate() {
    CooldownActivated = true;
    Active = false;
  }


  protected virtual void ApplySelfEffects(EAbilityEffectApplyContext Context) {
    FAbilityContext context = new FAbilityContext(owner: Owner, ability: this);
    foreach (var applier in AbilityData.Appliers) {
      if (applier.Context == Context) {
        foreach (var eff in applier.Effects) {
          Owner.EffectHaver.ApplyEffect(eff, context);
        }
      }
    }
  }


  protected virtual void RemoveSelfEffects() {
    foreach (var applier in AbilityData.Appliers) {
      if (applier.Context == EAbilityEffectApplyContext.SelfActivate) {
        foreach (var eff in applier.Effects) {
          if (eff.DurationType == FEffectData.EEffectDuration.Infinity) {
            Owner.EffectHaver.RemoveInfinityEffect(eff.Type);
          }
        }
      }
    }
  }


  public virtual bool ApplyEffects(Entity entity) {
    bool applied = false;
    FAbilityContext context = new FAbilityContext(owner: Owner, ability: this);
    foreach (var applier in AbilityData.Appliers) {
      if (applier.Context == EAbilityEffectApplyContext.TargetHit) {
        if (entity.TagContainer.TagsAgreement(applier.TagsInclude, applier.TagsExclude)) {
          foreach (var eff in applier.Effects) {
            entity.EffectHaver.ApplyEffect(eff, context);
          }
          applied = true;
        }
      }
    }
    return applied;
  }


  public virtual void RemoveEffects(Entity entity) {
    foreach (var applier in AbilityData.Appliers) {
      if (applier.Context == EAbilityEffectApplyContext.TargetHit) {
        if (entity.TagContainer.TagsAgreement(applier.TagsInclude, applier.TagsExclude)) {
          foreach (var eff in applier.Effects) {
            if (eff.DurationType == FEffectData.EEffectDuration.Infinity) {
              entity.EffectHaver.RemoveInfinityEffect(eff.Type);
            }
          }
        }
      }
    }
  }


  public void OnKill(Entity killed, FAbilityContext context) {
    ApplySelfEffects(EAbilityEffectApplyContext.SelfKill);
  }

}
