using System;
using System.Collections.Generic;
using UnityEngine;


public enum EEffectType {
  MovementLock, LookLock
}


public class EffectHaver : MonoBehaviour {

  private struct EffectPeriod {
    public EEffectType Effect;
    public float Time;

    public EffectPeriod(EEffectType effect, float time) {
      Effect = effect;
      Time = time;
    }
  }

  private Dictionary<EEffectType, int> ConstEffects = new Dictionary<EEffectType, int>();
  private Dictionary<EEffectType, int> PeriodicEffectsCount = new Dictionary<EEffectType, int>();
  private LinkedList<EffectPeriod> PeriodicEffects = new LinkedList<EffectPeriod>();

  private void Awake() {
    foreach (EEffectType effect in Enum.GetValues(typeof(EEffectType))) {
      ConstEffects.Add(effect, 0);
      PeriodicEffectsCount.Add(effect, 0);
    }
  }


  private void FixedUpdate() {
    var iter = PeriodicEffects.First;

    while (iter != null) {
      if (iter.Value.Time - Time.fixedDeltaTime <= 0) {
        PeriodicEffectsCount[iter.Value.Effect] -= 1;
        PeriodicEffects.Remove(iter);
      } else {
        iter.Value = new EffectPeriod(iter.Value.Effect, iter.Value.Time - Time.fixedDeltaTime);
      }
    }
  }


  public void ApplyEffect(EEffectType effect) {
    ConstEffects[effect] += 1;
  }


  public void RemoveEffect(EEffectType effect) {
    ConstEffects[effect] = Math.Max(0, ConstEffects[effect] - 1);
  }

  public void ApplyPeriodicalEffect(EEffectType effect, float time) {
    PeriodicEffects.AddLast(new EffectPeriod(effect, time));
    PeriodicEffectsCount[effect] += 1;
  }


  public bool HasEffect(EEffectType effect) {
    return ConstEffects[effect] > 0 || PeriodicEffectsCount[effect] > 0;
  }
}
