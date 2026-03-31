using System.Collections.Generic;
using UnityEngine;

public class Society : MonoBehaviour {

  public static Society Instance { get; private set; }

  private LinkedList<Entity> entities = new LinkedList<Entity>();


  private void Awake() {
    Instance = this;
  }


  public LinkedListNode<Entity> AddEntity(Entity entity) {
    return entities.AddLast(entity);
  }


  public void RemoveEntity(Entity entity) {
    entities.Remove(entity);
  }


  public void RemoveEntity(LinkedListNode<Entity> entity) {
    entities.Remove(entity);
  }


  public Entity FindNearestEntity(Vector3 position, ETag includeTags, ETag[] excludeTags) {
    Entity res = null;
    float nearest = 0;
    foreach (Entity entity in entities) {
      if (entity.TagContainer.TagsAgreement(includeTags, excludeTags)) {
        float magnitude = (entity.transform.position - position).sqrMagnitude;
        if (res == null || nearest > magnitude) {
          res = entity;
          nearest = magnitude;
        }
      }
    }
    return res;
  }


  public Entity FindAnyEntityAtDistance(Vector3 position, Vector2 distance, ETag includeTags, ETag[] excludeTags) {
    Vector2 sqrDistance = new Vector2(distance.x * distance.x, distance.y * distance.y);
    foreach (Entity entity in entities) {
      if (entity.TagContainer.TagsAgreement(includeTags, excludeTags)) {
        float magnitude = (entity.transform.position - position).sqrMagnitude;
        if (magnitude >= sqrDistance.x && magnitude <= sqrDistance.y) {
          return entity;
        }
      }
    }
    return null;
  }


  public List<Entity> FindAllEntityAtDistance(Vector3 position, Vector2 distance, ETag includeTags, ETag[] excludeTags) {
    List<Entity> result = new List<Entity>();
    Vector2 sqrDistance = new Vector2(distance.x * distance.x, distance.y * distance.y);
    foreach (Entity entity in entities) {
      if (entity.TagContainer.TagsAgreement(includeTags, excludeTags)) {
        float magnitude = (entity.transform.position - position).sqrMagnitude;
        if (magnitude >= sqrDistance.x && magnitude <= sqrDistance.y) {
          result.Add(entity);
        }
      }
    }
    return result;
  }

}
