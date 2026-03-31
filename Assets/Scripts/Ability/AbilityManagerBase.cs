using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class FPrefabInfo {
  public string Name;
  public GameObject GameObject;
  public bool Exists;
}


public class AbilityManagerBase : MonoBehaviour {


  [SerializeField]
  protected List<FPrefabInfo> AbilityPrefabs = new List<FPrefabInfo>();

  protected Dictionary<EAbilitySlot, AbilityBase> Abilities = new Dictionary<EAbilitySlot, AbilityBase>();


  private Entity entity;
  private AbilityData abilityData;


  protected virtual void Awake() {
    entity = GetComponent<Entity>();
  }

  protected void Start() {
    if (entity.EntityData.Ability?.Abilities != null) {
      abilityData = entity.EntityData.Ability;
      foreach (var item in abilityData.Abilities) {
        GameObject newObject;
        FPrefabInfo prefab = AbilityPrefabs.Find((FPrefabInfo el) => el.Name == item.Value.AbilityObj.PrefabName);

        if (prefab == null || prefab.GameObject == null) {
          Debug.LogError($"Prefab [{item.Value.AbilityObj.PrefabName}] not found for Ability [{item.Key}] of Object [{entity.EntityName}]");
          continue;
        }
        if (prefab.GameObject.gameObject.scene.rootCount == 0) {
          newObject = Instantiate(prefab.GameObject, transform.position, new Quaternion(), transform);
        } else {
          newObject = prefab.GameObject;
        }


        AbilityBase ability = newObject.GetComponent<AbilityBase>();
        Abilities.Add(item.Key, ability);
        ability.SetActivatedAbility(item.Value);
        ability.Initialize(item.Value.AbilityObj, AbilityPrefabs);

        AbilityAnimation anim = newObject.GetComponent<AbilityAnimation>();

        if (item.Key == EAbilitySlot.Death) {
          entity.OnDead += (FAbilityContext context) => {
            ability.Activate();
          };
        }

      }
    }
  }


  [ContextMenu("Load Ability Data")]
  protected void UpdateAbilityPrefabsList() {
    Entity entity = GetComponent<Entity>();
    GameData.LoadData();

    AbilityPrefabs.ForEach((FPrefabInfo info) => info.Exists = false);

    EntityData data = GameData.GetEntityData(entity.EntityName);
    if (data == null) {
      Debug.LogError($"Entity data not found for [{entity.EntityName}]");
    }

    var abilities = data.Ability?.Abilities;
    if (abilities == null) {
      return;
    }

    foreach (var item in abilities) {
      UpdateAbilityPrefabNames(item.Value.AbilityObj);
    }
  }


  private void UpdateAbilityPrefabNames(AbilityObjData data) {
    FPrefabInfo prefab = AbilityPrefabs.Find((FPrefabInfo p) => p.Name == data.PrefabName);
    if (prefab == null) {
      AbilityPrefabs.Add(new FPrefabInfo { Name = data.PrefabName, Exists = true, GameObject = null });
    } else {
      prefab.Exists = true;
    }
    if (data.ProjectileObj != null) {
      UpdateAbilityPrefabNames(data.ProjectileObj);
    }
  }

}
