using UnityEngine;

public class AbilityManagerPlayer : AbilityManagerBase {

  private void FixedUpdate() {
    foreach (var iter in Abilities) {
      if (Inputs.Instance.IsAbilityInputActive(iter.Key)) {
        iter.Value.TryActivate();
      }
    }
  }

}
