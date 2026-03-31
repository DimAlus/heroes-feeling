using System;
using System.Collections.Generic;
using UnityEngine;



public class EffectHaver : MonoBehaviour {

  private enum EEffectApplyContext {
    Apply, Tick, Cancel
  }

  private struct FEffectContainer {
    public FEffectData Effect;
    public FAbilityContext Context;
  }



  private Entity entity;

  private EEffectType ActiveEffects;
  private LinkedList<FEffectContainer> CurrentEffects = new LinkedList<FEffectContainer>();

  private void Awake() {
    entity = GetComponent<Entity>();
  }


  private void FixedUpdate() {
    EEffectType currentEffects = 0;

    var iter = CurrentEffects.First;
    while (iter != null) {
      if ((iter.Value.Effect.Type & EEffectType.Tickable) != 0 && iter.Value.Effect.Tick(Time.fixedDeltaTime) <= 0) {
        CurrentEffects.Remove(iter);
        ApplyEffectActionCancel(iter.Value.Effect, iter.Value.Context);
      } else {
        currentEffects = currentEffects | iter.Value.Effect.Type;
        if ((iter.Value.Effect.Type & EEffectType.Tickable) != 0) {
          ApplyEffectActionTick(iter.Value.Effect, iter.Value.Context);
        }
      }
      iter = iter.Next;
    }
  }


  public void RemoveInfinityEffect(EEffectType effectType) {
    bool hasEffect = false;
    bool removed = false;

    var iter = CurrentEffects.First;
    while (iter != null) {
      if (!removed && iter.Value.Effect.Type == effectType && iter.Value.Effect.DurationType == FEffectData.EEffectDuration.Infinity) {
        CurrentEffects.Remove(iter);
        ApplyEffectActionCancel(iter.Value.Effect, iter.Value.Context);
        removed = true;
      } else if (iter.Value.Effect.Type == effectType) {
        hasEffect = true;
      }
      if (removed && hasEffect) {
        break;
      }
      iter = iter.Next;
    }
    if (!hasEffect) {
      ActiveEffects &= ~effectType;
    }
  }


  public bool HasEffect(EEffectType effectType) {
    return (ActiveEffects & effectType) != 0;
  }


  public void ApplyEffect(FEffectData effect, FAbilityContext context) {
    if (effect.DurationType == FEffectData.EEffectDuration.Impact) {
      ApplyEffectActionInstant(effect, context);
    } else {
      ApplyEffectActionStart(effect, context);
      CurrentEffects.AddLast(new FEffectContainer { Effect = effect, Context = context });
      ActiveEffects |= effect.Type;
    }
  }


  private void ApplyEffectActionInstant(FEffectData effect, FAbilityContext context) {
    switch (effect.Type) {
    case (EEffectType.Damage):
      entity.Health.ApplyDamage(effect.Power, context);
      break;

    case EEffectType.Impulse:
      entity.Movement.AddImpulse(transform.position - context.Owner.transform.position, effect.Power);
      break;

    case EEffectType.Slowing:
    case EEffectType.MovementLock:
    case EEffectType.LookLock:
    default:
      break;
    }
  }


  private void ApplyEffectActionStart(FEffectData effect, FAbilityContext context) {
    switch (effect.Type) {
    case (EEffectType.Damage):
      break;

    case EEffectType.Impulse:
    case EEffectType.Slowing:
    case EEffectType.MovementLock:
    case EEffectType.LookLock:
    default:
      break;
    }
  }

  private void ApplyEffectActionTick(FEffectData effect, FAbilityContext context) {
    switch (effect.Type) {
    case (EEffectType.Damage):
      entity.Health.ApplyDamage(effect.Power * Time.fixedDeltaTime, context);
      break;

    case EEffectType.Impulse:
    case EEffectType.Slowing:
    case EEffectType.MovementLock:
    case EEffectType.LookLock:
    default:
      break;
    }
  }

  private void ApplyEffectActionCancel(FEffectData effect, FAbilityContext context) {
    switch (effect.Type) {
    case (EEffectType.Damage):
      break;

    case EEffectType.Impulse:
    case EEffectType.Slowing:
    case EEffectType.MovementLock:
    case EEffectType.LookLock:
    default:
      break;
    }
  }
}
