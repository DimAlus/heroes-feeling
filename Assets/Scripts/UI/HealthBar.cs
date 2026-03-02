using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

  private const float LESS_HEALTH_LIMIT = 0.3f;

  [SerializeField]
  private Entity Owner;

  [SerializeField]
  private Color LessHealthColor;
  [SerializeField]
  private Color HightHealthColor;

  [SerializeField]
  private Image FillImage;

  [SerializeField]
  private Image BackgroundImage;

  private Slider slider;

  private void Awake() {
    slider = GetComponent<Slider>();
  }


  private void Health_OnHealthChanging(float value) {
    SetHealth(value / Owner.Health.MaxHealth);
  }


  private void Update() {
    if (Owner == null) {
      RemoveOwner();
      return;
    }
    Camera camera = Camera.main;
    Vector2 screenPos = camera.WorldToScreenPoint(Owner.HealthBarTransform.position);

    if (NeedHealthBarHide(screenPos, camera.pixelRect)) {
      RemoveOwner();
      return;
    }
    UpdateTransform();
  }


  private void UpdateTransform() {
    transform.position = Camera.main.WorldToScreenPoint(Owner.HealthBarTransform.position);
  }


  public void SetHealth(float percents) {
    slider.value = percents;
    if (percents <= LESS_HEALTH_LIMIT) {
      FillImage.color = LessHealthColor;
    } else {
      FillImage.color = HightHealthColor;
    }
  }


  public void SetOwner(Entity owner) {
    Owner = owner;
    Owner.Health.OnHealthChanging += Health_OnHealthChanging;
    UpdateTransform();
    transform.localScale = Owner.HealthBarTransform.localScale;
    enabled = slider.enabled = FillImage.enabled = BackgroundImage.enabled = true;
    Health_OnHealthChanging(owner.Health.CurrentHealth);
  }


  private void RemoveOwner() {
    if (Owner != null) {
      Owner.Health.OnHealthChanging -= Health_OnHealthChanging;
    }
    Owner = null;
    enabled = slider.enabled = FillImage.enabled = BackgroundImage.enabled = false;
    GameUI.Instance.DetachHealthBar(this);
  }



  public static bool NeedHealthBarShow(Vector2 screenPosition, Rect screenPixels) {
    Vector2 pix = new Vector2(screenPixels.width / 2, screenPixels.height / 2);
    Vector2 offset = (screenPosition - pix).Abs() - pix;
    return offset.x < 0 && offset.y < 0;
  }


  public static bool NeedHealthBarHide(Vector2 screenPosition, Rect screenPixels) {
    Vector2 pix = new Vector2(screenPixels.width / 2, screenPixels.height / 2);
    Vector2 offset = (screenPosition - pix).Abs() - pix;
    return offset.x > 0 || offset.y > 0;
  }
}
