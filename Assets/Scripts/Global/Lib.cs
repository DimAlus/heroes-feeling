using System;
using Unity.Behavior;
using UnityEngine;


public delegate void FTouchSignature();
public delegate void FFloatSignature(float value);


public static class Lib {
  public static Vector3 RandomDirection() {
    float angle = UnityEngine.Random.Range(0, 2 * Mathf.PI);
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


[System.Serializable]
[BlackboardEnum]
public enum EAbilitySlot {
  None,
  Primary, Secondary,
  Dash
}


[Flags]
public enum EEffectType {
  MovementLock      = 0b_0000_0001,
  LookLock          = 0b_0000_0010,
  Damage            = 0b_0000_0100,
  Slowing           = 0b_0000_1000,
  Impulse           = 0b_0001_0000,

  [InspectorName(null)]
  Tickable = Damage,
}


public struct FEffectData {
  public enum EEffectDuration {
    Impact, Periodic, Infinity
  }

  public EEffectType Type;
  public float Power;
  public EEffectDuration DurationType;
  public float Duration;

  public float Tick(float DeltaTime) {
    return Duration -= DeltaTime;
  }
}
