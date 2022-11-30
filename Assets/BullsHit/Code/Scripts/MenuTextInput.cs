using UnityEngine;
using UnityEngine.Events;

public class MenuTextInput : MonoBehaviour {
  public UnityEvent clickEvent;

  [HideInInspector]
  public Vector3 mousePos;
  private MeshRenderer meshRenderer;

  private void Start() {
    meshRenderer = GetComponent<MeshRenderer>();
    if (clickEvent == null) {
      clickEvent = new UnityEvent();
    }
  }

  private void Update() {
    mousePos = Input.mousePosition;

    if (hoveringText() && Input.GetButtonUp("Click")) {
      clickEvent.Invoke();
    }
  }

  public bool hoveringText() {
    float boundsX = meshRenderer.bounds.extents.x;
    float boundsY = meshRenderer.bounds.extents.y;
    Vector3 topRight = new Vector3(transform.position.x + boundsX, transform.position.y + boundsY, transform.position.z);
    Vector3 bottomLeft = new Vector3(transform.position.x - boundsX, transform.position.y - boundsY, transform.position.z);

    Vector3 topRightScreen = Camera.main.WorldToScreenPoint(topRight);
    Vector3 bottomLeftScreen = Camera.main.WorldToScreenPoint(bottomLeft);

    return !(mousePos.x > bottomLeftScreen.x ||
      mousePos.x < topRightScreen.x ||
      mousePos.y < bottomLeftScreen.y ||
      mousePos.y > topRightScreen.y);
  }
}
