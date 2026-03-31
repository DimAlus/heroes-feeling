using UnityEngine;

public class Destroyer : MonoBehaviour {

  private enum EDestroyType {
    None, Timer, NextTick, Hit, AnimationEnd
  }

  [SerializeField]
  private EDestroyType type;

  private bool ToDestroy = false;
  [SerializeField]
  private float Timer = 0f;


  private void Awake() {
    if (type == EDestroyType.AnimationEnd) {
      AbilityAnimation anim = GetComponent<AbilityAnimation>();
      if (anim != null) {
        anim.OnAnimationStateChanging += (AbilityAnimation.EAbilityAnimationState state) => {
          if (state == AbilityAnimation.EAbilityAnimationState.Exit) {
            ToDestroy = true;
          }
        };
      }
    } else if (type == EDestroyType.Hit) {
      AbilityBase ability = GetComponent<AbilityBase>();
      if (ability != null) {
        ability.OnHit += () => ToDestroy = true;
      }
    }
  }


  private void FixedUpdate() {
    if (ToDestroy) {
      Destroy(gameObject);
    }
    if (type == EDestroyType.NextTick
      || (type == EDestroyType.Timer && (Timer -= Time.fixedDeltaTime) <= 0)) {
      ToDestroy = true;
    }
  }
}
