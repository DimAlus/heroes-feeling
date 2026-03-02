using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour {

  [SerializeField]
  private Transform healthBarTransform;


  public TagContainer TagContainer { get; private set; }
  public Health Health { get; private set; }
  public MovementBase Movement { get; private set; }
  public Transform HealthBarTransform { get => healthBarTransform; }

  public event FTouchSignature OnDeath;

  private LinkedListNode<Entity> myNode;


  private void Awake() {
    TagContainer = GetComponent<TagContainer>();
    Health = GetComponent<Health>();
    Movement = GetComponent<MovementBase>();
  }

  private void Start() {
    myNode = Society.Instance.AddEntity(this);
  }


  public void Destroy() {
    Society.Instance.RemoveEntity(myNode);
    OnDeath?.Invoke();
    Destroy(gameObject);
  }
}
