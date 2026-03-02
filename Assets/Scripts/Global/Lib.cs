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
