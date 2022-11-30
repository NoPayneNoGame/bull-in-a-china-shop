using UnityEngine;

public class DoorGlassHandler : MonoBehaviour {
  public void setFragmentsToNoCollide() {
    int noCollide = LayerMask.NameToLayer("NoCollide");

    for (int i = 0; i < transform.childCount; i++) {
      Transform child = transform.GetChild(i);
      if (child.name.Contains("Fragments")) {
        foreach (Transform childChild in child.GetComponentsInChildren<Transform>()) {
          childChild.gameObject.layer = noCollide;
        }
      }
    }
  }
}
