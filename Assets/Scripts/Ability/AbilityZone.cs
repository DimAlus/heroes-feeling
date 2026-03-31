using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;



public class AbilityZone : AbilityBase {



  protected void OnTriggerExit2D(Collider2D collision) {
    if (collision.transform.TryGetComponent(out Entity entity)) {
      RemoveEffects(entity);
    }
  }



  public override void Activate() {
    base.Activate();
    CooldownActivated = false;
    Collider.enabled = true;
  }

  public override void Deactivate() {
    base.Deactivate();
    Collider.enabled = false;
  }

}
