using System;
using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;


public delegate void FTouchSignature();
public delegate void FFloatSignature(float value);
public delegate void FAbilityContextSignature(FAbilityContext context);


public static class Lib {
  public static Vector3 RandomDirection() {
    float angle = UnityEngine.Random.value * (2 * Mathf.PI);
    return new Vector3(Mathf.Sin(angle), Mathf.Cos(angle));
  }
}


[BlackboardEnum]
public enum EAIState {
  Fail, Idle, Aggressive
}



[Flags]
[BlackboardEnum]
public enum ETag {
  Pawn          = 0b_0000_0001,

  Player        = 0b_0000_0010,
  Enemy         = 0b_0000_0100,
  [InspectorName(null)]
  Fraction      = 0b_0000_0110,

  Projectile    = 0b_0001_0000,
}


[Serializable]
[BlackboardEnum]
public enum EAbilitySlot {
  None,
  Primary, Secondary,
  Dash,
  Death
}

[Serializable]
public enum EAbilityEffectApplyContext {
  None, SelfActivate, SelfAffect, SelfDeactivate, SelfKill,
  TargetHit,
}


[Serializable]
public enum EEffectType {
  [InspectorName("Lock Movement")]
  MovementLock      = 0b_0000_0001,
  [InspectorName("Lock Look")]
  LookLock          = 0b_0000_0010,
  [InspectorName("Apply Damage")]
  Damage            = 0b_0000_0100,
  [InspectorName("Slow")]
  Slowing           = 0b_0000_1000,
  [InspectorName("Apply Impulse")]
  Impulse           = 0b_0001_0000,

  [InspectorName(null)]
  Tickable = Damage,
}



[Serializable]
public struct FEffectData {
  public enum EEffectDuration {
    Impact, Periodic, Infinity
  }

  public EEffectType Type;
  public float Power;
  public EEffectDuration DurationType;
  public float Duration;

  public float Tick(float DeltaTime) {
    return DurationType == EEffectDuration.Infinity ? 1 : Duration -= DeltaTime;
  }
}



[Serializable]
public struct FAbilityEffects {
  public EAbilityEffectApplyContext Context;
  public FEffectData[] Effects;
}



[Serializable]
public struct FAbilityData {
  public float Cooldown;
  public bool Autoactivate;
  public bool HitOnce;
  public FAbilityEffects[] Effects;
  public EAbilityTargetFinder TargetFinderType;
  public ETag TagsInclude;
  public ETag[] TagsExclude;
  public FProjectileInfo SpawnedProjectile;
}


[System.Serializable]
public class FProjectileInfo {
  public GameObject Prefab;
  public FAbilityData AbilityInitializer;
}


public struct FAbilityContext {
  public Entity Owner;
  public AbilityBase Ability;

  public FAbilityContext(Entity owner = null, AbilityBase ability = null) {
    Owner = owner;
    Ability = ability;
  }
}


public class HealthData {
  public float MaxHealth;
}

public class MovementData {
  public float Speed;
  public float SprintSpeed;
}

public class AbilityInfoData {
  public float Cooldown;
  public bool Autoactivate = false;
  public string Ability;
  public AbilityObjData AbilityObj;
}

public class AbilityData {
  public Dictionary<EAbilitySlot, AbilityInfoData> Abilities;
}

public class EntityData {
  public HealthData Health;
  public MovementData Movement;
  public AbilityData Ability;
}


public class EffectsApplierData {
  public EAbilityEffectApplyContext Context;
  public ETag TagsInclude;
  public ETag[] TagsExclude;
  public FEffectData[] Effects;
}


public class AbilityObjData {
  public string PrefabName;
  public bool HitOnce = true;
  public EffectsApplierData[] Appliers;
  public string Projectile;
  public AbilityObjData ProjectileObj;
}
