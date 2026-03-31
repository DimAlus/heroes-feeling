using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class GameData {

  static string SourceGO = "./Data/go.json";
  static string SourceAbility = "./Data/ability.json";

  static Dictionary<string, EntityData> entityData = new Dictionary<string, EntityData>();
  static Dictionary<string, AbilityObjData> abilityData = new Dictionary<string, AbilityObjData>();


  [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
  public static void LoadData() {
    LoadAbilities();
    LoadGameObjects();
  }


  private static void LoadAbilities() {
    abilityData.Clear();

    string fileContents = ReadFile(SourceAbility);
    if (fileContents == null) {
      return;
    }

    abilityData = JsonConvert.DeserializeObject<Dictionary<string, AbilityObjData>>(fileContents);

    foreach (string abilityName in abilityData.Keys) {
      AbilityObjData ability = abilityData[abilityName];
      if (ability.Projectile != null) {
        if (abilityData.ContainsKey(ability.Projectile)) {
          ability.ProjectileObj = abilityData[ability.Projectile];
        } else {
          Debug.LogError($"Ability [{ability.Projectile}] not found!");
        }
      }
    }
  }


  private static void LoadGameObjects() {
    entityData.Clear();

    string fileContents = ReadFile(SourceGO);
    if (fileContents == null) {
      return;
    }
    entityData = JsonConvert.DeserializeObject<Dictionary<string, EntityData>>(fileContents);
    foreach (string entity in entityData.Keys) {
      EntityData data = entityData[entity];
      var keys = data.Ability?.Abilities?.Keys;
      if (keys == null) {
        continue;
      }
      foreach (EAbilitySlot slot in keys) {
        AbilityInfoData abilityInfo = data.Ability.Abilities[slot];
        if (abilityData.ContainsKey(abilityInfo.Ability)) {
          abilityInfo.AbilityObj = abilityData[abilityInfo.Ability];
        } else {
          Debug.LogError($"Ability [{abilityInfo.Ability}] not found!");
        }
      }
    }
  }


  private static string ReadFile(string filename) {
    try {
      return File.ReadAllText(filename);
    } catch (Exception e) {
      Debug.LogError($"The file could not be read: {e.Message}");
      return null;
    }
  }


  public static EntityData GetEntityData(string Name) {
    if (entityData.ContainsKey(Name)) {
      return entityData[Name];
    }
    return null;
  }

  //struct JSHealthData {
  //  public float MaxHealth;
  //}

  //struct JSEntityData {
  //  public JSHealthData Health;
  //}

}
