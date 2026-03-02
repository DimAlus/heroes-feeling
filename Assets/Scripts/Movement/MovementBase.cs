using UnityEngine;

public class MovementBase : MonoBehaviour {

  protected Rigidbody2D rb;
  protected EffectHaver effectHaver;

  [SerializeField]
  private float movementSpeed = 1f;
  public float MovementSpeed { get => movementSpeed; protected set => movementSpeed = value; }
  [SerializeField]
  private float runSpeed = 2f;
  public float RunSpeed { get => runSpeed; protected set => runSpeed = value; }

  public Vector2 LookDirection { get; protected set; }
  public Vector2 Velocity { get; protected set; }
  public bool IsIdle { get => Velocity.magnitude < 0.1f; }


  protected virtual void Awake() {
    rb = GetComponent<Rigidbody2D>();
    effectHaver = GetComponent<EffectHaver>();
  }

  protected virtual void Start() {
  }

  protected virtual void FixedUpdate() {
    MovementTick(Time.fixedDeltaTime);
  }

  protected virtual void MovementTick(float deltaTime) {

  }


}
