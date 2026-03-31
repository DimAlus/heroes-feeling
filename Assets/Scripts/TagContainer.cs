using UnityEngine;


public class TagContainer : MonoBehaviour {
  [SerializeField]
  private ETag Tags;



  public void SetFraction(ETag fraction) {
    Tags = (Tags & ~ETag.Fraction) | fraction;
  }


  public bool HasTag(ETag tag) {
    return (Tags & tag) == tag;
  }


  public bool TagsAgreement(ETag includeTags, ETag[] excludeTags) {
    if (excludeTags != null) {
      foreach (ETag exclude in excludeTags) {
        if (HasTag(exclude)) {
          return false;
        }
      }
    }
    return HasTag(includeTags) || includeTags == 0;
  }
}
