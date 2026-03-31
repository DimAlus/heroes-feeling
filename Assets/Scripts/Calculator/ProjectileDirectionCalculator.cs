using System;
using UnityEngine;


[Serializable]
class ProjectileDirectionCalculator {

  public enum CalculatorType {
    LookDirection, Random
  }

  public delegate Vector2 DCalculator();



  [SerializeField]
  private bool CalculateOnce = true;

  [SerializeField]
  private CalculatorType calculatorType;
  public DCalculator Calculator { get; private set; }


  private Entity Owner;
  bool calculated = false;
  Vector2 savedResult;




  public void Initialize(Entity owner) {
    Owner = owner;

    switch (calculatorType) {
    case CalculatorType.LookDirection:
      Calculator = CalculatorLook;
      break;

    case CalculatorType.Random:
      Calculator = CalculatorRandom;
      break;
    }
  }


  public Vector2 Calculate() {
    if (CalculateOnce) {
      if (calculated) {
        return savedResult;
      } else {
        calculated = true;
        return savedResult = Calculator();
      }
    }
    return Calculator();
  }



  private Vector2 CalculatorLook() {
    return Owner.Movement.LookDirection;
  }


  private Vector2 CalculatorRandom() {
    return Lib.RandomDirection();
  }


}
