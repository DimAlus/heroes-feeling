using System.Collections.Generic;
using UnityEngine;

public class GameUI : MonoBehaviour {

  public static GameUI Instance { get; private set; }

  private void Awake() {
    Instance = this;
  }


  [SerializeField]
  private Canvas HealthCanvas;
  [SerializeField]
  private GameObject HealthBarPrefab;

  private LinkedList<HealthBar> HealthBarsActive = new LinkedList<HealthBar>();
  private List<HealthBar> HealthBarsInactive = new List<HealthBar>();

  public HealthBar AttachHealthBar(Entity entity) {
    HealthBar bar;
    if (HealthBarsInactive.Count > 0) {
      bar = HealthBarsInactive[HealthBarsInactive.Count - 1];
      HealthBarsInactive.RemoveAt(HealthBarsInactive.Count - 1);
    } else {
      bar = Instantiate(HealthBarPrefab, HealthCanvas.transform.position, new Quaternion(), HealthCanvas.transform).GetComponent<HealthBar>();
    }

    bar.SetOwner(entity);
    return bar;
  }

  public void DetachHealthBar(HealthBar healthBar) {
    HealthBarsActive.Remove(healthBar);
    HealthBarsInactive.Add(healthBar);
  }
}
