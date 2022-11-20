using UnityEngine;

[RequireComponent(typeof(MenuTextInput))]
public class LookAtCursor : MonoBehaviour {
  MeshRenderer meshRenderer;

  private MenuTextInput menuTextInput;
  public float lookAtZOffset = 20;
  [Tooltip("Seconds for the text to angle towards, or away, from the cursor.")]
  public float lookAtTime = 0.2f;

  private void Start () {
    menuTextInput = GetComponent<MenuTextInput>();
  }

  private void Update () {
    Quaternion lookRotation = menuTextInput.hoveringText() ? lookAtCursor() : Quaternion.identity;
    transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * (1/lookAtTime));
  }

  private Quaternion lookAtCursor() {
    // It only turns towards the mouse x, on y it points to screen center because it looked better
    Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(
      new Vector3(menuTextInput.mousePos.x, Screen.height / 2, lookAtZOffset)
    );
    mouseWorld.z *= -1;

    Vector3 forward = mouseWorld - transform.position;
    return Quaternion.LookRotation(forward, Vector3.up);
  }
}
