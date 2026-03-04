using System;
using System.Collections.Generic;
using UnityEngine;



public class EffectHaver : MonoBehaviour {

  private enum EEffectApplyContext {
    Apply, Tick, Cancel
  }

  private Entity entity;

  private EEffectType CurrentPeriodicEffects;
  private EEffectType CurrentInfinityEffects;
  private Dictionary<EEffectType, int> InfinityEffects = new Dictionary<EEffectType, int>();
  private LinkedList<FEffectData> PeriodicEffects = new LinkedList<FEffectData>();

  private void Awake() {
    entity = GetComponent<Entity>();
  }


  private void FixedUpdate() {
    EEffectType currentEffects = 0;

    var iter = PeriodicEffects.First;
    while (iter != null) {
      if (iter.Value.Tick(Time.fixedDeltaTime) <= 0) {
        PeriodicEffects.Remove(iter);
        ApplyEffectActionCancel(iter.Value);
      } else {
        currentEffects = currentEffects | iter.Value.Type;
        ApplyEffectActionTick(iter.Value);
      }
    }
  }


  public void ApplyEffect(FEffectData effect) {
    if (effect.DurationType == FEffectData.EEffectDuration.Periodic) {
      ApplyEffectActionStart(effect);
      PeriodicEffects.AddLast(effect);
    } else if (effect.DurationType == FEffectData.EEffectDuration.Infinity) {
      ApplyEffectActionStart(effect);
      if (InfinityEffects.ContainsKey(effect.Type)) {
        InfinityEffects[effect.Type] += 1;
      } else {
        InfinityEffects.Add(effect.Type, 1);
      }
      CurrentInfinityEffects |= effect.Type;
    } else {
      ApplyEffectActionInstant(effect);
    }
  }


  private void ApplyEffectActionInstant(FEffectData effect) {
    switch (effect.Type) {
    case (EEffectType.Damage):

      entity.Health.ApplyDamage(effect.Power);
      break;

    case EEffectType.Impulse:
      break;

    case EEffectType.Slowing:
    case EEffectType.MovementLock:
    case EEffectType.LookLock:
    default:
      break;
    }
  }


  private void ApplyEffectActionStart(FEffectData effect) {
    switch (effect.Type) {
    case (EEffectType.Damage):

      entity.Health.ApplyDamage(effect.Power);
      break;

    case EEffectType.Impulse:
      break;

    case EEffectType.Slowing:
    case EEffectType.MovementLock:
    case EEffectType.LookLock:
    default:
      break;
    }
  }

  private void ApplyEffectActionTick(FEffectData effect) {
    switch (effect.Type) {
    case (EEffectType.Damage):

      entity.Health.ApplyDamage(effect.Power);
      break;

    case EEffectType.Impulse:
      break;

    case EEffectType.Slowing:
    case EEffectType.MovementLock:
    case EEffectType.LookLock:
    default:
      break;
    }
  }

  private void ApplyEffectActionCancel(FEffectData effect) {
    switch (effect.Type) {
    case (EEffectType.Damage):

      entity.Health.ApplyDamage(effect.Power);
      break;

    case EEffectType.Impulse:
      break;

    case EEffectType.Slowing:
    case EEffectType.MovementLock:
    case EEffectType.LookLock:
    default:
      break;
    }
  }


  public void RemoveInfinityEffect(EEffectType effectType) {
    if (InfinityEffects.ContainsKey(effectType)) {
      InfinityEffects[effectType] = Math.Max(0, InfinityEffects[effectType] - 1);
      if (InfinityEffects[effectType] == 0) {
        CurrentInfinityEffects &= ~effectType;
      }
    }
  }


  public bool HasEffect(EEffectType effectType) {
    return ((CurrentPeriodicEffects | CurrentInfinityEffects) & effectType) > 0;
  }
}
