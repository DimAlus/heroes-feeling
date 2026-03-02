using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;



public class AbilityZone : AbilityBase {

  protected List<Entity> entities = new List<Entity>();

  protected override void OnTriggerEnter2D(Collider2D collision) {
    if (collision.transform.TryGetComponent(out Entity entity)) {
      if (entity.TagContainer.TagsAgreement(data.TagsInclude, data.TagsExclude)) {
        entities.Add(entity);
      }
    }
  }

  protected void OnTriggerExit2D(Collider2D collision) {
    if (collision.transform.TryGetComponent(out Entity entity)) {
      entities.Remove(entity);
    }
  }

  protected override void FixedUpdate() {
    base.FixedUpdate();
    if (GetCooldown() <= 0) {
      Activate();
      foreach (Entity entity in entities) {
        ApplyEffects(entity);
      }
    }
  }


  public override bool CanActivate() {
    return false;
  }

}
