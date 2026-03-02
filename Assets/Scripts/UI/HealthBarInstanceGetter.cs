using UnityEngine;

public class HealthBarInstanceGetter : MonoBehaviour {

  [SerializeField]
  private Entity entity;

  private HealthBar currentBar;

  void Update() {
    if (currentBar != null) {
      if (!currentBar.enabled) {
        currentBar = null;
      }
      return;
    }


    Camera camera = Camera.main;
    Vector2 screenPos = camera.WorldToScreenPoint(transform.position);
    if (HealthBar.NeedHealthBarShow(screenPos, camera.pixelRect)) {
      currentBar = GameUI.Instance.AttachHealthBar(entity);
    }
  }
}
