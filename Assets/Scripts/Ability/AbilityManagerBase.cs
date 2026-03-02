using System.Collections.Generic;
using UnityEngine;


public class AbilityManagerBase : MonoBehaviour {

  [System.Serializable]
  protected struct FAbilityPrefabInfo {
    public EAbilitySlot Slot;
    public GameObject Prefab;
    public FAbilityData AbilityInitializer;
    public bool LockMovement;
    public bool LockLook;
  }


  [SerializeField]
  protected List<FAbilityPrefabInfo> AbilitiesData = new List<FAbilityPrefabInfo>();

  protected Dictionary<EAbilitySlot, AbilityBase> Abilities = new Dictionary<EAbilitySlot, AbilityBase>();


  private EffectHaver effectHaver;



  protected virtual void Awake() {
    effectHaver = GetComponent<EffectHaver>();
    AbilitiesData.ForEach(info => {
      if (Abilities.ContainsKey(info.Slot)) {
        return;
      }
      GameObject newObject;
      if (info.Prefab.gameObject.scene.rootCount == 0) {
        newObject = Instantiate(info.Prefab, transform.position, new Quaternion(), transform);
      } else {
        newObject = info.Prefab;
      }
      AbilityBase ability = newObject.GetComponent<AbilityBase>();
      Abilities.Add(info.Slot, ability);
      ability.Initialize(ref info.AbilityInitializer);

      AbilityAnimation anim = newObject.GetComponent<AbilityAnimation>();
      if (info.LockMovement) {
        anim.OnAnimationStateChanging += (AbilityAnimation.EAbilityAnimationState state) => {
          if (state == AbilityAnimation.EAbilityAnimationState.Started) {
            effectHaver.ApplyEffect(EEffectType.MovementLock);
          } else if (state == AbilityAnimation.EAbilityAnimationState.Canceled || state == AbilityAnimation.EAbilityAnimationState.Ended) {
            effectHaver.RemoveEffect(EEffectType.MovementLock);
          }
        };
      }
      if (info.LockLook) {
        anim.OnAnimationStateChanging += (AbilityAnimation.EAbilityAnimationState state) => {
          if (state == AbilityAnimation.EAbilityAnimationState.Started) {
            effectHaver.ApplyEffect(EEffectType.LookLock);
          } else if (state == AbilityAnimation.EAbilityAnimationState.Canceled || state == AbilityAnimation.EAbilityAnimationState.Ended) {
            effectHaver.RemoveEffect(EEffectType.LookLock);
          }
        };
      }
    });
  }


}
