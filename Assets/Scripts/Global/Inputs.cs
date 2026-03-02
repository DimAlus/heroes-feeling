using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void FAbilitySlotSignature(EAbilitySlot slot);
public delegate void FAbilitySlotFloatSignature(EAbilitySlot slot, float floatValue);

public class Inputs : MonoBehaviour {

  public static Inputs Instance { get; private set; }

  private InputSystem_Actions input;
  private Dictionary<EAbilitySlot, UnityEngine.InputSystem.InputAction> ActionsAbilityMap;

  public event FAbilitySlotSignature OnAbilityActivate;
  public event FAbilitySlotFloatSignature OnAbilityDeactivate;


  private void Awake() {
    if (Instance == null) {
      Instance = this;
    }
    input = new InputSystem_Actions();
    input.Enable();

    ActionsAbilityMap = new Dictionary<EAbilitySlot, UnityEngine.InputSystem.InputAction> {
      { EAbilitySlot.Primary, input.Player.Attack },
    };
    InitializeAbilitiesInputs();
  }


  private void Update() {
    PlayerMove = input.Player.Move.ReadValue<Vector2>();
    PlayerLook = input.Player.Look.ReadValue<Vector2>();
  }


  private void InitializeAbilitiesInputs() {
    foreach (EAbilitySlot slot in ActionsAbilityMap.Keys) {
      var action = ActionsAbilityMap[slot];
      action.started += (obj) => {
        OnAbilityActivate?.Invoke(slot);
      };
      action.canceled += (obj) => {
        OnAbilityDeactivate?.Invoke(slot, (float)obj.time);
      };
    }
  }


  public bool IsAbilityInputActive(EAbilitySlot slot) {
    return ActionsAbilityMap.ContainsKey(slot) ? ActionsAbilityMap[slot].inProgress : false;
  }


  public Vector2 PlayerMove { get; private set; }

  public Vector2 PlayerLook { get; private set; }

}
