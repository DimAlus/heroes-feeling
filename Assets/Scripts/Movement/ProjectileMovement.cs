using UnityEngine;



public class ProjectileMovement : MonoBehaviour {

  [SerializeField]
  private float speed = 1.0f;
  [SerializeField]
  private bool rotate = true;

  [SerializeField]
  private ProjectileDirectionCalculator DirectionCaclulator;

  public bool IsInitialized { get; private set; }

  private AbilityBase ability;

  private void Awake() {
    ability = GetComponent<AbilityBase>();
  }


  private void Start() {
    if (ability.IsInitialized) {
      Initialize();
    } else {
      ability.OnInitialized += () => Initialize();
    }
  }


  private void Initialize() {
    if (ability.Owner != null) {
      IsInitialized = true;
      DirectionCaclulator.Initialize(ability.Owner);
    }
  }


  private void FixedUpdate() {
    if (!IsInitialized) {
      return;
    }
    Vector2 dir = DirectionCaclulator.Calculate().normalized;
    gameObject.transform.position += (dir * (speed * Time.fixedDeltaTime)).ToVector3();
    if (rotate) {
      gameObject.transform.rotation = Quaternion.LookRotation(Vector3.forward, new Vector3(-dir.y, dir.x, 0));
    }
  }
}
