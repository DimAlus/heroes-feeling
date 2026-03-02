using UnityEngine;

public delegate void FAbilityAnimationStateSignature(AbilityAnimation.EAbilityAnimationState state);

public class AbilityAnimation : MonoBehaviour {

  static string T_ATTACK = "Attack";

  public enum EAbilityAnimationState {
    None, Started, Canceled, Ended, Activated
  }


  public event FAbilityAnimationStateSignature OnAnimationStateChanging;


  //[SerializeField]
  //AnimationClip AttackAnimation;

  [SerializeField]
  private GameObject SpriteObject;
  private Animator animator;

  [SerializeField]
  private Vector3 SpritePosition;
  [SerializeField]
  private Quaternion SpriteRotation;
  [SerializeField]
  private float SpriteScale = 1;

  private AbilityBase ability;
  private TargetFinderBase targetFinder;


  private EAbilityAnimationState animationState;
  public EAbilityAnimationState AnimationState {
    get => animationState;
    set {
      OnAnimationStateChanging?.Invoke(value);

      if (value == EAbilityAnimationState.Started) {
        animationState = EAbilityAnimationState.Started;
      }
      if (value == EAbilityAnimationState.Ended || value == EAbilityAnimationState.Canceled) {
        animationState = EAbilityAnimationState.None;
      }
    }
  }


  private void Awake() {
    ability = GetComponent<AbilityBase>();
    animator = GetComponent<Animator>();
    targetFinder = GetComponentInParent<TargetFinderBase>();
    SpriteObject.transform.localPosition = SpritePosition;
    SpriteObject.transform.localRotation = SpriteRotation;
    SpriteObject.transform.localScale = new Vector3(SpriteScale, SpriteScale, SpriteScale);
  }


  public void StartAnimation() {
    Vector3 location = targetFinder.FindTarget(ability.data.TargetType, ability.data.TargetFinderType) - transform.parent.position;
    location.Normalize();
    float angle = Mathf.Rad2Deg * Mathf.Atan2(location.y, location.x);
    transform.localRotation = Quaternion.Euler(0, 0, angle);

    animator.SetTrigger(T_ATTACK);
  }
}
